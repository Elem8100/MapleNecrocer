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

public partial class FamiliarForm : Form
{
    public FamiliarForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static FamiliarForm Instance;
    public DataGridViewEx FamiliarListGrid;
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


        string CardName = "";
        Bitmap Bmp = null;
        foreach (var Img in Wz.Nodes("Character/Familiar"))
        {
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
}
