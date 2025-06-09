using System;
using System.Windows.Forms;
using TextReader.UI;

namespace TextReader.Services
{
    public class SearchManager
    {
        private readonly Form _owner;
        private readonly TextSearchService _searchService;
        private readonly TextFilterService _filterService;
        private readonly Action _updateDataGridView;
        private readonly Action _updateSearchStatus;
        private SearchBoxForm? _searchForm;

        public SearchManager(
            Form owner,
            TextSearchService searchService,
            TextFilterService filterService,
            Action updateDataGridView,
            Action<string> updateStatus,
            Action updateSearchStatus)
        {
            _owner = owner;
            _searchService = searchService;
            _filterService = filterService;
            _updateDataGridView = updateDataGridView;
            _updateSearchStatus = updateSearchStatus;
        }

        public void ShowSearchBox()
        {
            if (_searchForm == null || _searchForm.IsDisposed)
            {
                _searchForm = new SearchBoxForm();
                _searchForm.SearchTextChanged += SearchForm_SearchTextChanged;
                _searchForm.FilteringChanged += SearchForm_FilteringChanged;
                _searchForm.CaseSensitivityChanged += SearchForm_CaseSensitivityChanged;
                _searchForm.FormClosed += (s, e) => _searchForm = null;
                _searchForm.Show(_owner);
            }
            else
            {
                if (_searchForm.Visible)
                {
                    _searchForm.Hide();
                }
                else
                {
                    _searchForm.Show();
                    _searchForm.Activate();
                }
            }
        }

        private void SearchForm_SearchTextChanged(object? sender, string searchText)
        {
            try
            {
                _searchService.Search(searchText);
                
                if (_searchForm?.IsFiltering == true)
                {
                    _filterService.ApplyFilter(searchText);
                    _updateDataGridView();
                }
                else if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var result = _searchService.FindNext();
                    NavigateToResult(result);
                    _updateSearchStatus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during search: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchForm_FilteringChanged(object? sender, bool isFiltering)
        {
            if (isFiltering)
            {
                _filterService.ApplyFilter(_searchForm?.SearchQuery ?? string.Empty);
            }
            else
            {
                _filterService.ResetFilter();
            }
            _updateDataGridView();
        }

        private void SearchForm_CaseSensitivityChanged(object? sender, bool ignoreCase)
        {
            try
            {
                _searchService.IgnoreCase = ignoreCase;
                _filterService.IgnoreCase = ignoreCase;
                
                if (_searchForm?.IsFiltering == true)
                {
                    _filterService.ApplyFilter(_searchForm.SearchQuery);
                    _updateDataGridView();
                }
                else if (!string.IsNullOrWhiteSpace(_searchForm?.SearchQuery))
                {
                    _searchService.Search(_searchForm.SearchQuery);
                    var result = _searchService.FindNext();
                    NavigateToResult(result);
                    _updateSearchStatus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during search: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void FindNext()
        {
            try
            {
                var result = _searchService.FindNext();
                NavigateToResult(result);
                _updateSearchStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error finding next match: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void FindPrevious()
        {
            try
            {
                var result = _searchService.FindPrevious();
                NavigateToResult(result);
                _updateSearchStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error finding previous match: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NavigateToResult(int? index)
        {
            if (index.HasValue)
            {
                try
                {
                    int filteredIndex = _filterService.GetFilteredIndex(index.Value);
                    if (filteredIndex >= 0)
                    {
                        var dataGridView = _owner.Controls.Find("dataGridView", true)[0] as DataGridView;
                        if (dataGridView != null)
                        {
                            dataGridView.FirstDisplayedScrollingRowIndex = Math.Max(0, filteredIndex - dataGridView.DisplayedRowCount(true) / 2);
                            dataGridView.Rows[filteredIndex].Selected = true;
                            dataGridView.CurrentCell = dataGridView.Rows[filteredIndex].Cells[0];
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error navigating to result: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
} 