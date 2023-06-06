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
    public static string Country;
    private static List<NodeInfo> NodeList1 = new();
    private static List<NodeInfo> NodeList2 = new();
    static Texture2D TextureHSL(Bitmap Bitmap, int Hue, int Saturation)
    {
        return null;

    }


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
        return Data.ContainsKey(Path) != false;

    }
    public static bool HasNodeE(string Path)
    {
        return EquipData.ContainsKey(Path) != false;

    }
    public static int GetInt(string Path, int DefaultValue = 0)
    {
        if (Data.ContainsKey(Path))
            return Data[Path].GetValueEx<int>(0);
        else
            return DefaultValue;
    }

    public static string GetStr(string Path, string DefaultValue = "")
    {
        if (Data.ContainsKey(Path))
            return Data[Path].GetValueEx<string>("");
        return
            DefaultValue;
    }
    public static Wz_Vector GetVector(string Path)
    {
        if (Data.ContainsKey(Path))
        {
            return Data[Path].ToVector();
        }
        else
            return new Wz_Vector(0, 0);
    }

    public static Vector2 GetVectorE(string Path)
    {
        if (EquipData.ContainsKey(Path))
        {
            Vector2 V;
            V.X = EquipData[Path].ToVector().X;
            V.Y = EquipData[Path].ToVector().Y;
            return V;
        }
        else
            return new Vector2(0, 0);
    }
    public static bool GetBool(string Path)
    {
        if (Data.ContainsKey(Path))
        {
            return Data[Path].ToBool();
        }
        else
            return false;
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

}

public static class Wz_NodeExtension3
{
    public static int GetInt(this Wz_Node Node, string Path, int DefaultValue = 0)
    {
        return Node.GetNode(Path).GetValueEx(DefaultValue);
    }
    public static bool GetBool(this Wz_Node Node, string Path)
    {
        return Convert.ToBoolean(Node.GetNode(Path).GetValueEx<int>(0));
    }
    public static string GetStr(this Wz_Node Node, string Path)
    {
        return Node.GetNode(Path).GetValueEx<string>("");
    }
    public static Wz_Vector GetVector(this Wz_Node Node, string Path)
    {
        return Node.GetNode(Path).GetValueEx<Wz_Vector>(new Wz_Vector(0,0));
      
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

