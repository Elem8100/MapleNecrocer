namespace MapleNecrocer
{
    partial class OptionForm
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
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(89, 53);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(63, 22);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Mute";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // OptionForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(289, 126);
            Controls.Add(checkBox1);
            Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OptionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Option";
            TopMost = true;
            Shown += OptionForm_Shown;
            KeyDown += OptionForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox checkBox1;
    }
}