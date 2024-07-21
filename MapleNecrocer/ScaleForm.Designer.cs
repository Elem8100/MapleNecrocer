namespace MapleNecrocer
{
    partial class ScaleForm
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
            comboBox1 = new ComboBox();
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // comboBox1
            // 
            comboBox1.DropDownHeight = 300;
            comboBox1.FormattingEnabled = true;
            comboBox1.IntegralHeight = false;
            comboBox1.Items.AddRange(new object[] { "800X600 -> 1024X768", "800X600 -> 1366X768", "800X600 -> 1600X900", "800X600 -> 1920X1080", "800X600 -> 2560X1440", "800X600 -> 3200X1800", "800X600 -> 3840X2160", "1024X768 -> 1024X768", "1024X768 -> 1366X768", "1024X768 -> 1600X900", "1024X768 -> 1920X1080", "1024X768 -> 2560X1440", "1024X768 -> 3200X1800", "1024X768 -> 3840X2160", "1280X720 -> 1600X900", "1280X720 -> 1920X1080", "1280X720 -> 2560X1440", "1280X720 -> 3200X1800", "1280X720 -> 3840X2160", "1366X768 -> 1600X900", "1366X768 -> 1920X1080", "1366X768 -> 2560X1440", "1366X768 -> 3200X1800", "1366X768 -> 3840X2160", "1600X900 -> 1920X1080", "1600X900 -> 2560X1440", "1600X900 -> 3200X1800", "1600X900 -> 3840X2160", "1920X1080 -> 2560X1440", "1920X1080 -> 3200X1800", "1920X1080 -> 3840X2160", "2560X1440 -> 3200X1800", "2560X1440 -> 3840X2160" });
            comboBox1.Location = new Point(65, 90);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(248, 27);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(129, 37);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(89, 23);
            checkBox1.TabIndex = 1;
            checkBox1.Text = "Scanline";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // ScaleForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(376, 184);
            Controls.Add(checkBox1);
            Controls.Add(comboBox1);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ScaleForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ScaleForm";
            TopMost = true;
            Load += ScaleForm_Load;
            KeyDown += ScaleForm_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox1;
        private CheckBox checkBox1;
    }
}