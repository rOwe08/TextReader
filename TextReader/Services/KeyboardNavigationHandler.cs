using System;
using System.Windows.Forms;

namespace TextReader.Services
{
    public class KeyboardNavigationHandler
    {
        private readonly DataGridView _dataGridView;

        public KeyboardNavigationHandler(DataGridView dataGridView)
        {
            _dataGridView = dataGridView;
            _dataGridView.KeyDown += DataGridView_KeyDown;
        }

        private void DataGridView_KeyDown(object? sender, KeyEventArgs e)
        {
            if (_dataGridView.RowCount == 0) return;

            switch (e.KeyCode)
            {
                case Keys.Home:
                    e.Handled = true;
                    NavigateToStart();
                    break;

                case Keys.End:
                    e.Handled = true;
                    NavigateToEnd();
                    break;

                case Keys.PageUp:
                    e.Handled = true;
                    NavigatePageUp();
                    break;

                case Keys.PageDown:
                    e.Handled = true;
                    NavigatePageDown();
                    break;
            }
        }

        public void NavigateToStart()
        {
            _dataGridView.FirstDisplayedScrollingRowIndex = 0;
            _dataGridView.Rows[0].Selected = true;
            _dataGridView.CurrentCell = _dataGridView.Rows[0].Cells[0];
        }

        public void NavigateToEnd()
        {
            int lastRow = _dataGridView.RowCount - 1;
            _dataGridView.FirstDisplayedScrollingRowIndex = Math.Max(0, lastRow - _dataGridView.DisplayedRowCount(true));
            _dataGridView.Rows[lastRow].Selected = true;
            _dataGridView.CurrentCell = _dataGridView.Rows[lastRow].Cells[0];
        }

        public void NavigatePageUp()
        {
            int currentPageUp = _dataGridView.FirstDisplayedScrollingRowIndex;
            int newPageUpIndex = Math.Max(0, currentPageUp - _dataGridView.DisplayedRowCount(true));
            _dataGridView.FirstDisplayedScrollingRowIndex = newPageUpIndex;
            _dataGridView.Rows[newPageUpIndex].Selected = true;
            _dataGridView.CurrentCell = _dataGridView.Rows[newPageUpIndex].Cells[0];
        }

        public void NavigatePageDown()
        {
            int currentPageDown = _dataGridView.FirstDisplayedScrollingRowIndex;
            int newPageDownIndex = Math.Min(
                _dataGridView.RowCount - 1,
                currentPageDown + _dataGridView.DisplayedRowCount(true)
            );
            _dataGridView.FirstDisplayedScrollingRowIndex = Math.Max(0, newPageDownIndex - _dataGridView.DisplayedRowCount(true) + 1);
            _dataGridView.Rows[newPageDownIndex].Selected = true;
            _dataGridView.CurrentCell = _dataGridView.Rows[newPageDownIndex].Cells[0];
        }
    }
} 