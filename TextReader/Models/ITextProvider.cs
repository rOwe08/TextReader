using System.Collections.Generic;

namespace TextReader.Models
{
    /// <summary>
    /// Provides access to a collection of text lines.
    /// </summary>
    public interface ITextProvider
    {
        /// <summary>
        /// Gets the collection of text lines.
        /// </summary>
        IReadOnlyList<string> Lines { get; }
    }
}
