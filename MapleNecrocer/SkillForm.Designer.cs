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
            // SkillForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(871, 640);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            Name = "SkillForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SkillForm";
            TopMost = true;
            Shown += SkillForm_Shown;
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
    }
}