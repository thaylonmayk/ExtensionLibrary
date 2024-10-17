using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EnumExtensionsLibrary
{
    public static partial class EnumExtension
    {
        /// <summary>
        /// Gets the description attribute of the enum value.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The description of the enum value.</returns>
        public static string GetDescription<T>(this T enumValue) where T : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            return attribute == null ? enumValue.ToString() : ((DescriptionAttribute)attribute).Description;
        }

        /// <summary>
        /// Gets the description attribute of the enum value.
        /// </summary>
        /// <param name="value">The enum value.</param>
        /// <returns>The description of the enum value.</returns>
        public static string GetDescription(this Enum value)
        {
            if (value is null) return default;

            var attribute = value.GetAttribute<DescriptionAttribute>();

            return attribute is null ? value.ToString() : attribute.Description;
        }


        /// <summary>
        /// Parses the string to the corresponding enum value.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The string value.</param>
        /// <returns>The enum value.</returns>
        public static T Parse<T>(string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }


        /// <summary>
        /// Checks if a specific flag is set in a flagged enum.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="flag">The flag to check.</param>
        /// <returns>True if the flag is set; otherwise, false.</returns>
        public static bool HasFlag<T>(this T enumValue, T flag) where T : Enum
        {
            return ((Enum)(object)enumValue).HasFlag((Enum)(object)flag);
        }

        /// <summary>
        /// Converts the enum to a dictionary for easy lookup.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <returns>A dictionary with enum values as keys and their names as values.</returns>
        public static IDictionary<int, string> ToDictionary<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .ToDictionary(e => Convert.ToInt32(e), e => e.ToString());
        }
        /// <summary>
        /// Converts the enum to a dictionary for easy lookup.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>A dictionary with enum values as keys and their names as values.</returns>
        public static IDictionary<int, string> ToDictionary<T>(this T value) where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .ToDictionary(e => Convert.ToInt32(e), e => e.ToString());
        }

        /// <summary>
        /// Retrieves a custom attribute of the specified type from an enum value.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to retrieve.</typeparam>
        /// <param name="value">The enum value from which to retrieve the attribute.</param>
        /// <returns>The custom attribute of type T if found; otherwise, the default value for type T.</returns>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            if (value is null) return default;
            var member = value.GetType().GetMember(value.ToString());
            var attributes = member[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }


    }
}
