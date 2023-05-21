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

public partial class MobForm : Form
{
    public MobForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static MobForm Instance;
    public DataGridViewEx MobListGrid;

    private void MobForm_Load(object sender, EventArgs e)
    {



    }
    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        var Link = Wz.GetNode("Mob/" + ID + ".img/info/link");
        if (Link != null)
        {
            ID = Link.ToStr();

        }
        Bitmap Bitmap;
        if (Wz.GetNodeA("Mob/" + ID + ".img/stand/0") != null)
            Bitmap = Wz.GetNode("Mob/" + ID + ".img/stand/0").ExtractPng();
        else if ((Wz.GetNodeA("Mob/" + ID + ".img/fly/0") != null))
            Bitmap = Wz.GetNode("Mob/" + ID + ".img/fly/0").ExtractPng();
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
       
        string ID = null;
        string Name = null;
        Win32.SendMessage(MobListGrid.Handle, false);
        foreach (var Iter in MainForm.TreeNode.Nodes["Mob"].Nodes)
        {
            if (!Char.IsNumber(Iter.Text, 0))
                continue;
            ID = Iter.Text.LeftStr(7);
            Name = Wz.GetNodeA("String/Mob.img/" + ID.IntID()).GetStr("name");
            MobListGrid.Rows.Add(ID, Name);

        }
        Win32.SendMessage(MobListGrid.Handle, true);
        MobListGrid.Refresh();
    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {
        MobListGrid.Search(textBox2.Text);
    }
}
