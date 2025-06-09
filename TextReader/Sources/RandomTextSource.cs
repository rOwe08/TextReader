using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TextReader.Models;

namespace TextReader.Sources
{
    /// <summary>
    /// Generates random text content for testing purposes.
    /// </summary>
    public class RandomTextSource : ITextSource
    {
        private readonly int _lineCount;
        private static readonly Random _random = new Random();

        /// <summary>
        /// Initializes a new instance of the RandomTextSource.
        /// </summary>
        /// <param name="lineCount">The number of random lines to generate.</param>
        public RandomTextSource(int lineCount)
        {
            _lineCount = lineCount;
        }

        /// <summary>
        /// Asynchronously generates random text lines.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A collection of random text lines.</returns>
        public async Task<IEnumerable<string>> GetLinesAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                var lines = new List<string>();
                for (int i = 0; i < _lineCount; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    lines.Add(GenerateRandomLine());
                }
                return lines;
            }, cancellationToken);
        }

        /// <summary>
        /// Generates a single random line of text.
        /// </summary>
        /// <returns>A random string of alphanumeric characters.</returns>
        private string GenerateRandomLine()
        {
            int length = _random.Next(TextReaderConfig.MinRandomLineLength, TextReaderConfig.MaxRandomLineLength);
            return new string(Enumerable.Repeat(TextReaderConfig.RandomTextChars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
