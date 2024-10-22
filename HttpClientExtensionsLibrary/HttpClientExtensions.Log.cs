using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientExtensionsLibrary
{
    public static partial class HttpClientExtensions
    {

        /// <summary>
        /// Logs details of the HTTP request and response.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> LogRequestDetails(this HttpClient client, HttpRequestMessage request)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await client.SendAsync(request);
            stopwatch.Stop();

            Console.WriteLine($"Request to {request.RequestUri} took {stopwatch.ElapsedMilliseconds} ms.");
            Console.WriteLine($"Response status code: {response.StatusCode}");
            Console.WriteLine($"Response headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {h.Value}"))}");

            return response;
        }

        /// <summary>
        /// Logs details of the HTTP request and response.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> LogRequestAndResponse(this HttpClient client, HttpRequestMessage request)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await client.SendAsync(request);
            stopwatch.Stop();

            // Log request details
            Console.WriteLine($"Request URI: {request.RequestUri}");
            Console.WriteLine($"Request Method: {request.Method}");
            Console.WriteLine($"Request Headers: {string.Join(", ", request.Headers.Select(h => $"{h.Key}: {h.Value}"))}");
            if (request.Content != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync();
                Console.WriteLine($"Request Body: {requestBody}");
            }

            // Log response details
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {h.Value}"))}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Body: {responseBody}");
            Console.WriteLine($"Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

            return response;
        }

        /// <summary>
        /// Logs errors during the HTTP request.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> LogError(this HttpClient client, HttpRequestMessage request)
        {
            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request to {request.RequestUri} failed with exception: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Logs the status of the HTTP response.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> LogResponseStatus(this HttpClient client, HttpRequestMessage request)
        {
            var response = await client.SendAsync(request);
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            return response;
        }

        /// <summary>
        /// Logs request and response details to a file.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="filePath">Path to the log file.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> LogToFile(this HttpClient client, HttpRequestMessage request, string filePath)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await client.SendAsync(request);
            stopwatch.Stop();

            using (var writer = new StreamWriter(filePath, true))
            {
                // Log request details
                await writer.WriteLineAsync($"Request URI: {request.RequestUri}");
                await writer.WriteLineAsync($"Request Method: {request.Method}");
                await writer.WriteLineAsync($"Request Headers: {string.Join(", ", request.Headers.Select(h => $"{h.Key}: {h.Value}"))}");
                if (request.Content != null)
                {
                    var requestBody = await request.Content.ReadAsStringAsync();
                    await writer.WriteLineAsync($"Request Body: {requestBody}");
                }

                // Log response details
                await writer.WriteLineAsync($"Response Status Code: {response.StatusCode}");
                await writer.WriteLineAsync($"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {h.Value}"))}");
                var responseBody = await response.Content.ReadAsStringAsync();
                await writer.WriteLineAsync($"Response Body: {responseBody}");
                await writer.WriteLineAsync($"Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");
            }

            return response;
        }

    }
}
