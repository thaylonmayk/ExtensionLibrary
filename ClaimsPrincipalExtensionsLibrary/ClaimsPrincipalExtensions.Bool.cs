using System.Linq;
using System.Security.Claims;

namespace ClaimsPrincipalExtensionsLibrary
{
    public static partial class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Checks if the ClaimsPrincipal has a specific role claim.
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to check.</param>
        /// <param name="role">The role to check for.</param>
        /// <returns>True if the ClaimsPrincipal has the specified role; otherwise, false.</returns>
        public static bool HasRole(this ClaimsPrincipal claimsPrincipal, string role) =>
            claimsPrincipal?.ClaimRoles().Contains(role) ?? false;

        /// <summary>
        /// Checks if the ClaimsPrincipal is authenticated.
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to check.</param>
        /// <returns>True if the ClaimsPrincipal is authenticated; otherwise, false.</returns>
        public static bool IsAuthenticated(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal?.Identity?.IsAuthenticated ?? false;


    }
}
