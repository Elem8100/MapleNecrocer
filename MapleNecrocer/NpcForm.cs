using WzComparerR2.WzLib;
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
    string NpcID;

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        NpcID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        var Link = Wz.GetNode("Npc/" + NpcID + ".img/info/link");
        if (Link != null)
        {
            NpcID = Link.ToStr();

        }
        Bitmap Bitmap;
        if (Wz.GetNodeA("Npc/" + NpcID + ".img/stand/0") != null)
            Bitmap = Wz.GetNode("Npc/" + NpcID + ".img/stand/0").ExtractPng();
        else if ((Wz.GetNodeA("Npc/" + NpcID + ".img/fly/0") != null))
            Bitmap = Wz.GetNode("Npc/" + NpcID + ".img/fly/0").ExtractPng();
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

        NpcListGrid.SetToolTipEvent(WzType.Npc, this);

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

    private void button1_Click(object sender, EventArgs e)
    {

        if (Wz.GetNode("Npc/" + NpcID + ".img") == null)
            return;
        Random Random = new Random();
        Random RandomFlip = new Random();
        int Flip = RandomFlip.Next(0, 2);
        int Range = Random.Next((int)Game.Player.X - 100, (int)Game.Player.X + 100);
        if (Range > Map.Left && Range < Map.Right)
        {
            Npc.Spawn(NpcID, Range, (int)Game.Player.Y - 100, Flip);
            Npc.SummonedList.Add(NpcID);
        }

    }

    private void button2_Click(object sender, EventArgs e)
    {
        foreach (var Iter in EngineFunc.SpriteEngine.SpriteList)
        {
            if (Iter is Npc)
            {
                for (int I = 0; I < Npc.SummonedList.Count; I++)
                {
                    if (((Npc)Iter).LocalID == Npc.SummonedList[I])
                    {
                        Iter.Dead();
                        if (((Npc)Iter).Balloon != null)
                            ((Npc)Iter).Balloon.Dead();

                    }
                }
            }

            if (Iter is NpcText)
            {
                for (int I = 0; I < Npc.SummonedList.Count; I++)
                {
                    if (((NpcText)Iter).ID.RightStr(7) == Npc.SummonedList[I])
                    {
                        Iter.Dead();
                    }
                }
            }
        }
        EngineFunc.SpriteEngine.Dead();
    }

    private void NpcForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox2.Focused)
            ActiveControl = null;
    }

    private void NpcForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
    }
}
