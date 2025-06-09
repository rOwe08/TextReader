using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TextReader.Models;
using TextReader.Services;
using TextReader.Sources;

namespace TextReader.UI
{
    public partial class MainForm : Form, ITextProvider
    {
        private List<string> AllLines = new List<string>();
        public IReadOnlyList<string> Lines => AllLines;

        private readonly TextSearchService _searchService;
        private readonly TextFilterService _filterService;
        private readonly SearchManager _searchManager;
        private readonly StatusStrip _statusStrip = new();
        private readonly ToolStripStatusLabel _statusLabel = new("Ready");
        private readonly ToolStripProgressBar _progressBar = new() { Visible = false };
        private KeyboardNavigationHandler? _keyboardNavigationHandler;

        public MainForm()
        {
            InitializeComponent();
            InitializeStatusStrip();
            InitializeSmoothScrolling();

            _searchService = new TextSearchService(this);
            _filterService = new TextFilterService(this);
            _searchManager = new SearchManager(
                this,
                _searchService,
                _filterService,
                UpdateDataGridView,
                UpdateStatus,
                UpdateSearchStatus
            );

            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            _keyboardNavigationHandler = new KeyboardNavigationHandler(dataGridView);
        }

        private void InitializeStatusStrip()
        {
            _statusStrip.Items.Add(_statusLabel);
            _statusStrip.Items.Add(_progressBar);
            Controls.Add(_statusStrip);
        }

        private void InitializeSmoothScrolling()
        {
            dataGridView.VirtualMode = true;
            dataGridView.CellValueNeeded += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < _filterService.FilteredLines)
                {
                    int originalIndex = _filterService.GetOriginalIndex(e.RowIndex);
                    if (originalIndex >= 0 && originalIndex < AllLines.Count)
                    {
                        e.Value = AllLines[originalIndex];
                    }
                }
            };

            if (dataGridView.Columns.Count == 0)
            {
                dataGridView.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Text",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
            }
        }

        private void DataGridView_SelectionChanged(object? sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                int currentIndex = dataGridView.SelectedRows[0].Index;
                int actualIndex = _filterService.GetOriginalIndex(currentIndex);
                if (actualIndex >= 0)
                {
                    UpdateStatus($"Line {actualIndex + 1} of {AllLines.Count}");
                }
            }
        }

        private async Task LoadLinesFromSourceAsync(ITextSource source)
        {
            try
            {
                ShowProgress(true);
                UpdateStatus("Loading...");

                var lines = (await source.GetLinesAsync()).ToList();

                dataGridView.Rows.Clear();
                AllLines = lines;
                
                _filterService.ResetFilter();
                dataGridView.RowCount = _filterService.FilteredLines;
                dataGridView.Invalidate();

                UpdateStatus($"Loaded {AllLines.Count} lines");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data reading error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Error loading data");
            }
            finally
            {
                ShowProgress(false);
            }
        }

        private async void fromURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var inputForm = new UrlInputForm();
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                var source = new UrlTextSource(inputForm.EnteredUrl);
                await LoadLinesFromSourceAsync(source);
            }
        }

        private async void txtFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var source = new FileTextSource(openFileDialog.FileName);
                    await LoadLinesFromSourceAsync(source);
                }
            }
        }

        private async void randomTextStripMenuItem_Click(object sender, EventArgs e)
        {
            using var inputForm = new RandomTextInputForm();
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                var source = new RandomTextSource(inputForm.LineCount);
                await LoadLinesFromSourceAsync(source);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Save text to file";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = "output.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllLines(saveFileDialog.FileName, AllLines);
                        MessageBox.Show($"File saved successfully to:\n{saveFileDialog.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving file:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ShowSearchBox()
        {
            _searchManager.ShowSearchBox();
        }

        private void UpdateDataGridView()
        {
            // Handle cross-thread calls
            if (dataGridView.InvokeRequired)
            {
                dataGridView.Invoke(new Action(UpdateDataGridView));
                return;
            }

            // Preserve scroll position and selection
            int firstVisibleRow = dataGridView.FirstDisplayedScrollingRowIndex;
            int selectedRow = dataGridView.SelectedRows.Count > 0 ? dataGridView.SelectedRows[0].Index : -1;

            // Clear and repopulate the grid
            dataGridView.Rows.Clear();
            if (_filterService.FilteredLines > 0)
            {
                dataGridView.Rows.Add(_filterService.FilteredLines);
            }

            // Restore scroll position if valid
            if (firstVisibleRow >= 0 && firstVisibleRow < _filterService.FilteredLines)
            {
                dataGridView.FirstDisplayedScrollingRowIndex = firstVisibleRow;
            }

            // Restore selection if valid
            if (selectedRow >= 0 && selectedRow < _filterService.FilteredLines)
            {
                dataGridView.Rows[selectedRow].Selected = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Handle keyboard shortcuts
            switch (keyData)
            {
                case Keys.Control | Keys.F:
                    ShowSearchBox();
                    return true;
                case Keys.F3:
                    _searchManager.FindNext();
                    return true;
                case Keys.Shift | Keys.F3:
                    _searchManager.FindPrevious();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void UpdateStatus(string message)
        {
            // Handle cross-thread calls for status updates
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), message);
                return;
            }
            _statusLabel.Text = message;
        }

        private void UpdateSearchStatus()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateSearchStatus));
                return;
            }

            if (_searchService.TotalMatches > 0)
            {
                _statusLabel.Text = $"Match {_searchService.CurrentMatchIndex + 1} of {_searchService.TotalMatches}";
            }
            else
            {
                _statusLabel.Text = "No matches found";
            }
        }

        private void ShowProgress(bool show)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(ShowProgress), show);
                return;
            }
            _progressBar.Visible = show;
        }
    }
}
