using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manina.Windows.Forms;
using WzComparerR2.CharaSim;

namespace MapleNecrocer;

public partial class ConsumeForm : Form
{
    public ConsumeForm()
    {
        InitializeComponent();
        Instance = this;
    }
    ImageListView ImageGrid;
    public static ConsumeForm Instance;
    DataGridViewEx ConsumeListGrid;
    DataGridViewEx ConsumeEffectListGrid;
    bool HasLoaded1, HasLoaded2;
    private void ConsumeForm_Shown(object sender, EventArgs e)
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
        //ImageGrid.t.t.show.CacheMode= CacheMode.Continuous;
        ImageGrid.ItemClick += (o, e) =>
        {
            string ID = (e.Item.FileName);
            label1.Text = ID;
            pictureBox1.Image = Wz.GetBmp("Item/Consume/" + ID.LeftStr(4) + ".img/" + ID + "/info/icon");
            label2.Text = Wz.GetStr("String/Consume.img/" + ID.IntID() + "/name");
        };



        var Graphic = ImageGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        Win32.SendMessage(ImageGrid.Handle, false);
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNodes("Item/Consume"))
        {
            if (!Char.IsNumber(Img.Text[0]))
                continue;
            foreach (var Iter in Wz.GetNodes("Item/Consume/" + Img.Text))
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


            ConsumeListGrid = new(80, 185, 0, 20, 220, 530, true, tabControl1.TabPages[1]);
            ConsumeListGrid.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            ConsumeListGrid.Dock = DockStyle.Fill;
            tabControl1.TabPages[1].Padding = new Padding(0, 35, 0, 0);
            ConsumeListGrid.SearchGrid.Dock = DockStyle.Fill;
            ConsumeListGrid.RowTemplate.Height = 40;

            var Graphic = ConsumeListGrid.CreateGraphics();
            var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
            Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
            void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
            {
                string ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                label1.Text = ID;
                pictureBox1.Image = (Bitmap)ConsumeListGrid.Rows[e.RowIndex].Cells[1].Value;
                label2.Text = ConsumeListGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
            }

            ConsumeListGrid.CellClick += (s, e) =>
            {
                CellClick(ConsumeListGrid, e);
            };

            ConsumeListGrid.SearchGrid.CellClick += (s, e) =>
            {
                CellClick(ConsumeListGrid.SearchGrid, e);
            };

            Win32.SendMessage(ConsumeListGrid.Handle, false);
            Bitmap Bmp = null;
            string ConsumeName = "";
            foreach (var Img in Wz.GetNodes("Item/Consume"))
            {
                if (!Char.IsNumber(Img.Text[0]))
                    continue;
                foreach (var Iter in Wz.GetNodes("Item/Consume/" + Img.Text))
                {
                    string ID = Iter.Text;
                    string IntID = ID.IntID();
                    if (Wz.HasNode("String/Consume.img/" + IntID))
                        ConsumeName = Wz.GetStr("String/Consume.img/" + IntID + "/name");
                    if (Iter.HasNode("info/icon"))
                        Bmp = Iter.GetBmp("info/icon");
                    ConsumeListGrid.Rows.Add(ID, Bmp, ConsumeName);
                }
            }

            Win32.SendMessage(ConsumeListGrid.Handle, true);
            ConsumeListGrid.Refresh();
            for (int i = 0; i < ConsumeListGrid.Rows.Count; i++)
            {
                ConsumeListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ConsumeListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
            }
            HasLoaded1 = true;
        }

        if (tabControl1.SelectedIndex == 2)
        {
            if (HasLoaded2)
                return;
            ConsumeEffectListGrid = new(80, 185, 0, 20, 220, 530, true, tabControl1.TabPages[2]);
            ConsumeEffectListGrid.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            ConsumeEffectListGrid.Dock = DockStyle.Fill;
            tabControl1.TabPages[2].Padding = new Padding(0, 35, 0, 0);
            ConsumeEffectListGrid.SearchGrid.Dock = DockStyle.Fill;
            ConsumeEffectListGrid.RowTemplate.Height = 40;
            var Graphic = ConsumeEffectListGrid.CreateGraphics();
            var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
            Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
            void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
            {
                string ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                ItemEffect.Delete(EffectType.Consume);
                ItemEffect.Create(ID, EffectType.Consume);

            }

            ConsumeEffectListGrid.CellClick += (s, e) =>
            {
                CellClick(ConsumeEffectListGrid, e);
            };


            Win32.SendMessage(ConsumeEffectListGrid.Handle, false);
            Bitmap Bmp = null;
            string ConsumeEffectName = "";
            foreach (var Iter in Wz.GetNodes("Effect/ItemEff.img"))
            {
                if (Iter.Text[0] == '2')
                {
                    string ID = '0' + Iter.Text;
                    if (Wz.HasNode("Item/Consume/" + ID.LeftStr(4) + ".img/" + ID + "/info/icon"))
                    {
                        Bmp = Wz.GetBmp("Item/Consume/" + ID.LeftStr(4) + ".img/" + ID + "/info/icon");

                    }
                    ConsumeEffectName = Wz.GetStr("String/Consume.img/" + ID.IntID() + "/name");
                    ConsumeEffectListGrid.Rows.Add(ID, Bmp, ConsumeEffectName);
                }
            }
            Win32.SendMessage(ConsumeEffectListGrid.Handle, true);
            ConsumeEffectListGrid.Refresh();
            for (int i = 0; i < ConsumeEffectListGrid.Rows.Count; i++)
            {
                ConsumeEffectListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ConsumeEffectListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
            }
            HasLoaded2 = true;
        }

    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        ConsumeListGrid.Search(textBox1.Text);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        ItemEffect.Delete(EffectType.Consume);
    }
}
