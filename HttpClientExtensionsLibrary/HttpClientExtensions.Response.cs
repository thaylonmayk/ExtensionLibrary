using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientExtensionsLibrary
{
    public static partial class HttpClientExtensions
    {
        /// <summary>
        /// Sends an HTTP request with JSON payload and receives the response as an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <param name="method">HTTP method (GET, POST, etc.).</param>
        /// <param name="payload">Object to be serialized as JSON in the request body.</param>
        /// <returns>Object of type T with the response.</returns>
        public static async Task<T> SendJsonAsync<T>(this HttpClient client, string url, HttpMethod method, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(method, url) { Content = content };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseJson);
        }

        /// <summary>
        /// Measures the response time of an HTTP request.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <returns>TimeSpan representing the response time.</returns>
        public static async Task<TimeSpan> GetResponseTimeAsync(this HttpClient client, string url)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        /// <summary>
        /// Reads the response content as a JSON object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>Object of type T.</returns>
        public static async Task<T> ReadAsJsonAsync<T>(this HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Ensures the response was successful and reads the content as a string.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>Response content as a string.</returns>
        public static async Task<string> EnsureSuccessAndReadAsStringAsync(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Gets the headers from the response as a dictionary.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>Dictionary of response headers.</returns>
        public static IDictionary<string, IEnumerable<string>> GetHeaders(this HttpResponseMessage response)
        {
            return response.Headers.ToDictionary(header => header.Key, header => header.Value);
        }

        /// <summary>
        /// Extracts cookies from the response.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>List of cookies.</returns>
        public static List<string> ExtractCookies(this HttpResponseMessage response)
        {
            if (response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
            {
                return cookieHeaders.ToList();
            }
            return new List<string>();
        }

        /// <summary>
        /// Sends a GET request and deserializes the JSON response into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <returns>Object of type T.</returns>
        public static async Task<T> GetJsonAsync<T>(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Sends a POST request with JSON payload and deserializes the JSON response into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <param name="payload">Object to be serialized as JSON in the request body.</param>
        /// <returns>Object of type T.</returns>
        public static async Task<T> PostJsonAsync<T>(this HttpClient client, string url, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseJson);
        }

        /// <summary>
        /// Sends a PUT request with JSON payload and deserializes the JSON response into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <param name="payload">Object to be serialized as JSON in the request body.</param>
        /// <returns>Object of type T.</returns>
        public static async Task<T> PutJsonAsync<T>(this HttpClient client, string url, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseJson);
        }

        /// <summary>
        /// Sends a DELETE request and ensures the response status is successful.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> DeleteJsonAsync(this HttpClient client, string url)
        {
            var response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            return response;
        }

        /// <summary>
        /// Sends a GET request and deserializes the JSON response into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <returns>Object of type T.</returns>
        public static async Task<T> GetJsonValidationStatusCodeAsync<T>(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
        }

        /// <summary>
        /// Sends a POST request with JSON payload and deserializes the JSON response into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <param name="payload">Object to be serialized as JSON in the request body.</param>
        /// <returns>Object of type T.</returns>
        public static async Task<T> PostJsonValidationStatusCodeAsync<T>(this HttpClient client, string url, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseJson);
            }
            throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
        }

        /// <summary>
        /// Sends a PUT request with JSON payload and deserializes the JSON response into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <param name="payload">Object to be serialized as JSON in the request body.</param>
        /// <returns>Object of type T.</returns>
        public static async Task<T> PutJsonValidationStatusCodeAsync<T>(this HttpClient client, string url, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseJson);
            }
            throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
        }

        /// <summary>
        /// Sends a DELETE request and ensures the response status is successful.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> DeleteJsonValidationStatusCodeAsync(this HttpClient client, string url)
        {
            var response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.OK)
            {
                return response;
            }
            throw new HttpRequestException($"Unexpected status code: {response.StatusCode}");
        }

        /// <summary>
        /// Sends a PATCH request with the specified content and returns the HTTP response message.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <param name="content">Content to be sent in the request body.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string url, HttpContent content)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, url) { Content = content };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }


    }
}
