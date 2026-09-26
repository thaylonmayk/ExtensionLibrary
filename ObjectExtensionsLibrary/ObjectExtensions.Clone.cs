using System.Text.Json;
using System.Text.Json.Serialization;

namespace ObjectExtensionsLibrary
{
    public static partial class ObjectExtensions
    {
        private static readonly JsonSerializerOptions DefaultCloneOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Creates a deep copy of the object using JSON serialization with circular reference protection.
        /// </summary>
        /// <param name="obj">The object to clone.</param>
        /// <returns>A deep copy of the object.</returns>
        public static T Clone<T>(this T obj)
        {
            if (obj is null)
            {
                return default;
            }

            var json = JsonSerializer.Serialize(obj, DefaultCloneOptions);
            return JsonSerializer.Deserialize<T>(json, DefaultCloneOptions);
        }
    }
}
