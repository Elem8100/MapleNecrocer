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

public partial class SoulEffectForm : Form
{
    public SoulEffectForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static SoulEffectForm Instance;
    public DataGridViewEx SoulEffectListGrid;

    private void SoulEffectForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
        SoulEffectListGrid = new(80, 190, 0, 0, 220, 400, true, panel1);
        SoulEffectListGrid.Dock = DockStyle.Fill;
        SoulEffectListGrid.SearchGrid.Dock = DockStyle.Fill;
        SoulEffectListGrid.RowTemplate.Height = 40;
        var Str = new DataGridViewTextBoxColumn();
        SoulEffectListGrid.Columns.AddRange(Str);
        SoulEffectListGrid.Columns[3].Width = 0;

        var Graphic = SoulEffectListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
        {
            string ID = DataGrid.Rows[e.RowIndex].Cells[3].Value.ToString();
            ItemEffect.Remove(EffectType.Soul);
            ItemEffect.Create(ID, EffectType.Soul);
        }

        SoulEffectListGrid.CellClick += (s, e) =>
        {
            CellClick(SoulEffectListGrid, e);
        };

        SoulEffectListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(SoulEffectListGrid.SearchGrid, e);
        };

        Bitmap Bmp = null;
        string Name = "";
        foreach (var Iter in Wz.GetNodes("Etc/SoulCollection.img"))
        {
            string ID = Iter.GetInt("soulList/0/0").ToString();
            if (Wz.HasNode("Item/Consume/0259.img/0" + ID + "/info/icon"))
                Bmp = Wz.GetBmp("Item/Consume/0259.img/0" + ID + "/info/icon");
            if (Wz.HasNode("String/Consume.img/" + ID))
                Name = Wz.GetStr("String/Consume.img/" + ID + "/name");
            SoulEffectListGrid.Rows.Add('0' + ID, Bmp, Name, Iter.GetInt("soulSkill"));
        }
        for (int i = 0; i < SoulEffectListGrid.Rows.Count; i++)
        {
            SoulEffectListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SoulEffectListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        ItemEffect.Remove(EffectType.Soul);
    }

    private void SoulEffectForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
      
         ActiveControl = null;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }
}
