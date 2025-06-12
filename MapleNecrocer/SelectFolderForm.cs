using DevComponents.DotNetBar.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.PluginBase;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MapleNecrocer;

public partial class SelectFolderForm : Form
{
    public static partial class Directory
    {
        public static System.Collections.Generic.IEnumerable<string>
        EnumerateFiles
        (
          string Directory
        ,
          params string[] Pattern
        )
        {
            if ((Pattern == null) || (Pattern.Length == 0))
            {
                Pattern = new string[] { "*" };
            }

            foreach (string l in Pattern)
            {
                foreach (string p in l.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries))
                {
                    foreach (string f in System.IO.Directory.EnumerateFiles(Directory, p, SearchOption.AllDirectories))
                    {
                        yield return f;
                    }
                }
            }

            yield break;
        }
    }



    public SelectFolderForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static SelectFolderForm Instance;
    private void OpenWzForm_Load(object sender, EventArgs e)
    {

        RecentFilesGrid.Rows.Clear();
        var listOfLines = File.Exists("RecentFiles.txt")
            ? File.ReadAllLines("RecentFiles.txt").Where(x => !string.IsNullOrWhiteSpace(x))
            : Enumerable.Empty<string>();

        RecentFilesGrid.ColumnCount = 1;
        RecentFilesGrid.Columns[0].Width = 400;
        foreach (var Iter in listOfLines)
            RecentFilesGrid.Rows.Add(Iter.Trim());
        DataGridViewButtonColumn dgvButton = new DataGridViewButtonColumn();
        dgvButton.Width = 60;
        dgvButton.UseColumnTextForButtonValue = true;
        dgvButton.Text = "Load";
        RecentFilesGrid.Columns.Add(dgvButton);
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };


    }

    private void SelectFolderButton_Click(object sender, EventArgs e)
    {
        var ListOfLines = File.Exists("RecentFiles.txt")
            ? File.ReadAllLines("RecentFiles.txt").Where(x => !string.IsNullOrWhiteSpace(x))
            : Enumerable.Empty<string>();

        File.WriteAllLines("RecentFiles.txt", ListOfLines);
        FolderBrowserDialog Dialog = new FolderBrowserDialog();
        Dialog.InitialDirectory = ".\\";
        if (Dialog.ShowDialog(new Form() { TopMost = true }) == DialogResult.OK)
        {
            if (Dialog.SelectedPath.Length <= 3)
            {
                MessageBox.Show("Base.wz not found");
                return;
            }

            var FindBaseWz = Directory.EnumerateFiles(Dialog.SelectedPath, "Base.wz;Data.wz");
            if (FindBaseWz.Count() >= 1)
            {
                this.Hide();
                MainForm.Instance.RemoveWz();
                MainForm.Instance.MapListBox.Rows.Clear();
                var Graphic = MainForm.Instance.MapListBox.CreateGraphics();
                var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

                MainForm.OpenWZ(FindBaseWz.First());

                string StringPath = Wz.HasHardCodedStrings ? "Mob/0100100.img/info/name" : "String/Mob.img/100100/name";
                
                if (Wz.GetNode(StringPath).ToStr() == "Snail")
                {
                    MainForm.Instance.MapListBox.Columns[0].Width = 72;
                    MainForm.Instance.MapListBox.Font = new Font("Arial", 13f, GraphicsUnit.Pixel);
                    MainForm.Instance.MapListBox.SearchGrid.Font = new Font("Arial", 13f, GraphicsUnit.Pixel);
                }
                else
                {
                    MainForm.Instance.MapListBox.Columns[0].Width = 72 + 4;
                    MainForm.Instance.MapListBox.DefaultCellStyle.Font = new Font("Microsoft JhengHei", 13f, GraphicsUnit.Pixel);
                    MainForm.Instance.MapListBox.SearchGrid.DefaultCellStyle.Font = new Font("Microsoft JhengHei", 13f, GraphicsUnit.Pixel);
                }


                MainForm.Instance.DumpMapIDs();
                foreach (var Iter in ListOfLines)
                {
                    if (Iter.Trim() == Dialog.SelectedPath)
                        return;
                }
                //append  to first line
                string Content = File.ReadAllText("RecentFiles.txt");
                Content = Dialog.SelectedPath + "\n" + Content;
                File.WriteAllText("RecentFiles.txt", Content);
            }
            else
            {
                MessageBox.Show("Base.wz not found");
                return;
            }

        }

    }

    private void RecentFilesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        this.Hide();
        MainForm.Instance.MapListBox.Rows.Clear();
        var Graphic = MainForm.Instance.MapListBox.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);

        var Path = RecentFilesGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        var FindBaseWz = Directory.EnumerateFiles(Path, "Base.wz;Data.wz");
        MainForm.Instance.RemoveWz();

        if (FindBaseWz.Count() >= 1)
            MainForm.OpenWZ(FindBaseWz.First());
        string StringPath = Wz.HasHardCodedStrings ? "Mob/0100100.img/info/name" : "String/Mob.img/100100/name";
        if (Wz.GetNode(StringPath).ToStr() == "Snail")
        {
            MainForm.Instance.MapListBox.Columns[0].Width = 72;
            MainForm.Instance.MapListBox.Font = new Font("Arial", 13f, GraphicsUnit.Pixel);
            MainForm.Instance.MapListBox.SearchGrid.Font = new Font("Arial", 13f, GraphicsUnit.Pixel);
        }
        else
        {
            MainForm.Instance.MapListBox.Columns[0].Width = 72 + 4;
            MainForm.Instance.MapListBox.DefaultCellStyle.Font = new Font("Microsoft JhengHei", 13f, GraphicsUnit.Pixel);
            MainForm.Instance.MapListBox.SearchGrid.DefaultCellStyle.Font = new Font("Microsoft JhengHei", 13f, GraphicsUnit.Pixel);
        }
        MainForm.Instance.DumpMapIDs();


    }
}
