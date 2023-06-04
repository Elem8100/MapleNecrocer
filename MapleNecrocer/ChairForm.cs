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

public partial class ChairForm : Form
{
    public ChairForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static ChairForm Instance;
    public DataGridViewEx ChairListGrid;

    private void ChairForm_Load(object sender, EventArgs e)
    {

    }

    private void ChairForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        ChairListGrid = new(60, 164, 0, 0, 220, 400, true, tabControl1.TabPages[0]);
        ChairListGrid.Dock = DockStyle.Fill;
        ChairListGrid.SearchGrid.Dock = DockStyle.Fill;



    }
}
