using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionExtensionsLibrary
{
    public static partial class CollectionExtensions
    {
        [ThreadStatic]
        private static Random _threadLocalRandom;

        private static Random GetThreadLocalRandom()
        {
            if (_threadLocalRandom is null)
            {
                _threadLocalRandom = new Random();
            }
            return _threadLocalRandom;
        }

        /// <summary>
        /// Randomizes the order of elements in the collection using a thread-safe random generator.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <returns>A collection with elements in randomized order.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            var buffer = source.ToArray();
            var random = GetThreadLocalRandom();
            for (int i = buffer.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = buffer[i];
                buffer[i] = buffer[j];
                buffer[j] = temp;
            }
            return buffer;
        }
    }
}
