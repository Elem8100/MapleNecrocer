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

public partial class SkillForm : Form
{
    public SkillForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static SkillForm Instance;
    public DataGridViewEx SkillListGrid;

    private void SkillForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
        SkillListGrid = new(80, 185, 0, 0, 220, 400, true, panel1);
        SkillListGrid.Dock = DockStyle.Fill;
        SkillListGrid.SearchGrid.Dock = DockStyle.Fill;
        SkillListGrid.RowTemplate.Height = 40;


        var Graphic = SkillListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        foreach (var Img in Wz.Nodes("Skill"))
        {
            if (!Char.IsNumber(Img.Text[0]))
                continue;
            if (Img.Text[0] == 0)
                continue;
            if (!Wz.HasNode("Skill/" + Img.Text + "/skill"))
                continue;
            foreach (var ID in Wz.Nodes("Skill/" + Img.Text + "/skill"))
            {

                if (!ID.HasNode("hit"))
                    continue;
                if (!ID.HasNode("common"))
                    continue;
                if (!ID.HasNode("common/lt"))
                    continue;
                if (!ID.HasNode("effect"))
                    continue;
                Bitmap Bmp = Wz.GetBmp("Skill/" + Skill.GetJobImg(ID.Text) + ".img/skill/" + ID.Text+"/icon");
                string SkillName = "";
                if (Wz.HasNode("String/Skill.img/" + ID.Text))
                    SkillName = Wz.GetStr("String/Skill.img/" + ID.Text + "/name");
                SkillListGrid.Rows.Add(ID, Bmp, SkillName);

            }

        }



    }
}
