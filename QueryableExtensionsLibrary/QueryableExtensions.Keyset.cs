#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QueryableExtensionsLibrary
{
    /// <summary>
    /// Specifies navigation direction for keyset pagination.
    /// </summary>
    public enum SeekDirection
    {
        /// <summary>
        /// Forward navigation (next page).
        /// </summary>
        Forward,

        /// <summary>
        /// Backward navigation (previous page).
        /// </summary>
        Backward
    }

    /// <summary>
    /// Represents a paginated query result using keyset pagination.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type used for ordering.</typeparam>
    public class KeysetPagedList<T, TKey>
    {
        /// <summary>
        /// Items contained in the current page.
        /// </summary>
        public IReadOnlyList<T> Items { get; }

        /// <summary>
        /// Maximum number of items requested for the page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Indicates whether a subsequent page exists.
        /// </summary>
        public bool HasNextPage { get; }

        /// <summary>
        /// Indicates whether a preceding page exists.
        /// </summary>
        public bool HasPreviousPage { get; }

        /// <summary>
        /// The cursor key for requesting the next page.
        /// </summary>
        public TKey? NextCursor { get; }

        /// <summary>
        /// The cursor key for requesting the previous page.
        /// </summary>
        public TKey? PreviousCursor { get; }

        /// <summary>
        /// Initializes a new instance of KeysetPagedList.
        /// </summary>
        /// <param name="items">The items for the page.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="hasNextPage">True if there is a next page.</param>
        /// <param name="hasPreviousPage">True if there is a previous page.</param>
        /// <param name="nextCursor">The next cursor value.</param>
        /// <param name="previousCursor">The previous cursor value.</param>
        public KeysetPagedList(
            IReadOnlyList<T> items,
            int pageSize,
            bool hasNextPage,
            bool hasPreviousPage,
            TKey? nextCursor,
            TKey? previousCursor)
        {
            Items = items ?? Array.Empty<T>();
            PageSize = pageSize;
            HasNextPage = hasNextPage;
            HasPreviousPage = hasPreviousPage;
            NextCursor = nextCursor;
            PreviousCursor = previousCursor;
        }
    }

    public static partial class QueryableExtensions
    {
        /// <summary>
        /// Realiza a paginação inicial (primeira página) utilizando o método Keyset (Seek Method) sobre IQueryable.
        /// </summary>
        public static KeysetPagedList<T, TKey> ToKeysetPagedList<T, TKey>(
            this IQueryable<T> source,
            Expression<Func<T, TKey>> keySelector,
            int pageSize) where TKey : IComparable<TKey>
        {
            return source.ToKeysetPagedListInternal(keySelector, default, pageSize, SeekDirection.Forward, isFirstPage: true);
        }

        /// <summary>
        /// Realiza a paginação subsequente ou reversa a partir de um cursor de referência sobre IQueryable.
        /// </summary>
        public static KeysetPagedList<T, TKey> ToKeysetPagedList<T, TKey>(
            this IQueryable<T> source,
            Expression<Func<T, TKey>> keySelector,
            TKey cursor,
            int pageSize,
            SeekDirection direction = SeekDirection.Forward) where TKey : IComparable<TKey>
        {
            return source.ToKeysetPagedListInternal(keySelector, cursor, pageSize, direction, isFirstPage: false);
        }

        private static KeysetPagedList<T, TKey> ToKeysetPagedListInternal<T, TKey>(
            this IQueryable<T> source,
            Expression<Func<T, TKey>> keySelector,
            TKey? cursor,
            int pageSize,
            SeekDirection direction,
            bool isFirstPage) where TKey : IComparable<TKey>
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize), "O tamanho da página deve ser maior que zero.");

            IQueryable<T> query = source;

            if (!isFirstPage && cursor is not null)
            {
                var parameter = keySelector.Parameters[0];
                var keyAccess = keySelector.Body;
                var constant = Expression.Constant(cursor, typeof(TKey));

                BinaryExpression comparison = direction == SeekDirection.Forward
                    ? Expression.GreaterThan(keyAccess, constant)
                    : Expression.LessThan(keyAccess, constant);

                var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                query = query.Where(lambda);
            }

            query = direction == SeekDirection.Forward
                ? query.OrderBy(keySelector)
                : query.OrderByDescending(keySelector);

            var buffer = query.Take(pageSize + 1).ToList();
            bool hasMore = buffer.Count > pageSize;

            if (hasMore)
            {
                buffer.RemoveAt(buffer.Count - 1);
            }

            if (direction == SeekDirection.Backward)
            {
                buffer.Reverse();
            }

            bool hasNextPage = direction == SeekDirection.Forward ? hasMore : (!isFirstPage);
            bool hasPreviousPage = direction == SeekDirection.Forward ? (!isFirstPage) : hasMore;

            var compiledKey = keySelector.Compile();
            TKey? nextCursor = buffer.Count > 0 ? compiledKey(buffer[buffer.Count - 1]) : default;
            TKey? previousCursor = buffer.Count > 0 ? compiledKey(buffer[0]) : default;

            return new KeysetPagedList<T, TKey>(
                buffer,
                pageSize,
                hasNextPage,
                hasPreviousPage,
                nextCursor,
                previousCursor);
        }
    }
}

