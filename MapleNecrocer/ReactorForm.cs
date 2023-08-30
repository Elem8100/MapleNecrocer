using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MapleNecrocer;

public partial class ReactorForm : Form
{
    public ReactorForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static ReactorForm Instance;
    public DataGridViewEx ReactorListGrid;
    string ReactorID;

    private void ReactorForm_Shown(object sender, EventArgs e)
    {
        ReactorListGrid = new(65, 195, 0, 20, 220, 530, false, panel1);
        ReactorListGrid.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
        ReactorListGrid.Dock = DockStyle.Fill;
        ReactorListGrid.SearchGrid.Dock = DockStyle.Fill;
        ReactorListGrid.RowTemplate.Height = 20;
        var Graphic = ReactorListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            ReactorID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (Wz.HasNode("Reactor/" + ReactorID + ".img/0/0"))
            {
                var Bmp = Wz.GetBmp("Reactor/" + ReactorID + ".img/0/0");
                pictureBox1.Image = Bmp;
            }
        }

        ReactorListGrid.CellClick += (s, e) =>
        {
            CellClick(ReactorListGrid, e);
        };
        ReactorListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(ReactorListGrid.SearchGrid, e);
        };


        Win32.SendMessage(ReactorListGrid.Handle, false);
        foreach (var Img in Wz.GetNodes("Reactor"))
        {
            var Entry = Wz.GetNode("Reactor/" + Img.Text);
            if (!Entry.HasNode("0") || !Entry.HasNode("0/0"))
                continue;
            if ((Entry.HasNode("0/0")) && (!Entry.HasNode("0/0/_inlink")) && (Entry.GetBmp("0/0").Width <= 4))
                continue;
            string ReactorName = "";
            if (Entry.HasNode("info/info"))
                ReactorName = Entry.GetStr("info/info");
            else if (Entry.HasNode("info/viewName"))
                ReactorName = Entry.GetStr("info/viewName");
            ReactorListGrid.Rows.Add(Img.ImgID(), ReactorName);
        }
        Win32.SendMessage(ReactorListGrid.Handle, true);
        ReactorListGrid.Refresh();

    }

    private void button1_Click(object sender, EventArgs e)
    {
        Reactor.Create(ReactorID);
    }

    private void button2_Click(object sender, EventArgs e)
    {
        Reactor.Remove();
    }

    private void ReactorForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused)
            ActiveControl = null;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        ReactorListGrid.Search(textBox1.Text);
    }
}
