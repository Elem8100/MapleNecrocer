using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.Rendering;
using WzComparerR2.WzLib;
using WzComparerR2.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;
using WzComparerR2.PluginBase;
using SharpDX;
using System.Drawing;
using MonoGame.Framework.Utilities.Deflate;
using System.Xml.Linq;
using System.Xml;
using Microsoft.Xna.Framework.Audio;
using DevComponents.AdvTree;
using System.Reflection.Metadata.Ecma335;

namespace MapleNecrocer;

public struct NodeInfo
{
    public string OriNode;
    public string UOLNode;
    public Wz_Node UOLEntry;
}
internal class Wz
{
    public static Dictionary<string, Wz_Node> Data = new Dictionary<string, Wz_Node>();
    public static Dictionary<string, Wz_Node> EquipData = new Dictionary<string, Wz_Node>();
    public static Dictionary<string, Texture2D> ImageKeys = new Dictionary<string, Texture2D>();
    public static Dictionary<Wz_Node, Texture2D> ImageLib = new Dictionary<Wz_Node, Texture2D>();
    public static Dictionary<Wz_Node, Texture2D> EquipImageLib = new Dictionary<Wz_Node, Texture2D>();
    public static string Region;
    private static List<NodeInfo> NodeList1 = new();
    private static List<NodeInfo> NodeList2 = new();


    public static void DumpDataA(Wz_Node WzNode, Dictionary<string, Wz_Node> DataLib, Dictionary<Wz_Node, Texture2D> ImageLib)
    {

        switch (WzNode.Value)
        {
            case Wz_Png:
                ImageLib.AddOrReplace(WzNode, WzLibExtension.ToTexture((Wz_Png)GetNode(WzNode.FullPathToFile2()).Value, RenderFormDraw.Instance.GraphicsDevice));
                DataLib.AddOrReplace(WzNode.FullPathToFile2(), WzNode);
                break;
            case Wz_Uol:
                DataLib.AddOrReplace(WzNode.FullPathToFile2(), GetNode(WzNode.FullPathToFile2()));
                break;
            default:
                DataLib.AddOrReplace(WzNode.FullPathToFile2(), WzNode);
                break;
        }

        foreach (var E in WzNode.Nodes)
            // if (!(E.Value is Wz_Image))
            DumpDataA(E, DataLib, ImageLib);
    }

    public static void DumpData(Wz_Node WzNode, Dictionary<string, Wz_Node> DataLib, Dictionary<Wz_Node, Texture2D> ImageLib,
       bool UseDye = false, int Hue = 0, int Saturation = 0)
    {
        NodeList1.Clear();
        NodeList2.Clear();
        Scan1(WzNode, DataLib, ImageLib, UseDye, Hue, Saturation);
        foreach (var P in NodeList1)
            Scan2(P.OriNode, P.UOLNode, P.UOLEntry, DataLib, ImageLib, UseDye, Hue, Saturation);
        foreach (var P in NodeList2)
            Scan3(P.OriNode, P.UOLNode, P.UOLEntry, DataLib);
    }

    static void Scan1(Wz_Node WzNode, Dictionary<string, Wz_Node> DataLib, Dictionary<Wz_Node, Texture2D> ImageLib,
          bool UseDye = false, int Hue = 0, int Saturation = 0)
    {
        switch (WzNode.Value)
        {
            case Wz_Uol:
                var Entry = WzNode.ParentNode;
                var Child = Entry.Get(WzNode.ToStr());
                if (Child == null)
                    return;
                if (Child.Value is Wz_Uol)
                    Child = Child.ParentNode.Get(Child.ToStr());
                if (Child == null)
                    return;
                var NodeInfo = new NodeInfo();
                NodeInfo.OriNode = WzNode.FullPathToFile2();
                NodeInfo.UOLNode = Child.FullPathToFile2();
                NodeInfo.UOLEntry = Child;
                NodeList1.Add(NodeInfo);
                break;

            case Wz_Png:
                DataLib.AddOrReplace(WzNode.FullPathToFile2(), WzNode);
                if (UseDye)
                {
                    Texture2D Texture = ImageFilter.GetHSL(RenderFormDraw.Instance.GraphicsDevice, GetImgNode(WzNode.FullPathToFile2()).ExtractPng(), Hue, Saturation);
                    ImageLib.AddOrReplace(WzNode, Texture);
                }
                else
                {
                    ImageLib.AddOrReplace(WzNode, WzLibExtension.ToTexture((Wz_Png)GetImgNode(WzNode.FullPathToFile2()).Value, RenderFormDraw.Instance.GraphicsDevice));
                }
                break;

            default:
                DataLib.AddOrReplace(WzNode.FullPathToFile2(), WzNode);
                break;
        }

        foreach (var C in WzNode.Nodes)
            Scan1(C, DataLib, ImageLib, UseDye, Hue, Saturation);
    }

    static void Scan2(string OriNode, string UOLNode, Wz_Node WzNode, Dictionary<string, Wz_Node> DataLib, Dictionary<Wz_Node, Texture2D> ImageLib,
           bool UseDye = false, int Hue = 0, int Saturation = 0)
    {
        Wz_Node Child;
        if (WzNode.Value is Wz_Uol)
        {
            var Entry = WzNode.ParentNode;
            Child = Entry.Get(WzNode.ToStr());
            if (Child == null)
                return;
            if (Child.Value is Wz_Uol)
                Child = Child.ParentNode.Get(Child.ToStr());
            //  if (Child == null)
            //  return;
        }
        else
            Child = WzNode;

        if (!(Child.Value is Wz_Uol))
        {
            string Str = WzNode.FullPathToFile2().Replace(UOLNode, "");
            DataLib.AddOrReplace(OriNode + Str, Child);
            NodeInfo NodeInfo = new NodeInfo();
            NodeInfo.OriNode = OriNode + Str;
            NodeInfo.UOLNode = Child.FullPathToFile2();
            NodeInfo.UOLEntry = Child;
            NodeList2.Add(NodeInfo);
        }

        if (Child.Value is Wz_Png)
        {
            if (!DataLib.ContainsKey(WzNode.FullPathToFile2()))
            {
                DataLib.AddOrReplace(WzNode.FullPathToFile2(), WzNode);

                if (UseDye)
                {
                    Texture2D Texture = ImageFilter.GetHSL(RenderFormDraw.Instance.GraphicsDevice, GetImgNode(WzNode.FullPathToFile2()).ExtractPng(), Hue, Saturation);
                    ImageLib.AddOrReplace(WzNode, Texture);
                }
                else
                {
                    ImageLib.AddOrReplace(WzNode, WzLibExtension.ToTexture((Wz_Png)GetImgNode(WzNode.FullPathToFile2()).Value, RenderFormDraw.Instance.GraphicsDevice));
                }
            }
        }

        foreach (var C in WzNode.Nodes)
            Scan2(OriNode, UOLNode, C, DataLib, ImageLib, UseDye, Hue, Saturation);
    }

    static void Scan3(string OriNode, string UOLNode, Wz_Node WzNode, Dictionary<string, Wz_Node> DataLib)
    {
        string Str = WzNode.FullPathToFile2().Replace(UOLNode, "");
        DataLib.AddOrReplace(OriNode + Str, WzNode);
        foreach (var C in WzNode.Nodes)
            Scan3(OriNode, UOLNode, C, DataLib);
    }
    public static bool HasNode(string Path)
    {
        return Wz.GetNode(Path) != null;
    }
    public static bool HasData(string Path)
    {
        return Data.ContainsKey(Path) != false;

    }
    public static bool HasDataE(string Path)
    {
        return EquipData.ContainsKey(Path) != false;

    }

    public static int GetInt(string Path, int DefaultValue = 0)
    {
        if (Wz.HasNode(Path))
            return Wz.GetNode(Path).GetValueEx<int>(DefaultValue);
        else
            return DefaultValue;
    }
    public static string GetStr(string Path, string DefaultValue = "")
    {
        if (Wz.HasNode(Path))
            return Wz.GetNode(Path).GetValueEx<string>(DefaultValue);
        else
            return DefaultValue;
    }

    public static bool GetBool(string Path)
    {
        return Convert.ToBoolean(Wz.GetNode(Path).GetValueEx<int>(0));
    }

    public static Wz_Vector GetVector(string Path)
    {
        return Wz.GetNode(Path).GetValueEx<Wz_Vector>(new Wz_Vector(0, 0)); ;
    }

    public static Bitmap GetBmp(string Path)
    {
        if (Wz.GetNode(Path) != null && Wz.GetNode(Path).Value is Wz_Png)
        {
            return Wz.GetNode(Path).ExtractPng();
        }
        else
        {
            return new Bitmap(1, 1);
        }
    }

    public static Wz_Node.WzNodeCollection GetNodes(string Path)
    {
        return Wz.GetNode(Path).Nodes;
    }


    public static Wz_Node GetNodeA(string Path)
    {
        return PluginManager.FindWz(Path);
        /*
        if (Node != null)
        {
            if (Node.Value is Wz_Uol)
            {
                return Node.ResolveUol();
            }
            else
            {
                return Node;
            }
        }
        */
        // return null;
    }

    public static Wz_Node GetNode(string Path)
    {
        Wz_Node Node = PluginManager.FindWz(Path);
        if (Node != null)
        {
            if (Node.Value is Wz_Uol)
            {
                Wz_Node Node2;
                Node2 = Node.ResolveUol();
                if (Node2.FindNodeByPath("_outlink", true) != null)
                {
                    var LinkStr = Node2.FindNodeByPath("_outlink", true).Value.ToString();
                    string[] Split = LinkStr.Split('/');
                    string DestPath = "";
                    switch (Split[0])
                    {
                        case "Mob":
                            return PluginManager.FindWz(LinkStr);
                            break;

                        case "Map":
                            return PluginManager.FindWz(LinkStr);
                            /*
                             if(Split[1] == "Map")
                                 DestPath = LinkStr.Remove(0,4);
                             else
                                 DestPath = Regex.Replace(LinkStr,"Map/","");
                             return MainForm.GetNode(DestPath);
                             */
                            break;
                        case "Skill":
                            return GetNode(LinkStr);
                            break;
                        default:
                            DestPath = Regex.Replace(LinkStr, Node.GetNodeWzFile().Type.ToString() + "/", "");
                            //   return (Node.GetNodeWzFile().WzStructure.WzNode.GetNode(DestPath).Value as Wz_Png).ExtractPng();
                            break;
                    };
                }
                return Node2;
            }
            else
                return Node.GetLinkedSourceNode(PluginManager.FindWz);
        }
        else
        {
            string[] Split = Path.Split('/');
            var Node2 = PluginManager.FindWz(Split[0]);
            int Count = 0;
            string Str = "";
            string Path1 = "";
            string Path2 = "";
            bool HasUol = false;
            for (int i = 1; i < Split.Length; i++)
            {
                if (i == 1)
                    Str = Str + Split[i];
                else
                    Str += '/' + Split[i];
                if ((Node2.FindNodeByPathA(Str, true) != null) && (Node2.FindNodeByPathA(Str, true).Value is Wz_Uol))
                {
                    HasUol = true;
                    Count = i;
                    Path1 = Str;
                    break;
                }
            }

            if (HasUol)
            {
                Str = "";
                for (int i = Count + 1; i < Split.Length; i++)
                {
                    if (i == Count + 1)
                        Str = Str + Split[i];
                    else
                        Str = Str + '/' + Split[i];
                    Path2 = Str;
                }
                return Node2.FindNodeByPathA(Path1, true).ResolveUol().FindNodeByPathA(Path2, true);
            }
        }
        return null;
    }

    public static Wz_Node GetImgNodeA(string Path)
    {
        var Split = Path.Split(".img/");
        return PluginManager.FindWz(Split[0] + ".img").Get(Split[1]);
    }
    public static Wz_Node GetImgNode(string Path)
    {
        var Split = Path.Split(".img/");
        return PluginManager.FindWz(Split[0] + ".img").Get2(Split[1]);
    }
    public static Wz_Node GetTopEntry(Wz_Node Node)
    {
        var E = Node.ParentNode;
        while (E != null)
        {
            E = E.ParentNode;
            if ((E.Text.Length >= 4) && (E.Text.RightStr(4) == ".img"))
                return E;
        }
        return null;
    }

    public static Wz_Node GetIDNode(string ID)
    {
        switch (ID.LeftStr(2))
        {
            case "05":
                return GetNode("Item/Cash/" + ID.LeftStr(4) + ".img/" + ID);
                //return "Item/Cash/0501.img/05010000";
                break;
            case "03":
                if (GetNode("Item/Install/03010.img") != null)
                {
                    switch (ID.LeftStr(5))
                    {
                        case "03015":
                            return GetNode("Item/Install/" + ID.LeftStr(6) + ".img/" + ID);
                            break;
                        case "03010":
                        case "03011":
                        case "03012":
                        case "03013":
                        case "03014":
                        case "03016":
                        case "03017":
                        case "03018":
                            return GetNode("Item/Install/" + ID.LeftStr(5) + ".img/" + ID);
                            break;
                        default:
                            return GetNode("Item/Install/" + ID.LeftStr(4) + ".img/" + ID);
                            break;
                    }
                }
                else
                {
                    return GetNode("Item/Install/" + ID.LeftStr(4) + ".img/" + ID);
                }
                break;
        }

        switch (int.Parse(ID) / 10000)
        {
            case 2:
                return GetNode("Character/Face/" + ID + ".img");
                break;
            case 3:
            case 4:
            case 6:
                return GetNode("Character/Hair/" + ID + ".img");
                break;
            case 101:
            case 102:
            case 103:
            case 112:
            case 113:
            case 114:
            case 115:
            case 116:
            case 118:
            case 119:
                return GetNode("Character/Accessory/" + ID + ".img");
                break;
            case 120:
                return GetNode("Character/Totem/" + ID + ".img");
                break;
            case 100:
                return GetNode("Character/Cap/" + ID + ".img");
                break;
            case 110:
                return GetNode("Character/Cape/" + ID + ".img");
                break;
            case 104:
                return GetNode("Character/Coat/" + ID + ".img");
                break;
            case 105:
                return GetNode("Character/Longcoat/" + ID + ".img");
                break;
            case 106:
                return GetNode("Character/Pants/" + ID + ".img");
                break;
            case 107:
                return GetNode("Character/Shoes/" + ID + ".img");
                break;
            case 108:
                return GetNode("Character/Glove/" + ID + ".img");
            case 109:
                return GetNode("Character/Shield/" + ID + ".img");
                break;

            case 111:
                return GetNode("Character/Ring/" + ID + ".img");
                break;

            case 161:
                return GetNode("Character/Mechanic/" + ID + ".img");
                break;

            case 166:
            case 167:
                return GetNode("Character/Android/" + ID + ".img");
                break;
            case 168:
                return GetNode("Character/Bits/" + ID + ".img");
                break;
            case int n when (n >= 121 && n <= 170):
                return GetNode("Character/Weapon/" + ID + ".img");
                break;
            case int n when (n >= 190 && n <= 199):
                return GetNode("Character/TamingMob/" + ID + ".img");
                break;
            case 180:
                return GetNode("Character/PetEquip/" + ID + ".img");
                break;
            case 996:
            case 997:
                return GetNode("Character/Familiar/" + ID + ".img");
                break;
            case int n when (n >= 200 && n <= 294):
                return GetNode("Item/Consume/" + ID.LeftStr(4) + ".img/" + ID);
                break;
            case int n when (n >= 400 && n <= 446):
                return GetNode("Item/Etc/" + ID.LeftStr(4) + ".img/" + ID);
                break;

            case 500:
                return GetNode("Item/Pet/" + ID + ".img");
                break;
        }
        return null;
    }


}

internal class WzDict
{
    public static int GetInt(string Path, int DefaultValue = 0)
    {
        if (Wz.Data.ContainsKey(Path))
            return Wz.Data[Path].GetValueEx<int>(0);
        else
            return DefaultValue;
    }

    public static string GetStr(string Path, string DefaultValue = "")
    {
        if (Wz.Data.ContainsKey(Path))
            return Wz.Data[Path].GetValueEx<string>("");
        return
            DefaultValue;
    }
    public static Wz_Vector GetVector(string Path)
    {
        if (Wz.Data.ContainsKey(Path))
        {
            return Wz.Data[Path].ToVector();
        }
        else
            return new Wz_Vector(0, 0);
    }

    public static Vector2 GetVectorE(string Path)
    {
        if (Wz.EquipData.ContainsKey(Path))
        {
            Vector2 V;
            V.X = Wz.EquipData[Path].ToVector().X;
            V.Y = Wz.EquipData[Path].ToVector().Y;
            return V;
        }
        else
            return new Vector2(0, 0);
    }
    public static bool GetBool(string Path)
    {
        if (Wz.Data.ContainsKey(Path))
        {
            return Wz.Data[Path].ToBool();
        }
        else
            return false;
    }
}

public static class Wz_NodeExtension3
{
    public static Wz_Node GetNode(this Wz_Node Node, string Path)
    {

        if (Node.FindNodeByPathA(Path, true) != null)
        {
            if (Node.FindNodeByPathA(Path, true).Value is Wz_Uol)
            {
                Wz_Node Child = Node.FindNodeByPathA(Path, true).ResolveUol();

                if (Child != null)
                {
                    switch (Child.Value)
                    {
                        //uol link to uol
                        case Wz_Uol:
                            return Child.ParentNode.Get(Child.ToStr());
                            break;
                        // UOL link to Canvas
                        case Wz_Png:
                            if (Child.Nodes["_inlink"] != null)
                                return Wz.GetTopEntry(Child).Get(Child.Nodes["_inlink"].ToStr());
                            else if (Child.Nodes["_outlink"] != null)
                                return Wz.GetImgNode(Child.Nodes["_outlink"].ToStr());
                            break;
                    }
                }
                return Child;
            }
            else
            {
                var FullPath = Node.FindNodeByPathA(Path, true).FullPathToFileEx();

                string[] Split = FullPath.Split('/');
                switch (Split[0])
                {
                    case "Map001":
                        FullPath = FullPath.Replace("Map001", "Map");
                        break;
                    case "Map002":
                        FullPath = FullPath.Replace("Map002", "Map");
                        break;
                    case "Map2":
                        FullPath = FullPath.Replace("Map2", "Map");
                        break;
                    case "Mob001":
                        FullPath = FullPath.Replace("Mob001", "Mob");
                        break;
                    case "Mob002":
                        FullPath = FullPath.Replace("Mob002", "Mob");
                        break;
                    case "Mob2":
                        FullPath = FullPath.Replace("Mob2", "Mob");
                        break;
                    case "Skill001":
                        FullPath = FullPath.Replace("Skill001", "Skill");
                        break;
                    case "Skill002":
                        FullPath = FullPath.Replace("Skill002", "Skill");
                        break;
                    case "Skill003":
                        FullPath = FullPath.Replace("Skill003", "Skill");
                        break;
                    case "Sound001":
                        FullPath = FullPath.Replace("Sound001", "Sound");
                        break;
                    case "Sound002":
                        FullPath = FullPath.Replace("Sound002", "Sound");
                        break;
                    case "Sound2":
                        FullPath = FullPath.Replace("Sound2", "Sound");
                        break;
                }

                Node = PluginManager.FindWz(FullPath);
                return Node.GetLinkedSourceNode(PluginManager.FindWz);

            }
        }
        else
        {
            string[] Split = Path.Split('/');
            int Count = 0;
            string Str = "";
            string Path1 = "";
            string Path2 = "";
            bool HasUol = false;
            for (int i = 0; i < Split.Length; i++)
            {

                if (i == 0)
                    Str = Str + Split[i];
                else
                    Str += '/' + Split[i];
                if ((Node.FindNodeByPathA(Str, true) != null) && (Node.FindNodeByPathA(Str, true).Value is Wz_Uol))
                {
                    HasUol = true;
                    Count = i;
                    Path1 = Str;
                    break;
                }

            }

            if (HasUol)
            {
                Str = "";
                for (int i = Count + 1; i < Split.Length; i++)
                {
                    if (i == Count + 1)
                        Str = Str + Split[i];
                    else
                        Str = Str + '/' + Split[i];
                    Path2 = Str;
                }
                return Node.FindNodeByPathA(Path1, true).ResolveUol().FindNodeByPathA(Path2, true);
            }
        }
        return null;
    }

    public static Wz_Node.WzNodeCollection GetNodes(this Wz_Node Node, string Path)
    {
        return Node.GetNode(Path).Nodes;
    }

    public static bool HasNode(this Wz_Node Node, string Path)
    {
        return Node.GetNode(Path) != null;

    }
    public static int GetInt(this Wz_Node Node, string Path, int DefaultValue = 0)
    {
        return Node.GetNode(Path).GetValueEx(DefaultValue);
    }
    public static bool GetBool(this Wz_Node Node, string Path)
    {
        return Convert.ToBoolean(Node.GetNode(Path).GetValueEx<int>(0));
    }
    public static string GetStr(this Wz_Node Node, string Path, string DefaultStr = "")
    {
        return Node.GetNode(Path).GetValueEx<string>(DefaultStr);
    }
    public static Wz_Vector GetVector(this Wz_Node Node, string Path)
    {
        return Node.GetNode(Path).GetValueEx<Wz_Vector>(new Wz_Vector(0, 0));

    }
    public static Bitmap GetBmp(this Wz_Node Node, string Path)
    {
        return Node.GetNode(Path).ExtractPng();
    }

    public static Wz_Node Get2(this Wz_Node Node, string Path)
    {
        var Split = Path.Split('/');
        var Result = Node;
        for (int i = 0; i < Split.Length; i++)
        {
            if (Split[i] == "..")
                Result = Result.ParentNode;
            else
                Result = Result.Nodes[Split[i]];
        }

        switch (Result.Value)
        {
            case Wz_Uol:
                Wz_Node Entry = Result.ParentNode;
                Wz_Node Child = Entry.Get(Result.ToStr());
                if (Child == null)
                {
                    string Err = Result.FullPathToFile2();
                    if (Err.LeftStr(3) != "Npc")
                    {
                        string s1 = Result.FullPathToFile2();
                        string s2 = Result.ToStr().Replace("../", "");
                        Split = s2.Split("/");
                        s2 = "";
                        for (int i = 0; i < Split.Length; i++)
                            s2 = s2 + Split[i] + "/";
                        s2 = Split[0] + ".img/" + s2;
                        s2 = s2.Remove(s2.Length - 1);
                        Child = Wz.GetImgNode(s1.Replace(s1.RightStr(s2.Length), s2));
                        if (Child == null)
                            return Wz.GetImgNode("Character/00002000.img/alert/0/arm");
                    }
                    else
                        Child = Wz.GetImgNode("Character/00002000.img/alert/0/arm");
                }

                if (Child != null)
                {
                    switch (Child.Value)
                    {
                        //uol link to uol
                        case Wz_Uol:
                            return Child.ParentNode.Get(Child.ToStr());
                            break;
                        // UOL link to Canvas
                        case Wz_Png:
                            if (Child.Nodes["_inlink"] != null)
                                return Wz.GetTopEntry(Child).Get(Child.Nodes["_inlink"].ToStr());
                            else if (Child.Nodes["_outlink"] != null)
                                return Wz.GetImgNode(Child.Nodes["_outlink"].ToStr());
                            break;
                    }
                }
                return Child;
                break;

            case Wz_Png:
                if (Result.Nodes["_outlink"] != null)
                {
                    string OutLink = Result.Nodes["_outlink"].ToStr();
                    Result = Wz.GetImgNode(OutLink);
                }
                else if (Result.Nodes["_inlink"] != null)
                {
                    Result = Wz.GetTopEntry(Result).Get(Result.Nodes["_inlink"].ToStr());

                }
                else if (Result.Nodes["source"] != null)
                {
                    Result = Wz.GetImgNode(Result.Nodes["source"].ToStr());
                }
                break;

        }
        return Result;
    }
}

