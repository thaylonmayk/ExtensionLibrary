using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensionsLibrary
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Converts an object to an ExpandoObject.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>An ExpandoObject representing the object's properties and values.</returns>
        public static ExpandoObject ToExpando(this object obj)
        {
            var expando = new ExpandoObject() as IDictionary<string, object>;
            foreach (var property in obj.GetType().GetProperties())
            {
                expando[property.Name] = property.GetValue(obj);
            }
            return (ExpandoObject)expando;
        }

        /// <summary>
        /// Adds or updates a property on an ExpandoObject.
        /// </summary>
        /// <param name="expando">The ExpandoObject to modify.</param>
        /// <param name="name">The name of the property to add or update.</param>
        /// <param name="value">The value to set on the property.</param>
        public static void AddOrUpdateProperty(this ExpandoObject expando, string name, object value)
        {
            var dictionary = (IDictionary<string, object>)expando;
            dictionary[name] = value;
        }


    }
}
