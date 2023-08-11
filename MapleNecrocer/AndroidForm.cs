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

public partial class AndroidForm : Form
{
    public AndroidForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static AndroidForm Instance;
    public DataGridViewEx AndroidListGrid;

    private void AndroidForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        AndroidListGrid = new(80, 185, 0, 0, 220, 400, true, tabControl1.TabPages[0]);
        AndroidListGrid.Dock = DockStyle.Fill;
        AndroidListGrid.SearchGrid.Dock = DockStyle.Fill;
        AndroidListGrid.RowTemplate.Height = 40;

        var Graphic = AndroidListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        string AndroidName = "";
        Bitmap Bmp = null;
        foreach (var Img in Wz.Nodes("Character/Android"))
        {
            string ID = Img.ImgID();
            if (Wz.HasNode("String/Eqp.img/Eqp/android/" + ID.IntID()))
                AndroidName = Wz.GetStr("String/Eqp.img/Eqp/android/" + ID.IntID() + "/name");



        }
    }
}
