using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientExtensionsLibrary
{
    public static partial class HttpClientExtensions
    {
        /// <summary>
        /// Adds a Bearer authentication token to the request headers.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="token">Bearer authentication token.</param>
        public static void AddBearerToken(this HttpClient client, string token) =>
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        /// <summary>
        /// Adds an API key to the request headers.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="apiKey">API key.</param>
        /// <param name="headerName">Name of the header where the API key should be added.</param>
        public static void AddApiKey(this HttpClient client, string apiKey, string headerName = "x-api-key") =>
            client.DefaultRequestHeaders.Add(headerName, apiKey);

        /// <summary>
        /// Sends an authenticated HTTP request.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <param name="method">HTTP method (GET, POST, etc.).</param>
        /// <param name="token">Bearer authentication token.</param>
        /// <param name="content">Optional HTTP content (for POST/PUT requests).</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> SendAuthenticatedRequestAsync(this HttpClient client, string url, HttpMethod method, string token, HttpContent content = null)
        {
            client.AddBearerToken(token);
            var request = new HttpRequestMessage(method, url) { Content = content };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        /// <summary>
        /// Checks if a JWT token is expired.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <returns>True if the token is expired; otherwise, false.</returns>
        public static bool IsTokenExpired(string token)
        {
            var jwtToken = new JwtSecurityToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }

        /// <summary>
        /// Sends a request to refresh the authentication token.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">URL for the token refresh endpoint.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>New access token.</returns>
        public static async Task<string> RefreshTokenAsync(this HttpClient client, string url, string refreshToken)
        {
            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("refresh_token", refreshToken)
    });
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseJson);
            return tokenResponse.AccessToken;
        }

        public class TokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
        }

        /// <summary>
        /// Ensures the request is made with a valid authentication token, refreshing it if necessary.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="tokenProvider">Function to retrieve the current access token.</param>
        /// <param name="refreshTokenProvider">Function to retrieve the refresh token.</param>
        /// <param name="refreshUrl">URL for the token refresh endpoint.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> EnsureAuthenticatedRequestAsync(this HttpClient client, HttpRequestMessage request, Func<string> tokenProvider, Func<string> refreshTokenProvider, string refreshUrl)
        {
            var token = tokenProvider();
            client.AddBearerToken(token);
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshToken = refreshTokenProvider();
                token = await client.RefreshTokenAsync(refreshUrl, refreshToken);
                client.AddBearerToken(token);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await client.SendAsync(request);
            }

            return response;
        }

        /// <summary>
        /// Sets default headers for all requests.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="headers">Dictionary of headers to set.</param>
        public static void SetDefaultHeaders(this HttpClient client, IDictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

    }
}
