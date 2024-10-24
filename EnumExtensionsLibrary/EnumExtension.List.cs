using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumExtensionsLibrary
{
    public static partial class EnumExtension
    {

        /// <summary>
        /// Gets all values of the enum as a list.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>A list of enum values.</returns>
        public static IList<T> GetValues<T>() where T : Enum
        {
            Type e = typeof(T);
            if (!e.IsEnum)
            {
                throw new ArgumentException("T must be of type System.Enum");
            }
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        /// <summary>
        /// Gets all names of the enum as a list.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>A list of enum names.</returns>
        public static IList<string> GetNames<T>() where T : Enum
        {
            Type e = typeof(T);
            if (!e.IsEnum)
            {
                throw new ArgumentException("T must be of type System.Enum");
            }
            return Enum.GetNames(typeof(T)).ToList();
        }

        /// <summary>
        /// Gets all descriptions for an enum type.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>A dictionary of enum values and their descriptions.</returns>
        public static Dictionary<T, string> GetAllDescriptions<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(
                e => e,
                e => e.GetDescription()
            );
        }

        /// <summary>
        /// Gets all keys associated with an enum value.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>A list of all keys associated with the enum value.</returns>
        public static List<int> GetAllKeys<T>(this T enumValue) where T : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = field.GetCustomAttributes(typeof(EnumDescriptionAttribute), false)
                                  .Cast<EnumDescriptionAttribute>();

            return attributes.Select(a => a.Key).ToList();
        }

        /// <summary>
        /// Gets all enum values that have a specific description.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="description">The description to search for.</param>
        /// <returns>A list of enum values that have the specified description.</returns>
        public static List<T> GetEnumValuesByDescription<T>(string description) where T : Enum
        {
            var type = typeof(T);
            var values = Enum.GetValues(type).Cast<T>();

            return values.Where(value =>
            {
                var field = type.GetField(value.ToString());
                var attributes = field.GetCustomAttributes(typeof(EnumDescriptionAttribute), false)
                                      .Cast<EnumDescriptionAttribute>();

                return attributes.Any(a => a.Description == description);
            }).ToList();
        }

    }
}
