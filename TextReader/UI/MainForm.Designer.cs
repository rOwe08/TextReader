namespace TextReader.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            readToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            txtFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fromURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            randomTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            bindingSource1 = new System.Windows.Forms.BindingSource(components);
            dataGridView = new System.Windows.Forms.DataGridView();
            menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip.Size = new System.Drawing.Size(1004, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { readToolStripMenuItem, saveToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // readToolStripMenuItem
            // 
            readToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { txtFileToolStripMenuItem, fromURLToolStripMenuItem, randomTextToolStripMenuItem });
            readToolStripMenuItem.Name = "readToolStripMenuItem";
            readToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            readToolStripMenuItem.Text = "Read";
            // 
            // txtFileToolStripMenuItem
            // 
            txtFileToolStripMenuItem.Name = "txtFileToolStripMenuItem";
            txtFileToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            txtFileToolStripMenuItem.Text = "TXT file";
            txtFileToolStripMenuItem.Click += txtFileToolStripMenuItem_Click;
            // 
            // fromURLToolStripMenuItem
            // 
            fromURLToolStripMenuItem.Name = "fromURLToolStripMenuItem";
            fromURLToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            fromURLToolStripMenuItem.Text = "From URL";
            fromURLToolStripMenuItem.Click += fromURLToolStripMenuItem_Click;
            // 
            // randomTextToolStripMenuItem
            // 
            randomTextToolStripMenuItem.Name = "randomTextToolStripMenuItem";
            randomTextToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            randomTextToolStripMenuItem.Text = "Random Text";
            randomTextToolStripMenuItem.Click += randomTextStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridView.ColumnHeadersVisible = false;
            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                new System.Windows.Forms.DataGridViewTextBoxColumn() {
                    Name = "Text",
                    DataPropertyName = "Text",
                    AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill,
                    ReadOnly = true
                }
            });
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.GridColor = System.Drawing.SystemColors.Window;
            dataGridView.Location = new System.Drawing.Point(0, 24);
            dataGridView.MultiSelect = false;
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.RowTemplate = new System.Windows.Forms.DataGridViewRow();
            dataGridView.RowTemplate.Height = 20;
            dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new System.Drawing.Size(1004, 592);
            dataGridView.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ActiveCaption;
            ClientSize = new System.Drawing.Size(1004, 616);
            Controls.Add(dataGridView);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MainForm";
            Text = "MainForm";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripMenuItem readToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem txtFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromURLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem randomTextToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView;
    }
}

