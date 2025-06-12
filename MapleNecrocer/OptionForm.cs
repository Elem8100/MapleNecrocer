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

public partial class OptionForm : Form
{
    public OptionForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static OptionForm Instance;
    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox1.Checked)
        {
            Sound.isMute = true;
            Music.Pause();
        }
        else
        {
            Sound.isMute = false;
            Music.Resume();
        }
    }

    private void OptionForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
    }

    private void OptionForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;

        ActiveControl = null;
    }
}
