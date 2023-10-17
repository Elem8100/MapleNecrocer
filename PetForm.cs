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

public partial class PetForm : Form
{
    public PetForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static PetForm Instance;
    public DataGridViewEx PetListGrid, PetEquipListGrid;
    public static string PetID;
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x02000000;
            return cp;
        }
    }

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        PetID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        PetNameTag.Remove();
        Pet.Remove();
        PetEquip.Remove();
        Pet.Create(PetID);

        if (PetID == "5002120" || PetID == "5002125" || PetID == "5002126" || PetID == "5002189" || PetID == "5002190")
        {
            return;
        }
        else
        {
            if (Wz.HasNode("UI/NameTag.img/pet"))
            {
                PetNameTag.Create(PetID);
                PetNameTag.Instance.MedalName = DataGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                PetNameTag.Instance.InitData();
                PetNameTag.ReDraw();
            }
        }

        PetEquipListGrid.Rows.Clear();
        string EquipName = null;
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNodes("Character/PetEquip"))
        {
            foreach (var Iter in Wz.GetNodes("Character/PetEquip/" + Img.Text))
            {
                if (PetID != "" && Iter.Text == PetID)
                {
                    string ID = Img.ImgID();
                    if (Wz.HasNode("String/Eqp.img/Eqp/PetEquip/" + ID.IntID()))
                        EquipName = Wz.GetStr("String/Eqp.img/Eqp/PetEquip/" + ID.IntID() + "/name");
                    Bmp = Wz.GetBmp("Character/PetEquip/" + Img.Text + "/info/icon");
                    PetEquipListGrid.Rows.Add(ID, Bmp, EquipName);
                }
            }
        }

        for (int i = 0; i < PetEquipListGrid.Rows.Count; i++)
        {
            PetEquipListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PetEquipListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }
    }

    void CellClick2(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        PetEquip.Remove();
        PetEquip.Create(ID);
    }

    private void PetForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
        PetListGrid = new(80, 179, 0, 0, 220, 400, true, panel1);

        PetListGrid.Dock = DockStyle.Fill;
        PetListGrid.SearchGrid.Dock = DockStyle.Fill;
        PetListGrid.RowTemplate.Height = 40;

        PetEquipListGrid = new(90, 179, 0, 0, 220, 400, true, panel2);
        PetEquipListGrid.Dock = DockStyle.Fill;
        PetEquipListGrid.SearchGrid.Dock = DockStyle.Fill;
        PetEquipListGrid.RowTemplate.Height = 40;

        var Graphic = PetListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        PetListGrid.CellClick += (s, e) =>
        {
            CellClick(PetListGrid, e);
        };

        PetListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(PetListGrid.SearchGrid, e);
        };

        PetEquipListGrid.CellClick += (s, e) =>
        {
            CellClick2(PetEquipListGrid, e);
        };

        PetListGrid.SetToolTipEvent(WzType.Item, this);

        string PetName = null;
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNodes("Item/Pet"))
        {
            if (!Char.IsNumber(Img.Text, 0))
                continue;
            string ID = Img.ImgID();
            if (Wz.HasNode("String/Pet.img/" + ID))
                PetName = Wz.GetStr("String/Pet.img/" + ID + "/name");
            if (Wz.HasNode("String/Item.img/Pet/" + ID))
                PetName = Wz.GetStr("String/Item.img/Pet/" + ID + "/name");
            Bmp = Wz.GetBmp("Item/Pet/" + Img.Text + "/info/iconD");
            PetListGrid.Rows.Add(ID, Bmp, PetName);
        }
        for (int i = 0; i < PetListGrid.Rows.Count; i++)
        {
            PetListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            PetListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }
    }

    private void PetForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        PetListGrid.Search(textBox1.Text);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Pet.Remove();
        PetNameTag.Remove();
    }

    private void PetForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
    }
}
