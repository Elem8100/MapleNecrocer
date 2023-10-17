namespace MapleNecrocer
{
    partial class TitleForm
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
            button1 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(12, 44);
            panel1.Name = "panel1";
            panel1.Size = new Size(335, 517);
            panel1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            button1.Location = new Point(252, 7);
            button1.Name = "button1";
            button1.Size = new Size(95, 28);
            button1.TabIndex = 6;
            button1.Text = "Remove";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            textBox1.Location = new Point(62, 11);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(116, 23);
            textBox1.TabIndex = 5;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(13, 12);
            label1.Name = "label1";
            label1.Size = new Size(52, 18);
            label1.TabIndex = 4;
            label1.Text = "Search";
            // 
            // TitleForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(359, 573);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Point);
            KeyPreview = true;
            MaximumSize = new Size(377, 900);
            Name = "TitleForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TitleForm";
            TopMost = true;
            FormClosing += TitleForm_FormClosing;
            Shown += TitleForm_Shown;
            KeyDown += TitleForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button button1;
        private TextBox textBox1;
        private Label label1;
    }
}