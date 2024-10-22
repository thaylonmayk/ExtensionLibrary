using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientExtensionsLibrary
{
    public static partial class HttpClientExtensions
    {

        /// <summary>
        /// Downloads a file from a specified URL.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">URL of the file to be downloaded.</param>
        /// <param name="outputPath">Destination path to save the file.</param>
        public static async Task DownloadFileAsync(this HttpClient client, string url, string outputPath)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fileStream);
        }

        /// <summary>
        /// Uploads a file to the server using multipart/form-data.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">Request URL.</param>
        /// <param name="filePath">Path to the file to be uploaded.</param>
        /// <param name="formFieldName">Name of the form field for the file upload.</param>
        /// <returns>HTTP response message.</returns>
        public static async Task<HttpResponseMessage> UploadFileAsync(this HttpClient client, string url, string filePath, string formFieldName)
        {
            var content = new MultipartFormDataContent();
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileContent = new StreamContent(fileStream);
            content.Add(fileContent, formFieldName, Path.GetFileName(filePath));
            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return response;
        }

        /// <summary>
        /// Gets the metadata of a file from a specified URL.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">URL of the file.</param>
        /// <returns>Dictionary of metadata headers.</returns>
        public static async Task<IDictionary<string, string>> GetFileMetadataAsync(this HttpClient client, string url)
        {
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
            response.EnsureSuccessStatusCode();
            return response.Headers.ToDictionary(header => header.Key, header => string.Join(", ", header.Value));
        }

        /// <summary>
        /// Gets the size of a file from a specified URL.
        /// </summary>
        /// <param name="client">Instance of HttpClient.</param>
        /// <param name="url">URL of the file.</param>
        /// <returns>Size of the file in bytes.</returns>
        public static async Task<long> GetFileSizeAsync(this HttpClient client, string url)
        {
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
            response.EnsureSuccessStatusCode();
            return response.Content.Headers.ContentLength ?? 0;
        }

    }
}
