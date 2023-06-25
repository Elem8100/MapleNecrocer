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

public partial class MedalForm : Form
{
    public MedalForm()
    {
        InitializeComponent();
        Instance = this;
    }

    public static MedalForm Instance;
    public DataGridViewEx MedalListGrid;

    private void MedalForm_Shown(object sender, EventArgs e)
    {

        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        MedalListGrid = new(90, 164, 0, 0, 220, 400, true, this);
        MedalListGrid.Dock = DockStyle.Fill;
        MedalListGrid.SearchGrid.Dock = DockStyle.Fill;

        var Graphic = MedalListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        string MedalName = null;
        Bitmap Bmp=null;
        foreach (var Img in Wz.GetNode("Character/Accessory").Nodes)
        {
            if (Img.Text.LeftStr(4) != "0114")
                continue;
            if (!Wz.HasNode("Character/Accessory/" + Img.Text + "/info/medalTag"))
                continue;
            int TagNum = Wz.GetNode("Character/Accessory/" + Img.Text + "/info/medalTag").ToInt();
            if (!Wz.HasNode("UI/NameTag.img/medal/" + TagNum))
                continue;
            string ID = Img.ImgID();
            if (Wz.HasNode("String/Eqp.img/Eqp/Accessory/" + ID.IntID()))
                MedalName = Wz.GetNode("String/Eqp.img/Eqp/Accessory/" + ID.IntID() + "/name").ToStr();
            if( Wz.HasNode("Character/Accessory/" + Img.Text + "/info/icon"))
                Bmp= Wz.GetNode("Character/Accessory/" + Img.Text + "/info/icon").ExtractPng();
            MedalListGrid.Rows.Add(ID, Bmp, MedalName);

        }

    }
}
