using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TextReader.Models;

namespace TextReader.Sources
{
    /// <summary>
    /// Provides text content from a URL source, with HTML handling capabilities.
    /// </summary>
    public class UrlTextSource : ITextSource
    {
        private readonly string _url;
        private readonly HttpClient _httpClient;
        private static readonly string[] LineSeparators = new[] { "\r\n", "\n" };

        /// <summary>
        /// Initializes a new instance of the UrlTextSource.
        /// </summary>
        /// <param name="url">The URL to fetch text from.</param>
        /// <param name="httpClient">Optional HttpClient instance for making requests.</param>
        public UrlTextSource(string url, HttpClient? httpClient = null)
        {
            _url = url;
            _httpClient = httpClient ?? new HttpClient();
            // Set User-Agent if not already set (some servers require it)
            if (_httpClient.DefaultRequestHeaders.UserAgent.Count == 0)
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; TextReaderApp/1.0)");
        }

        /// <summary>
        /// Gets a descriptive name for this source.
        /// </summary>
        public string Name => $"URL: {_url}";

        /// <summary>
        /// Asynchronously retrieves text content from the URL.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A collection of text lines.</returns>
        public async Task<IEnumerable<string>> GetLinesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var content = await _httpClient.GetStringAsync(_url, cancellationToken);
                
                // Check if content is HTML based on file extension or content
                if (_url.EndsWith(".html", StringComparison.OrdinalIgnoreCase) || 
                    _url.EndsWith(".htm", StringComparison.OrdinalIgnoreCase) ||
                    content.TrimStart().StartsWith("<!DOCTYPE", StringComparison.OrdinalIgnoreCase) ||
                    content.TrimStart().StartsWith("<html", StringComparison.OrdinalIgnoreCase))
                {
                    // Remove HTML tags and decode HTML entities for better readability
                    content = WebUtility.HtmlDecode(
                        System.Text.RegularExpressions.Regex.Replace(content, "<[^>]*>", ""));
                }

                return content.Split(LineSeparators, StringSplitOptions.None);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TextSourceException($"Error reading URL {_url}", ex);
            }
        }
    }
}
