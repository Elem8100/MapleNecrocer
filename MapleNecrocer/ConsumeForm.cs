using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manina.Windows.Forms;

namespace MapleNecrocer;

public partial class ConsumeForm : Form
{
    public ConsumeForm()
    {
        InitializeComponent();
        Instance = this;
    }
    ImageListView ImageGrid;
    public static ConsumeForm Instance;

    private void ConsumeForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };


        ImageGrid = new ImageListView();
        ImageGrid.Parent = tabControl1.TabPages[0];
        ImageGrid.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
        ImageGrid.Dock = DockStyle.Fill;
        ImageGrid.BackColor = SystemColors.Window;
        ImageGrid.Colors.BackColor = SystemColors.ButtonFace;
        ImageGrid.Colors.SelectedBorderColor = Color.Red;
        ImageGrid.BorderStyle = BorderStyle.Fixed3D;
        ImageGrid.ThumbnailSize = new System.Drawing.Size(32, 32);
        //ImageGrid.t.t.show.CacheMode= CacheMode.Continuous;
        ImageGrid.ItemClick += (o, e) =>
        {
            Text = (e.Item.FileName);

        };


        var Graphic = ImageGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        Win32.SendMessage(ImageGrid.Handle, false);
        Bitmap Bmp = null;
        foreach (var Img in Wz.GetNodes("Item/Consume"))
        {
            if (!Char.IsNumber(Img.Text[0]))
                continue;
            foreach (var Iter in Wz.GetNodes("Item/Consume/" + Img.Text))
            {
                if (Iter.HasNode("info/icon"))
                    Bmp = Iter.GetBmp("info/icon");
                ImageGrid.Items.Add(Iter.Text, Bmp);
            }
        }
        ImageGrid.Sort();
        Win32.SendMessage(ImageGrid.Handle, true);
        ImageGrid.Refresh();
    }
}
