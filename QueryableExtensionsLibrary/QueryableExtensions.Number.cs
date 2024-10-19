using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QueryableExtensionsLibrary
{
    public static partial class QueryableExtensions
    {
        /// <summary>
        /// Counts the number of elements in the IQueryable sequence that satisfy the specified condition.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to count.</param>
        /// <param name="predicate">The condition to satisfy.</param>
        /// <returns>The number of elements that satisfy the condition.</returns>
        public static int Count<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            return queryable.Count(predicate);
        }
        /// <summary>
        /// Sums the values of the specified property for the elements in the IQueryable sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to sum.</param>
        /// <param name="property">The property to sum.</param>
        /// <returns>The sum of the values of the specified property.</returns>
        public static decimal Sum<T>(this IQueryable<T> queryable, string property)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Create(property, parameter);
            var selector = Expression.Lambda<Func<T, decimal>>(body, parameter);
            return queryable.Sum(selector);
        }
        /// <summary>
        /// Retrieves the minimum value of the specified property for the elements in the IQueryable sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to find the minimum value.</param>
        /// <param name="property">The property to find the minimum value.</param>
        /// <returns>The minimum value of the specified property.</returns>
        public static decimal Min<T>(this IQueryable<T> queryable, string property)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Create(property, parameter);
            var selector = Expression.Lambda<Func<T, decimal>>(body, parameter);
            return queryable.Min(selector);
        }
        /// <summary>
        /// Retrieves the maximum value of the specified property for the elements in the IQueryable sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to find the maximum value.</param>
        /// <param name="property">The property to find the maximum value.</param>
        /// <returns>The maximum value of the specified property.</returns>
        public static decimal Max<T>(this IQueryable<T> queryable, string property)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Create(property, parameter);
            var selector = Expression.Lambda<Func<T, decimal>>(body, parameter);
            return queryable.Max(selector);
        }
        /// <summary>
        /// Computes the average of the values of the specified property for the elements in the IQueryable sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to compute the average for.</param>
        /// <param name="property">The property to compute the average of.</param>
        /// <returns>The average value of the specified property.</returns>
        public static decimal Average<T>(this IQueryable<T> queryable, string property)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = Create(property, parameter);
            var selector = Expression.Lambda<Func<T, decimal>>(body, parameter);
            return queryable.Average(selector);
        }


    }
}
