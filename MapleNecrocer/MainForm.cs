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
using Spine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.CompilerServices;
using WzComparerR2.CharaSimControl;
using System.Text.RegularExpressions;
using WzComparerR2.CharaSim;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using DPIUtils;

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
        Sound.Init();
        ToolTipView = new AfrmTooltip();
        ToolTipView.Visible = true;
        stringLinker = new StringLinker();
        ToolTipView.StringLinker = this.stringLinker;
        //ToolTipView.KeyDown += new KeyEventHandler(afrm_KeyDown);
        ToolTipView.ShowID = true;
        ToolTipView.ShowMenu = true;
        ToolTipView.StartPosition = FormStartPosition.CenterParent;

        //  RenderForm.Show();
    }
    public static RenderForm RenderForm = new RenderForm();
    List<Wz_Structure> openedWz;
    public static Wz_Node TreeNode;
    public static MainForm Instance;
    public DataGridViewEx MapListBox;
    public Dictionary<string, string> MapNames = new();
    public StringLinker stringLinker;
    public AfrmTooltip ToolTipView;
    DefaultLevel skillDefaultLevel = DefaultLevel.Level0;
    int skillInterval = 32;
    public void CenterToScreen2()
    {
        this.CenterToScreen();
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
        //  if(Wz.HasNode("Map/Map/Map0"))
        foreach (var Iter in Wz.GetNodes("String/Map.img"))
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
        foreach (var Dir in Wz.GetNodes("Map/Map"))
        {
            if (LeftStr(Dir.Text, 3) != "Map")
                continue;
            foreach (var img in Dir.Nodes)
            {
                if (!Char.IsNumber(img.Text[0]))
                    continue;
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
        tabControl1.Enabled = true;
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

        Wz.IsDataWz = false;
        if (Wz.GetNode("Mob").FullPathToFile.LeftStr(4) == "Data")
            Wz.IsDataWz = true;

        Wz.HasStringWz = true;
        if (Wz.HasNode("Mob/0100100.img/info/name"))
        {
            Wz.HasStringWz = false;
        }

        Wz.HasMap9Dir = false;
        if (Wz.HasNode("Map/Map/Map1"))
        {
            Wz.HasMap9Dir = true;
        }
    }

    public void RemoveWz()
    {
        foreach (var Iter in this.openedWz)
            Iter.Clear();
    }

    enum DefaultLevel
    {
        Level0 = 0,
        Level1 = 1,
        LevelMax = 2,
        LevelMaxWithCO = 3,
    }

    public void QuickView(Wz_Node node)
    {
        Wz_Node selectedNode = node;
        if (selectedNode == null)
        {
            return;
        }
        if (!Wz.IsDataWz)
        {
            Wz_File findStringWz()
            {
                foreach (Wz_Structure wz in openedWz)
                {
                    foreach (Wz_File file in wz.wz_files)
                    {
                        if (file.Type == Wz_Type.String)
                        {
                            return file;
                        }
                    }
                }
                return null;
            }

            Wz_File findItemWz()
            {
                foreach (Wz_Structure wz in openedWz)
                {
                    foreach (Wz_File file in wz.wz_files)
                    {
                        if (file.Type == Wz_Type.Item)
                        {
                            return file;
                        }
                    }
                }
                return null;
            }

            Wz_File findEtcWz()
            {
                foreach (Wz_Structure wz in openedWz)
                {
                    foreach (Wz_File file in wz.wz_files)
                    {
                        if (file.Type == Wz_Type.Etc)
                        {
                            return file;
                        }
                    }
                }
                return null;
            }

            Wz_Image image;
            Wz_File wzf = selectedNode.GetNodeWzFile();
            if (wzf == null)
            {
                // labelItemStatus.Text = "The WZ file where the node belongs to has not been found.";
                return;
            }

            if (!this.stringLinker.HasValues)
            {
                this.stringLinker.Load(findStringWz(), findItemWz(), findEtcWz());
            }

            object obj = null;
            string fileName = null;
            switch (wzf.Type)
            {
                case Wz_Type.Character:
                    if ((image = selectedNode.GetValue<Wz_Image>()) == null || !image.TryExtract())
                        return;
                    CharaSimLoader.LoadSetItemsIfEmpty();
                    CharaSimLoader.LoadExclusiveEquipsIfEmpty();
                    CharaSimLoader.LoadCommoditiesIfEmpty();
                    var gear = Gear.CreateFromNode(image.Node, PluginManager.FindWz);
                    obj = gear;
                    if (gear != null)
                    {
                        fileName = gear.ItemID + ".png";
                    }
                    break;
                case Wz_Type.Item:
                    CharaSimLoader.LoadCommoditiesIfEmpty();
                    Wz_Node itemNode = selectedNode;
                    if (Regex.IsMatch(itemNode.FullPathToFile, @"^Item\\(Cash|Consume|Etc|Install|Cash)\\\d{4,6}.img\\\d+$") || Regex.IsMatch(itemNode.FullPathToFile, @"^Item\\Special\\0910.img\\\d+$"))
                    {
                        var item = Item.CreateFromNode(itemNode, PluginManager.FindWz);
                        obj = item;
                        if (item != null)
                        {
                            fileName = item.ItemID + ".png";
                        }
                    }
                    else if (Regex.IsMatch(itemNode.FullPathToFile, @"^Item\\Pet\\\d{7}.img"))
                    {
                        if (CharaSimLoader.LoadedSetItems.Count == 0) //宠物 预读套装
                        {
                            CharaSimLoader.LoadSetItemsIfEmpty();
                        }
                        if ((image = selectedNode.GetValue<Wz_Image>()) == null || !image.TryExtract())
                            return;
                        var item = Item.CreateFromNode(image.Node, PluginManager.FindWz);
                        obj = item;
                        if (item != null)
                        {
                            fileName = item.ItemID + ".png";
                        }
                    }

                    break;
                case Wz_Type.Skill:
                    Wz_Node skillNode = selectedNode;
                    //模式路径分析
                    if (Regex.IsMatch(skillNode.FullPathToFile, @"^Skill\d*\\Recipe_\d+.img\\\d+$"))
                    {
                        Recipe recipe = Recipe.CreateFromNode(skillNode);
                        obj = recipe;
                        if (recipe != null)
                        {
                            fileName = "recipe_" + recipe.RecipeID + ".png";
                        }
                    }
                    else if (Regex.IsMatch(skillNode.FullPathToFile, @"^Skill\d*\\\d+.img\\skill\\\d+$"))
                    {
                        WzComparerR2.CharaSim.Skill skill = WzComparerR2.CharaSim.Skill.CreateFromNode(skillNode, PluginManager.FindWz);
                        if (skill != null)
                        {
                            switch (this.skillDefaultLevel)
                            {
                                case DefaultLevel.Level0: skill.Level = 0; break;
                                case DefaultLevel.Level1: skill.Level = 1; break;
                                case DefaultLevel.LevelMax: skill.Level = skill.MaxLevel; break;
                                case DefaultLevel.LevelMaxWithCO: skill.Level = skill.MaxLevel + 2; break;
                            }
                            obj = skill;
                            fileName = "skill_" + skill.SkillID + ".png";
                        }
                    }
                    break;

                case Wz_Type.Mob:
                    if ((image = selectedNode.GetValue<Wz_Image>()) == null || !image.TryExtract())
                        return;
                    var mob = WzComparerR2.CharaSim.Mob.CreateFromNode(image.Node, PluginManager.FindWz);
                    obj = mob;
                    if (mob != null)
                    {
                        fileName = mob.ID + ".png";
                    }
                    break;

                case Wz_Type.Npc:
                    if ((image = selectedNode.GetValue<Wz_Image>()) == null || !image.TryExtract())
                        return;
                    var npc = WzComparerR2.CharaSim.Npc.CreateFromNode(image.Node, PluginManager.FindWz);
                    obj = npc;
                    if (npc != null)
                    {
                        fileName = npc.ID + ".png";
                    }
                    break;

                case Wz_Type.Etc:
                    CharaSimLoader.LoadSetItemsIfEmpty();
                    Wz_Node setItemNode = selectedNode;
                    if (Regex.IsMatch(setItemNode.FullPathToFile, @"^Etc\\SetItemInfo.img\\-?\d+$"))
                    {
                        SetItem setItem;
                        if (!CharaSimLoader.LoadedSetItems.TryGetValue(Convert.ToInt32(selectedNode.Text), out setItem))
                            return;
                        obj = setItem;
                        if (setItem != null)
                        {
                            fileName = setItem.SetItemID + ".png";
                        }
                    }
                    break;
            }

            if (obj != null)
            {
                ToolTipView.TargetItem = obj;
                ToolTipView.ImageFileName = fileName;
                ToolTipView.Refresh();
                ToolTipView.HideOnHover = false;
                ToolTipView.Show();
            }
        }
        else
        {
            if (!this.stringLinker.HasValues)
            {
                this.stringLinker.Load(Wz.GetNode("String"), Wz.GetNode("Item"), Wz.GetNode("Etc"));
            }

            string[] Split = selectedNode.FullPathToFileEx().Split('/');
            object obj = null;
            string fileName = null;
            Wz_Image image;
            switch (Split[1])
            {
                case "Character":
                    if ((image = selectedNode.GetValue<Wz_Image>()) == null || !image.TryExtract())
                        return;
                    CharaSimLoader.LoadSetItemsIfEmpty();
                    CharaSimLoader.LoadExclusiveEquipsIfEmpty();
                    CharaSimLoader.LoadCommoditiesIfEmpty();
                    var gear = Gear.CreateFromNode(image.Node, PluginManager.FindWz);
                    obj = gear;
                    if (gear != null)
                    {
                        fileName = gear.ItemID + ".png";
                    }
                    break;
                case "Item":
                    CharaSimLoader.LoadCommoditiesIfEmpty();

                    Wz_Node itemNode = selectedNode;
                    if (Regex.IsMatch(itemNode.FullPathToFile.Replace("Data\\", ""), @"^Item\\(Cash|Consume|Etc|Install|Cash)\\\d{4,6}.img\\\d+$") || Regex.IsMatch(itemNode.FullPathToFile, @"^Item\\Special\\0910.img\\\d+$"))
                    {
                        var item = Item.CreateFromNode(itemNode, PluginManager.FindWz);
                        obj = item;
                        if (item != null)
                        {
                            fileName = item.ItemID + ".png";
                        }
                    }
                    else if (Regex.IsMatch(itemNode.FullPathToFile.Replace("Data\\", ""), @"^Item\\Pet\\\d{7}.img"))
                    {
                        if (CharaSimLoader.LoadedSetItems.Count == 0) //宠物 预读套装
                        {
                            CharaSimLoader.LoadSetItemsIfEmpty();
                        }
                        if ((image = selectedNode.GetValue<Wz_Image>()) == null || !image.TryExtract())
                            return;
                        var item = Item.CreateFromNode(image.Node, PluginManager.FindWz);
                        obj = item;
                        if (item != null)
                        {
                            fileName = item.ItemID + ".png";
                        }
                    }

                    break;
                case "Skill":
                    Wz_Node skillNode = selectedNode;
                    //模式路径分析
                    if (Regex.IsMatch(skillNode.FullPathToFile, @"^Skill\d*\\Recipe_\d+.img\\\d+$"))
                    {
                        Recipe recipe = Recipe.CreateFromNode(skillNode);
                        obj = recipe;
                        if (recipe != null)
                        {
                            fileName = "recipe_" + recipe.RecipeID + ".png";
                        }
                    }
                    else if (Regex.IsMatch(skillNode.FullPathToFile, @"^Skill\d*\\\d+.img\\skill\\\d+$"))
                    {
                        WzComparerR2.CharaSim.Skill skill = WzComparerR2.CharaSim.Skill.CreateFromNode(skillNode, PluginManager.FindWz);
                        if (skill != null)
                        {
                            switch (this.skillDefaultLevel)
                            {
                                case DefaultLevel.Level0: skill.Level = 0; break;
                                case DefaultLevel.Level1: skill.Level = 1; break;
                                case DefaultLevel.LevelMax: skill.Level = skill.MaxLevel; break;
                                case DefaultLevel.LevelMaxWithCO: skill.Level = skill.MaxLevel + 2; break;
                            }
                            obj = skill;
                            fileName = "skill_" + skill.SkillID + ".png";
                        }
                    }
                    break;

                case "Mob":
                    if ((image = selectedNode.GetValue<Wz_Image>()) == null || !image.TryExtract())
                        return;
                    var mob = WzComparerR2.CharaSim.Mob.CreateFromNode(image.Node, PluginManager.FindWz);
                    obj = mob;
                    if (mob != null)
                    {
                        fileName = mob.ID + ".png";
                    }
                    break;

                case "Npc":
                    if ((image = selectedNode.GetValue<Wz_Image>()) == null || !image.TryExtract())
                        return;
                    var npc = WzComparerR2.CharaSim.Npc.CreateFromNode(image.Node, PluginManager.FindWz);
                    obj = npc;
                    if (npc != null)
                    {
                        fileName = npc.ID + ".png";
                    }
                    break;

                case "Etc":
                    CharaSimLoader.LoadSetItemsIfEmpty();
                    Wz_Node setItemNode = selectedNode;
                    if (Regex.IsMatch(setItemNode.FullPathToFile, @"^Etc\\SetItemInfo.img\\-?\d+$"))
                    {
                        SetItem setItem;
                        if (!CharaSimLoader.LoadedSetItems.TryGetValue(Convert.ToInt32(selectedNode.Text), out setItem))
                            return;
                        obj = setItem;
                        if (setItem != null)
                        {
                            fileName = setItem.SetItemID + ".png";
                        }
                    }
                    break;
            }

            if (obj != null)
            {
                ToolTipView.TargetItem = obj;
                ToolTipView.ImageFileName = fileName;
                ToolTipView.Refresh();
                ToolTipView.HideOnHover = false;
                ToolTipView.Show();
            }
        }

    }


    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        var LeftNum = LeftStr(ID, 1);
        var Node = Wz.GetNode("Map/Map/Map" + LeftNum + '/' + ID + ".img/info/link");
        if (Node == null)
            Map.ID = ID;
        else
            Map.ID = Node.Value.ToString();

        LeftNum = LeftStr(Map.ID, 1);
        Node = Wz.GetNode("Map/Map/Map" + LeftNum + "/" + Map.ID + ".img/miniMap");

        if (Node != null)
        {
            pictureBox1.Image = Wz.GetBmp("Map/Map/Map" + LeftNum + "/" + Map.ID + ".img/miniMap/canvas");
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
        Graphics graphics = this.CreateGraphics();
        float dpiX = graphics.DpiX;
        float dpiY = graphics.DpiY;
        DPIUtil.dpiX = dpiX;
        DPIUtil.dpiY = dpiY;
    }

    private void OpenFolderButton_Click(object sender, EventArgs e)
    {
        if (SelectFolderForm.Instance == null)
            new SelectFolderForm().Show();
        else
            SelectFolderForm.Instance.Show();
        OpenFolderButton.Enabled = false;
    }

    private void MapListBox_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    private void LoadMap()
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

        if (Pet.Instance != null)
        {
            Pet.Instance.X = Game.Player.X;
            Pet.Instance.Y = Game.Player.Y;
            Pet.Instance.JumpState = JumpState.jsFalling;
        }

        if (Familiar.Instance != null)
        {
            Familiar.Instance.X = Game.Player.X;
            Familiar.Instance.Y = Game.Player.Y - 50;
            Familiar.Instance.JumpState = JumpState.jsFalling;
        }

        if (AndroidPlayer.Instance != null)
        {
            AndroidPlayer.Instance.X = Game.Player.X;
            AndroidPlayer.Instance.Y = Game.Player.Y;
            AndroidPlayer.Instance.JumpState = JumpState.jsFalling;
        }

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
                    ((System.Windows.Forms.Button)Iter).Enabled = true;
                }
            }

            if (!Wz.HasNode("Character/TamingMob"))
                MountButton.Enabled = false;
            if (!Wz.HasNode("Item/Cash"))
            {
                CashButton.Enabled = false;
                CashEffectButton.Enabled = false;
            }
            if (!Wz.HasNode("Morph"))
                MorphButton.Enabled = false;
            if (!Wz.HasNode("Effect/DamageSkin.img"))
                DamageSkinButton.Enabled = false;
            if (!Wz.HasNode("Character/Accessory/01142000.img"))
                MedalButton.Enabled = false;
            if (!Wz.HasNode("Item/Install/0370.img"))
                TitleButton.Enabled = false;
            if (!Wz.HasNode("Character/Ring/01112100.img"))
                RingButton.Enabled = false;
            if (!Wz.HasNode("Character/Familiar"))
                FamiliarButton.Enabled = false;
            if (!Wz.HasNode("Character/Android"))
                AndroidButton.Enabled = false;
            if (!Wz.HasNode("Character/Totem"))
                TotemEffectButton.Enabled = false;
            if (!Wz.HasNode("Item/Consume/0259.img"))
                SoulEffectButton.Enabled = false;
            if (Wz.IsDataWz)
                ReactorButton.Enabled = false;

            LoadedEff = true;
        }

        if (ObjInfoForm.Instance != null)
        {
            ObjInfoForm.Instance.DumpObjs();
        }
    }

    static bool LoadedEff;
    private void LoadMapButton_Click(object sender, EventArgs e)
    {
        this.LoadMap();
    }

    [DllImport("User32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool Repaint);
    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
        RenderFormDraw.ScreenMode = ScreenMode.Normal;
        var Split = comboBox2.Text.Split("X");
        Map.DisplaySize.X = Split[0].ToInt();
        Map.DisplaySize.Y = Split[1].ToInt();
        bool Result;
        Result = MoveWindow(this.Handle, this.Left, this.Top, Map.DisplaySize.X + 287, Map.DisplaySize.Y + 152, true);
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
        Refresh();
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
        switch (((System.Windows.Forms.Button)sender).Name)
        {
            case "ViewButton": ShowForm(ViewForm.Instance, () => new ViewForm().Show()); break;
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
            case "PetButton": ShowForm(PetForm.Instance, () => new PetForm().Show()); break;
            case "FamiliarButton": ShowForm(FamiliarForm.Instance, () => new FamiliarForm().Show()); break;
            case "SkillButton": ShowForm(SkillForm.Instance, () => new SkillForm().Show()); break;
            case "AndroidButton": ShowForm(AndroidForm.Instance, () => new AndroidForm().Show()); break;
            case "ConsumeButton": ShowForm(ConsumeForm.Instance, () => new ConsumeForm().Show()); break;
            case "CashButton": ShowForm(CashForm.Instance, () => new CashForm().Show()); break;
            case "EtcButton": ShowForm(EtcForm.Instance, () => new EtcForm().Show()); break;
            case "TotemEffectButton": ShowForm(TotemEffectForm.Instance, () => new TotemEffectForm().Show()); break;
            case "SoulEffectButton": ShowForm(SoulEffectForm.Instance, () => new SoulEffectForm().Show()); break;
            case "ReactorButton": ShowForm(ReactorForm.Instance, () => new ReactorForm().Show()); break;
            case "SaveMapButton": ShowForm(SaveMapForm.Instance, () => new SaveMapForm().Show()); break;
            case "ScaleButton": ShowForm(ScaleForm.Instance, () => new ScaleForm().Show()); break;
            case "OptionButton": ShowForm(OptionForm.Instance, () => new OptionForm().Show()); break;
        }
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!SearchMapBox.Focused)
            ActiveControl = null;
        //  if (Skill.PlayEnded)
        //  SearchMapBox.Clear();
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (comboBox1.SelectedIndex)
        {
            case 0: Map.GameMode = GameMode.Play; break;
            case 1: Map.GameMode = GameMode.Viewer; break;
        }
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tabControl1.SelectedIndex == 0)
        {
            WorldMapForm.Instance.Close();
        }
        else
        {
            if (WorldMapForm.Instance == null)
            {
                new WorldMapForm().Show();
                WorldMapForm.Instance.Hide();
            }
            else
            {
                WorldMapForm.Instance.Show();
            }
        }
    }

    private List<PictureBox> PictureBoxList = new();
    private System.Windows.Forms.ToolTip ToolTip = new();
    private void WorldMapListGrid_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        WorldMapForm.Instance.Show();

        Wz_Node Img = Wz.GetNode("Map/WorldMap/" + WorldMapListGrid.Rows[e.RowIndex].Cells[1].Value.ToString().Trim(' '));
        if (!Img.HasNode("BaseImg/0"))
            return;
        ToolTip.AutoPopDelay = 5000;
        ToolTip.InitialDelay = 10;
        ToolTip.ReshowDelay = 10;
        // Force the ToolTip text to be displayed whether or not the form is active.
        ToolTip.ShowAlways = true;

        Bitmap Bmp = Img.GetBmp("BaseImg/0");
        int W = Bmp.Width;
        int H = Bmp.Height;
        WorldMapForm.Instance.ClientSize = new Size(W, H);
        WorldMapForm.Instance.pictureBox1.Image = Bmp;
        Wz_Vector Origin = Img.GetVector("BaseImg/0/origin");

        foreach (var Iter in PictureBoxList)
            Iter.Dispose();
        PictureBoxList.Clear();

        foreach (var Iter in Img.GetNodes("MapList"))
        {
            var SpotPos = Iter.GetVector("spot");
            string SpotType = Iter.GetInt("type").ToString();
            Bitmap SpotBmp = Wz.GetBmp("Map/MapHelper.img/worldMap/mapImage/" + SpotType);
            string SpotID = Iter.GetInt("mapNo/0").ToString().PadLeft(9, '0');
            var SpotPictureBox = new PictureBox();
            if (MapNames.ContainsKey(SpotID))
                SpotPictureBox.AccessibleDescription = SpotID + " - " + MapNames[SpotID];
            ToolTip.SetToolTip(SpotPictureBox, SpotPictureBox.AccessibleDescription);
            PictureBoxList.Add(SpotPictureBox);
            SpotPictureBox.Image = SpotBmp;
            SpotPictureBox.BackColor = System.Drawing.Color.Transparent;
            SpotPictureBox.Width = SpotBmp.Width;
            SpotPictureBox.Height = SpotBmp.Height;
            Wz_Vector SpotOrigin = Wz.GetVector("Map/MapHelper.img/worldMap/mapImage/" + SpotType + "/origin");
            SpotPictureBox.Left = W - Origin.X + SpotPos.X - SpotOrigin.X;
            SpotPictureBox.Top = H - Origin.Y + SpotPos.Y - SpotOrigin.Y;
            SpotPictureBox.Parent = WorldMapForm.Instance.pictureBox1;
            SpotPictureBox.BringToFront();

            SpotPictureBox.Click += (s, e) =>
            {
                string ID = ((PictureBox)s).AccessibleDescription.LeftStr(9).Trim(' ');
                if (Map.ID == ID)
                    return;
                if (!Wz.HasNode("Map/Map/Map" + ID.LeftStr(1) + "/" + ID + ".img"))
                    return;
                // Map.ReLoad = true;
                Map.ID = ID;
                this.LoadMap();
                if (LoadMapButton.Enabled == false)
                    LoadMapButton.Enabled = true;
            };
        }
    }

    private void FullScreenButton_Click(object sender, EventArgs e)
    {

    }

    Pen pen = new(System.Drawing.Color.FromArgb(153, 180, 209), 2);
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.DrawRectangle(pen, new System.Drawing.Rectangle(256, 92, RenderForm.Width + 2, RenderForm.Height + 2));
    }
}

