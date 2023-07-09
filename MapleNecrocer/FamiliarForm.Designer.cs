namespace MapleNecrocer
{
    partial class FamiliarForm
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
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(3, 42);
            panel1.Name = "panel1";
            panel1.Size = new Size(348, 503);
            panel1.TabIndex = 0;
            // 
            // FamiliarForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(359, 559);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximumSize = new Size(377, 1000);
            Name = "FamiliarForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Familiar";
            TopMost = true;
            Shown += FamiliarForm_Shown;
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
    }
}