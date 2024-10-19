using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ClaimsPrincipalExtensionsLibrary
{
    public static partial class ClaimsPrincipalExtensions
    {/// <summary>
     /// Retrieves all role claims from the ClaimsPrincipal instance.
     /// </summary>
     /// <param name="claimsPrincipal">The ClaimsPrincipal to retrieve roles from.</param>
     /// <returns>A collection of role claims.</returns>
        public static IEnumerable<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal?.Claims("role");


        /// <summary>
        /// Retrieves all claims of a specific type from the ClaimsPrincipal instance and returns their values.
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to retrieve claims from.</param>
        /// <param name="claimType">The type of claims to retrieve.</param>
        /// <returns>A collection of claim values.</returns>
        public static IEnumerable<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType) =>

             claimsPrincipal?.FindAll(claimType).Select(x => x.Value).ToList();


        /// <summary>
        /// Retrieves all role claims from the ClaimsPrincipal instance and casts them to a specified Enum type.
        /// </summary>
        /// <typeparam name="T">The Enum type to cast the role claims to.</typeparam>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to retrieve roles from.</param>
        /// <returns>A collection of role claims cast to the specified Enum type.</returns>
        public static IEnumerable<T> Roles<T>(this ClaimsPrincipal claimsPrincipal) where T : Enum =>
            claimsPrincipal.ClaimRoles().Select(value => (T)Enum.Parse(typeof(T), value)).ToList();

        /// <summary>
        /// Retrieves all claims from the ClaimsPrincipal instance.
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to retrieve claims from.</param>
        /// <returns>A collection of all claims.</returns>
        public static IEnumerable<Claim> AllClaims(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal?.Claims ?? Enumerable.Empty<Claim>();

    }
}
