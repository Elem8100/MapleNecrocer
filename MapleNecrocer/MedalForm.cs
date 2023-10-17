using DevComponents.DotNetBar;
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

public partial class MedalForm : Form
{
    public MedalForm()
    {
        InitializeComponent();
        Instance = this;
    }

    public static MedalForm Instance;
    public DataGridViewEx MedalListGrid;

    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x02000000;
            return cp;
        }
    }
    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        MedalTag.Delete();
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        MedalTag.Create(ID);

    }
    private void MedalForm_Shown(object sender, EventArgs e)
    {

        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        MedalListGrid = new(90, 164, 0, 0, 220, 400, true, panel1);
        MedalListGrid.Dock = DockStyle.Fill;
        MedalListGrid.SearchGrid.Dock = DockStyle.Fill;
        MedalListGrid.RowTemplate.Height = 40;

        var Graphic = MedalListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        MedalListGrid.CellClick += (s, e) =>
        {
            CellClick(MedalListGrid, e);
        };

        MedalListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(MedalListGrid.SearchGrid, e);
        };

        MedalListGrid.SetToolTipEvent(WzType.Character, this);

        string MedalName = null;
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNode("Character/Accessory").Nodes)
        {
            if (Img.Text.LeftStr(4) != "0114")
                continue;
            if (!Wz.HasNode("Character/Accessory/" + Img.Text + "/info/medalTag"))
                continue;
            int TagNum = Wz.GetNode("Character/Accessory/" + Img.Text + "/info/medalTag").ToInt();
            if (!Wz.HasNode("UI/NameTag.img/medal/" + TagNum))
                continue;
            string ID = Img.ImgID();
            if (Wz.HasNode("String/Eqp.img/Eqp/Accessory/" + ID.IntID()))
                MedalName = Wz.GetNode("String/Eqp.img/Eqp/Accessory/" + ID.IntID() + "/name").ToStr();
            if (Wz.HasNode("Character/Accessory/" + Img.Text + "/info/icon"))
                Bmp = Wz.GetNode("Character/Accessory/" + Img.Text + "/info/icon").ExtractPng();
            MedalListGrid.Rows.Add(ID, Bmp, MedalName);
        }

        for (int i = 0; i < MedalListGrid.Rows.Count; i++)
        {
            MedalListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MedalListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }

    }

    private void button1_Click(object sender, EventArgs e)
    {
        MedalTag.Delete();
    }

    private void MedalForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        MedalListGrid.Search(textBox1.Text);
    }

    private void MedalForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
    }
}
