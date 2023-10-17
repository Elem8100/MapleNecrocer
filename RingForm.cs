using WzComparerR2.WzLib;
namespace MapleNecrocer;

public partial class RingForm : Form
{
    public RingForm()
    {
        InitializeComponent();
    }
    public static RingForm Instance;
    public DataGridViewEx RingListGrid;
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
        NameTag.IsUse = false;
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        LabelRingTag.Delete();
        LabelRingTag.Create(ID);
        LabelRingTag.Instance.MedalName = Game.Player.Name;
        LabelRingTag.Instance.InitData();
        LabelRingTag.ReDraw();
    }
    private void RingForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
        RingListGrid = new(90, 179, 0, 0, 220, 400, true, panel1);
        RingListGrid.Dock = DockStyle.Fill;
        RingListGrid.SearchGrid.Dock = DockStyle.Fill;
        RingListGrid.RowTemplate.Height = 40;

        var Graphic = RingListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        RingListGrid.CellClick += (s, e) =>
        {
            CellClick(RingListGrid, e);
        };

        RingListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(RingListGrid.SearchGrid, e);
        };

        RingListGrid.SetToolTipEvent(WzType.Character,this);
       
        string RingName = null;
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNodes("Character/Ring"))
        {
            if (Img.Text.LeftStr(6) != "011121" && Img.Text.LeftStr(6) != "011151" && Img.Text.LeftStr(6) != "'011153")
                continue;
            if (!Wz.HasNode("Character/Ring/" + Img.Text + "/info/nameTag"))
                continue;
            int TagNum = Wz.GetInt("Character/Ring/" + Img.Text + "/info/nameTag");
            if (!Wz.HasNode("UI/NameTag.img/" + TagNum))
                continue;
            string ID = Img.ImgID();
            if (Wz.HasNode("String/Eqp.img/Eqp/Ring/" + ID.IntID()))
                RingName = Wz.GetStr("String/Eqp.img/Eqp/Ring/" + ID.IntID() + "/name");
            else if (Wz.HasNode("String/Item.img/Eqp/Ring/" + ID.IntID()))
                RingName = Wz.GetStr("String/Item.img/Eqp/Ring/" + ID.IntID() + "/name");
            if (Wz.HasNode("Character/Ring/" + Img.Text + "/info/icon"))
                Bmp = Wz.GetBmp("Character/Ring/" + Img.Text + "/info/icon");
            RingListGrid.Rows.Add(ID, Bmp, RingName);
        }
        for (int i = 0; i < RingListGrid.Rows.Count; i++)
        {
            RingListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            RingListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        NameTag.IsUse = true;
        LabelRingTag.Delete();
    }
    private void RingForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        RingListGrid.Search(textBox1.Text);
    }

    private void RingForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
    }
}
