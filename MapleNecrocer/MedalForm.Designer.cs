namespace MapleNecrocer
{
    partial class MedalForm
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
            panel1 = new Panel();
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(12, 43);
            panel1.Name = "panel1";
            panel1.Size = new Size(335, 557);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(12, 17);
            label1.Name = "label1";
            label1.Size = new Size(52, 18);
            label1.TabIndex = 1;
            label1.Text = "Search";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(65, 15);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(116, 23);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button1
            // 
            button1.Location = new Point(234, 10);
            button1.Name = "button1";
            button1.Size = new Size(95, 28);
            button1.TabIndex = 3;
            button1.Text = "Remove";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // MedalForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(359, 612);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximumSize = new Size(377, 900);
            Name = "MedalForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Medal";
            TopMost = true;
            FormClosing += MedalForm_FormClosing;
            Shown += MedalForm_Shown;
            KeyDown += MedalForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private TextBox textBox1;
        private Button button1;
    }
}