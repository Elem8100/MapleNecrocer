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

public partial class NpcForm : Form
{
    public NpcForm()
    {
        InitializeComponent();
        Instance = this;
    }

    public static NpcForm Instance;
    public DataGridViewEx NpcListGrid;


    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        var Link = Wz.GetNode("Npc/" + ID + ".img/info/link");
        if (Link != null)
        {
            ID = Link.ToStr();

        }
        Bitmap Bitmap;
        if (Wz.GetNodeA("Npc/" + ID + ".img/stand/0") != null)
            Bitmap = Wz.GetNode("Npc/" + ID + ".img/stand/0").ExtractPng();
        else if ((Wz.GetNodeA("Npc/" + ID + ".img/fly/0") != null))
            Bitmap = Wz.GetNode("Npc/" + ID + ".img/fly/0").ExtractPng();
        else
            return;
        pictureBox1.Image = Bitmap;


    }

    private void NpcForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };


        NpcListGrid = new(60, 164, 0, 0, 220, 400, false, tabControl1.TabPages[0]);
        NpcListGrid.Dock = DockStyle.Fill;
        NpcListGrid.SearchGrid.Dock = DockStyle.Fill;

        var Graphic = NpcListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);


        NpcListGrid.CellClick += (s, e) =>
        {
            CellClick(NpcListGrid, e);
        };

        NpcListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(NpcListGrid.SearchGrid, e);
        };

        string ID = null;
        string Name = null;
        Win32.SendMessage(NpcListGrid.Handle, false);
        foreach (var Iter in MainForm.TreeNode.Nodes["Npc"].Nodes)
        {
            if (!Char.IsNumber(Iter.Text, 0))
                continue;
            ID = Iter.Text.LeftStr(7);
            if (Wz.GetNodeA("String/Npc.img/" + ID.IntID()) != null)
                Name = Wz.GetNodeA("String/Npc.img/" + ID.IntID()).GetStr("name");
            NpcListGrid.Rows.Add(ID, Name);

        }
        Win32.SendMessage(NpcListGrid.Handle, true);
        NpcListGrid.Refresh();



    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {
        NpcListGrid.Search(textBox2.Text);
    }
}
