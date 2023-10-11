using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.CharaSim;
using WzComparerR2.WzLib;

namespace MapleNecrocer;

public partial class MobForm : Form
{
    public MobForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static MobForm Instance;
    public DataGridViewEx MobListGrid;
    string MobID;
    private void MobForm_Load(object sender, EventArgs e)
    {

    }

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        MobID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        var Link = Wz.GetNode("Mob/" + MobID + ".img/info/link");
        if (Link != null)
        {
            MobID = Link.ToStr();
        }
        Bitmap Bitmap;
        if (Wz.GetNodeA("Mob/" + MobID + ".img/stand/0") != null)
            Bitmap = Wz.GetNode("Mob/" + MobID + ".img/stand/0").ExtractPng();
        else if ((Wz.GetNodeA("Mob/" + MobID + ".img/fly/0") != null))
            Bitmap = Wz.GetNode("Mob/" + MobID + ".img/fly/0").ExtractPng();
        else
            return;
        pictureBox1.Image = Bitmap;
    }

    private void MobForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
        MobListGrid = new(60, 164, 0, 0, 220, 400, false, tabControl1.TabPages[0]);
        MobListGrid.Dock = DockStyle.Fill;
        MobListGrid.SearchGrid.Dock = DockStyle.Fill;

        var Graphic = MobListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        MobListGrid.CellClick += (s, e) =>
        {
            CellClick(MobListGrid, e);
        };

        MobListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(MobListGrid.SearchGrid, e);
        };

        MobListGrid.CellMouseEnter += (s, e) =>
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                MobListGrid[0, e.RowIndex].Style.BackColor = Color.LightCyan;
                MobListGrid[1, e.RowIndex].Style.BackColor = Color.LightCyan;
            }
            string MobID = MobListGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
            Wz_Node Node = Wz.GetIDNode(MobID, WzType.Mob);
            MainForm.Instance.QuickView(Node);
        };

        MobListGrid.CellMouseLeave += (s, e) =>
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                MobListGrid[0, e.RowIndex].Style.BackColor = Color.White;
                MobListGrid[1, e.RowIndex].Style.BackColor = Color.White;
            }
        };

        MobListGrid.SearchGrid.CellMouseEnter += (s, e) =>
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                MobListGrid.SearchGrid[0, e.RowIndex].Style.BackColor = Color.LightCyan;
                MobListGrid.SearchGrid[1, e.RowIndex].Style.BackColor = Color.LightCyan;
            }
            string MobID = MobListGrid.SearchGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
            Wz_Node Node = Wz.GetIDNode(MobID, WzType.Mob);
            MainForm.Instance.QuickView(Node);
            MainForm.Instance.ToolTipView.Owner = this;
        };

        MobListGrid.SearchGrid.CellMouseLeave += (s, e) =>
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                MobListGrid.SearchGrid[0, e.RowIndex].Style.BackColor = Color.White;
                MobListGrid.SearchGrid[1, e.RowIndex].Style.BackColor = Color.White;
            }
        };

        string ID = null;
        string Name = null;
        Win32.SendMessage(MobListGrid.Handle, false);
        foreach (var Iter in MainForm.TreeNode.Nodes["Mob"].Nodes)
        {
            if (!Char.IsNumber(Iter.Text, 0))
                continue;
            ID = Iter.Text.LeftStr(7);
            Name = Wz.GetStr("String/Mob.img/" + ID.IntID() + "/name");
            MobListGrid.Rows.Add(ID, Name);
        }
        Win32.SendMessage(MobListGrid.Handle, true);
        MobListGrid.Refresh();
    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {
        MobListGrid.Search(textBox2.Text);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (Wz.GetNode("Mob/" + MobID + ".img") == null)
            return;
        Random Random = new Random();
        if (Char.IsNumber(textBox1.Text[0]) && (textBox1.Text != ""))
        {
            for (int I = 0; I < textBox1.Text.ToInt(); I++)
            {
                int Range = Random.Next((int)Game.Player.X - 100, (int)Game.Player.X + 100);
                if (Range > Map.Left && Range < Map.Right)
                {
                    Mob.Spawn(MobID, Range, (int)Game.Player.Y - 100, Map.Left, Map.Right);
                    Mob.SummonedList.Add(MobID);
                }
            }
        }
    }

    private void button2_Click(object sender, EventArgs e)
    {
        foreach (var Iter in EngineFunc.SpriteEngine.SpriteList)
        {
            if (Iter is Mob)
            {
                for (int I = 0; I < Mob.SummonedList.Count; I++)
                {
                    if (((Mob)Iter).LocalID == Mob.SummonedList[I])
                    {
                        Iter.Dead();
                        Mob.MobList.Remove(Mob.SummonedList[I]);
                    }
                }
            }
        }
        EngineFunc.SpriteEngine.Dead();
    }

    private void MobForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox2.Focused)
            ActiveControl = null;

    }

    private void MobForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
    }
}
