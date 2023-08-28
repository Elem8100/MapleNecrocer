namespace MapleNecrocer
{
    partial class TotemEffectForm
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
            label1 = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(5, 42);
            panel1.Name = "panel1";
            panel1.Size = new Size(348, 443);
            panel1.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(260, 7);
            button1.Name = "button1";
            button1.Size = new Size(79, 28);
            button1.TabIndex = 6;
            button1.Text = "Remove";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(17, 14);
            label1.Name = "label1";
            label1.Size = new Size(52, 18);
            label1.TabIndex = 5;
            label1.Text = "Search";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(80, 11);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(113, 25);
            textBox1.TabIndex = 4;
            // 
            // TotemEffectForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(359, 496);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(panel1);
            Font = new Font("Microsoft JhengHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximumSize = new Size(377, 1200);
            Name = "TotemEffectForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TotemEffectForm";
            TopMost = true;
            Shown += TotemEffectForm_Shown;
            KeyDown += TotemEffectForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button button1;
        private Label label1;
        private TextBox textBox1;
    }
}