namespace MapleNecrocer
{
    partial class EffectForm
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
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(12, 41);
            panel1.Name = "panel1";
            panel1.Size = new Size(354, 507);
            panel1.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(251, 11);
            button1.Name = "button1";
            button1.Size = new Size(103, 24);
            button1.TabIndex = 2;
            button1.Text = "Remove";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // EffectForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(378, 560);
            Controls.Add(button1);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximumSize = new Size(396, 2000);
            Name = "EffectForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Effect";
            TopMost = true;
            Shown += EffectForm_Shown;
            KeyDown += EffectForm_KeyDown;
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Button button1;
    }
}