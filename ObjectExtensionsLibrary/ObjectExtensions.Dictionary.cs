using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObjectExtensionsLibrary
{
    public static partial class ObjectExtensions
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache =
            new ConcurrentDictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Converts an object to a dictionary with property names as keys and property values as values.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>A dictionary representing the object's properties and values, or null if the object is null.</returns>
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            if (obj is null) return default;

            var properties = PropertyCache.GetOrAdd(obj.GetType(), ResolveReadableProperties);
            var dictionary = new Dictionary<string, object>(properties.Length);

            foreach (var property in properties)
            {
                dictionary[property.Name] = property.GetValue(obj);
            }

            return dictionary;
        }

        /// <summary>
        /// Converts an object to a dictionary with property names as keys and property values as values.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>A dictionary representing the object's properties and values, or null if the object is null.</returns>
        public static Dictionary<string, object> Dictionary(this object obj) =>
            obj.ToDictionary();

        private static PropertyInfo[] ResolveReadableProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                .ToArray();
        }
    }
}
