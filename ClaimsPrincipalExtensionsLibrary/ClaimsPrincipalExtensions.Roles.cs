using System;
using System.Linq;
using System.Security.Claims;

namespace ClaimsPrincipalExtensionsLibrary
{
    public static partial class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Converts the aggregated values of role claims to a specified Enum type.
        /// </summary>
        /// <typeparam name="T">The Enum type to convert the role claims to.</typeparam>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to retrieve role claims from.</param>
        /// <returns>The aggregated value of role claims as a specified Enum type.</returns>
        public static T RolesFlag<T>(this ClaimsPrincipal claimsPrincipal) where T : Enum => 
            (T)Enum.Parse(typeof(T), claimsPrincipal.Roles<T>().Sum(value => Convert.ToInt64(value)).ToString(), true);

    }
}
