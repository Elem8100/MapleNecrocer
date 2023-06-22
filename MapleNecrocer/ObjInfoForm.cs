using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.WzLib;

namespace MapleNecrocer;

public partial class ObjInfoForm : Form
{
    public ObjInfoForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static ObjInfoForm Instance;
    public DataGridViewEx ObjInfoListGrid;
    Dictionary<string, Image> Images = new Dictionary<string, Image>();

    public void DumpObjs()
    {
        Images.Clear();
        //obj
        for (int Layer = 0; Layer <= 7; Layer++)
        {
            foreach (var Iter in Map.Img.GetNode(Layer.ToString() + "/obj").Nodes)
            {
                var oS = Iter.GetValue2("oS", "");
                var L0 = Iter.GetValue2("l0", "");
                var L1 = Iter.GetValue2("l1", "");
                var L2 = Iter.GetValue2("l2", "");
                if (oS == "")
                    continue;
                var ObjNode = Wz.GetNode("Map/Obj/" + oS + ".img/" + L0 + "/" + L1 + "/" + L2);
                if (ObjNode != null)
                {
                    foreach (var Iter2 in ObjNode.Nodes)
                    {
                        if (Wz.GetNode(Iter2.FullPathToFile2()).Value is Wz_Png)
                            Images.AddOrReplace(Iter2.FullPathToFile2(), Wz.GetNode(Iter2.FullPathToFile2()).ExtractPng());
                    }
                }
            }
        }
        //back
        Wz_Node BackEntry, AniBackEntry;
        foreach (var Iter in Map.Img.GetNode("back").Nodes)
        {
            var bS = Iter.GetValue2("bS", "");
            if (bS == "")
                continue;
            var No = Iter.GetValue2("no", "0");
            var Ani = Iter.GetValue2("ani", "0");
            if (int.Parse(Ani) > 1)
                Ani = "0";
            if (Ani == "0")
            {
                BackEntry = Wz.GetNode("Map/Back/" + bS + ".img/back/" + No);
                if (BackEntry == null)
                    continue;
                Images.AddOrReplace(BackEntry.FullPathToFile2(), Wz.GetNode(BackEntry.FullPathToFile2()).ExtractPng());
            }

            if (Ani == "1")
            {
                AniBackEntry = Wz.GetNode("Map/Back/" + bS + ".img/ani/" + No);
                if (AniBackEntry == null)
                    continue;
                foreach (var Iter2 in AniBackEntry.Nodes)
                {
                    Images.AddOrReplace(Iter2.FullPathToFile2(), Wz.GetNode(Iter2.FullPathToFile2()).ExtractPng());
                }
            }
        }

        //tile
        var tS = "";
        for (int Layer = 0; Layer <= 7; Layer++)
        {
            var tSNode = Map.Img.GetNode(Layer.ToString() + "/info/tS");
            if (tSNode == null)
                continue;
            else
                tS = tSNode.Value.ToString();
            foreach (var Iter in Map.Img.GetNode(Layer.ToString() + "/tile").Nodes)
            {
                var u = Iter.GetValue2("u", "");
                var no = Iter.GetValue2("no", "");
                var Entry = Wz.GetNode("Map/Tile/" + tS + ".img/" + u + "/" + no);
                if (Entry != null)
                {
                    if (!Images.ContainsKey(Entry.FullPathToFile2()))
                        Images.Add(Entry.FullPathToFile2(), Wz.GetNode(Entry.FullPathToFile2()).ExtractPng());
                }
            }
        }

        LoadImages(dataGridView1, 73, true);
    }

    void LoadImages(DataGridView dataViewImages, int GridSize, bool Resize = false)
    {
        dataViewImages.Rows.Clear();
        dataViewImages.Columns.Clear();
        // dataViewImages.Refresh();
        int numColumnsForWidth = (dataViewImages.Width - 10) / (GridSize + 20);
        int numRows = 0;
        int numImages = Images.Count;
        numRows = numImages / numColumnsForWidth;

        if (numImages % numColumnsForWidth > 0)
        {
            numRows += 1;
        }
        if (numImages < numColumnsForWidth)
        {
            numColumnsForWidth = numImages;
        }
        int numGeneratedCells = numRows * numColumnsForWidth;
        // Dynamically create the columns
        for (int index = 0; index < numColumnsForWidth; index++)
        {
            DataGridViewImageColumn dataGridViewColumn = new DataGridViewImageColumn();
            dataViewImages.Columns.Add(dataGridViewColumn);
            dataViewImages.Columns[index].Width = GridSize + 20;
        }
        // Create the rows
        for (int index = 0; index < numRows; index++)
        {
            dataViewImages.Rows.Add();
            dataViewImages.Rows[index].Height = GridSize + 20;
        }

        int columnIndex = 0;
        int rowIndex = 0;
        Image image;
        foreach (var Iter in Images.Keys)
        {
            image = Images[Iter];
            if (Resize)
            {
                if (image.Width > 90 || image.Height > 90)
                    image = ResizeImage2(image, 70, 70);
            }
            dataViewImages.Rows[rowIndex].Cells[columnIndex].Value = image;
            dataViewImages.Rows[rowIndex].Cells[columnIndex].ToolTipText = Iter.ToString();

            if (columnIndex == numColumnsForWidth - 1)
            {
                rowIndex++;
                columnIndex = 0;
            }
            else
            {
                columnIndex++;
            }
        }
    }
    public Bitmap ResizeImage2(Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);
        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }
        return destImage;
    }

    private void ObjInfoForm_Shown(object sender, EventArgs e)
    {
        DumpObjs();
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        label1.Text = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var dlg = new FolderBrowserDialog();
        dlg.SelectedPath = System.Environment.CurrentDirectory;
        dlg.Description = "選擇儲存路徑";
        if ((dlg.ShowDialog(new Form() { TopMost = true }) != DialogResult.OK))
            return;
        var Path1 = label1.Text;
        var Path2 = Path1;
        Path2 = Path2.Replace("/", ".");
        Images[Path1].Save(dlg.SelectedPath + "\\" + Path2 + ".png");
    }

    private void button2_Click(object sender, EventArgs e)
    {
        var dlg = new FolderBrowserDialog();
        dlg.SelectedPath = System.Environment.CurrentDirectory;
        dlg.Description = "選擇儲存路徑";
        if ((dlg.ShowDialog(new Form() { TopMost = true }) != DialogResult.OK))
            return;
        foreach (var Iter in Images.Keys)
        {
            var Path = Iter.ToString();
            Path = Path.Replace("/", ".");
            Images[Iter].Save(dlg.SelectedPath + "\\" + Path + ".png");
        }
    }
}
