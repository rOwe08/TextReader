using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TextReader.Models
{
    /// <summary>
    /// Represents a source of text data that can be loaded asynchronously.
    /// </summary>
    public interface ITextSource
    {
        /// <summary>
        /// Asynchronously retrieves all lines of text from the source.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A collection of text lines.</returns>
        Task<IEnumerable<string>> GetLinesAsync(CancellationToken cancellationToken = default);
    }
}
