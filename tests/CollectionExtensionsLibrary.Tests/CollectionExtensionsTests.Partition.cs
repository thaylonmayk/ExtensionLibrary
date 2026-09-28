using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CollectionExtensionsLibrary;
using Xunit;

namespace CollectionExtensionsLibrary.Tests
{
    public class CollectionExtensionsPartitionTests
    {
        [Fact]
        public void HasItems_ShouldIdentifyNonEmptyCollections()
        {
            var list = new List<int> { 1, 2, 3 };
            var empty = new List<int>();
            List<int>? nullList = null;

            Assert.True(list.HasItems());
            Assert.False(empty.HasItems());
            Assert.False(nullList.HasItems());
        }

        [Fact]
        public void Partition_ShouldSplitElementsCorrectly()
        {
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var (evens, odds) = numbers.Partition(n => n % 2 == 0);

            Assert.Equal(new[] { 2, 4, 6, 8, 10 }, evens);
            Assert.Equal(new[] { 1, 3, 5, 7, 9 }, odds);
        }

        [Fact]
        public void Partition_WhenEmpty_ShouldReturnEmptyLists()
        {
            var empty = new List<string>();

            var (matches, nonMatches) = empty.Partition(s => s.Length > 3);

            Assert.Empty(matches);
            Assert.Empty(nonMatches);
        }

        [Fact]
        public void Partition_WhenNullArguments_ShouldThrowArgumentNullException()
        {
            List<int>? nullList = null;
            Assert.Throws<ArgumentNullException>(() => nullList.Partition(n => n > 0));

            var validList = new List<int> { 1 };
            Assert.Throws<ArgumentNullException>(() => validList.Partition(null));
        }

        [Fact]
        public async Task ForEachAsync_ShouldExecuteForAllItemsWithThrottling()
        {
            var items = Enumerable.Range(1, 20).ToList();
            int currentConcurrentCount = 0;
            int maxObservedConcurrency = 0;
            var processed = new List<int>();
            var lockObj = new object();

            await items.ForEachAsync(async (item, ct) =>
            {
                int current = Interlocked.Increment(ref currentConcurrentCount);

                lock (lockObj)
                {
                    if (current > maxObservedConcurrency)
                    {
                        maxObservedConcurrency = current;
                    }
                }

                await Task.Delay(10, ct);

                lock (lockObj)
                {
                    processed.Add(item);
                }

                Interlocked.Decrement(ref currentConcurrentCount);
            }, maxDegreeOfParallelism: 4);

            Assert.Equal(20, processed.Count);
            Assert.True(maxObservedConcurrency <= 4, $"Observed concurrency {maxObservedConcurrency} exceeded limit 4.");
        }

        [Fact]
        public async Task ForEachAsync_WhenCancelled_ShouldAbort()
        {
            var items = Enumerable.Range(1, 100).ToList();
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            {
                await items.ForEachAsync(async (item, ct) =>
                {
                    await Task.Yield();
                }, maxDegreeOfParallelism: 2, cancellationToken: cts.Token);
            });
        }
    }
}
