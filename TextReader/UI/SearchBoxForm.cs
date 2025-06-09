using System;
using System.Windows.Forms;

namespace TextReader.UI
{
    public partial class SearchBoxForm : Form
    {
        public event EventHandler<string>? SearchTextChanged;
        public event EventHandler<bool>? FilteringChanged;
        public event EventHandler<bool>? CaseSensitivityChanged;

        public string SearchQuery => searchTextBox.Text;
        public bool IsFiltering => filterCheckBox.Checked;
        public bool IgnoreCase => !caseSensitiveCheckBox.Checked;

        public SearchBoxForm()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            searchTextBox.TextChanged += (s, e) => SearchTextChanged?.Invoke(this, searchTextBox.Text);
            filterCheckBox.CheckedChanged += (s, e) => FilteringChanged?.Invoke(this, filterCheckBox.Checked);
            caseSensitiveCheckBox.CheckedChanged += (s, e) => CaseSensitivityChanged?.Invoke(this, !caseSensitiveCheckBox.Checked);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            base.OnFormClosing(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Hide();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
