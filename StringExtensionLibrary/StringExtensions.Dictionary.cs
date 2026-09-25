using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace StringExtensionLibrary
{
    public static partial class StringExtensions
    {
        /// <summary>
        ///     Converts a Json string to dictionary object method applicable for single hierarchy objects i.e
        ///     no parent child relationships, for parent child relationships use ExpandoObject.
        /// </summary>
        /// <param name="val">string formated as Json</param>
        /// <returns>IDictionary Json object</returns>
        /// <remarks>
        ///     <exception cref="ArgumentNullException">if string parameter is null or empty</exception>
        /// </remarks>
        public static IDictionary<string, object> JsonToDictionary(this string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                throw new ArgumentNullException(nameof(val));
            }

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(val);
            if (dictionary is null)
            {
                return new Dictionary<string, object>();
            }

            return NormalizeJsonElements(dictionary);
        }

        private static Dictionary<string, object> NormalizeJsonElements(Dictionary<string, object> dictionary)
        {
            var result = new Dictionary<string, object>(dictionary.Count);
            foreach (var kvp in dictionary)
            {
                result[kvp.Key] = ConvertJsonElement(kvp.Value);
            }
            return result;
        }

        private static object ConvertJsonElement(object value)
        {
            if (value is JsonElement element)
            {
                switch (element.ValueKind)
                {
                    case JsonValueKind.String:
                        return element.GetString();
                    case JsonValueKind.Number:
                        if (element.TryGetInt64(out var longVal))
                        {
                            return longVal;
                        }
                        if (element.TryGetDouble(out var doubleVal))
                        {
                            return doubleVal;
                        }
                        return element.GetRawText();
                    case JsonValueKind.True:
                        return true;
                    case JsonValueKind.False:
                        return false;
                    case JsonValueKind.Null:
                        return null;
                    default:
                        return element.ToString();
                }
            }
            return value;
        }

        /// <summary>
        ///     Convert url query string to IDictionary value key pair
        /// </summary>
        /// <param name="queryString">query string value</param>
        /// <returns>IDictionary value key pair (empty dictionary if invalid or absent)</returns>
        public static IDictionary<string, string> QueryStringToDictionary(this string queryString)
        {
            if (string.IsNullOrWhiteSpace(queryString) || !queryString.Contains('?') || !queryString.Contains('='))
            {
                return new Dictionary<string, string>();
            }

            string query = queryString.Replace("?", string.Empty);

            return query
                .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Split('='))
                .Where(parts => parts.Length >= 2)
                .ToDictionary(
                    key => key[0].Trim().ToLowerInvariant(),
                    value => value[1]
                );
        }
    }
}