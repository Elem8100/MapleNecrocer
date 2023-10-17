namespace MapleNecrocer
{
    partial class MountForm
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
            button1 = new Button();
            textBox1 = new TextBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            label1 = new Label();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            button1.Location = new Point(247, 12);
            button1.Name = "button1";
            button1.Size = new Size(80, 26);
            button1.TabIndex = 0;
            button1.Text = "Remove";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            textBox1.Location = new Point(63, 14);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(132, 24);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(2, 53);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(353, 497);
            tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 21);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(345, 472);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 28);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(345, 465);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(10, 16);
            label1.Name = "label1";
            label1.Size = new Size(52, 18);
            label1.TabIndex = 3;
            label1.Text = "Search";
            // 
            // MountForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(359, 553);
            Controls.Add(label1);
            Controls.Add(tabControl1);
            Controls.Add(textBox1);
            Controls.Add(button1);
            DoubleBuffered = true;
            Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximumSize = new Size(377, 1000);
            Name = "MountForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mount";
            TopMost = true;
            Shown += MountForm_Shown;
            ForeColorChanged += MountForm_ForeColorChanged;
            KeyDown += MountForm_KeyDown;
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label1;
    }
}