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
        /// Returns the first element of the IQueryable sequence that satisfies the specified condition, or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to return an element from.</param>
        /// <param name="predicate">The condition to satisfy.</param>
        /// <returns>The first element that satisfies the condition, or default value if no such element is found.</returns>
        public static T FirstOrDefault<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            return queryable.FirstOrDefault(predicate);
        }


        /// <summary>
        /// Creates an expression for accessing a property by splitting the property name and aggregating the parts.
        /// </summary>
        /// <param name="property">The property name to create the expression for.</param>
        /// <param name="parameter">The parameter expression.</param>

        private static Expression Create(string property, Expression parameter) => property.Split('.').Aggregate(parameter, Expression.Property);

        /// <summary>
        /// Creates a binary expression for comparing two expressions using the specified comparison operator.
        /// </summary>
        /// <param name="left">The left expression.</param>
        /// <param name="comparison">The comparison operator.</param>
        /// <param name="right">The right expression.</param>
        /// <returns>A binary expression representing the comparison.</returns>
        private static Expression Create(Expression left, string comparison, Expression right)
        {
            if (string.IsNullOrWhiteSpace(comparison) && left.Type == typeof(string))
            {
                return Expression.Call(left, nameof(string.Contains), Type.EmptyTypes, right);
            }

            var type = comparison switch
            {
                "<" => ExpressionType.LessThan,
                "<=" => ExpressionType.LessThanOrEqual,
                ">" => ExpressionType.GreaterThan,
                ">=" => ExpressionType.GreaterThanOrEqual,
                "!=" => ExpressionType.NotEqual,
                _ => ExpressionType.Equal
            };

            return Expression.MakeBinary(type, left, right);
        }
    }
}
