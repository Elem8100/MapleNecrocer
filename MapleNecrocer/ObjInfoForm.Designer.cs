namespace MapleNecrocer
{
    partial class ObjInfoForm
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
            dataGridView1 = new DataGridView();
            label1 = new Label();
            button1 = new Button();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.Location = new Point(11, 44);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(675, 669);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(12, 25);
            label1.Name = "label1";
            label1.Size = new Size(18, 18);
            label1.TabIndex = 1;
            label1.Text = "  ";
            // 
            // button1
            // 
            button1.Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            button1.Location = new Point(501, 12);
            button1.Name = "button1";
            button1.Size = new Size(78, 28);
            button1.TabIndex = 2;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            button2.Location = new Point(598, 12);
            button2.Name = "button2";
            button2.Size = new Size(85, 28);
            button2.TabIndex = 3;
            button2.Text = "Save All";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // ObjInfoForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(695, 725);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Point);
            KeyPreview = true;
            MaximizeBox = false;
            Name = "ObjInfoForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ObjInfo";
            TopMost = true;
            Shown += ObjInfoForm_Shown;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Label label1;
        private Button button1;
        private Button button2;
    }
}