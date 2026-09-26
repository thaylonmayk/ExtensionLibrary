using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionExtensionsLibrary
{
    public static partial class CollectionExtensions
    {
        /// <summary>
        /// Adds a range of items to the list if they do not already exist in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to add items to.</param>
        /// <param name="items">The items to add.</param>
        public static void AddRangeIfNotExists<T>(this List<T> list, IEnumerable<T> items)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));
            if (items is null) throw new ArgumentNullException(nameof(items));

            var existing = new HashSet<T>(list);
            foreach (var item in items)
            {
                if (existing.Add(item))
                {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds the elements of the specified sequence to the destination collection if the sequence is not null.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="destination">The destination collection to which elements should be added.</param>
        /// <param name="source">The sequence of elements to add, or null.</param>
        public static void AddRangeIfNotNull<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (source is null)
            {
                return;
            }

            foreach (var item in source)
            {
                destination.Add(item);
            }
        }

        /// <summary>
        /// Sorts the list by the specified key selector.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <typeparam name="TKey">The type of the key to sort by.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="keySelector">The key selector used to sort the list.</param>
        public static void SortBy<T, TKey>(this List<T> list, Func<T, TKey> keySelector)
        {
            list.Sort((x, y) => Comparer<TKey>.Default.Compare(keySelector(x), keySelector(y)));
        }

        /// <summary>
        /// Returns distinct elements from a list by using a specified key selector.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source list.</typeparam>
        /// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>A list that contains distinct elements from the source list.</returns>
        public static IList<TSource> DistinctBy<TSource, TKey>(this IList<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (keySelector is null) throw new ArgumentNullException(nameof(keySelector));

            var seenKeys = new HashSet<TKey>();
            var result = new List<TSource>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    result.Add(element);
                }
            }
            return result;
        }

        /// <summary>
        /// Filters a list of values based on a predicate if a condition is true.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source list.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="condition">A boolean value that determines whether to apply the predicate.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A list that contains elements from the input list that satisfy the condition.</returns>
        public static IList<TSource> WhereIf<TSource>(this IList<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
            {
                return source.Where(predicate).ToList();
            }
            return source;
        }

        /// <summary>
        /// Splits the list into chunks of a specified size.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source list.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="chunkSize">The size of each chunk.</param>
        /// <returns>A list of list chunks.</returns>
        public static IList<IList<TSource>> ChunkBy<TSource>(this IList<TSource> source, int chunkSize)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (chunkSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chunkSize), "O tamanho do lote deve ser maior que zero.");
            }

            int totalCount = source.Count;
            int totalChunks = (totalCount + chunkSize - 1) / chunkSize;
            var chunks = new List<IList<TSource>>(totalChunks);

            for (int i = 0; i < totalCount; i += chunkSize)
            {
                int count = Math.Min(chunkSize, totalCount - i);
                var chunk = new List<TSource>(count);
                for (int j = 0; j < count; j++)
                {
                    chunk.Add(source[i + j]);
                }
                chunks.Add(chunk);
            }

            return chunks;
        }

        /// <summary>
        /// Creates a dictionary from a list according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source list.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <typeparam name="TValue">The type of the value returned by valueSelector.</typeparam>
        /// <param name="source">The source list.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="valueSelector">A function to extract a value from each element.</param>
        /// <returns>A dictionary that contains keys and values.</returns>
        public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(this IList<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector) => source.ToDictionary(keySelector, valueSelector);

        /// <summary>
        /// Applies a specified function to the corresponding elements of two IList sequences, producing a sequence of the results.
        /// </summary>
        /// <typeparam name="TFirst">The type of elements of the first input sequence.</typeparam>
        /// <typeparam name="TSecond">The type of elements of the second input sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements of the result sequence.</typeparam>
        /// <param name="first">The first input sequence.</param>
        /// <param name="second">The second input sequence.</param>
        /// <param name="resultSelector">A function that specifies how to combine the corresponding elements of the two sequences.</param>
        /// <returns>An IList that contains elements of type TResult that are obtained by applying the resultSelector function to the corresponding elements of the input sequences.</returns>
        public static IList<TResult> Zip<TFirst, TSecond, TResult>(this IList<TFirst> first, IList<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            var result = new List<TResult>();
            int minLength = Math.Min(first.Count, second.Count);
            for (int i = 0; i < minLength; i++)
            {
                result.Add(resultSelector(first[i], second[i]));
            }
            return result;
        }

        /// <summary>
        /// Moves an item from one index to another in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to modify.</param>
        /// <param name="oldIndex">The index of the item to move.</param>
        /// <param name="newIndex">The index to move the item to.</param>
        public static void Move<T>(this IList<T> list, int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= list.Count || newIndex < 0 || newIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            var item = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);
        }

        /// <summary>
        /// Replaces all occurrences of a specified value with a new value in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to modify.</param>
        /// <param name="oldValue">The value to replace.</param>
        /// <param name="newValue">The value to replace with.</param>
        public static void Replace<T>(this IList<T> list, T oldValue, T newValue)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));

            var comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < list.Count; i++)
            {
                if (comparer.Equals(list[i], oldValue))
                {
                    list[i] = newValue;
                }
            }
        }
        /// <summary>
        /// Paginates the list into pages of a specified size.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The source list.</param>
        /// <param name="pageNumber">The page number to retrieve (starting from 1).</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <returns>A list representing the specified page.</returns>
        public static List<T> Paginate<T>(this List<T> list, int pageNumber, int pageSize) =>
            list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

    }
}
