using System.Collections.Generic;
using System.Linq;

namespace CollectionExtensionsLibrary
{
    public static partial class CollectionExtensions
    {
        /// <summary>
        /// Adds a range of items to the HashSet.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The HashSet to add items to.</param>
        /// <param name="items">The items to add.</param>
        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                set.Add(item);
            }
        }

        /// <summary>
        /// Removes a range of items from the HashSet.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The HashSet to remove items from.</param>
        /// <param name="items">The items to remove.</param>
        public static void RemoveRange<T>(this HashSet<T> set, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                set.Remove(item);
            }
        }

        /// <summary>
        /// Modifies the current HashSet to contain only elements that are present in both the current HashSet and the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        public static void IntersectWith<T>(this HashSet<T> set, IEnumerable<T> other) =>
            set.IntersectWith(other);

        /// <summary>
        /// Removes all elements in the specified collection from the current HashSet.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        public static void ExceptWith<T>(this HashSet<T> set, IEnumerable<T> other) =>
            set.ExceptWith(other);

        /// <summary>
        /// Modifies the current HashSet to contain all elements that are present in itself or the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        public static void UnionWith<T>(this HashSet<T> set, IEnumerable<T> other) =>
            set.UnionWith(other);

        /// <summary>
        /// Modifies the current HashSet to contain only elements that are present either in the current HashSet or in the specified collection, but not both.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        public static void SymmetricExceptWith<T>(this HashSet<T> set, IEnumerable<T> other) =>
            set.SymmetricExceptWith(other);

        /// <summary>
        /// Determines whether the current HashSet is a proper subset of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        /// <returns>True if the current HashSet is a proper subset of the specified collection; otherwise, false.</returns>
        public static bool IsProperSubsetOf<T>(this HashSet<T> set, IEnumerable<T> other) =>
            set.IsProperSubsetOf(other);

        /// <summary>
        /// Determines whether the current HashSet is a proper superset of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        /// <returns>True if the current HashSet is a proper superset of the specified collection; otherwise, false.</returns>
        public static bool IsProperSupersetOf<T>(this HashSet<T> set, IEnumerable<T> other) =>
            set.IsProperSupersetOf(other);

        /// <summary>
        /// Determines whether the current HashSet is a subset of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        /// <returns>True if the current HashSet is a subset of the specified collection; otherwise, false.</returns>
        public static bool IsSubsetOf<T>(this HashSet<T> set, IEnumerable<T> other) =>
            set.IsSubsetOf(other);

        /// <summary>
        /// Determines whether the current HashSet is a superset of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        /// <returns>True if the current HashSet is a superset of the specified
        public static bool IsSupersetOf<T>(this HashSet<T> set, IEnumerable<T> other) =>
            set.IsSupersetOf(other);

        /// <summary>
        /// Determines whether the HashSet contains all elements in a specified collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        /// <returns>True if the HashSet contains all elements in the specified collection; otherwise, false.</returns>
        public static bool ContainsAll<T>(this HashSet<T> set, IEnumerable<T> other) =>
            other.All(set.Contains);

        /// <summary>
        /// Determines whether the HashSet and the specified collection share common elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        /// <returns>True if the HashSet and the specified collection share at least one common element; otherwise, false.</returns>
        public static bool Overlaps<T>(this HashSet<T> set, IEnumerable<T> other) =>
            set.Overlaps(other);

        /// <summary>
        /// Determines whether the current HashSet and the specified collection contain the same elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the HashSet.</typeparam>
        /// <param name="set">The current HashSet.</param>
        /// <param name="other">The collection to compare to the current HashSet.</param>
        /// <returns>True if the current HashSet and the specified collection contain the same elements; otherwise, false.</returns>
        public static bool SetEquals<T>(this HashSet<T> set, IEnumerable<T> other) =>
            set.SetEquals(other);

    }
}
