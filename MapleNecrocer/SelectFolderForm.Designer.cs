namespace MapleNecrocer
{
    partial class SelectFolderForm
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
            SelectWzButton = new Button();
            RecentFilesGrid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)RecentFilesGrid).BeginInit();
            SuspendLayout();
            // 
            // SelectWzButton
            // 
            SelectWzButton.Font = new Font("Microsoft JhengHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            SelectWzButton.Location = new Point(154, 12);
            SelectWzButton.Name = "SelectWzButton";
            SelectWzButton.Size = new Size(178, 38);
            SelectWzButton.TabIndex = 0;
            SelectWzButton.Text = "Select Folder";
            SelectWzButton.UseVisualStyleBackColor = true;
            SelectWzButton.Click += SelectFolderButton_Click;
            // 
            // RecentFilesGrid
            // 
            RecentFilesGrid.AllowUserToAddRows = false;
            RecentFilesGrid.BackgroundColor = SystemColors.ButtonHighlight;
            RecentFilesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            RecentFilesGrid.ColumnHeadersVisible = false;
            RecentFilesGrid.Location = new Point(12, 56);
            RecentFilesGrid.Name = "RecentFilesGrid";
            RecentFilesGrid.RowHeadersVisible = false;
            RecentFilesGrid.RowHeadersWidth = 51;
            RecentFilesGrid.RowTemplate.Height = 29;
            RecentFilesGrid.Size = new Size(494, 245);
            RecentFilesGrid.TabIndex = 1;
            RecentFilesGrid.CellContentClick += RecentFilesGrid_CellContentClick;
            // 
            // SelectFolderForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(520, 313);
            Controls.Add(RecentFilesGrid);
            Controls.Add(SelectWzButton);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "SelectFolderForm";
            Text = "Select wz";
            TopMost = true;
            Load += OpenWzForm_Load;
            ((System.ComponentModel.ISupportInitialize)RecentFilesGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button SelectWzButton;
        private DataGridView RecentFilesGrid;
    }
}