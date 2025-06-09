using System;
using System.Collections.Generic;
using System.Linq;
using TextReader.Models;

namespace TextReader.Services
{
    /// <summary>
    /// Provides text filtering functionality with case sensitivity options.
    /// </summary>
    public class TextFilterService
    {
        private readonly ITextProvider _provider;
        private List<int> _filteredIndices = new();
        private bool _ignoreCase = true;

        /// <summary>
        /// Gets the list of indices that match the current filter.
        /// </summary>
        public IReadOnlyList<int> FilteredIndices => _filteredIndices;

        /// <summary>
        /// Gets the number of lines that match the current filter.
        /// </summary>
        public int FilteredLines => _filteredIndices.Count;

        /// <summary>
        /// Gets or sets whether the filter should ignore case.
        /// </summary>
        public bool IgnoreCase 
        { 
            get => _ignoreCase;
            set
            {
                if (_ignoreCase != value)
                {
                    _ignoreCase = value;
                    // Re-apply filter with new case sensitivity if we have a current filter
                    if (_filteredIndices.Any())
                    {
                        ApplyFilter(_currentFilter);
                    }
                }
            }
        }
        private string _currentFilter = string.Empty;

        /// <summary>
        /// Initializes a new instance of the TextFilterService.
        /// </summary>
        /// <param name="provider">The text provider to filter.</param>
        public TextFilterService(ITextProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Applies a filter to the text, showing only lines that contain the specified text.
        /// </summary>
        /// <param name="filter">The text to filter by.</param>
        public void ApplyFilter(string filter)
        {
            _currentFilter = filter;
            if (string.IsNullOrWhiteSpace(filter))
            {
                ResetFilter();
                return;
            }

            _filteredIndices = _provider.Lines
                .Select((line, index) => new { line, index })
                .Where(x => x.line.Contains(filter, _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                .Select(x => x.index)
                .ToList();
        }

        /// <summary>
        /// Resets the filter, showing all lines.
        /// </summary>
        public void ResetFilter()
        {
            _filteredIndices = Enumerable.Range(0, _provider.Lines.Count).ToList();
            _currentFilter = string.Empty;
        }

        /// <summary>
        /// Gets the original index of a line in the filtered view.
        /// </summary>
        /// <param name="filteredIndex">The index in the filtered view.</param>
        /// <returns>The original index of the line, or -1 if the filtered index is invalid.</returns>
        public int GetOriginalIndex(int filteredIndex)
        {
            return filteredIndex >= 0 && filteredIndex < _filteredIndices.Count 
                ? _filteredIndices[filteredIndex] 
                : -1;
        }

        public int GetFilteredIndex(int originalIndex)
        {
            return _filteredIndices.IndexOf(originalIndex);
        }
    }
} 