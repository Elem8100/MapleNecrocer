using DevComponents.AdvTree;
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
using WzComparerR2.PluginBase;
using WzComparerR2;
using WzComparerR2.WzLib;
using WzComparerR2.Common;
using System.Reflection;
using WzComparerR2.CharaSim;
using System.Runtime.InteropServices;
using DevComponents.DotNetBar.Controls;

using Microsoft.Xna.Framework;

namespace MapleNecrocer;


public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        Instance = this;
        openedWz = new List<Wz_Structure>();
        PluginManager.WzFileFinding += new FindWzEventHandler(WzFileFinding);
        if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
        {
            var dgvType = this.GetType();
            var pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this, true, null);
        }
        RenderForm.TopLevel = false;
        RenderForm.Parent = this;
        RenderForm.Show();
        //  RenderForm.Show();
    }
    public static RenderForm RenderForm = new RenderForm();
    List<Wz_Structure> openedWz;
    public static Wz_Node TreeNode;
    public static MainForm Instance;
    public DataGridViewEx MapListBox;

    Wz_Node GetWzNode(string Path)
    {
        return Wz.GetNode(Path);
    }

    public static void OpenWZ(string wzFilePath)
    {
        MainForm.Instance.openWz(wzFilePath);

    }

    string LeftStr(string s, int count)
    {
        if (count > s.Length)
            count = s.Length;
        return s.Substring(0, count);
    }

    public void DumpMapIDs()
    {
        var MapNames = new Dictionary<string, string>();

        foreach (var Iter in GetWzNode("String/Map.img").Nodes)
        {
            foreach (var Iter2 in Iter.Nodes)
            {
                string ID = Iter2.Text.PadLeft(9, '0');
                var MapName = Iter2.GetValue2("mapName", "");
                if (!MapNames.ContainsKey(ID))
                    MapNames.Add(ID, MapName);
            }
        }

        Win32.SendMessage(MapListBox.Handle, false);
        foreach (var Dir in GetWzNode("Map/Map").Nodes)
        {
            if (LeftStr(Dir.Text, 3) != "Map")
                continue;
            foreach (var img in Dir.Nodes)
            {
                var ID = img.ImgID();
                if (MapNames.ContainsKey(ID))
                    MapListBox.Rows.Add(ID, MapNames[ID]);
                else
                    MapListBox.Rows.Add(ID, "");
            }
        }

        // foreach(var i in TreeNode.Nodes["Map"].Nodes["Map"].Nodes)
        //   MapListBox.Items.Add(i.Text);
        Win32.SendMessage(MapListBox.Handle, true);

        MapListBox.Refresh();

    }
    void WzFileFinding(object sender, FindWzEventArgs e)
    {
        string[] fullPath = null;
        if (!string.IsNullOrEmpty(e.FullPath)) //用fullpath作为输入参数
        {
            fullPath = e.FullPath.Split('/', '\\');
            e.WzType = Enum.TryParse<Wz_Type>(fullPath[0], true, out var wzType) ? wzType : Wz_Type.Unknown;
        }

        List<Wz_Node> preSearch = new List<Wz_Node>();
        if (e.WzType != Wz_Type.Unknown) //用wztype作为输入参数
        {
            IEnumerable<Wz_Structure> preSearchWz = e.WzFile?.WzStructure != null ?
                Enumerable.Repeat(e.WzFile.WzStructure, 1) :
                this.openedWz;
            foreach (var wzs in preSearchWz)
            {
                Wz_File baseWz = null;

                bool find = false;
                foreach (Wz_File wz_f in wzs.wz_files)
                {
                    if (wz_f.Header.FileName.RightStr(5) == "NL.wz") continue;
                    if (wz_f.Header.FileName.RightStr(5) == "ES.wz") continue;
                    if (wz_f.Header.FileName.RightStr(5) == "FR.wz") continue;
                    if (wz_f.Header.FileName.RightStr(5) == "DE.wz") continue;

                    if (wz_f.Type == e.WzType)
                    {
                        preSearch.Add(wz_f.Node);
                        find = true;
                        //e.WzFile = wz_f;
                    }
                    if (wz_f.Type == Wz_Type.Base)
                    {
                        baseWz = wz_f;
                    }
                }

                // detect data.wz
                if (baseWz != null && !find)
                {
                    string key = e.WzType.ToString();
                    foreach (Wz_Node node in baseWz.Node.Nodes)
                    {
                        if (node.Text == key && node.Nodes.Count > 0)
                        {
                            preSearch.Add(node);
                        }
                    }
                }
            }
        }

        if (fullPath == null || fullPath.Length <= 1)
        {
            if (e.WzType != Wz_Type.Unknown && preSearch.Count > 0) //返回wzFile
            {
                e.WzNode = preSearch[0];
                e.WzFile = preSearch[0].Value as Wz_File;
            }
            return;
        }

        if (preSearch.Count <= 0)
        {
            return;
        }

        foreach (var wzFileNode in preSearch)
        {
            var searchNode = wzFileNode;
            for (int i = 1; i < fullPath.Length && searchNode != null; i++)
            {
                searchNode = searchNode.Nodes[fullPath[i]];
                var img = searchNode.GetValueEx<Wz_Image>(null);
                if (img != null)
                {
                    searchNode = img.TryExtract() ? img.Node : null;
                }
            }

            if (searchNode != null)
            {
                e.WzNode = searchNode;
                e.WzFile = wzFileNode.Value as Wz_File;
                return;
            }
        }
        //寻找失败
        e.WzNode = null;
    }


    private Node createNode(Wz_Node wzNode)
    {
        if (wzNode == null)
            return null;

        Node parentNode = new Node(wzNode.Text) { Tag = new WeakReference(wzNode) };
        foreach (Wz_Node subNode in wzNode.Nodes)
        {
            Node subTreeNode = createNode(subNode);
            if (subTreeNode != null)
                parentNode.Nodes.Add(subTreeNode);
        }
        return parentNode;
    }

    private void sortWzNode(Wz_Node wzNode)
    {
        this.sortWzNode(wzNode, true);
    }

    private void sortWzNode(Wz_Node wzNode, bool sortByImgID)
    {
        if (wzNode.Nodes.Count > 1)
        {
            if (sortByImgID)
            {
                wzNode.Nodes.SortByImgID();
            }
            else
            {
                wzNode.Nodes.Sort();
            }
        }
        foreach (Wz_Node subNode in wzNode.Nodes)
        {
            sortWzNode(subNode, sortByImgID);
        }
    }

    private void btnItemOpenWz_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog dlg = new OpenFileDialog())
        {
            dlg.Title = "Wz檔";
            dlg.Filter = "Base.wz|*.wz";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                openWz(dlg.FileName);

            }
        }
    }

    public void openWz(string wzFilePath)
    {
        foreach (Wz_Structure wzs in openedWz)
        {
            foreach (Wz_File wz_f in wzs.wz_files)
            {
                if (string.Compare(wz_f.Header.FileName, wzFilePath, true) == 0)
                {
                    MessageBoxEx.Show("已經開啟的wz", "OK");
                    return;
                }
            }
        }

        var Path = System.IO.Path.GetDirectoryName(wzFilePath);

        Wz_Structure wz = new Wz_Structure();

        try
        {
            if (wz.IsKMST1125WzFormat(wzFilePath))
            {
                wz.LoadKMST1125DataWz(wzFilePath);
            }
            else
            {
                wz.Load(wzFilePath, true);
            }
            sortWzNode(wz.WzNode);

            Node node = createNode(wz.WzNode);
            TreeNode = node.AsWzNode();
            // node.Expand();
            // advTree1.Nodes.Add(node);
            this.openedWz.Add(wz);

            // QueryPerformance.End();

        }
        catch (FileNotFoundException)
        {
            MessageBoxEx.Show("檔案沒找到", "OK");
        }
        catch (Exception ex)
        {
            MessageBoxEx.Show(ex.ToString(), "OK");
            wz.Clear();
        }
        finally
        {
            //  advTree1.EndUpdate();
        }
    }

    public void RemoveWz()
    {
        foreach (var Iter in this.openedWz)
            Iter.Clear();
    }

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        var LeftNum = LeftStr(ID, 1);
        var Node = GetWzNode("Map/Map/Map" + LeftNum + '/' + ID + ".img/info/link");
        if (Node == null)
            Map.ID = ID;
        else
            Map.ID = Node.Value.ToString();

        LeftNum = LeftStr(Map.ID, 1);
        Node = GetWzNode("Map/Map/Map" + LeftNum + "/" + Map.ID + ".img/miniMap");

        if (Node != null)
        {
            pictureBox1.Image = GetWzNode("Map/Map/Map" + LeftNum + "/" + Map.ID + ".img/miniMap/canvas").ExtractPng();
            // Map.Img = GetWzNode("Map/Map/Map" + LeftNum + "/" + Map.ID + ".img");
        }
        else
            pictureBox1.Image = null;

        if (!LoadedEff)
            LoadMapButton.Enabled = true;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        MapListBox = new(72 + 2, 135, 0, 0, 220, 400, false, tabControl1.TabPages[0]);
        MapListBox.Dock = DockStyle.Fill;
        MapListBox.SearchGrid.Dock = DockStyle.Fill;
        MapListBox.CellClick += (s, e) =>
        {
            CellClick(MapListBox, e);
        };

        MapListBox.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(MapListBox.SearchGrid, e);
        };

        comboBox2.SelectedIndex = 1;
    }

    private void OpenFolderButton_Click(object sender, EventArgs e)
    {
        if (SelectFolderForm.Instance == null)
            new SelectFolderForm().Show();
        else
            SelectFolderForm.Instance.Show();
    }

    private void MapListBox_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    static bool LoadedEff;
    private void LoadMapButton_Click(object sender, EventArgs e)
    {

        Map.LoadMap(Map.ID);

        int PX = 0, PY = 0;
        foreach (var Portals in MapPortal.PortalList)
        {
            if (Portals.PortalType == 0)
            {
                PX = Portals.X;
                PY = Portals.Y;
                break;
            }
        }
        Game.Player.X = PX;
        Game.Player.Y = PY;
        Foothold BelowFH = null;
        Vector2 Below = FootholdTree.Instance.FindBelow(new Vector2(PX, PY - 2), ref BelowFH);
        Game.Player.FH = BelowFH;
        Game.Player.FaceDir = FaceDir.None;
        Game.Player.JumpState = JumpState.jsNone;

        EngineFunc.SpriteEngine.Camera.X = PX - Map.DisplaySize.X / 2;
        EngineFunc.SpriteEngine.Camera.Y = PY - (Map.DisplaySize.Y / 2) - 100;
        if (EngineFunc.SpriteEngine.Camera.X > Map.Right)
            EngineFunc.SpriteEngine.Camera.X = Map.Right;
        if (EngineFunc.SpriteEngine.Camera.X < Map.Left)
            EngineFunc.SpriteEngine.Camera.X = Map.Left;
        if (EngineFunc.SpriteEngine.Camera.Y > Map.Bottom)
            EngineFunc.SpriteEngine.Camera.Y = Map.Bottom;
        if (EngineFunc.SpriteEngine.Camera.Y < Map.Top)
            EngineFunc.SpriteEngine.Camera.Y = Map.Top;

        Map.OffsetY = (Map.DisplaySize.Y - 600) / 2;
        Back.ResetPos = true;
        Particle.ResetPos = true;
        EngineFunc.SpriteEngine.Move(1);

        Game.Player.JumpState = JumpState.jsFalling;
        if (!LoadedEff)
        {
            SetEffect.LoadList();
            ItemEffect.LoadList();
            TamingMob.LoadSaddleList();

            foreach (var Iter in this.panel1.Controls)
            {
                if (Iter.GetType().Name == "Button")
                {
                    ((Button)Iter).Enabled = true;
                }
            }
            LoadedEff = true;
        }

        if (ObjInfoForm.Instance != null)
        {
            ObjInfoForm.Instance.DumpObjs();
        }
    }

    [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool Repaint);
    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {

        var Split = comboBox2.Text.Split("X");
        Map.DisplaySize.X = Split[0].ToInt();
        Map.DisplaySize.Y = Split[1].ToInt();
        bool Result;
        Result = MoveWindow(this.Handle, this.Left, this.Top, Map.DisplaySize.X + 283, Map.DisplaySize.Y + 140, true);
        //this.Width = Map.DisplaySize.X + 283;
        //this.Height = Map.DisplaySize.Y + 124;

        Result = MoveWindow(RenderForm.Handle, RenderForm.Left, RenderForm.Top, Map.DisplaySize.X, Map.DisplaySize.Y, true);
        // RenderForm.Width = Map.DisplaySize.X;
        //RenderForm.Height = Map.DisplaySize.Y;
        RenderForm.RenderFormDraw.Width = Map.DisplaySize.X;
        RenderForm.RenderFormDraw.Height = Map.DisplaySize.Y;
        RenderForm.RenderFormDraw.Parent = RenderForm;
        EngineFunc.SpriteEngine.VisibleWidth = Map.DisplaySize.X + 200;
        EngineFunc.SpriteEngine.VisibleHeight = Map.DisplaySize.Y + 200;
        Map.ResetPos = true;
        this.CenterToScreen();
    }

    private void comboBox2_Click(object sender, EventArgs e)
    {


    }

    private void SerachMapBox_TextChanged(object sender, EventArgs e)
    {

        MapListBox.Search(SearchMapBox.Text);
    }



    private void MobButton_Click(object sender, EventArgs e)
    {
        void ShowForm(Form Instance, Action NewForm)
        {
            if (Instance == null)
                NewForm();
            else
                Instance.Show();
        }
        switch (((Button)sender).Name)
        {
            case "MobButton": ShowForm(MobForm.Instance, () => new MobForm().Show()); break;
            case "NpcButton": ShowForm(NpcForm.Instance, () => new NpcForm().Show()); break;
            case "AvatarButton": ShowForm(AvatarForm.Instance, () => new AvatarForm().Show()); break;
            case "ChairButton": ShowForm(ChairForm.Instance, () => new ChairForm().Show()); break;
            case "MountButton": ShowForm(MountForm.Instance, () => new MountForm().Show()); break;
            case "CashEffectButton": ShowForm(CashEffectForm.Instance, () => new CashEffectForm().Show()); break;
            case "MorphButton": ShowForm(MorphForm.Instance, () => new MorphForm().Show()); break;
            case "DamageSkinButton": ShowForm(DamageSkinForm.Instance, () => new DamageSkinForm().Show()); break;
            case "ObjInfoButton": new ObjInfoForm().Show(); break;
            case "MedalButton": ShowForm(MedalForm.Instance, () => new MedalForm().Show()); break;
            case "TitleButton": ShowForm(TitleForm.Instance, () => new TitleForm().Show()); break;
            case "RingButton": ShowForm(RingForm.Instance, () => new RingForm().Show()); break;
        }


    }



    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!SearchMapBox.Focused)
            ActiveControl = null;

    }


}

