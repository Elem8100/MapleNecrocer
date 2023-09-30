using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.CharaSim;

namespace MapleNecrocer;

public partial class CashEffectForm : Form
{
    public CashEffectForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static CashEffectForm Instance;
    DataGridViewEx CashEffectListGrid;

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {

        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        ItemEffect.Remove(EffectType.Cash);
        ItemEffect.Create(ID, EffectType.Cash);
    }
    private void CashEffectForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        CashEffectListGrid = new(90, 174, 0, 0, 220, 700, true, tabControl1.TabPages[0]);
        CashEffectListGrid.Dock = DockStyle.Fill;
        CashEffectListGrid.SearchGrid.Dock = DockStyle.Fill;

        var Graphic = CashEffectListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        CashEffectListGrid.CellClick += (s, e) =>
        {
            CellClick(CashEffectListGrid, e);
        };

        CashEffectListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(CashEffectListGrid.SearchGrid, e);
        };

        string CashEffectName = "";
        Bitmap Bmp = null;
        var Entry = Wz.GetNode("Item/Cash/0501.img");
        Win32.SendMessage(CashEffectListGrid.Handle, false);
        foreach (var Iter in Entry.Nodes)
        {
            if (Iter.Text == "05010044")
                continue;
            if (Iter.Text == "05012000")
                continue;
            if (Iter.Text == "05012001")
                continue;
            if (Iter.Text == "05010099")
                continue;
            if (Wz.HasNode("String/Cash.img/" + Iter.Text.IntID()))
                CashEffectName = Wz.GetNode("String/Cash.img/" + Iter.Text.IntID()).GetStr("name");
            else
                CashEffectName = "";
            if (Iter.HasNode("info/icon"))
                Bmp = Iter.GetNode("info/icon").ExtractPng();

            CashEffectListGrid.Rows.Add(Iter.Text, Bmp, CashEffectName);
        }
        Win32.SendMessage(CashEffectListGrid.Handle, true);
        CashEffectListGrid.Refresh();
        for (int i = 0; i < CashEffectListGrid.Rows.Count; i++)
        {
            CashEffectListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            CashEffectListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        ItemEffect.Remove(EffectType.Cash);
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        CashEffectListGrid.Search(textBox1.Text);
    }

    private void CashEffectForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }
}
