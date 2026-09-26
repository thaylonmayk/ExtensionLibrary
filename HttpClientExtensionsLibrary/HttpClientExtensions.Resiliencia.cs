using System;
using System.IO;
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
        /// Sends an HTTP request with a retry policy using a request factory.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="requestFactory">Factory producing the HTTP request message for each attempt.</param>
        /// <param name="retryCount">Number of retry attempts in case of failure.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> SendWithRetryAsync(this HttpClient client, Func<HttpRequestMessage> requestFactory, int retryCount = 3, CancellationToken cancellationToken = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (requestFactory is null) throw new ArgumentNullException(nameof(requestFactory));

            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                using var request = requestFactory();
                response = await client.SendAsync(request, cancellationToken);
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
        public static Task<HttpResponseMessage> SendWithRetryAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3) =>
            client.SendWithRetryAsync(() => CloneHttpRequestMessage(request), retryCount);

        /// <summary>
        /// Sends an HTTP request with a retry policy.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="request">Instance of the HTTP request message.</param>
        /// <param name="retryCount">Number of retry attempts in case of failure.</param>
        /// <returns>HTTP response message.</returns>
        public static Task<HttpResponseMessage> RetryPolicyAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3) =>
            client.SendWithRetryAsync(request, retryCount);

        /// <summary>
        /// Sends an HTTP request with an exponential backoff retry policy using a request factory.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="requestFactory">Factory producing the HTTP request message for each attempt.</param>
        /// <param name="retryCount">Number of retry attempts in case of failure.</param>
        /// <param name="baseDelayMilliseconds">Base delay in milliseconds for the exponential backoff.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> ExponentialBackoffRetryAsync(this HttpClient client, Func<HttpRequestMessage> requestFactory, int retryCount = 3, int baseDelayMilliseconds = 200, CancellationToken cancellationToken = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (requestFactory is null) throw new ArgumentNullException(nameof(requestFactory));

            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    using var request = requestFactory();
                    response = await client.SendAsync(request, cancellationToken);
                    if (response.IsSuccessStatusCode)
                        return response;
                }
                catch (HttpRequestException) when (i < retryCount - 1)
                {
                    await Task.Delay(baseDelayMilliseconds * (int)Math.Pow(2, i), cancellationToken);
                }
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
        public static Task<HttpResponseMessage> ExponentialBackoffRetryAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3, int baseDelayMilliseconds = 200) =>
            client.ExponentialBackoffRetryAsync(() => CloneHttpRequestMessage(request), retryCount, baseDelayMilliseconds);

        /// <summary>
        /// Sends an HTTP request with a retry policy in case of timeout using a request factory.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="requestFactory">Factory producing the HTTP request message for each attempt.</param>
        /// <param name="retryCount">Number of retry attempts in case of timeout.</param>
        /// <param name="timeout">Timeout duration for each request attempt.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> TimeoutRetryAsync(this HttpClient client, Func<HttpRequestMessage> requestFactory, int retryCount = 3, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (requestFactory is null) throw new ArgumentNullException(nameof(requestFactory));

            if (timeout == default)
                timeout = TimeSpan.FromSeconds(10);

            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    using var timeoutCts = new CancellationTokenSource(timeout);
                    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);
                    using var request = requestFactory();

                    response = await client.SendAsync(request, linkedCts.Token);
                    response.EnsureSuccessStatusCode();
                    return response;
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested && i < retryCount - 1)
                {
                    continue;
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
        public static Task<HttpResponseMessage> TimeoutRetryAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3, TimeSpan timeout = default) =>
            client.TimeoutRetryAsync(() => CloneHttpRequestMessage(request), retryCount, timeout);

        /// <summary>
        /// Sends an HTTP request and handles transient errors by retrying the request using a request factory.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="requestFactory">Factory producing the HTTP request message for each attempt.</param>
        /// <param name="retryCount">Number of retry attempts in case of transient errors.</param>
        /// <param name="retryDelay">Delay between retries.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> HandleTransientErrorsAsync(this HttpClient client, Func<HttpRequestMessage> requestFactory, int retryCount = 3, TimeSpan retryDelay = default, CancellationToken cancellationToken = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (requestFactory is null) throw new ArgumentNullException(nameof(requestFactory));

            if (retryDelay == default)
                retryDelay = TimeSpan.FromSeconds(2);

            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    using var request = requestFactory();
                    response = await client.SendAsync(request, cancellationToken);
                    if (response.IsSuccessStatusCode)
                        return response;

                    if (!IsTransientStatusCode(response.StatusCode))
                        return response;
                }
                catch (HttpRequestException) when (i < retryCount - 1)
                {
                    await Task.Delay(retryDelay, cancellationToken);
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
        public static Task<HttpResponseMessage> HandleTransientErrorsAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3, TimeSpan retryDelay = default) =>
            client.HandleTransientErrorsAsync(() => CloneHttpRequestMessage(request), retryCount, retryDelay);

        /// <summary>
        /// Sends an HTTP request and retries in case of rate limiting errors using a request factory.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="requestFactory">Factory producing the HTTP request message for each attempt.</param>
        /// <param name="retryCount">Number of retry attempts in case of rate limiting errors.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> RateLimitRetryAsync(this HttpClient client, Func<HttpRequestMessage> requestFactory, int retryCount = 3, CancellationToken cancellationToken = default)
        {
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (requestFactory is null) throw new ArgumentNullException(nameof(requestFactory));

            HttpResponseMessage response = null;
            for (int i = 0; i < retryCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                using var request = requestFactory();
                response = await client.SendAsync(request, cancellationToken);
                if (response.StatusCode != (HttpStatusCode)429)
                    return response;

                if (response.Headers.TryGetValues("Retry-After", out var values))
                {
                    var retryAfter = values.FirstOrDefault();
                    if (int.TryParse(retryAfter, out int delaySeconds))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken);
                    }
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
        public static Task<HttpResponseMessage> RateLimitRetryAsync(this HttpClient client, HttpRequestMessage request, int retryCount = 3) =>
            client.RateLimitRetryAsync(() => CloneHttpRequestMessage(request), retryCount);

        private static bool IsTransientStatusCode(HttpStatusCode status)
        {
            var code = (int)status;
            return code >= 500 || code == 408 || code == 429;
        }

        private static HttpRequestMessage CloneHttpRequestMessage(HttpRequestMessage request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version
            };

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (request.Content != null)
            {
                var ms = new MemoryStream();
                var readTask = request.Content.ReadAsStreamAsync();
                var stream = readTask.IsCompleted ? readTask.Result : readTask.ConfigureAwait(false).GetAwaiter().GetResult();
                stream.CopyTo(ms);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                foreach (var header in request.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return clone;
        }
    }
}