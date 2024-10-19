using System.Security.Claims;

namespace ClaimsPrincipalExtensionsLibrary
{
    public static partial class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Retrieves the first claim of the specified type from the ClaimsPrincipal instance.
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to retrieve the claim from.</param>
        /// <param name="claimType">The type of the claim to retrieve.</param>
        /// <returns>The first claim of the specified type, or null if not found.</returns>
        public static Claim Claim(this ClaimsPrincipal claimsPrincipal, string claimType) => claimsPrincipal?.FindFirst(claimType);


    }
}
