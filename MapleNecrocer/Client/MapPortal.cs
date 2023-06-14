using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WzComparerR2.Common;
using WzComparerR2.WzLib;

namespace MapleNecrocer;

public struct PortalInfo
{
    public string PortalName, ToName,ToMap;
    public int X, Y, PortalType;
}
public class MapPortal : SpriteEx
{
    public MapPortal(Sprite Parent) : base(Parent)
    {

    }
    float Time;
    int Frame;
    string InfoPath;
    string PortalName;
    int PortalType;
    string ToName;
    int ToMap;
    public static List<PortalInfo> PortalList=new();
    public static PortalInfo PortalInfo;
    public static void Create()
    {
        if (PortalList == null)
            PortalList = new();
        else
            PortalList.Clear();
        Wz_Node Node = null;
        if (Wz.GetNode("Map/MapHelper.img/portal/game/pv/default") == null)
            Node = Wz.GetNode("Map/MapHelper.img/portal/game/pv");
        else
            Node = Wz.GetNode("Map/MapHelper.img/portal/game/pv/default");
        Wz.DumpData(Node, Wz.Data, Wz.ImageLib);
        
        PortalInfo = new PortalInfo();
        int PType;
        foreach (var Iter in Map.Img.Nodes["portal"].Nodes)
        {
            PType = Iter.GetInt("pt");
            PortalInfo.X = Iter.GetInt("x");
            PortalInfo.Y = Iter.GetInt("y");
            PortalInfo.ToMap = Iter.GetInt("tm").ToString();
            PortalInfo.ToName = Iter.GetStr("tn");
            PortalInfo.PortalName = Iter.GetStr("pn");
            PortalInfo.PortalType = Iter.GetInt("pt");
            PortalList.Add(PortalInfo);
            if (PType == 2)
            {
                var MapPortal = new MapPortal(EngineFunc.SpriteEngine);
                MapPortal.ImageLib = Wz.ImageLib;
                MapPortal.InfoPath = Node.FullPathToFile2();
                MapPortal.ImageNode = Wz.Data[MapPortal.InfoPath + "/0"];
                MapPortal.X = Iter.GetInt("x");
                MapPortal.Y = Iter.GetInt("y");
                MapPortal.Z = 1000000;
                MapPortal.PortalName = Iter.GetStr("pn");
                MapPortal.ToMap = Iter.GetInt("tm");
                MapPortal.ToName = Iter.GetStr("tn");
                MapPortal.Width = MapPortal.ImageWidth;
                MapPortal.Height = MapPortal.ImageHeight + 100;
                //MapPortal.IntMove= false;
            }
        }
    }
    public static PortalInfo Find(Vector2 P, ref bool OnPortal)
    {
        OnPortal = false;
        foreach (var Portal in PortalList)
        {
            if ((P.X > Portal.X - 15) && (P.X < Portal.X + 15) && (P.Y < Portal.Y + 12) && (P.Y > Portal.Y - 12))
            {
                OnPortal = true;
                return Portal;
            }
        }
        return new PortalInfo();
    }
    public override void DoMove(float Delta)
    {

        base.DoMove(Delta);
        ImageNode = Wz.Data[InfoPath + "/" + Frame];
        string ImagePath = InfoPath + "/" + Frame;
        Time += 16.66f * Delta;
        if (Time > 100)
        {
            Frame += 1;
            if (!Wz.HasData(InfoPath + '/' + Frame))
                Frame = 0;
            Time = 0;
        }
        Wz_Vector origin = Wz.GetVector(ImagePath + "/origin");
        Origin.X = origin.X;
        Origin.Y = origin.Y;
    }



}

