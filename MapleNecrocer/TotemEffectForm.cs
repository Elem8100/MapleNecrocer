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

public partial class TotemEffectForm : Form
{
    public TotemEffectForm()
    {
        InitializeComponent();
    }
    public static TotemEffectForm Instance;
    public DataGridViewEx TotemEffectListGrid;

    private void TotemEffectForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
        TotemEffectListGrid = new(80, 185, 0, 0, 220, 400, true, panel1);
        TotemEffectListGrid.Dock = DockStyle.Fill;
        TotemEffectListGrid.SearchGrid.Dock = DockStyle.Fill;
        TotemEffectListGrid.RowTemplate.Height = 40;

        var Graphic = TotemEffectListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
        {
            string ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
            ItemEffect.Delete(EffectType.Totem);
            ItemEffect.Create(ID, EffectType.Totem);
        }

        TotemEffectListGrid.CellClick += (s, e) =>
        {
            CellClick(TotemEffectListGrid, e);
        };

        TotemEffectListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(TotemEffectListGrid.SearchGrid, e);
        };

        Bitmap Bmp = null;
        string Name = "";
        foreach (var Iter in Wz.GetNodes("Effect/ItemEff.img"))
        {
            if (Iter.Text.LeftStr(2) == "12")
            {
                string ID = Iter.Text;
                if (Wz.HasNode("Character/Totem/0" + ID + ".img/info/icon"))
                    Bmp = Wz.GetBmp("Character/Totem/0" + ID + ".img/info/icon");
                if (Wz.HasNode("String/Eqp.img/Eqp/Accessory/" + ID))
                    Name = Wz.GetStr("String/Eqp.img/Eqp/Accessory/" + ID + "/name");
                TotemEffectListGrid.Rows.Add('0' + ID, Bmp, Name);
            }

        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        ItemEffect.Delete(EffectType.Totem);
    }

    private void TotemEffectForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }
}
