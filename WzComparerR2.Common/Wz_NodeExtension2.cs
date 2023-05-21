using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WzComparerR2.WzLib;
using System.Drawing;

namespace WzComparerR2.Common
{
    public static class Wz_NodeExtension2
    {
        public static Wz_Node GetLinkedSourceNode(this Wz_Node node, GlobalFindNodeFunction findNode)
        {
            string path;

            if (!string.IsNullOrEmpty(path = node.Nodes["source"].GetValueEx<string>(null)))
            {
                return findNode?.Invoke(path);
            }
            else if (!string.IsNullOrEmpty(path = node.Nodes["_inlink"].GetValueEx<string>(null)))
            {
                var img = node.GetNodeWzImage();
                return img?.Node.FindNodeByPath(true, path.Split('/'));
            }
            else if (!string.IsNullOrEmpty(path = node.Nodes["_outlink"].GetValueEx<string>(null)))
            {
                return findNode?.Invoke(path);
            }
            else
            {
                return node;
            }
        }

        public static Wz_Node Get(this Wz_Node Node, string Path)
        {
            var Split = Path.Split('/');
            var Result = Node;
            for (int i = 0; i < Split.Length; i++)
            {
                if (Split[i] == "..")
                    Result = Result.ParentNode;
                else
                    Result = Result.Nodes[Split[i]];
                if (Result == null)
                    return null;
            }
            return Result;
        }

        public static string IDString(this string Str)
        {
            return int.Parse(Str).ToString();

        }

        public static string GetPathD(this Wz_Node Node)
        {
            if (Node != null)
            {
                Stack<string> Path = new Stack<string>();
                Wz_Node ThisNode = Node;
                do
                {
                    Path.Push(ThisNode.Text);
                    ThisNode = ThisNode.ParentNode;
                } while (ThisNode != null);
                return string.Join(".", Path.ToArray());
            }
            return null;
        }

        public static string GetPath(this Wz_Node Node)
        {
            Stack<string> Path = new Stack<string>();
            Wz_Node ThisNode = Node;
            do
            {
                Path.Push(ThisNode.Text);
                ThisNode = ThisNode.ParentNode;
            } while (ThisNode != null);
            return string.Join("/", Path.ToArray());

        }

        public static int ToInt(this Wz_Node Node, int DefaultValue = 0)
        {
            return Node.GetValueEx<int>(DefaultValue);
        }
        public static string ToStr(this Wz_Node Node)
        {
            if (Node.Value is string)
                return Node.GetValueEx<string>("");
            else if (Node.Value is Wz_Uol)
                return Node.GetValue<Wz_Uol>().Uol;
            else
                return "";
        }
        public static string ToStr(this Wz_Node Node, string DefaultValue)
        {
            return Node.GetValueEx<string>(DefaultValue);
        }

        public static bool ToBool(this Wz_Node Node)
        {
            return Convert.ToBoolean(Node.GetValueEx<int>(0));
        }
        public static Wz_Vector ToVector(this Wz_Node Node)
        {

            return Node.GetValueEx<Wz_Vector>(new Wz_Vector(0, 0));

        }


        public static bool HasNode(this Wz_Node Node, string Path)
        {
            return Node.GetNode(Path) != null;

        }

        public static string ImgName(this Wz_Node Node)
        {

            return Node.GetNodeWzImage().Name;

        }

        public static string ImgID(this Wz_Node Node)
        {

            return Node.GetNodeWzImage().Name.Replace(".img", "");

        }
        public static T GetValue2<T>(this Wz_Node Node, string Path, T DefaultValue)
        {
            if (Node.FindNodeByPathA(Path, true) != null)
                return Node.FindNodeByPathA(Path, true).GetValueEx(DefaultValue);
            else
                return DefaultValue;
        }

        public static Wz_Node FindNodeByPathA(this Wz_Node Node, string FullPath, bool ExtractImage)
        {

            string[] Patten = FullPath.Split('/');
            return Node.FindNodeByPath(ExtractImage, Patten);

        }

        public static string FullPathToFileEx(this Wz_Node Node)
        {


            Stack<string> path = new Stack<string>();
            Wz_Node node = Node;
            do
            {
                if (node.Value is Wz_File wzf && !wzf.IsSubDir)
                {
                    if (node.Text.EndsWith(".wz", StringComparison.OrdinalIgnoreCase))
                    {
                        path.Push(node.Text.Substring(0, node.Text.Length - 3));
                    }
                    else
                    {
                        path.Push(node.Text);
                    }
                    break;
                }

                path.Push(node.Text);

                var img = node.GetValue<Wz_Image>();
                if (img != null)
                {
                    node = img.OwnerNode;
                }

                if (node != null)
                {
                    node = node.ParentNode;
                }
            } while (node != null);
            return string.Join("/", path.ToArray());

        }

        public static string FullPathToFile2(this Wz_Node Node)
        {
            var FullPath = Node.FullPathToFileEx();
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
            return FullPath;

        }

        public static string FullPathToFile2D(this Wz_Node Node)
        {
            var Path = FullPathToFile2(Node);
            Path = Path.Replace("/", ".");
            return Path;
        }


        public static Wz_Node GetNode(this Wz_Node Node, string Path)
        {

            if (Node.FindNodeByPathA(Path, true) != null)
            {
                if (Node.FindNodeByPathA(Path, true).Value is Wz_Uol)
                {
                    return Node.FindNodeByPathA(Path, true).ResolveUol();
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

                    Node = PluginBase.PluginManager.FindWz(FullPath);
                    return Node.GetLinkedSourceNode(PluginBase.PluginManager.FindWz);

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




        public static Bitmap ExtractPng(this Wz_Node Node)
        {
            return (Node.Value as Wz_Png).ExtractPng();

        }





    }
}


