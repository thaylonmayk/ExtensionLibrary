using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EnumExtensionsLibrary
{
    public static partial class EnumExtension
    {
        private static readonly ConcurrentDictionary<Type, object> CachedEnumDictionaries = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Safely parses a string value to the specified enum type, returning a default value on failure.
        /// </summary>
        /// <typeparam name="TEnum">The target enum type.</typeparam>
        /// <param name="value">The string value to parse.</param>
        /// <param name="defaultValue">The fallback value to return if parsing fails.</param>
        /// <param name="ignoreCase">True to ignore casing during matching; otherwise false.</param>
        /// <returns>The parsed enum value, or defaultValue if parsing fails or input is null/empty.</returns>
        public static TEnum ToEnum<TEnum>(this string value, TEnum defaultValue = default, bool ignoreCase = true) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            if (Enum.TryParse<TEnum>(value.Trim(), ignoreCase, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Safely parses a string value to the specified enum type, returning null on failure.
        /// </summary>
        /// <typeparam name="TEnum">The target enum type.</typeparam>
        /// <param name="value">The string value to parse.</param>
        /// <param name="ignoreCase">True to ignore casing during matching; otherwise false.</param>
        /// <returns>The parsed nullable enum value, or null if parsing fails or input is null/empty.</returns>
        public static TEnum? ToEnumOrNull<TEnum>(this string value, bool ignoreCase = true) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (Enum.TryParse<TEnum>(value.Trim(), ignoreCase, out var result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Safely converts an integer value to the specified enum type, verifying whether it is defined.
        /// </summary>
        /// <typeparam name="TEnum">The target enum type.</typeparam>
        /// <param name="value">The integer value to evaluate.</param>
        /// <param name="defaultValue">The fallback value to return if not defined.</param>
        /// <returns>The defined enum value, or defaultValue if undefined.</returns>
        public static TEnum ToEnum<TEnum>(this int value, TEnum defaultValue = default) where TEnum : struct, Enum
        {
            if (Enum.IsDefined(typeof(TEnum), value))
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), value);
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns a cached dictionary containing all enum members and their descriptions,
        /// avoiding repeated reflection across high-throughput request cycles.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>An immutable read-only dictionary of enum members to description strings.</returns>
        public static IReadOnlyDictionary<TEnum, string> GetCachedDescriptions<TEnum>() where TEnum : struct, Enum
        {
            Type enumType = typeof(TEnum);

            object cachedMap = CachedEnumDictionaries.GetOrAdd(enumType, static t =>
            {
                var values = (TEnum[])Enum.GetValues(t);
                var map = new Dictionary<TEnum, string>(values.Length);

                for (int i = 0; i < values.Length; i++)
                {
                    TEnum current = values[i];
                    map[current] = current.GetDescription();
                }

                return new ReadOnlyDictionary<TEnum, string>(map);
            });

            return (IReadOnlyDictionary<TEnum, string>)cachedMap;
        }
    }
}
