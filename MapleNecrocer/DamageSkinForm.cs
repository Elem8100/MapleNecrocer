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
using WzComparerR2.WzLib;
namespace MapleNecrocer;

public partial class DamageSkinForm : Form
{
    public DamageSkinForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static DamageSkinForm Instance;
    public DataGridViewEx DamageSkinListGrid;

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        DamageNumber.UseNewDamage = true;
        var DamageStyle = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        DamageNumber.Style = DamageStyle;
        DamageNumber.Load(DamageStyle);
    }
    private void DamageSkinForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        DamageSkinListGrid = new(120, 1, 0, 0, 210, 400, true, tabControl1.TabPages[0]);
        DamageSkinListGrid.Dock = DockStyle.Fill;
        DamageSkinListGrid.SearchGrid.Dock = DockStyle.Fill;
        DamageSkinListGrid.RowTemplate.Height = 70;
        DamageSkinListGrid.Columns[1].Width = 130;
        DamageSkinListGrid.Columns[2].Width = 0;
        var Graphic = DamageSkinListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        DamageSkinListGrid.CellClick += (s, e) =>
        {
            CellClick(DamageSkinListGrid, e);
        };


        Wz_Node Entry = null;
        if (Wz.HasNode("Effect/DamageSkin.img"))
            Entry = Wz.GetNode("Effect/DamageSkin.img");
        else if (Wz.HasNode("Effect/BasicEff.img/damageSkin"))
            Entry = Wz.GetNode("Effect/BasicEff.img/damageSkin");

        if (Entry == null)
        {
            MessageBoxEx.Show("Older versions of .wz are not supported", "OK");
            return;
        }

        Bitmap Bmp = null;
        foreach (var Iter in Entry.Nodes)
        {
            foreach (var Iter2 in Iter.Nodes)
            {
                if (Iter2.Text == "NoCri1" || Iter2.Text == "NoRed1")
                {
                    if (Iter2.HasNode("5") && Iter2.Nodes["5"].Value is Wz_Png)
                        Bmp = Iter2.GetNode("5").ExtractPng();
                    DamageSkinListGrid.Rows.Add(Iter.Text + "/" + Iter2.Text, Bmp, "");
                }
            }
        }

        for (int i = 0; i < DamageSkinListGrid.Rows.Count; i++)
        {
            DamageSkinListGrid.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DamageSkinListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
    }

    private void DamageSkinForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        ActiveControl = null;
    }
}
