using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensionsLibrary
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Converts an object to a dictionary with property names as keys and property values as values.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>A dictionary representing the object's properties and values, or null if the object is null.</returns>

        public static Dictionary<string, object> Dictionary(this object obj)
        {
            if (obj is null) return default;

            var dictionary = new Dictionary<string, object>();

            foreach (var property in obj.GetType().GetProperties())
            {
                dictionary[property.Name] = property.GetValue(obj);
            }

            return dictionary;
        }

    }
}
