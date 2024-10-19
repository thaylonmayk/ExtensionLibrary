using System.Text;
using System.Text.Json;

namespace ObjectExtensionsLibrary
{
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// Converts an object to a byte array using JSON serialization.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>A byte array representing the serialized object.</returns>
        public static byte[] Bytes(this object obj) => Encoding.Default.GetBytes(JsonSerializer.Serialize(obj));

    }
}
