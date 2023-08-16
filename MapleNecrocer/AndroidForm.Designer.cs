namespace MapleNecrocer
{
    partial class AndroidForm
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
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(24, 15);
            label1.Name = "label1";
            label1.Size = new Size(52, 18);
            label1.TabIndex = 4;
            label1.Text = "Search";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(82, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(120, 24);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button1
            // 
            button1.Location = new Point(291, 12);
            button1.Name = "button1";
            button1.Size = new Size(79, 28);
            button1.TabIndex = 5;
            button1.Text = "Remove";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(6, 42);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(379, 592);
            tabControl1.TabIndex = 6;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(371, 562);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Android";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 28);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(371, 560);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Custom";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // AndroidForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(389, 640);
            Controls.Add(tabControl1);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximumSize = new Size(407, 687);
            Name = "AndroidForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Android";
            TopMost = true;
            Shown += AndroidForm_Shown;
            KeyDown += AndroidForm_KeyDown;
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
    }
}