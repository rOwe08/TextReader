using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextReader
{
    public partial class UrlInputForm : Form
    {
        private TextBox urlBox;
        private Button readButton;

        public string EnteredUrl => urlBox.Text;

        public UrlInputForm()
        {
            this.Text = "Enter URL";
            this.Width = 400;
            this.Height = 150;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            var label = new Label
            {
                Text = "URL:",
                Left = 10,
                Top = 20,
                Width = 40
            };

            urlBox = new TextBox
            {
                Left = 60,
                Top = 18,
                Width = 300
            };

            readButton = new Button
            {
                Text = "Read",
                Left = 270,
                Top = 50,
                Width = 90
            };
            readButton.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(urlBox.Text))
                {
                    MessageBox.Show("URL can't be empty.");
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            };

            Controls.Add(label);
            Controls.Add(urlBox);
            Controls.Add(readButton);
        }
    }
}
