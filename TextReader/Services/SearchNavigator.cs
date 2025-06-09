namespace TextReader.Services
{
    /// <summary>
    /// Handles navigation through search results.
    /// </summary>
    public class SearchNavigator
    {
        private readonly TextSearchService _searchService;
        private int _currentIndex = -1;

        /// <summary>
        /// Gets the index of the current match.
        /// </summary>
        public int CurrentMatchIndex => _currentIndex;

        /// <summary>
        /// Gets the total number of matches found.
        /// </summary>
        public int TotalMatches => _searchService.Matches.Count;

        public SearchNavigator(TextSearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// Resets the navigator to the initial state.
        /// </summary>
        public void Reset()
        {
            _currentIndex = -1;
        }

        /// <summary>
        /// Moves to the next match in the search results.
        /// </summary>
        /// <returns>The index of the next match, or null if no more matches are found.</returns>
        public int? Next()
        {
            // Return null if no matches exist
            if (_searchService.Matches.Count == 0) return null;
            
            // Handle first search (currentIndex is -1)
            if (_currentIndex < 0)
            {
                _currentIndex = 0;
            }
            // Move to next match if possible
            else if (_currentIndex < _searchService.Matches.Count - 1)
            {
                _currentIndex++;
            }
            // No more matches
            else
            {
                return null;
            }
            
            return _searchService.Matches[_currentIndex];
        }

        /// <summary>
        /// Moves to the previous match in the search results.
        /// </summary>
        /// <returns>The index of the previous match, or null if no more matches are found.</returns>
        public int? Previous()
        {
            // Return null if no matches exist
            if (_searchService.Matches.Count == 0) return null;
            
            // Handle first search (currentIndex is -1)
            if (_currentIndex < 0)
            {
                _currentIndex = _searchService.Matches.Count - 1;
            }
            // Move to previous match if possible
            else if (_currentIndex > 0)
            {
                _currentIndex--;
            }
            // No more matches
            else
            {
                return null;
            }
            
            return _searchService.Matches[_currentIndex];
        }
    }
}
