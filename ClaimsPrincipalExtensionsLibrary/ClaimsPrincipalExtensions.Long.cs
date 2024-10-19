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

    }
}
