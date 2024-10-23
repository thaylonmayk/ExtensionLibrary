namespace CollectionExtensionsLibrary
{
    public static partial class CollectionExtensions
    {

        /// <summary>
        /// Adds a range of items to the end of the Queue.
        /// </summary>
        /// <typeparam name="T">The type of elements in the Queue.</typeparam>
        /// <param name="queue">The Queue to add items to.</param>
        /// <param name="items">The items to add.</param>
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// Removes and returns the specified number of items from the beginning of the Queue.
        /// </summary>
        /// <typeparam name="T">The type of elements in the Queue.</typeparam>
        /// <param name="queue">The Queue to remove items from.</param>
        /// <param name="count">The number of items to remove.</param>
        /// <returns>An IEnumerable containing the removed items.</returns>
        public static IEnumerable<T> DequeueMultiple<T>(this Queue<T> queue, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (queue.Count == 0)
                {
                    yield break;
                }
                yield return queue.Dequeue();
            }
        }


    }
}
