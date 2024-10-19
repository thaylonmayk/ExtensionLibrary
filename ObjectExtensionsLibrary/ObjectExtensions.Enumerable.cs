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
        /// Retrieves all properties of an object that are decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to look for.</typeparam>
        /// <param name="obj">The object to inspect.</param>
        /// <returns>An enumerable of properties that are decorated with the specified attribute.</returns>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this object obj) where T : Attribute => obj.GetType().GetProperties().Where(property => Attribute.IsDefined(property, typeof(T)));

        /// <summary>
        /// Retrieves the names of all properties of an object.
        /// </summary>
        /// <param name="obj">The object to inspect.</param>
        /// <returns>An enumerable of property names.</returns>
        public static IEnumerable<string> GetPropertyNames(this object obj) =>
             obj.GetType().GetProperties().Select(p => p.Name);

    }
}
