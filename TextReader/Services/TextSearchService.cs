using System;
using System.Collections.Generic;
using System.Linq;
using TextReader.Models;

namespace TextReader.Services
{
    /// <summary>
    /// Provides text search functionality with case sensitivity options and navigation.
    /// </summary>
    public class TextSearchService
    {
        private readonly ITextProvider _provider;
        private List<int> _matchedIndices = new();
        private readonly SearchNavigator _navigator;
        private bool _ignoreCase = true;
        private string _currentQuery = string.Empty;

        /// <summary>
        /// Gets the list of indices where matches were found.
        /// </summary>
        public IReadOnlyList<int> Matches => _matchedIndices;

        /// <summary>
        /// Gets or sets whether the search should ignore case.
        /// </summary>
        public bool IgnoreCase 
        { 
            get => _ignoreCase;
            set
            {
                if (_ignoreCase != value)
                {
                    _ignoreCase = value;

                    if (_matchedIndices.Any())
                    {
                        Search(_currentQuery);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the TextSearchService.
        /// </summary>
        /// <param name="provider">The text provider to search in.</param>
        public TextSearchService(ITextProvider provider)
        {
            _provider = provider;
            _navigator = new SearchNavigator(this);
        }

        /// <summary>
        /// Searches for the specified query in the text.
        /// </summary>
        /// <param name="query">The text to search for.</param>
        public void Search(string query)
        {
            _currentQuery = query;
            if (string.IsNullOrWhiteSpace(query))
            {
                _matchedIndices.Clear();
            }
            else
            {
                _matchedIndices = _provider.Lines
                    .Select((line, index) => new { line, index })
                    .Where(x => x.line.Contains(query, _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                    .Select(x => x.index)
                    .ToList();
            }
            
            _navigator.Reset();
        }

        /// <summary>
        /// Finds the next occurrence of the search query.
        /// </summary>
        /// <returns>The index of the next match, or null if no more matches are found.</returns>
        public int? FindNext()
        {
            return _navigator.Next();
        }

        /// <summary>
        /// Finds the previous occurrence of the search query.
        /// </summary>
        /// <returns>The index of the previous match, or null if no more matches are found.</returns>
        public int? FindPrevious()
        {
            return _navigator.Previous();
        }

        /// <summary>
        /// Gets the index of the current match.
        /// </summary>
        public int CurrentMatchIndex => _navigator.CurrentMatchIndex;

        /// <summary>
        /// Gets the total number of matches found.
        /// </summary>
        public int TotalMatches => _navigator.TotalMatches;
    }
}
