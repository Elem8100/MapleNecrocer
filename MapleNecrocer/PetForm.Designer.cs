namespace MapleNecrocer
{
    partial class PetForm
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
            panel2 = new Panel();
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            label2 = new Label();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panel1.Location = new Point(12, 46);
            panel1.Name = "panel1";
            panel1.Size = new Size(337, 594);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Location = new Point(368, 46);
            panel2.Name = "panel2";
            panel2.Size = new Size(341, 594);
            panel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(12, 18);
            label1.Name = "label1";
            label1.Size = new Size(52, 18);
            label1.TabIndex = 2;
            label1.Text = "Search";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(73, 18);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(117, 23);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // button1
            // 
            button1.Location = new Point(245, 13);
            button1.Name = "button1";
            button1.Size = new Size(80, 28);
            button1.TabIndex = 4;
            button1.Text = "Remove";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label2.Location = new Point(488, 19);
            label2.Name = "label2";
            label2.Size = new Size(68, 18);
            label2.TabIndex = 5;
            label2.Text = "Pet Equip";
            // 
            // PetForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(721, 652);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximumSize = new Size(739, 1000);
            Name = "PetForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PetForm";
            TopMost = true;
            FormClosing += PetForm_FormClosing;
            Shown += PetForm_Shown;
            KeyDown += PetForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private Label label2;
    }
}