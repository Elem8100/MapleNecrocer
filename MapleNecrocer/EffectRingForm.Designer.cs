namespace MapleNecrocer
{
    partial class EffectRingForm
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
            label1 = new Label();
            panel1 = new Panel();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            button1.Location = new Point(256, 19);
            button1.Margin = new Padding(5);
            button1.Name = "button1";
            button1.Size = new Size(98, 31);
            button1.TabIndex = 10;
            button1.Text = "Remove";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            textBox1.Location = new Point(68, 23);
            textBox1.Margin = new Padding(5);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(143, 23);
            textBox1.TabIndex = 9;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(18, 25);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(52, 18);
            label1.TabIndex = 8;
            label1.Text = "Search";
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(18, 56);
            panel1.Margin = new Padding(5);
            panel1.Name = "panel1";
            panel1.Size = new Size(352, 775);
            panel1.TabIndex = 7;
            // 
            // EffectRingForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(384, 845);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Point);
            KeyPreview = true;
            Margin = new Padding(5);
            Name = "EffectRingForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EffectRing";
            TopMost = true;
            FormClosing += EffectRingForm_FormClosing;
            Shown += EffectRingForm_Shown;
            KeyDown += EffectRingForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private Panel panel1;
    }
}