namespace CollectionExtensionsLibrary
{
    public static partial class CollectionExtensions
    {
        /// <summary>
        /// Adds a range of key/value pairs to the dictionary if they do not already exist in the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to add key/value pairs to.</param>
        /// <param name="items">The key/value pairs to add.</param>
        public static void AddRangeIfNotExists<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (var item in items)
            {
                if (!dictionary.ContainsKey(item.Key))
                {
                    dictionary.Add(item.Key, item.Value);
                }
            }
        }
        /// <summary>
        /// Removes all key/value pairs from the dictionary that match the specified predicate.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to remove key/value pairs from.</param>
        /// <param name="predicate">The predicate used to determine which key/value pairs to remove.</param>
        /// <returns>The number of elements removed.</returns>
        public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            var keysToRemove = dictionary.Where(predicate).Select(kvp => kvp.Key).ToList();
            foreach (var key in keysToRemove)
            {
                dictionary.Remove(key);
            }
            return keysToRemove.Count;
        }
        /// <summary>
        /// Finds all key/value pairs in the dictionary that match the specified predicate.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="predicate">The predicate used to determine which key/value pairs to find.</param>
        /// <returns>A dictionary of key/value pairs that match the predicate.</returns>
        public static Dictionary<TKey, TValue> FindAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> predicate) =>
            dictionary.Where(predicate).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        /// <summary>
        /// Sorts the dictionary by the specified key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <typeparam name="TKeySelector">The type of the key to sort by.</typeparam>
        /// <param name="dictionary">The dictionary to sort.</param>
        /// <param name="keySelector">The key selector used to sort the dictionary.</param>
        /// <returns>A sorted dictionary.</returns>
        public static Dictionary<TKey, TValue> SortBy<TKey, TValue, TKeySelector>(this Dictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, TKeySelector> keySelector) => dictionary.OrderBy(keySelector).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


        /// <summary>
        /// Adds a key/value pair to the dictionary if the key does not already exist, or updates a key/value pair in the dictionary if the key already exists.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to add or update the key/value pair in.</param>
        /// <param name="key">The key of the element to add or update.</param>
        /// <param name="value">The value to add or update.</param>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key, or adds a new key/value pair if the key does not already exist in the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to get or add the key/value pair in.</param>
        /// <param name="key">The key of the element to get or add.</param>
        /// <param name="valueFactory">The function used to generate a value if the key does not exist.</param>
        /// <returns>The value associated with the key.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = valueFactory(key);
                dictionary.Add(key, value);
            }
            return value;
        }

        /// <summary>
        /// Removes the entry with the specified key from the dictionary if the condition is met.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to remove the entry from.</param>
        /// <param name="key">The key of the entry to remove.</param>
        /// <param name="condition">The condition that determines whether to remove the entry.</param>
        /// <returns>True if the entry was removed; otherwise, false.</returns>
        public static bool RemoveIf<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, bool> condition)
        {
            if (condition(key))
            {
                return dictionary.Remove(key);
            }
            return false;
        }

        /// <summary>
        /// Merges the specified dictionary into the current dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The current dictionary.</param>
        /// <param name="other">The dictionary to merge into the current dictionary.</param>
        /// <param name="updateValueFactory">The function used to resolve conflicts by updating the value.</param>
        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> other, Func<TValue, TValue, TValue> updateValueFactory)
        {
            foreach (var kvp in other)
            {
                if (dictionary.TryGetValue(kvp.Key, out var existingValue))
                {
                    dictionary[kvp.Key] = updateValueFactory(existingValue, kvp.Value);
                }
                else
                {
                    dictionary.Add(kvp.Key, kvp.Value);
                }
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key or the default value if the key does not exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to retrieve the value from.</param>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>The value associated with the specified key or the default value if the key does not exist.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) =>
            dictionary.TryGetValue(key, out var value) ? value : default;

        /// <summary>
        /// Merges another dictionary into the current dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The current dictionary.</param>
        /// <param name="otherDictionary">The dictionary to merge into the current dictionary.</param>
        public static void MergeDictionaries<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> otherDictionary)
        {
            foreach (var kvp in otherDictionary)
            {
                dictionary[kvp.Key] = kvp.Value;
            }
        }


    }
}
