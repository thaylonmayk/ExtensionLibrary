using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensionsLibrary
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Compares two objects for equality based on their properties.
        /// </summary>
        /// <param name="obj">The first object to compare.</param>
        /// <param name="other">The second object to compare.</param>
        /// <returns>True if the objects are equal; otherwise, false.</returns>
        public static bool PropertiesEqual(this object obj, object other)
        {
            if (obj == null || other == null) return false;
            if (obj.GetType() != other.GetType()) return false;

            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value1 = property.GetValue(obj);
                var value2 = property.GetValue(other);
                if (!Equals(value1, value2)) return false;
            }
            return true;
        }

        /// <summary>
        /// Tries to get the value of a specified property from an object.
        /// </summary>
        /// <param name="obj">The object to get the property value from.</param>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value of the property if found; otherwise, null.</param>
        /// <returns>True if the property is found; otherwise, false.</returns>
        public static bool TryGetProperty(this object obj, string name, out object value)
        {
            var property = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property != null)
            {
                value = property.GetValue(obj);
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Determines whether the object is equal to its default value.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>True if the object is equal to its default value; otherwise, false.</returns>
        public static bool IsDefault<T>(this T obj) =>
            EqualityComparer<T>.Default.Equals(obj, default(T));

        /// <summary>
        /// Determines whether a specified property is defined on an object.
        /// </summary>
        /// <param name="obj">The object to inspect.</param>
        /// <param name="propertyName">The name of the property to check.</param>
        /// <returns>True if the property is defined; otherwise, false.</returns>
        public static bool IsPropertyDefined(this object obj, string propertyName) =>
            obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) != null;

    }
}
