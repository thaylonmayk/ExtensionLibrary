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
        /// Determines whether any elements of the IQueryable sequence satisfy the specified condition.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IQueryable sequence.</typeparam>
        /// <param name="queryable">The IQueryable sequence to check.</param>
        /// <param name="predicate">The condition to satisfy.</param>
        /// <returns>True if any elements satisfy the condition; otherwise, false.</returns>
        public static bool Any<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate)
        {
            return queryable.Any(predicate);
        }

    }
}
