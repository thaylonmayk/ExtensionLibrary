using System.Collections.Generic;

namespace CollectionExtensionsLibrary
{
    public static partial class CollectionExtensions
    {

        /// <summary>
        /// Pushes a range of items onto the Stack.
        /// </summary>
        /// <typeparam name="T">The type of elements in the Stack.</typeparam>
        /// <param name="stack">The Stack to push items onto.</param>
        /// <param name="items">The items to push.</param>
        public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                stack.Push(item);
            }
        }

        /// <summary>
        /// Removes and returns the specified number of items from the top of the Stack.
        /// </summary>
        /// <typeparam name="T">The type of elements in the Stack.</typeparam>
        /// <param name="stack">The Stack to remove items from.</param>
        /// <param name="count">The number of items to remove.</param>
        /// <returns>An IEnumerable containing the removed items.</returns>
        public static IEnumerable<T> PopMultiple<T>(this Stack<T> stack, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (stack.Count == 0)
                {
                    yield break;
                }
                yield return stack.Pop();
            }
        }
    }
}
