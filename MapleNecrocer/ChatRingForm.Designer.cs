namespace MapleNecrocer
{
    partial class ChatRingForm
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
            richTextBox1 = new RichTextBox();
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(14, 147);
            panel1.Margin = new Padding(5);
            panel1.Name = "panel1";
            panel1.Size = new Size(364, 614);
            panel1.TabIndex = 8;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(14, 18);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(364, 86);
            richTextBox1.TabIndex = 9;
            richTextBox1.Text = "MapleStory";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 119);
            label1.Name = "label1";
            label1.Size = new Size(52, 18);
            label1.TabIndex = 10;
            label1.Text = "Search";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(77, 116);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(143, 24);
            textBox1.TabIndex = 11;
            // 
            // button1
            // 
            button1.Location = new Point(276, 115);
            button1.Name = "button1";
            button1.Size = new Size(92, 24);
            button1.TabIndex = 12;
            button1.Text = "Remove";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // ChatRingForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(392, 775);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(richTextBox1);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            Name = "ChatRingForm";
            Text = "ChatRingForm";
            TopMost = true;
            FormClosing += ChatRingForm_FormClosing;
            Shown += ChatRingForm_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private RichTextBox richTextBox1;
        private Label label1;
        private TextBox textBox1;
        private Button button1;
    }
}