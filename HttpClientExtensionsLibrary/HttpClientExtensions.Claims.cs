using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientExtensionsLibrary
{
    public static partial class HttpClientExtensions
    {

        /// <summary>
        /// Extracts claims from a JWT token.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <returns>List of claims.</returns>
        public static IEnumerable<Claim> GetClaimsFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims;
        }

        /// <summary>
        /// Checks if a JWT token contains a specific claim.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="claimValue">Value of the claim (optional).</param>
        /// <returns>True if the claim exists; otherwise, false.</returns>
        public static bool HasClaim(string token, string claimType, string claimValue = null)
        {
            var claims = GetClaimsFromToken(token);
            return claimValue == null ?
                claims.Any(c => c.Type == claimType) :
                claims.Any(c => c.Type == claimType && c.Value == claimValue);
        }

        /// <summary>
        /// Ensures the request is made with a valid authentication token and checks for specific claims.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="tokenProvider">Function to retrieve the current access token.</param>
        /// <param name="refreshTokenProvider">Function to retrieve the refresh token.</param>
        /// <param name="refreshUrl">URL for the token refresh endpoint.</param>
        /// <param name="requiredClaim">The claim required to proceed with the request.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> EnsureAuthenticatedRequestWithClaimsAsync(this HttpClient client, HttpRequestMessage request, Func<string> tokenProvider, Func<string> refreshTokenProvider, string refreshUrl, string requiredClaim)
        {
            var token = tokenProvider();
            if (!HasClaim(token, requiredClaim))
            {
                throw new UnauthorizedAccessException("The token does not contain the required claim.");
            }

            client.AddBearerToken(token);
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshToken = refreshTokenProvider();
                token = await client.RefreshTokenAsync(refreshUrl, refreshToken);

                if (!HasClaim(token, requiredClaim))
                {
                    throw new UnauthorizedAccessException("The refreshed token does not contain the required claim.");
                }

                client.AddBearerToken(token);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.SendAsync(request);
            }

            return response;
        }

        /// <summary>
        /// Adds a specific role claim to a JWT token.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="role">Role to add as a claim.</param>
        /// <returns>New JWT token with the role claim.</returns>
        public static string AddRoleClaim(string token, string role)
        {
            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                var claims = new List<Claim>(jwtToken.Claims)
        {
            new Claim(ClaimTypes.Role, role)
        };

                var newJwtToken = new JwtSecurityToken(
                    issuer: jwtToken.Issuer,
                    audience: jwtToken.Audiences.First(),
                    claims: claims,
                    notBefore: jwtToken.ValidFrom,
                    expires: jwtToken.ValidTo,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key")), SecurityAlgorithms.HmacSha256)
                );

                return handler.WriteToken(newJwtToken);
            }
            else
            {
                throw new ArgumentException("Invalid JWT token");
            }
        }


        /// <summary>
        /// Sends an authenticated HTTP request and checks for a specific role claim.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <param name="method">HTTP method (GET, POST, etc.).</param>
        /// <param name="token">Bearer authentication token.</param>
        /// <param name="requiredRole">The required role claim.</param>
        /// <param name="content">Optional HTTP content (for POST/PUT requests).</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> SendRequestWithRoleCheckAsync(this HttpClient client, string url, HttpMethod method, string token, string requiredRole, HttpContent content = null)
        {
            if (!HasClaim(token, ClaimTypes.Role, requiredRole))
            {
                throw new UnauthorizedAccessException("The token does not contain the required role.");
            }

            client.AddBearerToken(token);
            var request = new HttpRequestMessage(method, url) { Content = content };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
