using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;

namespace ClaimsPrincipalExtensionsLibrary
{
    public static partial class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Retrieves the "sub" claim from the ClaimsPrincipal instance and attempts to convert it to a long.
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to retrieve the "sub" claim from.</param>
        /// <returns>The value of the "sub" claim as a long, or 0 if the conversion fails.</returns>
        public static long Id(this ClaimsPrincipal claimsPrincipal) => long.TryParse(claimsPrincipal.ClaimSub(), out var value) ? value : 0;

        /// <summary>
        /// Retrieves the user identifier claim ("sub" or NameIdentifier) converted to the specified type.
        /// </summary>
        /// <typeparam name="T">The target type (e.g. Guid, int, long, string).</typeparam>
        /// <param name="claimsPrincipal">The ClaimsPrincipal instance.</param>
        /// <returns>The parsed identifier, or default(T) if absent or conversion fails.</returns>
        public static T GetUserId<T>(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null) return default;

            var sub = claimsPrincipal.ClaimSub();
            if (string.IsNullOrWhiteSpace(sub)) return default;

            return ParseUserId<T>(sub);
        }

        /// <summary>
        /// Retrieves the user identifier claim converted to the specified type, or returns a default value.
        /// </summary>
        /// <typeparam name="T">The target type (e.g. Guid, int, long, string).</typeparam>
        /// <param name="claimsPrincipal">The ClaimsPrincipal instance.</param>
        /// <param name="defaultValue">Fallback value if the claim is missing or conversion fails.</param>
        /// <returns>The parsed identifier or the fallback value.</returns>
        public static T GetUserIdOrDefault<T>(this ClaimsPrincipal claimsPrincipal, T defaultValue = default)
        {
            var id = claimsPrincipal.GetUserId<T>();
            return EqualityComparer<T>.Default.Equals(id, default) ? defaultValue : id;
        }

        private static T ParseUserId<T>(string rawValue)
        {
            var targetType = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingType == typeof(Guid))
            {
                return Guid.TryParse(rawValue, out var guidValue) ? (T)(object)guidValue : default;
            }

            if (underlyingType == typeof(int))
            {
                return int.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue) ? (T)(object)intValue : default;
            }

            if (underlyingType == typeof(long))
            {
                return long.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue) ? (T)(object)longValue : default;
            }

            if (underlyingType == typeof(string))
            {
                return (T)(object)rawValue;
            }

            return ConvertFallback<T>(rawValue, underlyingType);
        }

        private static T ConvertFallback<T>(string rawValue, Type underlyingType)
        {
            try
            {
                return (T)Convert.ChangeType(rawValue, underlyingType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
