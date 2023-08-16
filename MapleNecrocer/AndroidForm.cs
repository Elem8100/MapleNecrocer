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

public partial class AndroidForm : Form
{
    public AndroidForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static AndroidForm Instance;
    DataGridViewEx AndroidListGrid;
    ImageListView ImageGrid;
    List<string> IDList = new();

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        if (AndroidPlayer.Instance == null)
            AndroidPlayer.SpawnNew();

        IDList.Clear();
        string AndroidID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        int Num = Wz.GetInt("Character/Android/" + AndroidID + ".img/info/android");
        string ImgNum = Num.ToString().PadLeft(4, '0') + ".img";

        foreach (var Iter in Wz.GetNodes("Etc/Android/" + ImgNum))
        {
            if (Iter.Text == "basic")
            {
                foreach (var Iter2 in Iter.Nodes)
                    IDList.Add(Iter2.ToInt().ToString().PadLeft(8, '0'));

            }
            if (Iter.Text == "costume")
            {
                foreach (var Iter2 in Iter.Nodes)
                {
                    foreach (var Iter3 in Iter2.Nodes)
                    {
                        if (Iter3.Text == "0")
                            IDList.Add(Iter3.ToInt().ToString().PadLeft(8, '0'));
                    }
                }
            }
        }
        //add head
        for (int i = IDList.Count - 1; i >= 0; i--)
        {
            if (IDList[i].LeftStr(4) == "0000")
                IDList.Add("000120" + IDList[i].RightStr(2).PadLeft(2, '0'));
        }

        string Str = "";
        foreach (var i in IDList)
            Str = Str + i + '-';
        AndroidPlayer.Instance.Spawn(Str);
        AndroidNameTag.Remove();
        AndroidNameTag.Create(ImgNum);
        AndroidNameTag.Instance.MedalName = "Android";
        AndroidNameTag.Instance.InitData();
        AndroidNameTag.ReDraw();
    }
    private void AndroidForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        AndroidListGrid = new(85, 203, 0, 0, 220, 400, true, tabControl1.TabPages[0]);
        AndroidListGrid.Dock = DockStyle.Fill;
        AndroidListGrid.SearchGrid.Dock = DockStyle.Fill;
        AndroidListGrid.RowTemplate.Height = 40;

        var Graphic = AndroidListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        AndroidListGrid.CellClick += (s, e) =>
        {
            CellClick(AndroidListGrid, e);
        };

        AndroidListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(AndroidListGrid.SearchGrid, e);
        };

        ImageGrid = new ImageListView();
        ImageGrid.Parent = tabControl1.TabPages[1];
        ImageGrid.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
        ImageGrid.Dock = DockStyle.Fill;
        ImageGrid.BackColor = SystemColors.Window;
        ImageGrid.Colors.BackColor = SystemColors.ButtonFace;
        ImageGrid.Colors.SelectedBorderColor = Color.Red;
        ImageGrid.Colors.HoverColor1 = SystemColors.ButtonFace;
        ImageGrid.Colors.HoverColor2 = SystemColors.ButtonFace;
        ImageGrid.BorderStyle = BorderStyle.Fixed3D;
        ImageGrid.ThumbnailSize = new System.Drawing.Size(100, 100);
        ImageGrid.ItemClick += (o, e) =>
        {
            if (AndroidPlayer.Instance == null)
                AndroidPlayer.SpawnNew();
            string IDs = e.Item.FileName;
            AndroidPlayer.Instance.Spawn(IDs);

            AndroidNameTag.Remove();
            AndroidNameTag.Create("0001.img");
            AndroidNameTag.Instance.MedalName = "Android";
            AndroidNameTag.Instance.InitData();
            AndroidNameTag.ReDraw();

        };
        if (!Wz.HasNode("Character/Android"))
            return;

        string AndroidName = "";
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNodes("Character/Android"))
        {
            if (Img.Text.LeftStr(4) == "0167")
                continue;
            if (!Char.IsNumber(Img.Text[0]))
                continue;
            string ID = Img.ImgID();
            if (Wz.HasNode("String/Eqp.img/Eqp/Android/" + ID.IntID()))
                AndroidName = Wz.GetStr("String/Eqp.img/Eqp/Android/" + ID.IntID() + "/name");
            if (Wz.HasNode("Character/Android/" + Img.Text + "/info/iconD"))
                Bmp = Wz.GetBmp("Character/Android/" + Img.Text + "/info/iconD");
            AndroidListGrid.Rows.Add(ID, Bmp, AndroidName);
        }
        for (int i = 0; i < AndroidListGrid.Rows.Count; i++)
        {
            AndroidListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            AndroidListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        AndroidNameTag.Remove();
        if (AndroidPlayer.Instance != null)
        {
            AndroidPlayer.Instance.RemoveSprites();
            AndroidPlayer.Instance.Dead();
            AndroidPlayer.Instance = null;
            EngineFunc.SpriteEngine.Dead();
        }
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
        void LoadAvatarPics()
        {
            string[] Files = Directory.GetFiles(System.Environment.CurrentDirectory + "\\Images");
            foreach (var i in Files)
            {
                string Name = Path.GetFileName(i);
                Name = Name.Replace(".png", "");
                Bitmap Png = new Bitmap(i);
                ImageGrid.Items.Add(Name, Png);
                Png.Dispose();
            }
        }
        if (tabControl1.SelectedIndex == 1)
        {
            LoadAvatarPics();
        }

    }

    private void AndroidForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        AndroidListGrid.Search(textBox1.Text);
    }
}
