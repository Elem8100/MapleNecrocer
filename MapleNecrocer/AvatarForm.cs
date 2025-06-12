using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using Manina.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using WzComparerR2.CharaSim;
using WzComparerR2.CharaSimControl;
using WzComparerR2.Common;
using WzComparerR2.PluginBase;
using WzComparerR2.Text;
using WzComparerR2.WzLib;
using static Manina.Windows.Forms.Utility;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Imaging;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System.Diagnostics;
using System.Reflection;

namespace MapleNecrocer;

public partial class AvatarForm : Form
{
    public AvatarForm()
    {
        InitializeComponent();
        AvatarFormDraw = new();
        AvatarFormDraw.Width = 260;
        AvatarFormDraw.Height = 200;
        AvatarFormDraw.Left = 453;
        AvatarFormDraw.Top = 9;
        AvatarFormDraw.Parent = this;

        FrameListDraw = new();
        FrameListDraw.Width = 512;
        FrameListDraw.Height = 512;
        FrameListDraw.Left = 0;
        FrameListDraw.Top = 0;
        FrameListDraw.Parent = panel2;
        Instance = this;
    }
    private string[] AllFrames = {"walk1.0", "walk1.1", "walk1.2", "walk1.3", "walk2.0", "walk2.1", "walk2.2", "walk2.3",
        "stand1.0", "stand1.1", "stand1.2", "stand2.0", "stand2.1", "stand2.2", "alert.0", "alert.1", "alert.2",
        "swingO1.0", "swingO1.1", "swingO1.2", "swingO2.0", "swingO2.1", "swingO2.2", "swingO3.0", "swingO3.1", "swingO3.2",
        "swingOF.0", "swingOF.1", "swingOF.2", "swingOF.3",
        "swingT1.0", "swingT1.1", "swingT1.2", "swingT2.0", "swingT2.1", "swingT2.2", "swingT3.0", "swingT3.1", "swingT3.2",
        "swingTF.0", "swingTF.1", "swingTF.2", "swingTF.3",
        "swingP1.0", "swingP1.1", "swingP1.2", "swingP2.0", "swingP2.1", "swingP2.2",
        "swingPF.0", "swingPF.1", "swingPF.2", "swingPF.3",
        "stabO1.0", "stabO1.1", "stabO2.0", "stabO2.1", "stabOF.0", "stabOF.1", "stabOF.2",
        "stabT1.0", "stabT1.1", "stabT1.2", "stabT2.0", "stabT2.1", "stabT2.2", "stabTF.0", "stabTF.1", "stabTF.2", "stabTF.3",
        "shoot1.0", "shoot1.1", "shoot1.2", "shoot2.0", "shoot2.1", "shoot2.2", "shoot2.3", "shoot2.4", "shootF.0", "shootF.1", "shootF.2",
        "proneStab.0", "proneStab.1", "prone.0", "heal.0", "heal.1", "heal.2", "fly.0", "fly.1", "jump.0", "sit.0", "ladder.0", "ladder.1", "rope.0", "rope.1", "rope.1" };
    private static bool LoadedFrameList;
    public static bool SelectedFrame;
    public static int SelectedFrameNum;
    public static string SelectedAction;
    public static int AdjustX = 45, AdjustY = 18, AdjustW = 200, AdjustH = 200;
    public static bool ChangeExpressionListBox;
    public static AvatarForm Instance;
    public static AvatarFormDraw AvatarFormDraw;
    public static FrameListDraw FrameListDraw;
    static bool ShowToolTip = true;
    ImageListView[] ImageGrids = new ImageListView[21];
    ImageListView AvatarListView;
    public DataGridViewEx Inventory;
    public DataGridViewEx SearchGrid;
    void AddInventory()
    {
        if (Inventory.Columns.Count == 4)
        {
            Inventory.Columns.RemoveAt(3);
        }
        DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();
        dgvButton.Width = 29;
        dgvButton.UseColumnTextForButtonValue = true;
        dgvButton.Text = "X";
        dgvButton.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        dgvButton.CellTemplate.Style.Padding = new Padding(2, 10, 2, 10);
        Inventory.Columns.Add(dgvButton);

        Inventory.Rows.Clear();

        Player.EqpList.Sort();
        for (int i = 0; i < Player.EqpList.Count; i++)
        {
            var ID = Player.EqpList[i];

            string Dir = Equip.GetDir(ID);
            string Name = "";
            if (Wz.HasNode("String/Eqp.img"))
                Name = Wz.GetNodeA("String/Eqp.img/Eqp").GetStr(Dir + ID.IntID() + "/name");
            else if (Wz.HasNode("String/Item.img/Eqp"))
                Name = Wz.GetNodeA("String/Item.img/Eqp").GetStr(Dir + ID.IntID() + "/name");
            var Entry = Wz.GetNodeA("Character/" + Dir + ID + ".img");
            PartName PartName = Equip.GetPart(ID);

            Bitmap Bmp;
            switch (PartName)
            {
                case PartName.Head:
                    Bmp = Entry.GetBmp("front/head");
                    break;
                case PartName.Body:
                    Bmp = Entry.GetBmp("stand1/0/body");
                    break;
                case PartName.Hair:
                    Bmp = Entry.GetBmp("default/hairOverHead");
                    break;
                case PartName.Face:
                    Bmp = Entry.GetBmp("default/face");
                    break;
                default:
                    Bmp = Entry.GetBmp("info/icon");
                    break;
            }
            Inventory.Rows.Add(ID, Bmp, Name);
        }

        for (int i = 0; i < Inventory.Rows.Count; i++)
        {
            Inventory.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Inventory.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }
        if (Inventory.Columns.Count > 2)
        {
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.Padding = new Padding(0, 0, 1000, 0);
            Inventory.Rows[0].Cells[3].Style = dataGridViewCellStyle2;
            Inventory.Rows[1].Cells[3].Style = dataGridViewCellStyle2;
        }
    }

    void AddEqps(string EqpID)
    {
        string NewID = EqpID;
        PartName NewPart = Equip.GetPart(EqpID);
        if (NewPart == PartName.SitTamingMob)
            return;

        for (int i = Player.EqpList.Count - 1; i >= 0; i--)
        {
            var OldPart = Equip.GetPart(Player.EqpList[i]);

            if (NewPart == OldPart)
            {
                SetEffect.Remove(Player.EqpList[i]);
                ItemEffect.Remove(Player.EqpList[i]);
                Player.EqpList.RemoveAt(i);
            }
            if ((NewPart == PartName.Weapon) && (OldPart == PartName.CashWeapon))
                Player.EqpList.RemoveAt(i);
            if ((NewPart == PartName.CashWeapon) && (OldPart == PartName.Weapon))
                Player.EqpList.RemoveAt(i);
            if ((NewPart == PartName.Coat) || (NewPart == PartName.Pants))
            {
                if (OldPart == PartName.Longcoat)
                    Player.EqpList.RemoveAt(i);
            }
            if (NewPart == PartName.Longcoat)
            {
                if ((OldPart == PartName.Coat) || (OldPart == PartName.Pants))
                    Player.EqpList.RemoveAt(i);
            }
        }

        Player.EqpList.Add(NewID);

        if (ItemEffect.AllList.Contains(EqpID))
            ItemEffect.Create(EqpID, EffectType.Equip);

        if (SetEffect.AllList.ContainsKey(EqpID))
            SetEffect.Create(EqpID);
    }
    void ResetDyeGrid()
    {
        DyeGrid.Rows.Clear();
        DyeGrid.Columns.Clear();
        for (int i = 0; i <= 16; i++)
        {
            var Icon = new DataGridViewImageColumn();
            DyeGrid.Columns.AddRange(Icon);
        }

        for (int i = 0; i < Inventory.RowCount; i++)
        {
            if (Inventory.Rows[i].Cells[1].Value is Bitmap)
            {
                var Bmp = (Bitmap)Inventory.Rows[i].Cells[1].Value;
                DyeGrid.Rows.Add(Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp, Bmp);
            }
        }

        for (int Col = 0; Col <= 16; Col++)
        {
            for (int Row = 0; Row < DyeGrid.RowCount; Row++)
            {
                var Bmp = (Bitmap)DyeGrid.Rows[Row].Cells[Col].Value;
                if (Col <= 15)
                    ImageFilter.HSL(ref Bmp, Col * 25);
                else
                    ImageFilter.HSL(ref Bmp, 0, -200);
                DyeGrid.Rows[Row].Cells[Col].Value = Bmp;
            }
            DyeGrid.Columns[Col].Width = 45;
        }
    }

    void ResetDyeGrid2()
    {
        DyeGrid2.Rows.Clear();
        DyeGrid2.Columns.Clear();
        if (DyeGrid2.Columns.Count == 0)
        {
            foreach (DataGridViewColumn dgvc in Inventory.Columns)
            {
                DyeGrid2.Columns.Add(dgvc.Clone() as DataGridViewColumn);
            }
        }

        DataGridViewRow row = new DataGridViewRow();

        for (int i = 0; i < Inventory.Rows.Count; i++)
        {
            row = (DataGridViewRow)Inventory.Rows[i].Clone();
            int intColIndex = 0;
            foreach (DataGridViewCell cell in Inventory.Rows[i].Cells)
            {
                row.Cells[intColIndex].Value = cell.Value;
                intColIndex++;
            }
            DyeGrid2.Rows.Add(row);
        }

        DyeGrid2.AllowUserToAddRows = false;
        DyeGrid2.Refresh();
        DyeGrid2.Columns[3].Visible = false;
    }

    private void AvatarForm_Load(object sender, EventArgs e)
    {

        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        for (int i = 1; i <= 20; i++)
        {
            ImageGrids[i] = new ImageListView();
            ImageGrids[i].Parent = tabControl1.TabPages[0];
            ImageGrids[i].Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            ImageGrids[i].Dock = DockStyle.Fill;
            ImageGrids[i].BackColor = SystemColors.Window;
            ImageGrids[i].Colors.BackColor = SystemColors.ButtonFace;
            ImageGrids[i].Colors.SelectedBorderColor = Color.Red;

            ImageGrids[i].BorderStyle = BorderStyle.Fixed3D;
            ImageGrids[i].ThumbnailSize = new System.Drawing.Size(32, 32);
            ImageGrids[i].ItemClick += (o, e) =>
            {
                AddEqps(e.Item.FileName);
                AddInventory();
                Game.Player.RemoveSprites();
                for (int i = 0; i < Player.EqpList.Count; i++)
                {
                    Game.Player.Spawn(Player.EqpList[i]);
                }
                ResetDye2();
            };
            ImageGrids[i].ItemHover += (o, e) =>
            {
                if (ShowToolTip)
                {
                    if (e.Item == null) return;
                    Wz_Node Node = Wz.GetNodeByID(e.Item.FileName, WzType.Character);
                    MainForm.Instance.QuickView(Node);
                    MainForm.Instance.ToolTipView.Owner = this;

                }
            };

        }

        AvatarListView = new ImageListView();
        AvatarListView.Parent = tabControl1.TabPages[1];
        AvatarListView.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
        AvatarListView.Dock = DockStyle.Fill;
        AvatarListView.BackColor = SystemColors.Window;
        AvatarListView.Colors.BackColor = SystemColors.ButtonFace;
        AvatarListView.Colors.SelectedBorderColor = Color.Red;
        AvatarListView.Colors.HoverColor1 = SystemColors.ButtonFace;
        AvatarListView.Colors.HoverColor2 = SystemColors.ButtonFace;
        AvatarListView.BorderStyle = BorderStyle.Fixed3D;
        AvatarListView.ThumbnailSize = new System.Drawing.Size(100, 100);
        AvatarListView.ItemClick += (o, e) =>
        {
            ResetDye2();
            switch (tabControl1.SelectedIndex)
            {
                case 1:
                    foreach (var Iter in ItemEffect.UseList.Keys)
                        ItemEffect.UseList[Iter].Dead();
                    ItemEffect.UseList.Clear();

                    foreach (var Iter in SetEffect.UseList.Keys)
                        SetEffect.UseList[Iter].Dead();
                    SetEffect.UseList.Clear();

                    Player.EqpList.Clear();
                    Game.Player.ShowHair = false;
                    Game.Player.DressCap = false;
                    Game.Player.RemoveSprites();

                    string[] Split = e.Item.FileName.Split("-");
                    var EqpList = Split.ToList();
                    EqpList.Sort();
                    for (int i = 1; i < EqpList.Count; i++)
                    {
                        AddEqps(EqpList[i]);
                        Game.Player.Spawn(EqpList[i]);
                    }
                    AddInventory();
                    break;
                case 5:
                    string IDs = e.Item.FileName;
                    PlayerEx.Spawn(IDs);
                    break;
            }
        };

        Inventory = new(75, 174, 795, 8, 300, 880, true, this);
        Inventory.RowTemplate.Height = 45;
        Inventory.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        Inventory.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        Inventory.DefaultCellStyle.Font = new Font("Tahoma", 15, GraphicsUnit.Pixel);
        Inventory.Anchor = (AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
        Inventory.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
        Inventory.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
        Inventory.CellContentClick += (s, e) =>
        {
            if (e.ColumnIndex != 3)
                return;
            int Row = e.RowIndex;
            var Part = Equip.GetPart(Player.EqpList[Row]);
            string DeleteID = Player.EqpList[Row];
            Player.EqpList.RemoveAt(Row);

            if (ItemEffect.AllList.Contains(DeleteID))
                ItemEffect.Remove(DeleteID);
            if (SetEffect.AllList.ContainsKey(DeleteID))
                SetEffect.Remove(DeleteID);

            var ID = Inventory.Rows[e.RowIndex].Cells[0].Value.ToString();
            Inventory.Rows.RemoveAt(e.RowIndex);
            switch (Part)
            {
                case PartName.Hair:
                    Game.Player.ShowHair = false;
                    break;
                case PartName.Cap:
                    Game.Player.DressCap = false;
                    break;
            }
            Player.EqpList.Sort();
            Game.Player.RemoveSprites();
            for (int i = 0; i < Player.EqpList.Count; i++)
            {
                Game.Player.Spawn(Player.EqpList[i]);
            }
            if (tabControl1.SelectedIndex == 2)
                ResetDyeGrid();
            if (tabControl1.SelectedIndex == 3)
                ResetDyeGrid2();
            ResetDye2();
        };

        Inventory.CellMouseEnter += (s, e) =>
        {
            if (ShowToolTip)
            {
                Inventory.Refresh();
                string _ID = Inventory.Rows[e.RowIndex].Cells[0].Value.ToString();
                Wz_Node Node = Wz.GetNodeByID(_ID, WzType.Character);
                MainForm.Instance.QuickView(Node);
                MainForm.Instance.ToolTipView.Owner = this;
            }
        };
        // Inventory.SetToolTipEvent(WzType.Character, this);
        AddEqps("00002000");
        AddInventory();
        ResetDyeGrid();
        ResetDyeGrid2();
        foreach (var Iter in Wz.GetNodes("Character/00012000.img/front"))
        {
            if (Iter.Text != "head")
            {
                EarListBox.Items.Add(Iter.Text);
            }
        }

        if (EarListBox.Items.Count == 0)
        {
            EarListBox.Visible = false;
            label5.Visible = false;
        }

    }


    List<string> PartList = new();
    private void button1_Click(object sender, EventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
        tabControl1.SelectedIndex = 0;
        string CharacterDir = "";
        string ButtonText = ((System.Windows.Forms.Button)sender).Text.Trim(' ');

        switch (ButtonText)
        {
            case "Head":
            case "Body":
                CharacterDir = "";
                break;
            case "Weapon-1":
            case "Weapon-2":
                CharacterDir = "Weapon";
                break;
            case "Cap-1":
            case "Cap-2":
                CharacterDir = "Cap";
                break;
            case "Hair-1":
            case "Hair-2":
                CharacterDir = "Hair";
                break;
            case "Face-1":
            case "Face-2":
                CharacterDir = "Face";
                break;
            case "FaceAcc":
            case "Glass":
            case "Earring":
                CharacterDir = "Accessory";
                break;
            default:
                CharacterDir = ButtonText;
                break;
        }
        int PartIndex = ((System.Windows.Forms.Button)sender).Tag.ToString().ToInt();
        for (int i = 1; i <= 20; i++)
            ImageGrids[i].Visible = false;
        ImageGrids[PartIndex].Visible = true;

        if (!PartList.Contains(ButtonText))
        {
            var Graphic = ImageGrids[PartIndex].CreateGraphics();
            var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
            Graphic.DrawString("Loading...", Font, Brushes.Black, 150, 150);

            Wz_Node.WzNodeCollection Dir = null;
            string Path = "";
            if (CharacterDir == "")
            {
                Dir = MainForm.TreeNode.Nodes["Character"].Nodes;
                Path = "Character/";
            }
            else
            {
                Dir = MainForm.TreeNode.Nodes["Character"].Nodes[CharacterDir].Nodes;
                Path = "Character/" + CharacterDir + "/";
            }

            int Num = 0;
            bool InRange(int Low, int High) => (Num >= Low) && (Num <= High);

            Win32.SendMessage(ImageGrids[PartIndex].Handle, false);
            foreach (var img in Dir)
            {
                if (!Char.IsNumber(img.Text[0]))
                    continue;
                switch (ButtonText)
                {
                    case "Head":
                        if (img.Text.LeftStr(4) == "0000")
                            continue;
                        break;
                    case "Body":
                        if (img.Text.LeftStr(4) == "0001")
                            continue;
                        break;
                }

                foreach (var Iter in Wz.GetNodeA(Path + img.Text).Nodes)
                {
                    string Left4() => Iter.ImgName().LeftStr(4);
                    Num = Iter.ImgID().ToInt() / 1000;
                    switch (ButtonText)
                    {
                        case "Weapon-1":
                            if (Left4() == "0170")
                                continue;
                            break;
                        case "Weapon-2":
                            if (Left4() != "0170")
                                continue;
                            break;
                        case "Cap-1":
                            if (!InRange(1000, 1003))
                                continue;
                            break;
                        case "Cap-2":
                            if (!InRange(1004, 1006))
                                continue;
                            break;
                        case "Hair-1":
                            if (!InRange(30, 56))
                                continue;
                            break;
                        case "Hair-2":
                            if (!InRange(57, 85))
                                continue;
                            break;
                        case "Face-1":
                            if (!InRange(20, 23))
                                continue;
                            break;
                        case "Face-2":
                            if (!InRange(24, 65))
                                continue;
                            break;
                        case "FaceAcc":
                            if (Left4() != "0101")
                                continue;
                            break;
                        case "Glass":
                            if (Left4() != "0102")
                                continue;
                            break;
                        case "Earring":
                            if (Left4() != "0103")
                                continue;
                            break;
                    }

                    switch (ButtonText)
                    {
                        case "Head":
                            if (Iter.Text == "front")
                                ImageGrids[PartIndex].Items.Add(img.ImgID(), Iter.GetBmp("head"));
                            break;
                        case "Body":
                            if (Iter.Text == "stand1")
                                ImageGrids[PartIndex].Items.Add(img.ImgID(), Iter.GetBmp("0/body"));
                            break;
                        case "Face-1":
                        case "Face-2":
                            if (Iter.Nodes["face"] != null)
                                ImageGrids[PartIndex].Items.Add(Iter.ImgID(), Iter.GetBmp("face"));
                            break;
                        case "Hair-1":
                        case "Hair-2":
                            if (Iter.Nodes["hairOverHead"] != null)
                                ImageGrids[PartIndex].Items.Add(Iter.ImgID(), Iter.GetBmp("hairOverHead"));
                            break;
                        default:
                            if (Iter.Nodes["icon"] != null)
                                ImageGrids[PartIndex].Items.Add(Iter.ImgID(), Iter.GetBmp("icon"));
                            break;
                    }
                }

            }
            Win32.SendMessage(ImageGrids[PartIndex].Handle, true);
            ImageGrids[PartIndex].Refresh();
            PartList.Add(ButtonText);
        }

    }

    private void SaveCharButton_Click(object sender, EventArgs e)
    {
        RenderTarget2D SaveTexture = null;
        int WX = (int)(Game.Player.X - EngineFunc.SpriteEngine.Camera.X - 55);
        int WY = (int)(Game.Player.Y - EngineFunc.SpriteEngine.Camera.Y - 90);
        EngineFunc.Canvas.DrawTarget(ref SaveTexture, 100, 100, () =>
        {
            EngineFunc.Canvas.DrawCropArea(AvatarFormDraw.AvatarPanelTexture, 0, 0, new Microsoft.Xna.Framework.Rectangle(WX, WY, WX + 100, WY + 100), 0, 0, 1, 1, 0, false, false, 255, 255, 255, 255, false);
        });

        string PngName = "";
        for (int i = 0; i < Player.EqpList.Count; i++)
            PngName = PngName + Player.EqpList[i] + "-";

        Stream stream = File.OpenWrite(System.Environment.CurrentDirectory + "\\Images\\" + PngName + ".png");
        SaveTexture.SaveAsPng(stream, 100, 100);
        stream.Dispose();
        SaveTexture.Dispose();

        Bitmap Png = new Bitmap(System.Environment.CurrentDirectory + "\\Images\\" + PngName + ".png");
        AvatarListView.Items.Add(PngName, Png);
        Png.Dispose();
    }

    static bool Loaded;
    static bool SearchGridLoaded;

    void DumpEqpString(Wz_Node Node)
    {
        if (Node.Text == "name")
        {
            if (Node.ParentNode.Text.Length == 5)
                SearchGrid.Rows.Add("000" + Node.ParentNode.Text, " " + Node.ToStr());
            else
                SearchGrid.Rows.Add("0" + Node.ParentNode.Text, " " + Node.ToStr());
        }
        foreach (var Iter in Node.Nodes)
        {
            if ((Node.Text != "Android") && (Node.Text != "ArcaneForce") && (Node.Text != "Bits") && (Node.Text != "Dragon") &&
             (Node.Text != "Mechanic") && (Node.Text != "PetEquip") && (Node.Text != "Skillskin") && (Node.Text != "Taming") &&
             (Node.Text != "MonsterBattle") && (Node.Text.LeftStr(3) != "135") && (Node.Text.LeftStr(3) != "150") &&
             (Node.Text.LeftStr(3) != "151") && (Node.Text.LeftStr(3) != "160") && (Node.Text.LeftStr(3) != "169") &&
             (Node.Text.LeftStr(3) != "111") && (Node.Text.LeftStr(3) != "112") && (Node.Text.LeftStr(3) != "114"))
                DumpEqpString(Iter);
        }
    }

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        label1.Text = ID;
        label2.Text = DataGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
        string Dir = Equip.GetDir(ID);
        string Name = "";
        // if (Wz.IsDataWz)
        //   Name = Wz.GetStr("String/Eqp.img/Eqp/" + Dir + ID.IntID() + "/name");
        // else
        //   Name = Wz.GetStr("String/Item.img/Eqp/" + Dir + ID.IntID() + "/name");
        var Entry = Wz.GetNodeA("Character/" + Dir + ID + ".img");
        PartName PartName = Equip.GetPart(ID);
        Bitmap Bmp = null;
        if (Bmp != null)
            Bmp.Dispose();
        switch (PartName)
        {
            case PartName.Hair:
                Bmp = Entry.GetBmp("default/hairOverHead");
                break;
            case PartName.Face:
                Bmp = Entry.GetBmp("default/face");
                break;
            default:
                Bmp = Entry.GetBmp("info/icon");
                break;
        }
        pictureBox1.Image = Bmp;

    }
    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tabControl1.SelectedIndex != 0)
            MainForm.Instance.ToolTipView.Visible = false;

        void LoadAvatarPics()
        {
            string[] Files = Directory.GetFiles(System.Environment.CurrentDirectory + "\\Images");
            foreach (var i in Files)
            {
                string Name = Path.GetFileName(i);
                Name = Name.Replace(".png", "");
                Bitmap Png = new Bitmap(i);
                AvatarListView.Items.Add(Name, Png);
                Png.Dispose();
            }
        }

        switch (tabControl1.SelectedIndex)
        {
            case 0:

                if (MainForm.Instance.ToolTipView.Parent != null)
                {
                    MainForm.Instance.ToolTipView.Dispose();
                    MainForm.Instance.ToolTipView = null;
                    MainForm.Instance.ToolTipView = new AfrmTooltip();
                    MainForm.Instance.ToolTipView.Visible = true;
                    MainForm.Instance.ToolTipView.StringLinker = MainForm.Instance.stringLinker;
                    MainForm.Instance.ToolTipView.ShowID = true;
                    MainForm.Instance.ToolTipView.ShowMenu = true;
                    MainForm.Instance.ToolTipView.StartPosition = FormStartPosition.CenterParent;
                }
                SelectedFrame = false;
                break;
            case 1:
                if (!Loaded)
                {
                    LoadAvatarPics();
                    Loaded = true;
                }
                AvatarListView.Parent = tabControl1.TabPages[1];
                SelectedFrame = false;
                break;
            case 2:
                ResetDyeGrid();

                SelectedFrame = false;
                break;

            case 3:
                ResetDyeGrid2();
                SelectedFrame = false;
                break;
            case 4:
                if (!SearchGridLoaded)
                {
                    SearchGrid = new(60, 184, 114, 109, 315, 400, false, tabControl1.TabPages[4]);
                    SearchGrid.CellClick += (s, e) =>
                    {
                        CellClick(SearchGrid, e);
                    };

                    SearchGrid.SearchGrid.CellClick += (s, e) =>
                    {
                        CellClick(SearchGrid.SearchGrid, e);
                    };


                    SearchGrid.CellMouseEnter += (s, e) =>
                    {
                        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                        {
                            SearchGrid[0, e.RowIndex].Style.BackColor = Color.LightCyan;
                            SearchGrid[1, e.RowIndex].Style.BackColor = Color.LightCyan;
                        }
                        if (ShowToolTip)
                        {
                            string ID = SearchGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                            Wz_Node Node = Wz.GetNodeByID(ID, WzType.Character);
                            MainForm.Instance.QuickView(Node);
                            // MainForm.Instance.ToolTipView.Location = new Point(448, 395);
                        }
                    };

                    SearchGrid.CellMouseLeave += (s, e) =>
                    {
                        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                        {
                            SearchGrid[0, e.RowIndex].Style.BackColor = Color.White;
                            SearchGrid[1, e.RowIndex].Style.BackColor = Color.White;
                        }
                    };

                    SearchGrid.SearchGrid.CellMouseEnter += (s, e) =>
                    {
                        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                        {
                            SearchGrid.SearchGrid[0, e.RowIndex].Style.BackColor = Color.LightCyan;
                            SearchGrid.SearchGrid[1, e.RowIndex].Style.BackColor = Color.LightCyan;
                        }
                        if (ShowToolTip)
                        {
                            string ID = SearchGrid.SearchGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                            Wz_Node Node = Wz.GetNodeByID(ID, WzType.Character);
                            MainForm.Instance.QuickView(Node);
                            // MainForm.Instance.ToolTipView.Location = new Point(448, 395);
                        }
                    };

                    SearchGrid.SearchGrid.CellMouseLeave += (s, e) =>
                    {
                        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                        {
                            SearchGrid.SearchGrid[0, e.RowIndex].Style.BackColor = Color.White;
                            SearchGrid.SearchGrid[1, e.RowIndex].Style.BackColor = Color.White;
                        }
                    };

                    var Graphic = SearchGrid.CreateGraphics();
                    var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                    Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

                    string ID = null;
                    string name = null;
                    Win32.SendMessage(SearchGrid.Handle, false);

                    if (!Wz.HasHardCodedStrings)
                    {
                        if (Wz.HasNode("String/Eqp.img"))
                            DumpEqpString(Wz.GetNodeA("String/Eqp.img/Eqp"));
                        else
                            DumpEqpString(Wz.GetNodeA("String/Item.img/Eqp"));
                    }
                    Win32.SendMessage(SearchGrid.Handle, true);
                    SearchGrid.Refresh();
                    SearchGridLoaded = true;
                }
                // MainForm.Instance.ToolTipView.TopLevel = false;
                // MainForm.Instance.ToolTipView.IsMdiContainer = false;
                // MainForm.Instance.ToolTipView.Parent = this;
                SelectedFrame = false;
                break;

            case 5:
                if (!Loaded)
                {
                    LoadAvatarPics();
                    Loaded = true;
                }
                AvatarListView.Parent = tabControl1.TabPages[5];
                SelectedFrame = false;
                break;

            case 6:
                if (!LoadedFrameList)
                {
                    foreach (var i in AllFrames)
                        FrameListBox.Items.Add(i);
                    LoadedFrameList = true;
                }

                break;
        }

    }

    private void DyeGrid_CellClick(object sender, DataGridViewCellEventArgs e)
    {

        int Col = e.ColumnIndex;
        string ID = Inventory.Rows[e.RowIndex].Cells[0].Value.ToString();
        string Dir = Equip.GetDir(ID);
        Wz_Node Entry;
        if (ItemEffect.AllList.Contains(ID))
            Entry = Wz.GetNodeA("Effect/ItemEff.img/" + ID.IntID());
        else
            Entry = Wz.GetNodeA("Character/" + Dir + ID + ".img");

        if (Col <= 15)
            Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib, true, Col * 25);
        else
            Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib, true, 0, -200);
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        SearchGrid.Search(textBox1.Text);
    }

    private void UseButton_Click(object sender, EventArgs e)
    {
        if (label1.Text == "")
            return;
        AddEqps(label1.Text);
        AddInventory();
        Game.Player.RemoveSprites();
        for (int i = 0; i < Player.EqpList.Count; i++)
        {
            Game.Player.Spawn(Player.EqpList[i]);
        }

        ResetDye2();
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChangeExpressionListBox = true;
    }



    private void ShowToolTil_CheckBox_CheckedChanged(object sender, EventArgs e)
    {
        ShowToolTip = !ShowToolTip;
        MainForm.Instance.ToolTipView.Visible = ShowToolTip;
    }


    private void AvatarForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
        SelectedFrame = false;
    }

    private void EarListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Game.Player.EarType = EarListBox.Text;
    }

    private void FrameListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedFrame = true;
        string SelectedItem = FrameListBox.SelectedItem.ToString();
        var Split = SelectedItem.Split('.');
        SelectedFrameNum = Split[1].ToInt();
        SelectedAction = Split[0];
    }

    private void button21_Click(object sender, EventArgs e)
    {
        RenderTarget2D SaveTexture = null;
        int WX = (int)(Game.Player.X - EngineFunc.SpriteEngine.Camera.X - 155) + AdjustX;
        int WY = (int)(Game.Player.Y - EngineFunc.SpriteEngine.Camera.Y - 160) + AdjustY;

        int Width = WX + AdjustW - WX;
        int Height = WY + AdjustH - WY;
        EngineFunc.Canvas.DrawTarget(ref SaveTexture, Width, Height, () =>
        {
            EngineFunc.Canvas.DrawCropArea(FrameListDraw.AvatarPanelTexture, 0, 0, new Microsoft.Xna.Framework.Rectangle(WX, WY, WX + AdjustW, WY + AdjustH), 0, 0, 1, 1, 0, false, false, 255, 255, 255, 255, false);
        });
        Stream stream = File.OpenWrite(System.Environment.CurrentDirectory + "\\Export\\" + FrameListBox.SelectedItem.ToString() + ".png");
        SaveTexture.SaveAsPng(stream, Width, Height);
        stream.Dispose();
        SaveTexture.Dispose();
    }

    private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
    {
        switch (((HScrollBar)sender).Name)
        {
            case "ScrollBarX":
                AdjX.Text = ScrollBarX.Value.ToString();
                AdjustX = ScrollBarX.Value;
                break;
            case "ScrollBarY":
                AdjY.Text = ScrollBarY.Value.ToString();
                AdjustY = AdjY.Text.ToInt();
                break;
            case "ScrollBarW":
                AdjW.Text = ScrollBarW.Value.ToString();
                AdjustW = AdjW.Text.ToInt();
                break;
            case "ScrollBarH":
                AdjH.Text = ScrollBarH.Value.ToString();
                AdjustH = AdjH.Text.ToInt();
                break;

        }
    }


    int RowIndex;

    private void DyeGrid2_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        HueTrackBar.Value = 0;
        SatTrackBar.Value = 0;
        LightnessTrackBar.Value = 0;
        LabelHue.Text = "0";
        LabelSat.Text = "0";
        LabelLightness.Text = "0";
        DyePicture.Image = (Bitmap)DyeGrid2.Rows[e.RowIndex].Cells[1].Value;
        RowIndex = e.RowIndex;
    }


    void SetDye2()
    {
        if (DyePicture.Image == null)
            return;
        Bitmap Bmp = (Bitmap)DyeGrid2.Rows[RowIndex].Cells[1].Value;
        Bitmap Image = null;
        ImageFilter.HSL(ref Bmp, HueTrackBar.Value, SatTrackBar.Value, LightnessTrackBar.Value);

        DyePicture.Image = Bmp;
        LabelHue.Text = HueTrackBar.Value.ToString();
        LabelSat.Text = SatTrackBar.Value.ToString();
        LabelLightness.Text = LightnessTrackBar.Value.ToString();
    }

    void ResetDye2()
    {
        DyePicture.Image = null;
        HueTrackBar.Value = 0;
        SatTrackBar.Value = 0;
        LightnessTrackBar.Value = 0;
        LabelHue.Text = "0";
        LabelSat.Text = "0";
        LabelLightness.Text = "0";
    }

    private void HueTrackBar_Scroll(object sender, EventArgs e)
    {
        SetDye2();
    }

    private void SatTrackBar_Scroll(object sender, EventArgs e)
    {
        SetDye2();
    }

    private void LightnessTrackBar_Scroll(object sender, EventArgs e)
    {
        SetDye2();
    }

    private void button22_Click(object sender, EventArgs e)
    {
        string ID = DyeGrid2.Rows[RowIndex].Cells[0].Value.ToString();
        string Dir = Equip.GetDir(ID);
        Wz_Node Entry;
        if (ItemEffect.AllList.Contains(ID))
            Entry = Wz.GetNodeA("Effect/ItemEff.img/" + ID.IntID());
        else
            Entry = Wz.GetNodeA("Character/" + Dir + ID + ".img");

        Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib, true, HueTrackBar.Value, SatTrackBar.Value, LightnessTrackBar.Value);
    }
}

