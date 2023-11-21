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

public partial class EffectRingForm : Form
{
    public EffectRingForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static EffectRingForm Instance;
    public DataGridViewEx EffectRingListGrid;

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        if (ID == "01112127" || ID == "01112804" || ID == "01113021" || ID == "01113228")
            return;
        foreach (var i in SetEffect.UseList)
            SetEffect.Remove(i.Key);
        ItemEffect.Remove(EffectType.Ring);
        if (ItemEffect.AllList.Contains(ID))
            ItemEffect.Create(ID, EffectType.Equip);
        if (SetEffect.AllList.ContainsKey(ID))
            SetEffect.Create(ID);
    }
    private void EffectRingForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        EffectRingListGrid = new(90, 179, 0, 0, 220, 400, true, panel1);
        EffectRingListGrid.Dock = DockStyle.Fill;
        EffectRingListGrid.SearchGrid.Dock = DockStyle.Fill;
        EffectRingListGrid.RowTemplate.Height = 40;

        var Graphic = EffectRingListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        EffectRingListGrid.CellClick += (s, e) =>
        {
            CellClick(EffectRingListGrid, e);
        };

        EffectRingListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(EffectRingListGrid.SearchGrid, e);
        };

        EffectRingListGrid.SetToolTipEvent(WzType.Character, this);

        foreach (var Iter in Wz.GetNodes("Effect/ItemEff.img"))
        {
            if (Iter.Text.LeftStr(3) != "111")
                continue;
            string ID = '0' + Iter.Text;
            if (!Wz.HasNode("Character/Ring/" + ID + ".img"))
                continue;
            string EffectRingName = Wz.GetStr("String/Eqp.img/Eqp/Ring/" + ID.IntID() + "/name");
            Bitmap Icon = null;
            if (Wz.HasNode("Character/Ring/" + ID + ".img/info/icon"))
                Icon = Wz.GetBmp("Character/Ring/" + ID + ".img/info/icon");
            EffectRingListGrid.Rows.Add(ID, Icon, EffectRingName);
        }

        foreach (var Iter in Wz.GetNodes("Effect/SetEff.img"))
        {
            foreach (var Iter2 in Iter.Nodes)
            {
                if (Iter2.Text == "info")
                {
                    foreach (var Iter3 in Iter2.Nodes)
                    {
                        foreach (var Iter4 in Iter3.Nodes)
                        {
                            if (Iter4.Value.ToString().LeftStr(3) == "111")
                            {
                                string ID = "0" + Iter4.Value.ToString();
                                if (!Wz.HasNode("Character/Ring/" + ID + ".img"))
                                    continue;
                                string EffectRingName = Wz.GetStr("String/Eqp.img/Eqp/Ring/" + ID.IntID() + "/name");
                                Bitmap Icon = null;
                                if (Wz.HasNode("Character/Ring/" + ID + ".img/info/icon"))
                                    Icon = Wz.GetBmp("Character/Ring/" + ID + ".img/info/icon");
                                EffectRingListGrid.Rows.Add(ID, Icon, EffectRingName);
                            }
                        }
                    }
                }
            }
        }

    }

    private void button1_Click(object sender, EventArgs e)
    {
        foreach (var i in SetEffect.UseList)
            SetEffect.Remove(i.Key);
        ItemEffect.Remove(EffectType.Ring);
    }

    private void EffectRingForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }

    private void EffectRingForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        EffectRingListGrid.Search(textBox1.Text);
    }
}
