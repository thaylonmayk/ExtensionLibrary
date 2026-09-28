using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CollectionExtensionsLibrary
{
    public static partial class CollectionExtensions
    {
        /// <summary>
        /// Determines whether the collection contains at least one element.
        /// Performs an O(1) check when the collection implements ICollection or IReadOnlyCollection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The collection to evaluate.</param>
        /// <returns>True if the collection is not null and has at least one element; otherwise false.</returns>
        public static bool HasItems<T>(this IEnumerable<T> source)
        {
            return !source.IsNullOrEmpty();
        }

        /// <summary>
        /// Partitions a collection into two lists based on a predicate in a single iteration pass.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="predicate">The condition used to partition items.</param>
        /// <returns>A tuple containing (Matches, NonMatches).</returns>
        /// <exception cref="ArgumentNullException">Thrown when source or predicate is null.</exception>
        public static (List<T> Matches, List<T> NonMatches) Partition<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            int estimatedCapacity = EstimateInitialCapacity(source);
            var matches = new List<T>(estimatedCapacity);
            var nonMatches = new List<T>(estimatedCapacity);

            foreach (T item in source)
            {
                if (predicate(item))
                {
                    matches.Add(item);
                }
                else
                {
                    nonMatches.Add(item);
                }
            }

            return (matches, nonMatches);
        }

        /// <summary>
        /// Executes an asynchronous operation across collection elements with controlled concurrency throttling.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The collection to iterate.</param>
        /// <param name="action">The asynchronous action to execute for each element.</param>
        /// <param name="maxDegreeOfParallelism">The maximum number of concurrent tasks allowed.</param>
        /// <param name="cancellationToken">Cancellation token to cancel ongoing execution.</param>
        /// <returns>A task that completes when all operations have finished.</returns>
        /// <exception cref="ArgumentNullException">Thrown when source or action is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when maxDegreeOfParallelism is less than 1.</exception>
        public static async Task ForEachAsync<T>(
            this IEnumerable<T> source,
            Func<T, CancellationToken, Task> action,
            int maxDegreeOfParallelism = 4,
            CancellationToken cancellationToken = default)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (maxDegreeOfParallelism < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism), "Max degree of parallelism must be at least 1.");
            }

            using (var throttler = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism))
            {
                var tasks = new List<Task>();

                foreach (T item in source)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await throttler.WaitAsync(cancellationToken).ConfigureAwait(false);

                    tasks.Add(ExecuteThrottledActionAsync(item, action, throttler, cancellationToken));
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Executes an asynchronous operation across collection elements with controlled concurrency throttling.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The collection to iterate.</param>
        /// <param name="action">The asynchronous action to execute for each element.</param>
        /// <param name="maxDegreeOfParallelism">The maximum number of concurrent tasks allowed.</param>
        /// <returns>A task that completes when all operations have finished.</returns>
        public static Task ForEachAsync<T>(
            this IEnumerable<T> source,
            Func<T, Task> action,
            int maxDegreeOfParallelism = 4)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return source.ForEachAsync((item, ct) => action(item), maxDegreeOfParallelism, CancellationToken.None);
        }

        private static int EstimateInitialCapacity<T>(IEnumerable<T> source)
        {
            if (source is ICollection<T> genericCollection)
            {
                return genericCollection.Count / 2;
            }

            if (source is IReadOnlyCollection<T> readOnlyCollection)
            {
                return readOnlyCollection.Count / 2;
            }

            return 0;
        }

        private static async Task ExecuteThrottledActionAsync<T>(
            T item,
            Func<T, CancellationToken, Task> action,
            SemaphoreSlim throttler,
            CancellationToken cancellationToken)
        {
            try
            {
                await action(item, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                throttler.Release();
            }
        }
    }
}
