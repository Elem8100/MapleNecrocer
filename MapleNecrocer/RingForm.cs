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

public partial class RingForm : Form
{
    public RingForm()
    {
        InitializeComponent();
    }
    public static RingForm Instance;
    public DataGridViewEx RingListGrid;
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

        string RingName = null;
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNode("Character/Ring").Nodes)
        {
            if (Img.Text.LeftStr(6) != "011121" && Img.Text.LeftStr(6) != "011151" && Img.Text.LeftStr(6) != "'011153")
                continue;
            if (!Wz.HasNode("Character/Ring/" + Img.Text + "/info/nameTag"))
                continue;
            int TagNum = Wz.GetNode("Character/Ring/" + Img.Text + "/info/nameTag").ToInt();
            if (!Wz.HasNode("UI/NameTag.img/" + TagNum))
                continue;
            string ID = Img.ImgID();
            if (Wz.HasNode("String/Eqp.img/Eqp/Ring/" + ID.IntID()))
                RingName = Wz.GetNode("String/Eqp.img/Eqp/Ring/" + ID.IntID() + "/name").ToStr();
            if (Wz.HasNode("Character/Ring/" + Img.Text + "/info/icon"))
                Bmp = Wz.GetNode("Character/Ring/" + Img.Text + "/info/icon").ExtractPng();
            RingListGrid.Rows.Add(ID, Bmp, RingName);
        }
        for (int i = 0; i < RingListGrid.Rows.Count; i++)
        {
            RingListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            RingListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }


    }
}
