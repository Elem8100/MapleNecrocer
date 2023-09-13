namespace MapleNecrocer
{
    partial class SaveMapForm
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
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(122, 136);
            button1.Name = "button1";
            button1.Size = new Size(110, 37);
            button1.TabIndex = 0;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "0", "-50", "-100", "-150", "-200", "-250", "-300", "-350", "-400", "-450", "-500", "-550", "-600", "-650", "-700", "-750", "-800", "-850", "-900", "-950", "-1000", "-1050", "-1100", "-1150", "-1200", "-1250", "-1300", "-1350", "-1400", "-1450", "-1500", "-1550", "-1600", "50", "100", "150", "200" });
            comboBox1.Location = new Point(122, 38);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(110, 25);
            comboBox1.TabIndex = 1;
            comboBox1.Text = "0";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "1", "-1", "-1.5", "-2" });
            comboBox2.Location = new Point(122, 84);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(114, 25);
            comboBox2.TabIndex = 2;
            comboBox2.Text = "1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 41);
            label1.Name = "label1";
            label1.Size = new Size(100, 18);
            label1.TabIndex = 3;
            label1.Text = "Back Y Adjust";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(60, 87);
            label2.Name = "label2";
            label2.Size = new Size(40, 18);
            label2.TabIndex = 4;
            label2.Text = "Ratio";
            // 
            // SaveMapForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(336, 246);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(button1);
            Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SaveMapForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SaveMap";
            TopMost = true;
            Shown += SaveMapForm_Shown;
            KeyDown += SaveMapForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        public ComboBox comboBox1;
        public ComboBox comboBox2;
        private Label label1;
        private Label label2;
    }
}