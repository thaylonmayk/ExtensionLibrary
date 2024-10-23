namespace CollectionExtensionsLibrary
{
    public static partial class CollectionExtensions
    {
        /// <summary>
        /// Adds a range of items to the collection if they do not already exist in the collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The updated collection.</returns>
        public static IEnumerable<T> AddRangeIfNotExists<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            var list = source.ToList();
            foreach (var item in items)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }
            return list;
        }
        /// <summary>
        /// Removes all items from the collection that match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="predicate">The predicate used to determine which items to remove.</param>
        /// <returns>The updated collection.</returns>
        public static IEnumerable<T> RemoveAll<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            source.Where(item => !predicate(item));

        /// <summary>
        /// Finds all elements in the collection that match the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="predicate">The predicate used to determine which elements to find.</param>
        /// <returns>A collection of elements that match the predicate.</returns>
        public static IEnumerable<T> FindAll<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            source.Where(predicate);

        /// <summary>
        /// Sorts the collection by the specified key selector.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <typeparam name="TKey">The type of the key to sort by.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="keySelector">The key selector used to sort the collection.</param>
        /// <returns>A sorted collection.</returns>
        public static IEnumerable<T> SortBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) =>
            source.OrderBy(keySelector);


        /// <summary>
        /// Performs the specified action on each element of the IEnumerable.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IEnumerable.</typeparam>
        /// <param name="source">The source IEnumerable.</param>
        /// <param name="action">The action to perform on each element.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified key selector.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source IEnumerable.</typeparam>
        /// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
        /// <param name="source">The source IEnumerable.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>An IEnumerable that contains distinct elements from the source sequence.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate if a condition is true.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source IEnumerable.</typeparam>
        /// <param name="source">The source IEnumerable.</param>
        /// <param name="condition">A boolean value that determines whether to apply the predicate.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An IEnumerable that contains elements from the input sequence that satisfy the condition.</returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (condition)
            {
                return source.Where(predicate);
            }
            return source;
        }

        /// <summary>
        /// Splits the IEnumerable into chunks of a specified size.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source IEnumerable.</typeparam>
        /// <param name="source">The source IEnumerable.</param>
        /// <param name="chunkSize">The size of each chunk.</param>
        /// <returns>An IEnumerable of IEnumerable chunks.</returns>
        public static IEnumerable<IEnumerable<TSource>> ChunkBy<TSource>(this IEnumerable<TSource> source, int chunkSize)
        {
            while (source.Any())
            {
                yield return source.Take(chunkSize);
                source = source.Skip(chunkSize);
            }
        }

        /// <summary>
        /// Creates a dictionary from an IEnumerable according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source IEnumerable.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <typeparam name="TValue">The type of the value returned by valueSelector.</typeparam>
        /// <param name="source">The source IEnumerable.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="valueSelector">A function to extract a value from each element.</param>
        /// <returns>A dictionary that contains keys and values.</returns>
        public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector) =>
            source.ToDictionary(keySelector, valueSelector);

        /// <summary>
        /// Applies a specified function to the corresponding elements of two sequences, producing a sequence of the results.
        /// </summary>
        /// <typeparam name="TFirst">The type of elements of the first input sequence.</typeparam>
        /// <typeparam name="TSecond">The type of elements of the second input sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements of the result sequence.</typeparam>
        /// <param name="first">The first input sequence.</param>
        /// <param name="second">The second input sequence.</param>
        /// <param name="resultSelector">A function that specifies how to combine the corresponding elements of the two sequences.</param>
        /// <returns>An IEnumerable that contains elements of type TResult that are obtained by applying the resultSelector function to the corresponding elements of the input sequences.</returns>
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            using (var iterator1 = first.GetEnumerator())
            using (var iterator2 = second.GetEnumerator())
            {
                while (iterator1.MoveNext() && iterator2.MoveNext())
                {
                    yield return resultSelector(iterator1.Current, iterator2.Current);
                }
            }
        }

        /// <summary>
        /// Randomizes the order of elements in the IEnumerable.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IEnumerable.</typeparam>
        /// <param name="source">The source IEnumerable.</param>
        /// <returns>An IEnumerable with elements in random order.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var random = new Random();
            return source.OrderBy(x => random.Next());
        }

        /// <summary>
        /// Paginates the IEnumerable into pages of a specified size.
        /// </summary>
        /// <typeparam name="T">The type of elements in the IEnumerable.</typeparam>
        /// <param name="source">The source IEnumerable.</param>
        /// <param name="pageNumber">The page number to retrieve (starting from 1).</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <returns>An IEnumerable representing the specified page.</returns>
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int pageNumber, int pageSize) =>
            source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

    }
}
