
namespace MapleNecrocer;

public partial class FamiliarForm : Form
{
    public FamiliarForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static FamiliarForm Instance;
    public DataGridViewEx FamiliarListGrid;

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        string ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        if (Wz.HasNode("Etc/FamiliarInfo.img/" + ID + "/mob"))
            ID = Wz.GetStr("Etc/FamiliarInfo.img/" + ID + "/mob");
        else
            ID = Wz.GetStr("Character/Familiar/" + ID + ".img/" + "info/MobID");
        Familiar.Remove();
        ID = ID.PadLeft(7, '0');
        Familiar.Create(ID);
        FamiliarNameTag.Remove();
        FamiliarNameTag.Create("");
        FamiliarNameTag.MobName = Wz.GetStr("String/Mob.img/" + ID + "/name", ID);
        FamiliarNameTag.ReDraw = true;

    }
    private void FamiliarForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
        FamiliarListGrid = new(80, 185, 0, 0, 220, 400, true, panel1);
        FamiliarListGrid.Dock = DockStyle.Fill;
        FamiliarListGrid.SearchGrid.Dock = DockStyle.Fill;
        FamiliarListGrid.RowTemplate.Height = 40;
     

        var Graphic = FamiliarListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        FamiliarListGrid.CellClick += (s, e) =>
        {
            CellClick(FamiliarListGrid, e);
        };

        FamiliarListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(FamiliarListGrid.SearchGrid, e);
        };


        string CardName = "";
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNodes("Character/Familiar"))
        {
           if(Img.Text.RightStr(4)!=".img")
                continue;
            string ID = Img.ImgID();
            string CardID = "";

            if (Wz.HasNode("Etc/FamiliarInfo.img/" + ID))
            {
                CardID = Wz.GetStr("Etc/FamiliarInfo.img/" + ID + "/consume");
            }
            else if (Wz.HasNode("Character/Familiar/" + Img.Text + "/info/monsterCardID"))
            {
                CardID = Wz.GetStr("Character/Familiar/" + Img.Text + "/info/monsterCardID");
            }
            else
                continue;

            if (Wz.HasNode("String/Consume.img/" + CardID))
                CardName = Wz.GetStr("String/Consume.img/" + CardID + "/name");
            if (Wz.HasNode("Item/Consume/0287.img/" + "0" + CardID + "/info/icon"))
                Bmp = Wz.GetBmp("Item/Consume/0287.img/" + "0" + CardID + "/info/icon");
            else if (Wz.HasNode("Item/Consume/0238.img/" + "0" + CardID + "/info/iconRaw"))
                Bmp = Wz.GetBmp("Item/Consume/0238.img/" + "0" + CardID + "/info/iconRaw");
            FamiliarListGrid.Rows.Add(ID, Bmp, CardName);
        }
        for (int i = 0; i < FamiliarListGrid.Rows.Count; i++)
        {
            FamiliarListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            FamiliarListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }

    }

    private void FamiliarForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        FamiliarListGrid.Search(textBox1.Text);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        FamiliarNameTag.Remove();
        Familiar.Remove();
    }
}
