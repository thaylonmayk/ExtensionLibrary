using System.Security.Claims;

namespace ClaimsPrincipalExtensionsLibrary
{
    public static partial class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Retrieves the "sub" claim from the ClaimsPrincipal instance.
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to retrieve the "sub" claim from.</param>
        /// <returns>The value of the "sub" claim, or null if not found.</returns>
        public static string ClaimSub(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal?.Claim("sub")?.Value;

        /// <summary>
        /// Retrieves the email claim from the ClaimsPrincipal instance.
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to retrieve the email from.</param>
        /// <returns>The email claim value, or null if not found.</returns>
        public static string Email(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal?.Claim("email")?.Value;

        /// <summary>
        /// Retrieves the full name claim from the ClaimsPrincipal instance.
        /// </summary>
        /// <param name="claimsPrincipal">The ClaimsPrincipal to retrieve the full name from.</param>
        /// <returns>The full name claim value, or null if not found.</returns>
        public static string FullName(this ClaimsPrincipal claimsPrincipal) =>
            claimsPrincipal?.Claim("name")?.Value;


    }
}
