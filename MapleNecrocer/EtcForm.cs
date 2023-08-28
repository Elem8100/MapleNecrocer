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

namespace MapleNecrocer;

public partial class EtcForm : Form
{
    public EtcForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static EtcForm Instance;
    ImageListView ImageGrid;
    DataGridViewEx EtcListGrid;
    bool HasLoaded1;

    private void EtcForm_Shown(object sender, EventArgs e)
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
            pictureBox1.Image = Wz.GetBmp("Item/Etc/" + ID.LeftStr(4) + ".img/" + ID + "/info/icon");
            label2.Text = Wz.GetStr("String/Etc.img/Etc" + ID.IntID() + "/name");
        };

        var Graphic = ImageGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        Win32.SendMessage(ImageGrid.Handle, false);
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNodes("Item/Etc"))
        {
            if (!Char.IsNumber(Img.Text[0]))
                continue;
            foreach (var Iter in Wz.GetNodes("Item/Etc/" + Img.Text))
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

            EtcListGrid = new(80, 185, 0, 20, 220, 530, true, tabControl1.TabPages[1]);
            EtcListGrid.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            EtcListGrid.Dock = DockStyle.Fill;
            tabControl1.TabPages[1].Padding = new Padding(0, 35, 0, 0);
            EtcListGrid.SearchGrid.Dock = DockStyle.Fill;
            EtcListGrid.RowTemplate.Height = 40;

            var Graphic = EtcListGrid.CreateGraphics();
            var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
            Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
            void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
            {
                string ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                label1.Text = ID;
                pictureBox1.Image = (Bitmap)EtcListGrid.Rows[e.RowIndex].Cells[1].Value;
                label2.Text = EtcListGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
            }

            EtcListGrid.CellClick += (s, e) =>
            {
                CellClick(EtcListGrid, e);
            };

            EtcListGrid.SearchGrid.CellClick += (s, e) =>
            {
                CellClick(EtcListGrid.SearchGrid, e);
            };

            Win32.SendMessage(EtcListGrid.Handle, false);
            Bitmap Bmp = null;
            string ConsumeName = "";
            foreach (var Img in Wz.GetNodes("Item/Etc"))
            {
                if (!Char.IsNumber(Img.Text[0]))
                    continue;
                foreach (var Iter in Wz.GetNodes("Item/Etc/" + Img.Text))
                {
                    string ID = Iter.Text;
                    string IntID = ID.IntID();
                    if (Wz.HasNode("String/Etc.img/Etc/" + IntID))
                        ConsumeName = Wz.GetStr("String/Etc.img/Etc/" + IntID + "/name");
                    if (Iter.HasNode("info/icon"))
                        Bmp = Iter.GetBmp("info/icon");
                    EtcListGrid.Rows.Add(ID, Bmp, ConsumeName);
                }
            }

            Win32.SendMessage(EtcListGrid.Handle, true);
            EtcListGrid.Refresh();
            for (int i = 0; i < EtcListGrid.Rows.Count; i++)
            {
                EtcListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                EtcListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
            }
            HasLoaded1 = true;

        }
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        EtcListGrid.Search(textBox1.Text);
    }

    private void UseButton_Click(object sender, EventArgs e)
    {
        if (label1.Text.Trim(' ') != "")
            ItemDrop.Drop((int)Game.Player.X, (int)Game.Player.Y, 0, label1.Text.Trim(' '));
    }

    private void EtcForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }
}
