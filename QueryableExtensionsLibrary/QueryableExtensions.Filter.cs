using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryableExtensionsLibrary
{
    public static partial class QueryableExtensions
    {
        private static readonly MethodInfo OrderByMethod = typeof(Queryable).GetMethods()
            .First(m => m.Name == nameof(Queryable.OrderBy) && m.GetParameters().Length == 2);

        private static readonly MethodInfo OrderByDescendingMethod = typeof(Queryable).GetMethods()
            .First(m => m.Name == nameof(Queryable.OrderByDescending) && m.GetParameters().Length == 2);
        /// <summary>
        /// Filters the IQueryable sequence based on the specified property and value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to filter.</param>
        /// <param name="property">The property to filter on.</param>
        /// <param name="value">The value to filter by.</param>
        /// <returns>A filtered IQueryable sequence.</returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, string property, object value) =>
            queryable.Filter(property, string.Empty, value);

        /// <summary>
        /// Filters the IQueryable sequence based on the specified property, comparison, and value.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to filter.</param>
        /// <param name="property">The property to filter on.</param>
        /// <param name="comparison">The comparison operator to use.</param>
        /// <param name="value">The value to filter by.</param>
        /// <returns>A filtered IQueryable sequence.</returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, string property, string comparison, object value)
        {
            if (queryable is null) throw new ArgumentNullException(nameof(queryable));
            if (string.IsNullOrWhiteSpace(property) || value is null || string.IsNullOrWhiteSpace(value.ToString())) return queryable;

            var parameter = Expression.Parameter(typeof(T));
            Expression left;
            try
            {
                left = Create(property, parameter);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"A propriedade '{property}' não foi encontrada no tipo '{typeof(T).FullName}'.", nameof(property), ex);
            }

            var propertyType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
            var convertedValue = Change(value, propertyType);

            var right = Expression.Constant(convertedValue, left.Type);

            var body = Create(left, comparison, right);

            var expression = Expression.Lambda<Func<T, bool>>(body, parameter);

            return queryable.Where(expression);
        }

        /// <summary>
        /// Orders the IQueryable sequence based on the specified property and direction.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to order.</param>
        /// <param name="property">The property to order by.</param>
        /// <param name="ascending">True to order in ascending direction; false to order in descending direction.</param>
        /// <returns>An ordered IQueryable sequence.</returns>
        public static IQueryable<T> Order<T>(this IQueryable<T> queryable, string property, bool ascending)
        {
            if (queryable is null || string.IsNullOrWhiteSpace(property)) return queryable;

            var parameter = Expression.Parameter(typeof(T));
            var body = Create(property, parameter);
            var lambda = Expression.Lambda(body, parameter);

            var methodDefinition = ascending ? OrderByMethod : OrderByDescendingMethod;
            var genericMethod = methodDefinition.MakeGenericMethod(typeof(T), body.Type);

            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { queryable, lambda });
        }

        /// <summary>
        /// Paginates the IQueryable sequence based on the specified page index and size.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to paginate.</param>
        /// <param name="index">The page index (starting from 1).</param>
        /// <param name="size">The size of each page.</param>
        /// <returns>A paginated IQueryable sequence.</returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, int index, int size) =>
            queryable is null || index <= 0 || size <= 0 ? queryable : queryable.Skip((index - 1) * size).Take(size);

        /// <summary>
        /// Groups the IQueryable sequence by the specified property.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key to group by.</typeparam>
        /// <param name="queryable">The IQueryable sequence to group.</param>
        /// <param name="property">The property to group by.</param>
        /// <returns>An IQueryable sequence grouped by the specified property.</returns>
        public static IQueryable<IGrouping<TKey, T>> GroupBy<T, TKey>(this IQueryable<T> queryable, string property)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Create(property, parameter);
            var keySelector = Expression.Lambda<Func<T, TKey>>(body, parameter);
            return queryable.GroupBy(keySelector);
        }

        /// <summary>
        /// Removes duplicate elements from the IQueryable sequence based on the specified property.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key to compare for distinctness.</typeparam>
        /// <param name="queryable">The IQueryable sequence to remove duplicates from.</param>
        /// <param name="property">The property to use for comparing distinctness.</param>
        /// <returns>An IQueryable sequence with duplicates removed.</returns>
        public static IQueryable<T> DistinctBy<T, TKey>(this IQueryable<T> queryable, string property)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Create(property, parameter);
            var keySelector = Expression.Lambda<Func<T, TKey>>(body, parameter);
            return queryable.GroupBy(keySelector).Select(g => g.First());
        }


        /// <summary>
        /// Changes the type of the value to the specified type.
        /// </summary>
        /// <param name="value">The value to change the type of.</param>
        /// <param name="type">The type to change to.</param>
        /// <returns>The value converted to the specified type.</returns>
        private static object Change(object value, Type type)
        {
            if (value is null) return null;

            var targetType = Nullable.GetUnderlyingType(type) ?? type;

            if (value.GetType() == targetType)
            {
                return value;
            }

            var stringValue = value.ToString();
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            if (targetType == typeof(Guid))
            {
                return Guid.Parse(stringValue);
            }

            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, stringValue, ignoreCase: true);
            }

            if (targetType == typeof(DateTime))
            {
                return DateTime.Parse(stringValue, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind);
            }

            if (targetType == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(stringValue, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind);
            }

            if (targetType == typeof(TimeSpan))
            {
                return TimeSpan.Parse(stringValue, System.Globalization.CultureInfo.InvariantCulture);
            }

            return Convert.ChangeType(value, targetType, System.Globalization.CultureInfo.InvariantCulture);
        }

    }
}