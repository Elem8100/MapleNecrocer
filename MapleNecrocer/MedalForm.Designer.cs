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
            SuspendLayout();
            // 
            // MedalForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(342, 612);
            Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            MaximumSize = new Size(360, 900);
            Name = "MedalForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MedalForm";
            TopMost = true;
            Shown += MedalForm_Shown;
            ResumeLayout(false);
        }

        #endregion
    }
}