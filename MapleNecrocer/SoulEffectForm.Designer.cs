namespace MapleNecrocer
{
    partial class SoulEffectForm
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
            panel1 = new Panel();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(115, 10);
            button1.Name = "button1";
            button1.Size = new Size(79, 28);
            button1.TabIndex = 10;
            button1.Text = "Remove";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(5, 44);
            panel1.Name = "panel1";
            panel1.Size = new Size(348, 443);
            panel1.TabIndex = 7;
            // 
            // SoulEffectForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(359, 496);
            Controls.Add(button1);
            Controls.Add(panel1);
            Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximumSize = new Size(377, 1200);
            Name = "SoulEffectForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SoulEffect";
            TopMost = true;
            Shown += SoulEffectForm_Shown;
            KeyDown += SoulEffectForm_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Panel panel1;
    }
}