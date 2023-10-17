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

public partial class TitleForm : Form
{
    public TitleForm()
    {
        InitializeComponent();
    }
    public static TitleForm Instance;
    public DataGridViewEx TitleListGrid;
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
        NickNameTag.Delete();
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        int TagNum = Wz.GetNode("Item/Install/0370.img/" + ID + "/info/nickTag").ToInt();
        if (Wz.GetNode("UI/NameTag.img/nick/" + TagNum) != null)
            NickNameTag.Create(ID);
    }

    private void TitleForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        TitleListGrid = new(90, 164, 0, 0, 220, 400, true, panel1);
        TitleListGrid.Dock = DockStyle.Fill;
        TitleListGrid.SearchGrid.Dock = DockStyle.Fill;
        TitleListGrid.RowTemplate.Height = 40;

        var Graphic = TitleListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        TitleListGrid.CellClick += (s, e) =>
        {
            CellClick(TitleListGrid, e);
        };

        TitleListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(TitleListGrid.SearchGrid, e);
        };
        TitleListGrid.SetToolTipEvent(WzType.Item, this);


        string TitleName = null;
        Bitmap Bmp = null;
        foreach (var Iter in Wz.GetNode("Item/Install/0370.img").Nodes)
        {
            string ID = Iter.Text;
            if (Wz.HasNode("String/Ins.img/" + ID.IntID()))
                TitleName = Wz.GetNode("String/Ins.img/" + ID.ToInt() + "/name").ToStr();
            if (Iter.HasNode("info/icon"))
                Bmp = Iter.GetNode("info/icon").ExtractPng();
            TitleListGrid.Rows.Add(ID, Bmp, TitleName);
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        NickNameTag.Delete();
    }

    private void TitleForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        TitleListGrid.Search(textBox1.Text);
    }

    private void TitleForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
    }
}
