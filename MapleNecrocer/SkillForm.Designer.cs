namespace MapleNecrocer
{
    partial class SkillForm
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
            comboBox1 = new ComboBox();
            panel2 = new Panel();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Location = new Point(425, 35);
            panel1.MaximumSize = new Size(434, 1000);
            panel1.Name = "panel1";
            panel1.Size = new Size(434, 593);
            panel1.TabIndex = 0;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Tahoma", 15F, FontStyle.Regular, GraphicsUnit.Pixel);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" });
            comboBox1.Location = new Point(395, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(67, 26);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // panel2
            // 
            panel2.Location = new Point(12, 35);
            panel2.Name = "panel2";
            panel2.Size = new Size(377, 593);
            panel2.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(570, 11);
            label1.Name = "label1";
            label1.Size = new Size(119, 19);
            label1.TabIndex = 3;
            label1.Text = "Hotkey Settings";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            label2.Location = new Point(157, 9);
            label2.Name = "label2";
            label2.Size = new Size(64, 19);
            label2.TabIndex = 4;
            label2.Text = "Use List";
            // 
            // SkillForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(871, 640);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(panel2);
            Controls.Add(comboBox1);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            Name = "SkillForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Skill";
            TopMost = true;
            Shown += SkillForm_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private ComboBox comboBox1;
        private Panel panel2;
        private Label label1;
        private Label label2;
    }
}