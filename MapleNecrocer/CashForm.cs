using Manina.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.WzLib;

namespace MapleNecrocer;

public partial class CashForm : Form
{
    public CashForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static CashForm Instance;
    ImageListView ImageGrid;
    DataGridViewEx CashListGrid;
    bool HasLoaded1;
   
    private void CashForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        ImageGrid = new ImageListView();
        ImageGrid.Parent = tabControl1.TabPages[0];
        ImageGrid.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
        ImageGrid.Dock = DockStyle.Fill;
        ImageGrid.BackColor = SystemColors.Window;
        ImageGrid.Colors.BackColor = SystemColors.ButtonFace;
        ImageGrid.Colors.SelectedBorderColor = Color.Red;
        ImageGrid.BorderStyle = BorderStyle.Fixed3D;
        ImageGrid.ThumbnailSize = new System.Drawing.Size(32, 32);
        ImageGrid.ItemClick += (o, e) =>
        {
            string ID = (e.Item.FileName);
            label1.Text = ID;
            pictureBox1.Image = Wz.GetBmp("Item/Cash/" + ID.LeftStr(4) + ".img/" + ID + "/info/icon");
            label2.Text = Wz.GetStr("String/Cash.img/" + ID.IntID() + "/name");
        };

        ImageGrid.ItemHover += (o, e) =>
        {
            if (e.Item == null) return;
            Wz_Node Node = Wz.GetNodeByID(e.Item.FileName, WzType.Item);
            MainForm.Instance.QuickView(Node);
            MainForm.Instance.ToolTipView.Owner = this;
        };

        var Graphic = ImageGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        Win32.SendMessage(ImageGrid.Handle, false);
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNodes("Item/Cash"))
        {
            if (!Char.IsNumber(Img.Text[0]))
                continue;
            foreach (var Iter in Wz.GetNodes("Item/Cash/" + Img.Text))
            {
                if (Iter.HasNode("info/icon"))
                    Bmp = Iter.GetBmp("info/icon");
                ImageGrid.Items.Add(Iter.Text, Bmp);
            }
        }
        ImageGrid.Sort();
        Win32.SendMessage(ImageGrid.Handle, true);
        ImageGrid.Refresh();
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
        label1.Text = "";
        pictureBox1.Image = null;
        label2.Text = "";
        if (tabControl1.SelectedIndex == 1)
        {
            if (HasLoaded1)
                return;

            CashListGrid = new(80, 185, 0, 20, 220, 530, true, tabControl1.TabPages[1]);
            CashListGrid.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            CashListGrid.Dock = DockStyle.Fill;
            tabControl1.TabPages[1].Padding = new Padding(0, 35, 0, 0);
            CashListGrid.SearchGrid.Dock = DockStyle.Fill;
            CashListGrid.RowTemplate.Height = 40;

            var Graphic = CashListGrid.CreateGraphics();
            var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
            Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
            void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
            {
                string ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                label1.Text = ID;
                pictureBox1.Image = (Bitmap)CashListGrid.Rows[e.RowIndex].Cells[1].Value;
                label2.Text = CashListGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
            }

            CashListGrid.CellClick += (s, e) =>
            {
                CellClick(CashListGrid, e);
            };

            CashListGrid.SearchGrid.CellClick += (s, e) =>
            {
                CellClick(CashListGrid.SearchGrid, e);
            };
            CashListGrid.SetToolTipEvent(WzType.Item, this);


            Win32.SendMessage(CashListGrid.Handle, false);
            Bitmap Bmp = null;
            string ConsumeName = "";
            foreach (var Img in Wz.GetNodes("Item/Cash"))
            {
                if (!Char.IsNumber(Img.Text[0]))
                    continue;
                foreach (var Iter in Wz.GetNodes("Item/Cash/" + Img.Text))
                {
                    string ID = Iter.Text;
                    string IntID = ID.IntID();
                    if (Wz.HasNode("String/Cash.img/" + IntID))
                        ConsumeName = Wz.GetStr("String/Cash.img/" + IntID + "/name");
                    if (Iter.HasNode("info/icon"))
                        Bmp = Iter.GetBmp("info/icon");
                    CashListGrid.Rows.Add(ID, Bmp, ConsumeName);
                }
            }

            Win32.SendMessage(CashListGrid.Handle, true);
            CashListGrid.Refresh();
            for (int i = 0; i < CashListGrid.Rows.Count; i++)
            {
                CashListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                CashListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
            }
            HasLoaded1 = true;
        }
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        CashListGrid.Search(textBox1.Text);
    }

    private void UseButton_Click(object sender, EventArgs e)
    {
        if (label1.Text.Trim(' ') != "")
            ItemDrop.Drop((int)Game.Player.X, (int)Game.Player.Y, 0, label1.Text.Trim(' '));
    }

    private void CashForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }

    private void CashForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
    }
}
