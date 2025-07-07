namespace MapleNecrocer
{
    partial class AvatarForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AvatarForm));
            Head = new Button();
            Body = new Button();
            Weapon_1 = new Button();
            Weapon_2 = new Button();
            Cap_1 = new Button();
            Cap_2 = new Button();
            Coat = new Button();
            Pants = new Button();
            Longcoat = new Button();
            Cape = new Button();
            Shield = new Button();
            Glove = new Button();
            Shoes = new Button();
            Hair_1 = new Button();
            Hair_2 = new Button();
            Face_1 = new Button();
            Face_2 = new Button();
            FaceAcc = new Button();
            Glass = new Button();
            Earring = new Button();
            SaveCharButton = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            label9 = new Label();
            tabPage3 = new TabPage();
            DyeGrid = new DataGridView();
            tabPage7 = new TabPage();
            button22 = new Button();
            LabelLightness = new Label();
            LabelSat = new Label();
            LabelHue = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            LightnessTrackBar = new TrackBar();
            SatTrackBar = new TrackBar();
            HueTrackBar = new TrackBar();
            DyePicture = new PictureBox();
            DyeGrid2 = new DataGridView();
            tabPage4 = new TabPage();
            textBox1 = new TextBox();
            label3 = new Label();
            panel1 = new Panel();
            UseButton = new Button();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            tabPage5 = new TabPage();
            tabPage6 = new TabPage();
            SavePsdButton = new Button();
            checkBox1 = new CheckBox();
            saveSprite_button = new Button();
            saveSpriteSheet_button = new Button();
            saveAllSprite_button = new Button();
            customAABB_checkBox = new CheckBox();
            groupBox1 = new GroupBox();
            ScrollBarH = new HScrollBar();
            label10 = new Label();
            AdjH = new Label();
            Xlabel = new Label();
            AdjX = new Label();
            ScrollBarX = new HScrollBar();
            ScrollBarW = new HScrollBar();
            AdjW = new Label();
            YLabel = new Label();
            AdjY = new Label();
            label13 = new Label();
            ScrollBarY = new HScrollBar();
            panel2 = new Panel();
            FrameListBox = new ListBox();
            label4 = new Label();
            comboBox1 = new ComboBox();
            label5 = new Label();
            EarListBox = new ComboBox();
            timer1 = new System.Windows.Forms.Timer(components);
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DyeGrid).BeginInit();
            tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LightnessTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SatTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)HueTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DyePicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DyeGrid2).BeginInit();
            tabPage4.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabPage6.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // Head
            // 
            Head.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Head.Image = (Image)resources.GetObject("Head.Image");
            Head.ImageAlign = ContentAlignment.MiddleRight;
            Head.Location = new Point(12, 12);
            Head.Name = "Head";
            Head.Size = new Size(103, 41);
            Head.TabIndex = 0;
            Head.Tag = "20";
            Head.Text = "  頭";
            Head.TextAlign = ContentAlignment.MiddleLeft;
            Head.UseVisualStyleBackColor = true;
            Head.Click += button1_Click;
            // 
            // Body
            // 
            Body.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Body.Image = (Image)resources.GetObject("Body.Image");
            Body.ImageAlign = ContentAlignment.MiddleRight;
            Body.Location = new Point(121, 12);
            Body.Name = "Body";
            Body.Size = new Size(103, 41);
            Body.TabIndex = 1;
            Body.Tag = "1";
            Body.Text = "   身體";
            Body.TextAlign = ContentAlignment.MiddleLeft;
            Body.UseVisualStyleBackColor = true;
            Body.Click += button1_Click;
            // 
            // Weapon_1
            // 
            Weapon_1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Weapon_1.Image = (Image)resources.GetObject("Weapon_1.Image");
            Weapon_1.ImageAlign = ContentAlignment.MiddleRight;
            Weapon_1.Location = new Point(448, 106);
            Weapon_1.Name = "Weapon_1";
            Weapon_1.Size = new Size(103, 41);
            Weapon_1.TabIndex = 2;
            Weapon_1.Tag = "2";
            Weapon_1.Text = "  武器-1";
            Weapon_1.TextAlign = ContentAlignment.MiddleLeft;
            Weapon_1.UseVisualStyleBackColor = true;
            Weapon_1.Click += button1_Click;
            // 
            // Weapon_2
            // 
            Weapon_2.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Weapon_2.Image = (Image)resources.GetObject("Weapon_2.Image");
            Weapon_2.ImageAlign = ContentAlignment.MiddleRight;
            Weapon_2.Location = new Point(557, 106);
            Weapon_2.Name = "Weapon_2";
            Weapon_2.Size = new Size(103, 41);
            Weapon_2.TabIndex = 3;
            Weapon_2.Tag = "3";
            Weapon_2.Text = "  武器-2";
            Weapon_2.TextAlign = ContentAlignment.MiddleLeft;
            Weapon_2.UseVisualStyleBackColor = true;
            Weapon_2.Click += button1_Click;
            // 
            // Cap_1
            // 
            Cap_1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Cap_1.Image = (Image)resources.GetObject("Cap_1.Image");
            Cap_1.ImageAlign = ContentAlignment.MiddleRight;
            Cap_1.Location = new Point(12, 59);
            Cap_1.Name = "Cap_1";
            Cap_1.Size = new Size(103, 41);
            Cap_1.TabIndex = 4;
            Cap_1.Tag = "4";
            Cap_1.Text = "  帽子-1";
            Cap_1.TextAlign = ContentAlignment.MiddleLeft;
            Cap_1.UseVisualStyleBackColor = true;
            Cap_1.Click += button1_Click;
            // 
            // Cap_2
            // 
            Cap_2.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Cap_2.Image = (Image)resources.GetObject("Cap_2.Image");
            Cap_2.ImageAlign = ContentAlignment.MiddleRight;
            Cap_2.Location = new Point(121, 59);
            Cap_2.Name = "Cap_2";
            Cap_2.Size = new Size(103, 41);
            Cap_2.TabIndex = 5;
            Cap_2.Tag = "5";
            Cap_2.Text = "  帽子-2";
            Cap_2.TextAlign = ContentAlignment.MiddleLeft;
            Cap_2.UseVisualStyleBackColor = true;
            Cap_2.Click += button1_Click;
            // 
            // Coat
            // 
            Coat.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Coat.Image = (Image)resources.GetObject("Coat.Image");
            Coat.ImageAlign = ContentAlignment.MiddleRight;
            Coat.Location = new Point(230, 59);
            Coat.Name = "Coat";
            Coat.Size = new Size(103, 41);
            Coat.TabIndex = 6;
            Coat.Tag = "6";
            Coat.Text = "  上衣";
            Coat.TextAlign = ContentAlignment.MiddleLeft;
            Coat.UseVisualStyleBackColor = true;
            Coat.Click += button1_Click;
            // 
            // Pants
            // 
            Pants.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Pants.Image = (Image)resources.GetObject("Pants.Image");
            Pants.ImageAlign = ContentAlignment.MiddleRight;
            Pants.Location = new Point(339, 59);
            Pants.Name = "Pants";
            Pants.Size = new Size(103, 41);
            Pants.TabIndex = 7;
            Pants.Tag = "7";
            Pants.Text = "  褲子";
            Pants.TextAlign = ContentAlignment.MiddleLeft;
            Pants.UseVisualStyleBackColor = true;
            Pants.Click += button1_Click;
            // 
            // Longcoat
            // 
            Longcoat.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Longcoat.Image = (Image)resources.GetObject("Longcoat.Image");
            Longcoat.ImageAlign = ContentAlignment.MiddleRight;
            Longcoat.Location = new Point(448, 59);
            Longcoat.Name = "Longcoat";
            Longcoat.Size = new Size(103, 41);
            Longcoat.TabIndex = 8;
            Longcoat.Tag = "8";
            Longcoat.Text = "  套服";
            Longcoat.TextAlign = ContentAlignment.MiddleLeft;
            Longcoat.UseVisualStyleBackColor = true;
            Longcoat.Click += button1_Click;
            // 
            // Cape
            // 
            Cape.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Cape.Image = (Image)resources.GetObject("Cape.Image");
            Cape.ImageAlign = ContentAlignment.MiddleRight;
            Cape.Location = new Point(557, 59);
            Cape.Name = "Cape";
            Cape.Size = new Size(103, 41);
            Cape.TabIndex = 9;
            Cape.Tag = "9";
            Cape.Text = "  披風";
            Cape.TextAlign = ContentAlignment.MiddleLeft;
            Cape.UseVisualStyleBackColor = true;
            Cape.Click += button1_Click;
            // 
            // Shield
            // 
            Shield.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Shield.Image = (Image)resources.GetObject("Shield.Image");
            Shield.ImageAlign = ContentAlignment.MiddleRight;
            Shield.Location = new Point(121, 153);
            Shield.Name = "Shield";
            Shield.Size = new Size(103, 41);
            Shield.TabIndex = 10;
            Shield.Tag = "10";
            Shield.Text = "  盾";
            Shield.TextAlign = ContentAlignment.MiddleLeft;
            Shield.UseVisualStyleBackColor = true;
            Shield.Click += button1_Click;
            // 
            // Glove
            // 
            Glove.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Glove.Image = (Image)resources.GetObject("Glove.Image");
            Glove.ImageAlign = ContentAlignment.MiddleRight;
            Glove.Location = new Point(121, 106);
            Glove.Name = "Glove";
            Glove.Size = new Size(103, 41);
            Glove.TabIndex = 11;
            Glove.Tag = "11";
            Glove.Text = "  手套";
            Glove.TextAlign = ContentAlignment.MiddleLeft;
            Glove.UseVisualStyleBackColor = true;
            Glove.Click += button1_Click;
            // 
            // Shoes
            // 
            Shoes.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Shoes.Image = (Image)resources.GetObject("Shoes.Image");
            Shoes.ImageAlign = ContentAlignment.MiddleRight;
            Shoes.Location = new Point(12, 106);
            Shoes.Name = "Shoes";
            Shoes.Size = new Size(103, 41);
            Shoes.TabIndex = 12;
            Shoes.Tag = "12";
            Shoes.Text = "  鞋子";
            Shoes.TextAlign = ContentAlignment.MiddleLeft;
            Shoes.UseVisualStyleBackColor = true;
            Shoes.Click += button1_Click;
            // 
            // Hair_1
            // 
            Hair_1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Hair_1.Image = (Image)resources.GetObject("Hair_1.Image");
            Hair_1.ImageAlign = ContentAlignment.MiddleRight;
            Hair_1.Location = new Point(230, 12);
            Hair_1.Name = "Hair_1";
            Hair_1.Size = new Size(103, 41);
            Hair_1.TabIndex = 13;
            Hair_1.Tag = "13";
            Hair_1.Text = "髮型-1";
            Hair_1.TextAlign = ContentAlignment.MiddleLeft;
            Hair_1.UseVisualStyleBackColor = true;
            Hair_1.Click += button1_Click;
            // 
            // Hair_2
            // 
            Hair_2.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Hair_2.Image = (Image)resources.GetObject("Hair_2.Image");
            Hair_2.ImageAlign = ContentAlignment.MiddleRight;
            Hair_2.Location = new Point(339, 12);
            Hair_2.Name = "Hair_2";
            Hair_2.Size = new Size(103, 41);
            Hair_2.TabIndex = 14;
            Hair_2.Tag = "14";
            Hair_2.Text = "髮型-2";
            Hair_2.TextAlign = ContentAlignment.MiddleLeft;
            Hair_2.UseVisualStyleBackColor = true;
            Hair_2.Click += button1_Click;
            // 
            // Face_1
            // 
            Face_1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Face_1.Image = (Image)resources.GetObject("Face_1.Image");
            Face_1.ImageAlign = ContentAlignment.MiddleRight;
            Face_1.Location = new Point(448, 12);
            Face_1.Name = "Face_1";
            Face_1.Size = new Size(103, 41);
            Face_1.TabIndex = 15;
            Face_1.Tag = "15";
            Face_1.Text = "  臉型-1";
            Face_1.TextAlign = ContentAlignment.MiddleLeft;
            Face_1.UseVisualStyleBackColor = true;
            Face_1.Click += button1_Click;
            // 
            // Face_2
            // 
            Face_2.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Face_2.Image = (Image)resources.GetObject("Face_2.Image");
            Face_2.ImageAlign = ContentAlignment.MiddleRight;
            Face_2.Location = new Point(557, 12);
            Face_2.Name = "Face_2";
            Face_2.Size = new Size(103, 41);
            Face_2.TabIndex = 16;
            Face_2.Tag = "16";
            Face_2.Text = "  臉型-2";
            Face_2.TextAlign = ContentAlignment.MiddleLeft;
            Face_2.UseVisualStyleBackColor = true;
            Face_2.Click += button1_Click;
            // 
            // FaceAcc
            // 
            FaceAcc.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            FaceAcc.Image = (Image)resources.GetObject("FaceAcc.Image");
            FaceAcc.ImageAlign = ContentAlignment.MiddleRight;
            FaceAcc.Location = new Point(12, 153);
            FaceAcc.Name = "FaceAcc";
            FaceAcc.Size = new Size(103, 41);
            FaceAcc.TabIndex = 17;
            FaceAcc.Tag = "17";
            FaceAcc.Text = "  臉飾";
            FaceAcc.TextAlign = ContentAlignment.MiddleLeft;
            FaceAcc.UseVisualStyleBackColor = true;
            FaceAcc.Click += button1_Click;
            // 
            // Glass
            // 
            Glass.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Glass.Image = (Image)resources.GetObject("Glass.Image");
            Glass.ImageAlign = ContentAlignment.MiddleRight;
            Glass.Location = new Point(230, 106);
            Glass.Name = "Glass";
            Glass.Size = new Size(103, 41);
            Glass.TabIndex = 18;
            Glass.Tag = "18";
            Glass.Text = "  眼鏡";
            Glass.TextAlign = ContentAlignment.MiddleLeft;
            Glass.UseVisualStyleBackColor = true;
            Glass.Click += button1_Click;
            // 
            // Earring
            // 
            Earring.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Earring.Image = (Image)resources.GetObject("Earring.Image");
            Earring.ImageAlign = ContentAlignment.MiddleRight;
            Earring.Location = new Point(339, 106);
            Earring.Name = "Earring";
            Earring.Size = new Size(103, 41);
            Earring.TabIndex = 19;
            Earring.Tag = "19";
            Earring.Text = "  耳環";
            Earring.TextAlign = ContentAlignment.MiddleLeft;
            Earring.UseVisualStyleBackColor = true;
            Earring.Click += button1_Click;
            // 
            // SaveCharButton
            // 
            SaveCharButton.BackColor = SystemColors.ControlLight;
            SaveCharButton.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            SaveCharButton.Location = new Point(285, 159);
            SaveCharButton.Name = "SaveCharButton";
            SaveCharButton.Size = new Size(87, 35);
            SaveCharButton.TabIndex = 20;
            SaveCharButton.Text = "儲存角色";
            SaveCharButton.UseVisualStyleBackColor = false;
            SaveCharButton.Click += SaveCharButton_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage7);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Font = new Font("微軟正黑體", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            tabControl1.Location = new Point(12, 222);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 584);
            tabControl1.TabIndex = 21;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Font = new Font("微軟正黑體", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(792, 551);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "裝備";
            tabPage1.UseVisualStyleBackColor = true;
            tabPage1.MouseLeave += tabPage1_MouseLeave;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(label9);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 551);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "載入角色";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 48F, FontStyle.Regular, GraphicsUnit.Pixel);
            label9.Location = new Point(277, 278);
            label9.Name = "label9";
            label9.Size = new Size(239, 59);
            label9.TabIndex = 0;
            label9.Text = "Loading...";
            label9.Visible = false;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(DyeGrid);
            tabPage3.Location = new Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(792, 551);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "染色1";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // DyeGrid
            // 
            DyeGrid.AllowUserToAddRows = false;
            DyeGrid.AllowUserToResizeColumns = false;
            DyeGrid.AllowUserToResizeRows = false;
            DyeGrid.BackgroundColor = SystemColors.ButtonFace;
            DyeGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            DyeGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DyeGrid.ColumnHeadersVisible = false;
            DyeGrid.Dock = DockStyle.Fill;
            DyeGrid.Location = new Point(3, 3);
            DyeGrid.MultiSelect = false;
            DyeGrid.Name = "DyeGrid";
            DyeGrid.RowHeadersVisible = false;
            DyeGrid.RowHeadersWidth = 40;
            DyeGrid.RowTemplate.Height = 40;
            DyeGrid.ShowCellToolTips = false;
            DyeGrid.Size = new Size(786, 545);
            DyeGrid.TabIndex = 0;
            DyeGrid.CellClick += DyeGrid_CellClick;
            // 
            // tabPage7
            // 
            tabPage7.Controls.Add(button22);
            tabPage7.Controls.Add(LabelLightness);
            tabPage7.Controls.Add(LabelSat);
            tabPage7.Controls.Add(LabelHue);
            tabPage7.Controls.Add(label8);
            tabPage7.Controls.Add(label7);
            tabPage7.Controls.Add(label6);
            tabPage7.Controls.Add(LightnessTrackBar);
            tabPage7.Controls.Add(SatTrackBar);
            tabPage7.Controls.Add(HueTrackBar);
            tabPage7.Controls.Add(DyePicture);
            tabPage7.Controls.Add(DyeGrid2);
            tabPage7.Location = new Point(4, 29);
            tabPage7.Name = "tabPage7";
            tabPage7.Size = new Size(792, 551);
            tabPage7.TabIndex = 6;
            tabPage7.Text = "染色2";
            tabPage7.UseVisualStyleBackColor = true;
            // 
            // button22
            // 
            button22.Location = new Point(295, 526);
            button22.Name = "button22";
            button22.Size = new Size(144, 44);
            button22.TabIndex = 11;
            button22.Text = "OK";
            button22.UseVisualStyleBackColor = true;
            button22.Click += button22_Click;
            // 
            // LabelLightness
            // 
            LabelLightness.AutoSize = true;
            LabelLightness.Font = new Font("Tahoma", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            LabelLightness.Location = new Point(152, 477);
            LabelLightness.Name = "LabelLightness";
            LabelLightness.Size = new Size(18, 19);
            LabelLightness.TabIndex = 10;
            LabelLightness.Text = "0";
            // 
            // LabelSat
            // 
            LabelSat.AutoSize = true;
            LabelSat.Font = new Font("Tahoma", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            LabelSat.Location = new Point(152, 435);
            LabelSat.Name = "LabelSat";
            LabelSat.Size = new Size(18, 19);
            LabelSat.TabIndex = 9;
            LabelSat.Text = "0";
            // 
            // LabelHue
            // 
            LabelHue.AutoSize = true;
            LabelHue.Font = new Font("Tahoma", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            LabelHue.Location = new Point(152, 393);
            LabelHue.Name = "LabelHue";
            LabelHue.Size = new Size(18, 19);
            LabelHue.TabIndex = 8;
            LabelHue.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            label8.Location = new Point(68, 477);
            label8.Name = "label8";
            label8.Size = new Size(86, 19);
            label8.TabIndex = 7;
            label8.Text = "Lightness :";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            label7.Location = new Point(62, 435);
            label7.Name = "label7";
            label7.Size = new Size(92, 19);
            label7.TabIndex = 6;
            label7.Text = "Saturation :";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            label6.Location = new Point(106, 393);
            label6.Name = "label6";
            label6.Size = new Size(48, 19);
            label6.TabIndex = 5;
            label6.Text = "Hue :";
            // 
            // LightnessTrackBar
            // 
            LightnessTrackBar.AutoSize = false;
            LightnessTrackBar.Location = new Point(202, 475);
            LightnessTrackBar.Maximum = 100;
            LightnessTrackBar.Minimum = -100;
            LightnessTrackBar.Name = "LightnessTrackBar";
            LightnessTrackBar.Size = new Size(356, 25);
            LightnessTrackBar.TabIndex = 4;
            LightnessTrackBar.TickStyle = TickStyle.None;
            LightnessTrackBar.Scroll += LightnessTrackBar_Scroll;
            // 
            // SatTrackBar
            // 
            SatTrackBar.AutoSize = false;
            SatTrackBar.LargeChange = 1;
            SatTrackBar.Location = new Point(202, 431);
            SatTrackBar.Maximum = 100;
            SatTrackBar.Minimum = -100;
            SatTrackBar.Name = "SatTrackBar";
            SatTrackBar.Size = new Size(356, 25);
            SatTrackBar.TabIndex = 3;
            SatTrackBar.TickStyle = TickStyle.None;
            SatTrackBar.Scroll += SatTrackBar_Scroll;
            // 
            // HueTrackBar
            // 
            HueTrackBar.AutoSize = false;
            HueTrackBar.LargeChange = 1;
            HueTrackBar.Location = new Point(202, 389);
            HueTrackBar.Maximum = 360;
            HueTrackBar.Name = "HueTrackBar";
            HueTrackBar.Size = new Size(356, 25);
            HueTrackBar.TabIndex = 2;
            HueTrackBar.TickStyle = TickStyle.None;
            HueTrackBar.Scroll += HueTrackBar_Scroll;
            // 
            // DyePicture
            // 
            DyePicture.BorderStyle = BorderStyle.FixedSingle;
            DyePicture.Location = new Point(325, 300);
            DyePicture.Name = "DyePicture";
            DyePicture.Size = new Size(93, 79);
            DyePicture.SizeMode = PictureBoxSizeMode.StretchImage;
            DyePicture.TabIndex = 1;
            DyePicture.TabStop = false;
            // 
            // DyeGrid2
            // 
            DyeGrid2.AllowUserToAddRows = false;
            DyeGrid2.AllowUserToDeleteRows = false;
            DyeGrid2.AllowUserToResizeColumns = false;
            DyeGrid2.AllowUserToResizeRows = false;
            DyeGrid2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DyeGrid2.ColumnHeadersVisible = false;
            DyeGrid2.Location = new Point(202, 24);
            DyeGrid2.Name = "DyeGrid2";
            DyeGrid2.RowHeadersVisible = false;
            DyeGrid2.RowHeadersWidth = 51;
            DyeGrid2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DyeGrid2.Size = new Size(356, 268);
            DyeGrid2.TabIndex = 0;
            DyeGrid2.CellClick += DyeGrid2_CellClick;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(textBox1);
            tabPage4.Controls.Add(label3);
            tabPage4.Controls.Add(panel1);
            tabPage4.Location = new Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(792, 551);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "搜尋";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(184, 76);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(245, 29);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(116, 79);
            label3.Name = "label3";
            label3.Size = new Size(60, 20);
            label3.TabIndex = 1;
            label3.Text = "Search";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(UseButton);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(114, 21);
            panel1.Name = "panel1";
            panel1.Size = new Size(315, 49);
            panel1.TabIndex = 0;
            // 
            // UseButton
            // 
            UseButton.Location = new Point(263, 6);
            UseButton.Name = "UseButton";
            UseButton.Size = new Size(45, 35);
            UseButton.TabIndex = 3;
            UseButton.Text = "Use";
            UseButton.UseVisualStyleBackColor = true;
            UseButton.Click += UseButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(74, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(40, 40);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 16);
            label1.Name = "label1";
            label1.Size = new Size(0, 20);
            label1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(110, 16);
            label2.Name = "label2";
            label2.Size = new Size(0, 20);
            label2.TabIndex = 2;
            // 
            // tabPage5
            // 
            tabPage5.Location = new Point(4, 29);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(792, 551);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Spawn";
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(SavePsdButton);
            tabPage6.Controls.Add(checkBox1);
            tabPage6.Controls.Add(saveSprite_button);
            tabPage6.Controls.Add(saveSpriteSheet_button);
            tabPage6.Controls.Add(saveAllSprite_button);
            tabPage6.Controls.Add(customAABB_checkBox);
            tabPage6.Controls.Add(groupBox1);
            tabPage6.Controls.Add(panel2);
            tabPage6.Controls.Add(FrameListBox);
            tabPage6.Location = new Point(4, 29);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(792, 551);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "匯出";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // SavePsdButton
            // 
            SavePsdButton.Font = new Font("微軟正黑體", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            SavePsdButton.Location = new Point(475, 10);
            SavePsdButton.Name = "SavePsdButton";
            SavePsdButton.Size = new Size(150, 31);
            SavePsdButton.TabIndex = 29;
            SavePsdButton.Text = "儲存psd";
            SavePsdButton.UseVisualStyleBackColor = true;
            SavePsdButton.Click += SavePsdButton_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(634, 16);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(121, 24);
            checkBox1.TabIndex = 28;
            checkBox1.Text = "debug draw";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // saveSprite_button
            // 
            saveSprite_button.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            saveSprite_button.Location = new Point(155, 10);
            saveSprite_button.Name = "saveSprite_button";
            saveSprite_button.Size = new Size(150, 31);
            saveSprite_button.TabIndex = 2;
            saveSprite_button.Text = "Export Current Sprite";
            saveSprite_button.UseVisualStyleBackColor = true;
            saveSprite_button.Click += ExportSprite;
            // 
            // saveSpriteSheet_button
            // 
            saveSpriteSheet_button.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            saveSpriteSheet_button.Location = new Point(686, 63);
            saveSpriteSheet_button.Name = "saveSpriteSheet_button";
            saveSpriteSheet_button.Size = new Size(68, 31);
            saveSpriteSheet_button.TabIndex = 17;
            saveSpriteSheet_button.Text = "Export SpriteSheet";
            saveSpriteSheet_button.UseVisualStyleBackColor = true;
            saveSpriteSheet_button.Visible = false;
            saveSpriteSheet_button.Click += ExportSpriteSheet;
            // 
            // saveAllSprite_button
            // 
            saveAllSprite_button.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            saveAllSprite_button.Location = new Point(311, 10);
            saveAllSprite_button.Name = "saveAllSprite_button";
            saveAllSprite_button.Size = new Size(150, 31);
            saveAllSprite_button.TabIndex = 16;
            saveAllSprite_button.Text = "Export All Sprites";
            saveAllSprite_button.UseVisualStyleBackColor = true;
            saveAllSprite_button.Click += ExportAllSprite;
            // 
            // customAABB_checkBox
            // 
            customAABB_checkBox.AutoSize = true;
            customAABB_checkBox.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            customAABB_checkBox.Location = new Point(165, 50);
            customAABB_checkBox.Name = "customAABB_checkBox";
            customAABB_checkBox.Size = new Size(172, 22);
            customAABB_checkBox.TabIndex = 27;
            customAABB_checkBox.Text = "Custom bounding box";
            customAABB_checkBox.UseVisualStyleBackColor = true;
            customAABB_checkBox.CheckedChanged += customAABB_checkBox_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(ScrollBarH);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(AdjH);
            groupBox1.Controls.Add(Xlabel);
            groupBox1.Controls.Add(AdjX);
            groupBox1.Controls.Add(ScrollBarX);
            groupBox1.Controls.Add(ScrollBarW);
            groupBox1.Controls.Add(AdjW);
            groupBox1.Controls.Add(YLabel);
            groupBox1.Controls.Add(AdjY);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(ScrollBarY);
            groupBox1.Enabled = false;
            groupBox1.Location = new Point(155, 47);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(512, 91);
            groupBox1.TabIndex = 15;
            groupBox1.TabStop = false;
            // 
            // ScrollBarH
            // 
            ScrollBarH.LargeChange = 1;
            ScrollBarH.Location = new Point(290, 60);
            ScrollBarH.Maximum = 512;
            ScrollBarH.Minimum = 32;
            ScrollBarH.Name = "ScrollBarH";
            ScrollBarH.Size = new Size(160, 17);
            ScrollBarH.TabIndex = 13;
            ScrollBarH.Value = 256;
            ScrollBarH.Scroll += hScrollBar1_Scroll;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label10.Location = new Point(240, 60);
            label10.Name = "label10";
            label10.Size = new Size(23, 18);
            label10.TabIndex = 14;
            label10.Text = "H:";
            // 
            // AdjH
            // 
            AdjH.AutoSize = true;
            AdjH.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            AdjH.Location = new Point(260, 60);
            AdjH.Name = "AdjH";
            AdjH.Size = new Size(32, 18);
            AdjH.TabIndex = 12;
            AdjH.Text = "256";
            // 
            // Xlabel
            // 
            Xlabel.AutoSize = true;
            Xlabel.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            Xlabel.Location = new Point(10, 30);
            Xlabel.Name = "Xlabel";
            Xlabel.Size = new Size(22, 18);
            Xlabel.TabIndex = 3;
            Xlabel.Text = "X:";
            // 
            // AdjX
            // 
            AdjX.AutoSize = true;
            AdjX.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            AdjX.Location = new Point(30, 30);
            AdjX.Name = "AdjX";
            AdjX.Size = new Size(32, 18);
            AdjX.TabIndex = 5;
            AdjX.Text = "128";
            // 
            // ScrollBarX
            // 
            ScrollBarX.LargeChange = 1;
            ScrollBarX.Location = new Point(60, 30);
            ScrollBarX.Maximum = 512;
            ScrollBarX.Name = "ScrollBarX";
            ScrollBarX.Size = new Size(160, 18);
            ScrollBarX.TabIndex = 4;
            ScrollBarX.Value = 128;
            ScrollBarX.Scroll += hScrollBar1_Scroll;
            // 
            // ScrollBarW
            // 
            ScrollBarW.LargeChange = 1;
            ScrollBarW.Location = new Point(60, 60);
            ScrollBarW.Maximum = 512;
            ScrollBarW.Minimum = 32;
            ScrollBarW.Name = "ScrollBarW";
            ScrollBarW.ScaleScrollBarForDpiChange = false;
            ScrollBarW.Size = new Size(160, 18);
            ScrollBarW.TabIndex = 10;
            ScrollBarW.Value = 256;
            ScrollBarW.Scroll += hScrollBar1_Scroll;
            // 
            // AdjW
            // 
            AdjW.AutoSize = true;
            AdjW.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            AdjW.Location = new Point(30, 60);
            AdjW.Name = "AdjW";
            AdjW.Size = new Size(32, 18);
            AdjW.TabIndex = 11;
            AdjW.Text = "256";
            // 
            // YLabel
            // 
            YLabel.AutoSize = true;
            YLabel.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            YLabel.Location = new Point(240, 30);
            YLabel.Name = "YLabel";
            YLabel.Size = new Size(23, 18);
            YLabel.TabIndex = 8;
            YLabel.Text = "Y:";
            // 
            // AdjY
            // 
            AdjY.AutoSize = true;
            AdjY.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            AdjY.Location = new Point(260, 30);
            AdjY.Name = "AdjY";
            AdjY.Size = new Size(32, 18);
            AdjY.TabIndex = 6;
            AdjY.Text = "128";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label13.Location = new Point(10, 60);
            label13.Name = "label13";
            label13.Size = new Size(27, 18);
            label13.TabIndex = 9;
            label13.Text = "W:";
            // 
            // ScrollBarY
            // 
            ScrollBarY.LargeChange = 1;
            ScrollBarY.Location = new Point(290, 30);
            ScrollBarY.Maximum = 512;
            ScrollBarY.Name = "ScrollBarY";
            ScrollBarY.Size = new Size(160, 17);
            ScrollBarY.TabIndex = 7;
            ScrollBarY.Value = 128;
            ScrollBarY.Scroll += hScrollBar1_Scroll;
            // 
            // panel2
            // 
            panel2.Location = new Point(155, 144);
            panel2.Name = "panel2";
            panel2.Size = new Size(512, 512);
            panel2.TabIndex = 1;
            // 
            // FrameListBox
            // 
            FrameListBox.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            FrameListBox.FormattingEnabled = true;
            FrameListBox.ItemHeight = 17;
            FrameListBox.Location = new Point(6, 6);
            FrameListBox.Name = "FrameListBox";
            FrameListBox.Size = new Size(143, 531);
            FrameListBox.TabIndex = 0;
            FrameListBox.SelectedIndexChanged += FrameListBox_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label4.Location = new Point(482, 162);
            label4.Name = "label4";
            label4.Size = new Size(38, 18);
            label4.TabIndex = 22;
            label4.Text = "表情";
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "blink", "hit", "smile", "troubled", "cry", "angry", "bewildered", "stunned", "vomit", "oops", "cheers", "chu", "wink", "pain", "glitter", "despair", "love", "shine", "blaze", "hum", "bowing", "hot", "dam", "qBlue" });
            comboBox1.Location = new Point(526, 159);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(134, 25);
            comboBox1.TabIndex = 23;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            label5.Location = new Point(482, 193);
            label5.Name = "label5";
            label5.Size = new Size(38, 18);
            label5.TabIndex = 25;
            label5.Text = "耳朵";
            // 
            // EarListBox
            // 
            EarListBox.Font = new Font("Tahoma", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            EarListBox.FormattingEnabled = true;
            EarListBox.Location = new Point(526, 190);
            EarListBox.Name = "EarListBox";
            EarListBox.Size = new Size(134, 25);
            EarListBox.TabIndex = 26;
            EarListBox.SelectedIndexChanged += EarListBox_SelectedIndexChanged;
            // 
            // timer1
            // 
            timer1.Interval = 10;
            timer1.Tick += timer1_Tick;
            // 
            // AvatarForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(1130, 813);
            Controls.Add(EarListBox);
            Controls.Add(label5);
            Controls.Add(comboBox1);
            Controls.Add(label4);
            Controls.Add(tabControl1);
            Controls.Add(SaveCharButton);
            Controls.Add(Earring);
            Controls.Add(Glass);
            Controls.Add(FaceAcc);
            Controls.Add(Face_2);
            Controls.Add(Face_1);
            Controls.Add(Hair_2);
            Controls.Add(Hair_1);
            Controls.Add(Shoes);
            Controls.Add(Glove);
            Controls.Add(Shield);
            Controls.Add(Cape);
            Controls.Add(Longcoat);
            Controls.Add(Pants);
            Controls.Add(Coat);
            Controls.Add(Cap_2);
            Controls.Add(Cap_1);
            Controls.Add(Weapon_2);
            Controls.Add(Weapon_1);
            Controls.Add(Body);
            Controls.Add(Head);
            Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Regular, GraphicsUnit.Pixel);
            MinimumSize = new Size(1000, 0);
            Name = "AvatarForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Avatar";
            TopMost = true;
            FormClosing += AvatarForm_FormClosing;
            Load += AvatarForm_Load;
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DyeGrid).EndInit();
            tabPage7.ResumeLayout(false);
            tabPage7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LightnessTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)SatTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)HueTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)DyePicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)DyeGrid2).EndInit();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabPage6.ResumeLayout(false);
            tabPage6.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Head;
        private Button Body;
        private Button Weapon_1;
        private Button Weapon_2;
        private Button Cap_1;
        private Button Cap_2;
        private Button Coat;
        private Button Pants;
        private Button Longcoat;
        private Button Cape;
        private Button Shield;
        private Button Glove;
        private Button Shoes;
        private Button Hair_1;
        private Button Hair_2;
        private Button Face_1;
        private Button Face_2;
        private Button FaceAcc;
        private Button Glass;
        private Button Earring;
        private Button SaveCharButton;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private DataGridView DyeGrid;
        private TabPage tabPage4;
        private Panel panel1;
        private Label label2;
        private PictureBox pictureBox1;
        private Label label1;
        private TextBox textBox1;
        private Label label3;
        private Button UseButton;
        private TabPage tabPage5;
        private Label label4;
        public ComboBox comboBox1;
        private Label label5;
        private ComboBox EarListBox;
        private TabPage tabPage6;
        private ListBox FrameListBox;
        private Panel panel2;
        private Label Xlabel;
        private Button saveSprite_button;
        private Label label10;
        private HScrollBar ScrollBarH;
        private Label AdjH;
        private Label AdjW;
        private HScrollBar ScrollBarW;
        private Label label13;
        private Label YLabel;
        private HScrollBar ScrollBarY;
        private Label AdjY;
        private Label AdjX;
        private HScrollBar ScrollBarX;
        private TabPage tabPage7;
        private DataGridView DyeGrid2;
        private PictureBox DyePicture;
        private TrackBar SatTrackBar;
        private TrackBar HueTrackBar;
        private TrackBar LightnessTrackBar;
        private Label LabelLightness;
        private Label LabelSat;
        private Label LabelHue;
        private Label label8;
        private Label label7;
        private Label label6;
        private Button button22;
        private GroupBox groupBox1;
        private Button saveSpriteSheet_button;
        private Button saveAllSprite_button;
        private CheckBox customAABB_checkBox;
        private System.Windows.Forms.Timer timer1;
        private Label label9;
        private CheckBox checkBox1;
        public Button SavePsdButton;
    }
}