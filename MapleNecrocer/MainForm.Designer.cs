
namespace MapleNecrocer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            OpenFolderButton = new Button();
            pictureBox1 = new PictureBox();
            SearchMapBox = new TextBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            LoadMapButton = new Button();
            comboBox2 = new ComboBox();
            comboBox1 = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            panel1 = new Panel();
            CashEffectButton = new Button();
            MountButton = new Button();
            NpcButton = new Button();
            ScaleButton = new Button();
            FullScreenButton = new Button();
            ChairButton = new Button();
            MobButton = new Button();
            AvatarButton = new Button();
            SaveMapButton = new Button();
            DisplayButton = new Button();
            label4 = new Label();
            MorphButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabControl1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // OpenFolderButton
            // 
            OpenFolderButton.Font = new Font("Verdana", 7F, FontStyle.Regular, GraphicsUnit.Point);
            OpenFolderButton.Location = new Point(204, 6);
            OpenFolderButton.Name = "OpenFolderButton";
            OpenFolderButton.Size = new Size(41, 23);
            OpenFolderButton.TabIndex = 0;
            OpenFolderButton.Text = "...";
            OpenFolderButton.TextAlign = ContentAlignment.TopCenter;
            OpenFolderButton.UseVisualStyleBackColor = true;
            OpenFolderButton.Click += OpenFolderButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(8, 33);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(236, 137);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // SearchMapBox
            // 
            SearchMapBox.Location = new Point(61, 206);
            SearchMapBox.Name = "SearchMapBox";
            SearchMapBox.Size = new Size(183, 25);
            SearchMapBox.TabIndex = 2;
            SearchMapBox.TextChanged += SerachMapBox_TextChanged;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(8, 241);
            tabControl1.Name = "tabControl1";
            tabControl1.Padding = new Point(6, 1);
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(240, 417);
            tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 25);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(0, 2, 2, 1);
            tabPage1.Size = new Size(232, 388);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(232, 387);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // LoadMapButton
            // 
            LoadMapButton.Enabled = false;
            LoadMapButton.Location = new Point(8, 174);
            LoadMapButton.Name = "LoadMapButton";
            LoadMapButton.Size = new Size(236, 28);
            LoadMapButton.TabIndex = 5;
            LoadMapButton.Text = "Load Map";
            LoadMapButton.UseVisualStyleBackColor = true;
            LoadMapButton.Click += LoadMapButton_Click;
            // 
            // comboBox2
            // 
            comboBox2.Font = new Font("Arial", 13F, FontStyle.Regular, GraphicsUnit.Pixel);
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "800X600", "1024X768", "1280X720", "1280X800", "1280X1024", "1360X768", "1366X768", "1440X900", "1600X900", "1600X1200", "1680X1050", "1920X1080", "1920X1200", "2048X1152", "2048X1536", "2560X1080", "2560X1440", "2560X1600", "3440X1440", "3840X2160" });
            comboBox2.Location = new Point(306, 36);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(90, 24);
            comboBox2.TabIndex = 6;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            comboBox2.Click += comboBox2_Click;
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(306, 7);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(99, 24);
            comboBox1.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label1.Location = new Point(257, 8);
            label1.Name = "label1";
            label1.Size = new Size(43, 17);
            label1.TabIndex = 8;
            label1.Text = "Mode";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label2.Location = new Point(257, 42);
            label2.Name = "label2";
            label2.Size = new Size(37, 17);
            label2.TabIndex = 9;
            label2.Text = "Size";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label3.Location = new Point(8, 210);
            label3.Name = "label3";
            label3.Size = new Size(55, 17);
            label3.TabIndex = 10;
            label3.Text = "Search";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.AutoScroll = true;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(MorphButton);
            panel1.Controls.Add(CashEffectButton);
            panel1.Controls.Add(MountButton);
            panel1.Controls.Add(NpcButton);
            panel1.Controls.Add(ScaleButton);
            panel1.Controls.Add(FullScreenButton);
            panel1.Controls.Add(ChairButton);
            panel1.Controls.Add(MobButton);
            panel1.Controls.Add(AvatarButton);
            panel1.Controls.Add(SaveMapButton);
            panel1.Controls.Add(DisplayButton);
            panel1.Location = new Point(442, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(723, 78);
            panel1.TabIndex = 11;
            // 
            // CashEffectButton
            // 
            CashEffectButton.AutoSize = true;
            CashEffectButton.Enabled = false;
            CashEffectButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            CashEffectButton.Image = (Image)resources.GetObject("CashEffectButton.Image");
            CashEffectButton.ImageAlign = ContentAlignment.TopCenter;
            CashEffectButton.Location = new Point(703, 2);
            CashEffectButton.Name = "CashEffectButton";
            CashEffectButton.RightToLeft = RightToLeft.No;
            CashEffectButton.Size = new Size(84, 52);
            CashEffectButton.TabIndex = 9;
            CashEffectButton.Text = "Cash Effect";
            CashEffectButton.TextAlign = ContentAlignment.BottomCenter;
            CashEffectButton.UseVisualStyleBackColor = true;
            CashEffectButton.Click += MobButton_Click;
            // 
            // MountButton
            // 
            MountButton.Enabled = false;
            MountButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            MountButton.Image = (Image)resources.GetObject("MountButton.Image");
            MountButton.ImageAlign = ContentAlignment.TopCenter;
            MountButton.Location = new Point(625, 1);
            MountButton.Name = "MountButton";
            MountButton.Size = new Size(77, 52);
            MountButton.TabIndex = 8;
            MountButton.Text = "Mount";
            MountButton.TextAlign = ContentAlignment.BottomCenter;
            MountButton.UseVisualStyleBackColor = true;
            MountButton.Click += MobButton_Click;
            // 
            // NpcButton
            // 
            NpcButton.Enabled = false;
            NpcButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            NpcButton.Image = (Image)resources.GetObject("NpcButton.Image");
            NpcButton.ImageAlign = ContentAlignment.TopCenter;
            NpcButton.Location = new Point(547, 1);
            NpcButton.Name = "NpcButton";
            NpcButton.Size = new Size(77, 52);
            NpcButton.TabIndex = 7;
            NpcButton.Text = "Npc";
            NpcButton.TextAlign = ContentAlignment.BottomCenter;
            NpcButton.UseVisualStyleBackColor = true;
            NpcButton.Click += MobButton_Click;
            // 
            // ScaleButton
            // 
            ScaleButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            ScaleButton.Location = new Point(235, 1);
            ScaleButton.Name = "ScaleButton";
            ScaleButton.Size = new Size(77, 52);
            ScaleButton.TabIndex = 6;
            ScaleButton.Text = "Scale";
            ScaleButton.TextAlign = ContentAlignment.BottomCenter;
            ScaleButton.UseVisualStyleBackColor = true;
            // 
            // FullScreenButton
            // 
            FullScreenButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            FullScreenButton.Location = new Point(157, 1);
            FullScreenButton.Name = "FullScreenButton";
            FullScreenButton.Size = new Size(77, 52);
            FullScreenButton.TabIndex = 5;
            FullScreenButton.Text = "Display";
            FullScreenButton.TextAlign = ContentAlignment.BottomCenter;
            FullScreenButton.UseVisualStyleBackColor = true;
            // 
            // ChairButton
            // 
            ChairButton.Enabled = false;
            ChairButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            ChairButton.Image = (Image)resources.GetObject("ChairButton.Image");
            ChairButton.ImageAlign = ContentAlignment.TopCenter;
            ChairButton.Location = new Point(391, 1);
            ChairButton.Name = "ChairButton";
            ChairButton.Size = new Size(77, 52);
            ChairButton.TabIndex = 4;
            ChairButton.Text = "Chair";
            ChairButton.TextAlign = ContentAlignment.BottomCenter;
            ChairButton.UseVisualStyleBackColor = true;
            ChairButton.Click += MobButton_Click;
            // 
            // MobButton
            // 
            MobButton.Enabled = false;
            MobButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            MobButton.Image = (Image)resources.GetObject("MobButton.Image");
            MobButton.ImageAlign = ContentAlignment.TopCenter;
            MobButton.Location = new Point(469, 1);
            MobButton.Name = "MobButton";
            MobButton.Size = new Size(77, 52);
            MobButton.TabIndex = 3;
            MobButton.Text = "Mob";
            MobButton.TextAlign = ContentAlignment.BottomCenter;
            MobButton.UseVisualStyleBackColor = true;
            MobButton.Click += MobButton_Click;
            // 
            // AvatarButton
            // 
            AvatarButton.Enabled = false;
            AvatarButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            AvatarButton.Image = Properties.Resources._01050088_img_info_iconRaw;
            AvatarButton.ImageAlign = ContentAlignment.TopCenter;
            AvatarButton.Location = new Point(313, 1);
            AvatarButton.Name = "AvatarButton";
            AvatarButton.Size = new Size(77, 52);
            AvatarButton.TabIndex = 2;
            AvatarButton.Text = "Avatar";
            AvatarButton.TextAlign = ContentAlignment.BottomCenter;
            AvatarButton.UseVisualStyleBackColor = true;
            AvatarButton.Click += MobButton_Click;
            // 
            // SaveMapButton
            // 
            SaveMapButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            SaveMapButton.Location = new Point(79, 1);
            SaveMapButton.Name = "SaveMapButton";
            SaveMapButton.Size = new Size(77, 52);
            SaveMapButton.TabIndex = 1;
            SaveMapButton.Text = "Save Map";
            SaveMapButton.TextAlign = ContentAlignment.BottomCenter;
            SaveMapButton.UseVisualStyleBackColor = true;
            // 
            // DisplayButton
            // 
            DisplayButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            DisplayButton.Location = new Point(1, 1);
            DisplayButton.Name = "DisplayButton";
            DisplayButton.Size = new Size(77, 54);
            DisplayButton.TabIndex = 0;
            DisplayButton.Text = "Display";
            DisplayButton.TextAlign = ContentAlignment.BottomCenter;
            DisplayButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label4.Location = new Point(74, 9);
            label4.Name = "label4";
            label4.Size = new Size(124, 18);
            label4.TabIndex = 12;
            label4.Text = "MapleStory Folder";
            // 
            // MorphButton
            // 
            MorphButton.AutoSize = true;
            MorphButton.Enabled = false;
            MorphButton.Font = new Font("Microsoft JhengHei UI", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            MorphButton.Image = (Image)resources.GetObject("MorphButton.Image");
            MorphButton.ImageAlign = ContentAlignment.TopCenter;
            MorphButton.Location = new Point(788, 2);
            MorphButton.Name = "MorphButton";
            MorphButton.RightToLeft = RightToLeft.No;
            MorphButton.Size = new Size(74, 52);
            MorphButton.TabIndex = 10;
            MorphButton.Text = "Morph";
            MorphButton.TextAlign = ContentAlignment.BottomCenter;
            MorphButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1175, 664);
            Controls.Add(label4);
            Controls.Add(panel1);
            Controls.Add(SearchMapBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBox1);
            Controls.Add(comboBox2);
            Controls.Add(LoadMapButton);
            Controls.Add(tabControl1);
            Controls.Add(pictureBox1);
            Controls.Add(OpenFolderButton);
            DoubleBuffered = true;
            Font = new Font("Microsoft JhengHei UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            KeyPreview = true;
            MaximumSize = new Size(5000, 5000);
            Name = "MainForm";
            Text = "Form1";
            Load += MainForm_Load;
            KeyDown += MainForm_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabControl1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button OpenFolderButton;
        private PictureBox pictureBox1;
        private TextBox SearchMapBox;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button LoadMapButton;
        private ComboBox comboBox2;
        private ComboBox comboBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Panel panel1;
        private Button DisplayButton;
        private Button ScaleButton;
        private Button FullScreenButton;
        private Button ChairButton;
        private Button MobButton;
        private Button AvatarButton;
        private Button SaveMapButton;
        private Button NpcButton;
        private Label label4;
        private Button MountButton;
        private Button CashEffectButton;
        private Button MorphButton;
    }
}