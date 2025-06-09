using TextReader.Services;
using TextReader.Models;

namespace TextReader.Tests
{
    public class KeyboardNavigationTests
    {
        private class MockTextProvider : ITextProvider
        {
            public IReadOnlyList<string> Lines { get; }

            public MockTextProvider(IReadOnlyList<string> lines)
            {
                Lines = lines;
            }
        }

        [Fact]
        public void NavigateToStart_SelectsFirstRow()
        {
            // Arrange
            var dataGridView = new DataGridView();
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView.Rows.Add("Line 1");
            dataGridView.Rows.Add("Line 2");
            dataGridView.Rows.Add("Line 3");

            var handler = new KeyboardNavigationHandler(dataGridView);

            // Act
            handler.NavigateToStart();

            // Assert
            Assert.Equal(0, dataGridView.FirstDisplayedScrollingRowIndex);
            Assert.True(dataGridView.Rows[0].Selected);
            Assert.Equal(dataGridView.Rows[0].Cells[0], dataGridView.CurrentCell);
        }

        [Fact]
        public void NavigateToEnd_SelectsLastRow()
        {
            // Arrange
            var dataGridView = new DataGridView();
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView.Rows.Add("Line 1");
            dataGridView.Rows.Add("Line 2");
            dataGridView.Rows.Add("Line 3");

            var handler = new KeyboardNavigationHandler(dataGridView);

            // Act
            handler.NavigateToEnd();

            // Assert
            int lastRow = dataGridView.RowCount - 1;
            Assert.True(dataGridView.Rows[lastRow].Selected);
            Assert.Equal(dataGridView.Rows[lastRow].Cells[0], dataGridView.CurrentCell);
        }

        [Fact]
        public void NavigatePageUp_MovesUpOnePage()
        {
            // Arrange
            var dataGridView = new DataGridView();
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn());
            for (int i = 0; i < 20; i++)
            {
                dataGridView.Rows.Add($"Line {i + 1}");
            }

            var handler = new KeyboardNavigationHandler(dataGridView);
            dataGridView.FirstDisplayedScrollingRowIndex = 10;

            // Act
            handler.NavigatePageUp();

            // Assert
            int expectedIndex = Math.Max(0, 10 - dataGridView.DisplayedRowCount(true));
            Assert.Equal(expectedIndex, dataGridView.FirstDisplayedScrollingRowIndex);
            Assert.True(dataGridView.Rows[expectedIndex].Selected);
            Assert.Equal(dataGridView.Rows[expectedIndex].Cells[0], dataGridView.CurrentCell);
        }

        [Fact]
        public void NavigatePageDown_MovesDownOnePage()
        {
            // Arrange
            var dataGridView = new DataGridView();
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn());
            for (int i = 0; i < 20; i++)
            {
                dataGridView.Rows.Add($"Line {i + 1}");
            }

            var handler = new KeyboardNavigationHandler(dataGridView);
            dataGridView.FirstDisplayedScrollingRowIndex = 0;

            // Act
            handler.NavigatePageDown();

            // Assert
            int expectedIndex = Math.Min(dataGridView.RowCount - 1, dataGridView.DisplayedRowCount(true));
            Assert.True(dataGridView.Rows[expectedIndex].Selected);
            Assert.Equal(dataGridView.Rows[expectedIndex].Cells[0], dataGridView.CurrentCell);
        }

        [Fact]
        public void KeyDown_Home_HandlesEvent()
        {
            // Arrange
            var dataGridView = new DataGridView();
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView.Rows.Add("Line 1");
            dataGridView.Rows.Add("Line 2");
            dataGridView.Rows.Add("Line 3");

            var handler = new KeyboardNavigationHandler(dataGridView);
            var keyEventArgs = new KeyEventArgs(Keys.Home);

            // Act
            dataGridView.Focus();
            dataGridView.OnKeyDown(keyEventArgs);

            // Assert
            Assert.Equal(0, dataGridView.FirstDisplayedScrollingRowIndex);
            Assert.True(dataGridView.Rows[0].Selected);
        }

        [Fact]
        public void KeyDown_End_HandlesEvent()
        {
            // Arrange
            var dataGridView = new DataGridView();
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn());
            dataGridView.Rows.Add("Line 1");
            dataGridView.Rows.Add("Line 2");
            dataGridView.Rows.Add("Line 3");

            var handler = new KeyboardNavigationHandler(dataGridView);
            var keyEventArgs = new KeyEventArgs(Keys.End);

            // Act
            dataGridView.Focus();
            dataGridView.OnKeyDown(keyEventArgs);

            // Assert
            int lastRow = dataGridView.RowCount - 1;
            Assert.True(dataGridView.Rows[lastRow].Selected);
        }
    }

    public static class ControlExtensions
    {
        public static void OnKeyDown(this Control control, KeyEventArgs e)
        {
            var method = typeof(Control).GetMethod("OnKeyDown", 
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.NonPublic);
            
            method?.Invoke(control, new object[] { e });
        }
    }
} 