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

public partial class EffectForm : Form
{
    public EffectForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static EffectForm Instance;
    public DataGridViewEx EffectListGrid;


    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        string Path = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        NormalEffect.Remove();
        NormalEffect.Create(Path);

    }
    private void EffectForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        EffectListGrid = new(290, 64, 0, 0, 220, 400, false, panel1);
        EffectListGrid.Dock = DockStyle.Fill;
        EffectListGrid.SearchGrid.Dock = DockStyle.Fill;

        var Graphic = EffectListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        EffectListGrid.CellClick += (s, e) =>
        {
            CellClick(EffectListGrid, e);
        };

        EffectListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(EffectListGrid.SearchGrid, e);
        };

        Win32.SendMessage(EffectListGrid.Handle, false);

        if (Wz.HasNode("Effect/CharacterEff.img/MeisterEff"))
        {
            foreach (var Iter in Wz.GetNodes("Effect/CharacterEff.img/MeisterEff"))
            {
                EffectListGrid.Rows.Add(Iter.FullPathToFile2());
            }
        }

       // if (Wz.HasNode("Effect/CharacterEff.img/LevelUpHyper"))
       // {
       //   EffectListGrid.Rows.Add("Effect/CharacterEff.img/LevelUpHyper");
        //}
        Win32.SendMessage(EffectListGrid.Handle, true);
        EffectListGrid.Refresh();
    }

    private void listBox1_SelectedValueChanged(object sender, EventArgs e)
    {

    }

    private void EffectForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        ActiveControl = null;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        NormalEffect.Remove();
    }
}
