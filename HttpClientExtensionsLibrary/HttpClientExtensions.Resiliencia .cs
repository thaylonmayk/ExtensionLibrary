using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientExtensionsLibrary
{
    public static partial class HttpClientExtensions
    {

        /// <summary>
        /// Sends an HTTP request with a retry policy.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="retryCount">Number of retry attempts in case of failure.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> SendWithRetryAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return response;
            }
            return response;
        }

        /// <summary>
        /// Sends an HTTP request with a retry policy.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="retryCount">Number of retry attempts in case of failure.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> RetryPolicyAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return response;
            }
            return response;
        }

        /// <summary>
        /// Sends an HTTP request with an exponential backoff retry policy.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="retryCount">Number of retry attempts in case of failure.</param>
        /// <param name="baseDelayMilliseconds">Base delay in milliseconds for the exponential backoff.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> ExponentialBackoffRetryAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3, int baseDelayMilliseconds = 200)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                        return response;
                }
                catch (Exception ex) when (i < retryCount - 1)
                {
                    await Task.Delay(baseDelayMilliseconds * (int)Math.Pow(2, i));
                }
            }
            return response;
        }

        /// <summary>
        /// Sends an HTTP request with a Circuit Breaker retry policy.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="retryCount">Number of retry attempts in case of failure.</param>
        /// <param name="circuitBreakerDuration">Duration to wait before retrying after the Circuit Breaker is opened.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> CircuitBreakerRetryAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3, TimeSpan circuitBreakerDuration = default)
        {
            if (circuitBreakerDuration == default)
                circuitBreakerDuration = TimeSpan.FromSeconds(30);

            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                        return response;
                }
                catch (Exception ex) when (i < retryCount - 1)
                {
                    await Task.Delay(circuitBreakerDuration);
                }
            }
            return response;
        }

        /// <summary>
        /// Sends an HTTP request with a retry policy in case of timeout.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="retryCount">Number of retry attempts in case of timeout.</param>
        /// <param name="timeout">Timeout duration for each request attempt.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> TimeoutRetryAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3, TimeSpan timeout = default)
        {
            if (timeout == default)
                timeout = TimeSpan.FromSeconds(10);

            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    using (var cts = new CancellationTokenSource(timeout))
                    {
                        response = await client.SendAsync(request, cts.Token);
                        response.EnsureSuccessStatusCode();
                        return response;
                    }
                }
                catch (TaskCanceledException ex) when (i < retryCount - 1)
                {
                    // Timeout occurred
                    Console.WriteLine($"Request timed out. Attempt {i + 1} of {retryCount}");
                    if (i == retryCount - 1)
                        throw;
                }
            }
            return response;
        }

        /// <summary>
        /// Sends an HTTP request and handles transient errors by retrying the request.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="retryCount">Number of retry attempts in case of transient errors.</param>
        /// <param name="retryDelay">Delay between retries.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> HandleTransientErrorsAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3, TimeSpan retryDelay = default)
        {
            if (retryDelay == default)
                retryDelay = TimeSpan.FromSeconds(2);

            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                        return response;
                }
                catch (HttpRequestException ex) when (IsTransientError(ex))
                {
                    // Handle transient error
                    await Task.Delay(retryDelay);
                }
            }
            return response;
        }


        /// <summary>
        /// Sends an HTTP request and retries in case of rate limiting errors.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="retryCount">Number of retry attempts in case of rate limiting errors.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> RateLimitRetryAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                response = await client.SendAsync(request);
                if (response.StatusCode != (HttpStatusCode)429) // 429 Too Many Requests
                    return response;

                // Extract retry-after header value and wait before retrying
                if (response.Headers.TryGetValues("Retry-After", out var values))
                {
                    var retryAfter = values.First();
                    if (int.TryParse(retryAfter, out int delaySeconds))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                    }
                }
            }
            return response;
        }


        private static bool IsTransientError(HttpRequestException ex)
        {
            // Determine if the error is transient based on the exception details
            return true; // Simplified for example purposes
        }


    }
}
