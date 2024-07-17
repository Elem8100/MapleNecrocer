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
    public string PortalName, ToName, ToMap;
    public int X, Y, Type;
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
    int Type;
    string ToName;
    int ToMap;
    static int Version;
    public static List<PortalInfo> PortalList = new();
    public static PortalInfo PortalInfo;
    byte c;
    public static void Create()
    {
        if (PortalList == null)
            PortalList = new();
        else
            PortalList.Clear();

        if (!Wz.HasNode("Map/MapHelper.img/portal/game/pv/default"))
        {
            Wz.DumpData(Wz.GetNode("Map/MapHelper.img/portal/game/pv"), Wz.Data, Wz.ImageLib);
            Wz.DumpData(Wz.GetNode("Map/MapHelper.img/portal/game/ph/default/portalContinue"), Wz.Data, Wz.ImageLib);
            Version = 0;
        }
        else
        {
            Wz.DumpData(Wz.GetNode("Map/MapHelper.img/portal/game/pv/default"), Wz.Data, Wz.ImageLib);
            Wz.DumpData(Wz.GetNode("Map/MapHelper.img/portal/game/ph/default/portalStart"), Wz.Data, Wz.ImageLib);
            Version = 1;
        }

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
            PortalInfo.Type = Iter.GetInt("pt");
            PortalList.Add(PortalInfo);
            if (PType == 2 || PType == 10)
            {
                var MapPortal = new MapPortal(EngineFunc.SpriteEngine);
                MapPortal.ImageLib = Wz.ImageLib;
                if (Version == 0)
                {
                    switch (PType)
                    {
                        case 2:
                            MapPortal.InfoPath = "Map/MapHelper.img/portal/game/pv"; ;
                            break;
                        case 10:
                            MapPortal.InfoPath = "Map/MapHelper.img/portal/game/ph/default/portalContinue";
                            MapPortal.Alpha = 0;
                            break;
                    }
                }
                else if (Version == 1)
                {
                    switch (PType)
                    {
                        case 2:
                            MapPortal.InfoPath = "Map/MapHelper.img/portal/game/pv/default"; ;
                            break;
                        case 10:
                            MapPortal.InfoPath = "Map/MapHelper.img/portal/game/ph/default/portalStart";
                            MapPortal.Alpha = 0;
                            break;
                    }
                }

                MapPortal.ImageNode = Wz.Data[MapPortal.InfoPath + "/0"];
                MapPortal.Type = PortalInfo.Type;
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
        Time += 16.66f;
        if (Time > 100)
        {
            Frame += 1;
            if (!Wz.HasData(InfoPath + '/' + Frame))
                Frame = 0;
            Time = 0;
        }
        Wz_Vector origin = WzDict.GetVector(ImagePath + "/origin");
        Origin.X = origin.X;
        Origin.Y = origin.Y;

        if (Type == 10)
        {
            float DistanceX = Math.Abs(Game.Player.X - X);
            if (DistanceX < 180 && Alpha < 255)
            {
                Alpha += 5;
            }
            else if (Alpha > 0)
            {
                Alpha -= 5;
            }
        }
    }

    public override void DoDraw()
    {
        if (Map.ShowPortal)
            base.DoDraw();

        if (Map.ShowPortalInfo)
        {
            int WX = (int)X - (int)Engine.Camera.X;
            int WY = (int)Y - (int)Engine.Camera.Y;

            Engine.Canvas.DrawString(Map.ToolTipFont, "pn (Name)  : " + PortalName, WX - 50, WY - 170, Microsoft.Xna.Framework.Color.Red);
            Engine.Canvas.DrawString(Map.ToolTipFont, "tm (ToMap) : " + ToMap.ToString().PadLeft(9, '0'), WX - 50, WY - 150, Microsoft.Xna.Framework.Color.Red);
            Engine.Canvas.DrawString(Map.ToolTipFont, "tn (ToName): " + ToName, WX - 50, WY - 130, Microsoft.Xna.Framework.Color.Red);
        }
    }


}

