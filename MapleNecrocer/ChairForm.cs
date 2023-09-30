using MonoGame.SpriteEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.CharaSim;

namespace MapleNecrocer;

public partial class ChairForm : Form
{
    public ChairForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static ChairForm Instance;
    public DataGridViewEx ChairListGrid;

    private void ChairForm_Load(object sender, EventArgs e)
    {

    }

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        if (Morph.IsUse)
            return;
        if (!MapleChair.CanUse)
            return;
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();

        MapleChair.Remove();

        TamingMob.Remove();
        ItemEffect.Remove(EffectType.Chair);

        MapleChair.Create(ID);

        if (ItemEffect.AllList.Contains(ID))
            ItemEffect.Create(ID, EffectType.Chair);
        MapleChair.IsUse = true;


    }

    private void ChairForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        ChairListGrid = new(90, 164, 0, 0, 220, 400, true, tabControl1.TabPages[0]);
        ChairListGrid.Dock = DockStyle.Fill;
        ChairListGrid.SearchGrid.Dock = DockStyle.Fill;

        var Graphic = ChairListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);


        ChairListGrid.CellClick += (s, e) =>
        {
            CellClick(ChairListGrid, e);
        };

        ChairListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(ChairListGrid.SearchGrid, e);
        };


        Dictionary<string, string> Dict = new();
        foreach (var Iter in Wz.GetNode("String/Ins.img").Nodes)
        {
            if (Iter.Text.LeftStr(3) == "301" || Iter.Text.LeftStr(3) == "302")
            {
                Dict.AddOrReplace(Iter.Text, Iter.GetStr("name"));
            }
        }

        string ChairName = "";
        Bitmap Bmp = null;
        Win32.SendMessage(ChairListGrid.Handle, false);
        foreach (var Img in Wz.GetNodeA("Item/Install").Nodes)
        {
            if (Img.Text.LeftStr(4) != "0301" && Img.Text.LeftStr(4) != "0302")
                continue;
            foreach (var Iter in Wz.GetNodeA("Item/Install/" + Img.Text).Nodes)
            {
                if (Dict.ContainsKey(Iter.Text.IntID()))
                    ChairName = Dict[Iter.Text.IntID()];
                else
                    ChairName = "";
                if (Iter.HasNode("info/icon"))
                    Bmp = Iter.GetNode("info/icon").ExtractPng();
                ChairListGrid.Rows.Add(Iter.Text, Bmp, ChairName);
            }
        }
        Win32.SendMessage(ChairListGrid.Handle, true);
        ChairListGrid.Refresh();

        for (int i = 0; i < ChairListGrid.Rows.Count; i++)
        {
            ChairListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ChairListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }

        ChairListGrid.Sort(ChairListGrid.Columns[0], ListSortDirection.Ascending);
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        ChairListGrid.Search(textBox1.Text);
    }

    private void ChairForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }
}
