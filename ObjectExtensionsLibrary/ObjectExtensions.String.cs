using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ObjectExtensionsLibrary
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Serializes an object to a JSON string, ignoring null values.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A JSON string representing the object.</returns>
        public static string Serialize(this object obj) => JsonSerializer.Serialize(obj, new JsonSerializerOptions(JsonSerializerDefaults.Web) { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });

        /// <summary>
        /// Sets the value of a specified property on an object.
        /// </summary>
        /// <param name="obj">The object on which to set the property value.</param>
        /// <param name="name">The name of the property to set.</param>
        public static void SetProperty(this object obj, string name, object value) => obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)?.SetValue(obj, value);

        /// <summary>
        /// Serializes an object to a JSON string with indented formatting.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A JSON string representing the object with indented formatting.</returns>
        public static string ToJsonIndented(this object obj) =>
             JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

    }
}
