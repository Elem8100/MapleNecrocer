using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.UI.Forms;
using MapleNecrocer;
using WzComparerR2.WzLib;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;
namespace GameUI;
public class MiniMap : UIForm
{
    public static int Version;
    public static bool HasMiniMap;
    int PWidth;
    int cx, cy;
    int OffX, OffY;
    int AddHeight, AddWidth;
    bool HasMark;
    RenderTarget2D RenderTarget;
    Wz_Node PlayerMark;
    void DrawVersion1()
    {
        Wz_Node UIEntry = Wz.GetNode("UI/UIWindow.img/MiniMap/MaxMap");
        Wz.DumpData(UIEntry, Wz.UIData, Wz.UIImageLib);
        int PicWidth, PicHeight;
        var Canvas = EngineFunc.Canvas;
        if (Map.Img.HasNode("miniMap"))
        {
            HasMiniMap = true;
            cx = Map.Img.GetInt("miniMap/centerX");
            cy = Map.Img.GetInt("miniMap/centerY");
            var MiniMapNode = Map.Img.GetNode("miniMap");
            Wz.DumpData(MiniMapNode, Wz.UIData, Wz.UIImageLib);
            var MiniMapPng = Map.Img.GetBmp("miniMap/canvas");

            PicHeight = MiniMapPng.Height;
            PicWidth = PWidth;
            OffX = (PicWidth - MiniMapPng.Width) / 2;
            var Left = ((PicWidth + 13) - MiniMapPng.Width) / 2;
            Canvas.FillRect(7, 72, Left, PicHeight, new Color(128, 128, 128, 128));
            Canvas.FillRect(OffX + 13 + MiniMapPng.Width, 72, Left, PicHeight, new Color(128, 128, 128, 128));
            Canvas.FillRect(OffX + 13, 72, MiniMapPng.Width, PicHeight, new Color(0, 0, 0, 128));
            Canvas.Draw(Wz.UIImageLib[MiniMapNode.GetNode("canvas")], 9 + OffX + 3, 72);
        }
        else
        {
            cx = 0;
            cy = 0;
            OffX = 0;
            OffY = 0;
            PicWidth = 150;
            PicHeight = 100;
            Canvas.FillRect(9, 62, PicWidth, PicHeight, new Color(0, 0, 0, 180));
        }

        for (int X = 0; X <= PicWidth + 10; X++)
        {
            Canvas.Draw(Wz.UIImageLib[UIEntry.GetNode("n")], 4 + X + 3, 0);
            Canvas.Draw(Wz.UIImageLib[UIEntry.GetNode("s")], 4 + X + 3, PicHeight + 62 + 10);
        }

        for (int Y = 0; Y <= PicHeight - 1; Y++)
        {
            Canvas.Draw(Wz.UIImageLib[UIEntry.GetNode("w")], 1, 72 + Y);
            Canvas.Draw(Wz.UIImageLib[UIEntry.GetNode("e")], PicWidth + 18, 72 + Y);
        }
        Canvas.Draw(Wz.UIImageLib[UIEntry.GetNode("nw")], 1, 0); //left top
        Canvas.Draw(Wz.UIImageLib[UIEntry.GetNode("ne")], PicWidth + 18, 0); //right top
        Canvas.Draw(Wz.UIImageLib[UIEntry.GetNode("sw")], 1, PicHeight + 72); // right bottom
        Canvas.Draw(Wz.UIImageLib[UIEntry.GetNode("se")], PicWidth + 18, PicHeight + 72); // left botton

        if (Wz.HasNode("Map/MapHelper.img/minimap"))
            HasMark = true;

        if (HasMark)
        {
            Wz.DumpData(Wz.GetNode("Map/MapHelper.img/minimap"), Wz.UIData, Wz.UIImageLib);
            var NpcMark = Wz.GetNode("Map/MapHelper.img/minimap/npc");
            foreach (var Iter in Map.Img.GetNodes("life"))
            {
                if (Iter.GetStr("type") == "n" && Iter.GetInt("hide") != 1)
                    Canvas.Draw(Wz.UIImageLib[NpcMark], ((Iter.GetInt("x") + cx) / 16)
                      + OffX + 12, ((Iter.GetInt("y") + cy) / 16) + 65);
            }
            var PortalMark = Wz.GetNode("Map/MapHelper.img/minimap/portal");
            foreach (var Iter in Map.Img.GetNodes("portal"))
            {
                if (Iter.GetInt("pt") == 2 || Iter.GetInt("pt") == 7)
                    Canvas.Draw(Wz.UIImageLib[PortalMark], ((Iter.GetInt("x") + cx) /
                      16) + OffX + 10, ((Iter.GetInt("y") + cy) / 16) + 63);
            }
            PlayerMark = Wz.GetNode("Map/MapHelper.img/minimap/user");
        }
        else
        {
            foreach (var Iter in Map.Img.GetNodes("portal"))
            {
                if (Iter.GetInt("pt") == 2 || (Iter.GetInt("pt") == 7))
                {
                    var X = ((Iter.GetInt("x") + cx) / 16) + OffX + 10;
                    var Y = ((Iter.GetInt("y") + cy) / 16) + 67;
                    Canvas.FillRect(X, Y, 5, 5, new Color(132, 216, 243, 255));
                }
            }
        }

        var MapMarkName = Map.Img.GetStr("info/mapMark");
        if (MapMarkName != "None")
        {
            var MapMarkPic = Wz.GetNode("Map/MapHelper.img/mark/" + MapMarkName);
            Wz.DumpData(MapMarkPic, Wz.UIData, Wz.UIImageLib);
            Canvas.Draw(Wz.UIImageLib[MapMarkPic], 7, 22);
        }

        if (Map.MapNameList.ContainsKey(Map.ID))
        {
            Canvas.DrawString(Map.NpcNameTagFont, Map.MapNameList[Map.ID].StreetName, 49, 26, Color.White);
            Canvas.DrawString(Map.NpcNameTagFont, Map.MapNameList[Map.ID].MapName, 49, 43, Color.White);
        }
    }

    void DrawVersion3()
    {
        Wz_Node UIEntry = Wz.GetNodeA("UI/UIWindow2.img/MiniMap/MaxMap");
        // Wz.UIImageLib.Clear();
        if (!Wz.UIData.ContainsKey("UI/UIWindow2.img/MiniMap/MaxMap"))
            Wz.DumpData(UIEntry, Wz.UIData, Wz.UIImageLib);
        int PicWidth, PicHeight;
        var Canvas = EngineFunc.Canvas;
        if (Map.Img.HasNode("miniMap"))
        {
            HasMiniMap = true;
            cx = Map.Img.GetInt("miniMap/centerX");
            cy = Map.Img.GetInt("miniMap/centerY");
            var MiniMapNode = Map.Img.GetNode("miniMap");
            Wz.DumpData(MiniMapNode, Wz.UIData, Wz.UIImageLib);
            var MiniMapPng = Map.Img.GetBmp("miniMap/canvas");

            PicHeight = MiniMapPng.Height;
            PicWidth = PWidth;
            OffX = (PicWidth - MiniMapPng.Width) / 2;
            Canvas.FillRect(9, 62, PicWidth, PicHeight, new Color(0, 0, 0, 180));
            Canvas.Draw(Wz.UIImageLib[MiniMapNode.Get("canvas")], 9 + OffX, 62);
        }
        else
        {
            cx = 0;
            cy = 0;
            OffX = 0;
            OffY = 0;
            PicWidth = 150;
            PicHeight = 100;
            Canvas.FillRect(9, 62, PicWidth, PicHeight, new Color(0, 0, 0, 180));
        }

        for (int X = 0; X <= PicWidth - 111; X++)
        {
            Canvas.Draw(Wz.UIImageLib[UIEntry.Get("n")], 64 + X, 0);
            Canvas.Draw(Wz.UIImageLib[UIEntry.Get("s")], 64 + X, PicHeight + 62);
        }

        for (int Y = 0; Y <= PicHeight - 24; Y++)
        {
            Canvas.Draw(Wz.UIImageLib[UIEntry.Get("w")], 0, 67 + Y);
            Canvas.Draw(Wz.UIImageLib[UIEntry.Get("e")], PicWidth + 9, 67 + Y);
        }
        Canvas.Draw(Wz.UIImageLib[UIEntry.Get("nw")], 0, 0); //left top
        Canvas.Draw(Wz.UIImageLib[UIEntry.Get("ne")], PicWidth - 46, 0); //right top
        Canvas.Draw(Wz.UIImageLib[UIEntry.Get("sw")], 0, PicHeight + 44); // right bottom
        Canvas.Draw(Wz.UIImageLib[UIEntry.Get("se")], PicWidth - 46, PicHeight + 44); // left botton
        Wz.DumpData(Wz.GetNode("Map/MapHelper.img/minimap"), Wz.UIData, Wz.UIImageLib);

        var NpcMark = Wz.GetNodeA("Map/MapHelper.img/minimap/npc");
        foreach (var Iter in Map.Img.GetNodes("life"))
        {
            if (Iter.GetStr("type") == "n" && Iter.GetInt("hide") != 1)
                Canvas.Draw(Wz.UIImageLib[NpcMark], ((Iter.GetInt("x") + cx) / 16)
                  + OffX + 4, ((Iter.GetInt("y") + cy) / 16) + 50);
        }

        var PortalMark = Wz.GetNodeA("Map/MapHelper.img/minimap/portal");
        foreach (var Iter in Map.Img.GetNodes("portal"))
        {
            if (Iter.GetInt("pt") == 2 || Iter.GetInt("pt") == 7)
                Canvas.Draw(Wz.UIImageLib[PortalMark], ((Iter.GetInt("x") + cx) /
                  16) + OffX + 2, ((Iter.GetInt("y") + cy) / 16) + 48);
        }

        var MapMarkName = Map.Img.GetStr("info/mapMark");
        if (MapMarkName != "None")
        {
            var MapMarkPic = Wz.GetNodeA("Map/MapHelper.img/mark/" + MapMarkName);
            Wz.DumpData(MapMarkPic, Wz.UIData, Wz.UIImageLib);
            Canvas.Draw(Wz.UIImageLib[MapMarkPic], 7, 17);
        }
        PlayerMark = Wz.GetNodeA("Map/MapHelper.img/minimap/user");

        if (Map.MapNameList.ContainsKey(Map.ID))
        {
            Canvas.DrawString(Map.NpcNameTagFont, Map.MapNameList[Map.ID].StreetName, 50, 20, Color.White);
            Canvas.DrawString(Map.NpcNameTagFont, Map.MapNameList[Map.ID].MapName, 50, 37, Color.White);
        }
    }

    public void RenderTargetFunc()
    {
        switch (Version)
        {
            case 1:
                DrawVersion1();
                break;
            case 3:
                DrawVersion3();
                break;
        }
    }

    public void ReDraw()
    {
        float Length = 0;
        float Length1 = 0, Length2 = 0;

        if (Map.MapNameList.ContainsKey(Map.ID))
        {
            Length1 = Map.MeasureStringX(Map.NpcNameTagFont, Map.MapNameList[Map.ID].StreetName);
            Length2 = Map.MeasureStringX(Map.NpcNameTagFont, Map.MapNameList[Map.ID].MapName);
        }
        Length = Math.Max(Length1, Length2);

        if (Version == 1)
        {
            AddWidth = -50;
            AddHeight = 12;
        }
        else
        {
            AddWidth = 0;
            AddHeight = 0;
        }

        if (Map.Img.HasNode("miniMap"))
        {
            var MiniMapPng = Map.Img.GetBmp("miniMap/canvas");
            PWidth = Math.Max((int)Length, MiniMapPng.Width + AddWidth) + 40;
            EngineFunc.Canvas.DrawTarget(ref RenderTarget, PWidth + 50, MiniMapPng.Height + 80 + AddHeight, () => RenderTargetFunc());
            this.Size = new Microsoft.Xna.Framework.Vector2(PWidth + 20, MiniMapPng.Height + 40);
        }
        else
        {
            EngineFunc.Canvas.DrawTarget(ref RenderTarget, 1, 1, () => RenderTargetFunc());
            this.Size = new Microsoft.Xna.Framework.Vector2(1, 1);
        }
    }

    internal override void DoDraw(Vector2 offset)
    {
        if (!Map.ShowMiniMap)
            return;
        if (!IsVisible)
            return;

        if (HasMiniMap)
        {
            SpriteBatch.Draw(RenderTarget, new Vector2(Location.X, Location.Y), Color.White);
            int px = (int)(Game.Player.X + cx) / 16;
            int py = (int)(Game.Player.Y + cy) / 16;
            if (Version == 1)
            {
                if (HasMark)
                    SpriteBatch.Draw(Wz.UIImageLib[PlayerMark], new Vector2(Location.X + px + OffX + 2 + 8, Location.Y + py + OffY + 50 + 15), Color.White);
                else
                    SpriteBatch.FillRectangle(new Microsoft.Xna.Framework.Rectangle((int)Location.X + px + OffX + 2 + 8, (int)Location.Y + py + OffY +
                      50 + 17, 5, 5), new Color(0, 255, 255, 255));
            }
            else
            {
                SpriteBatch.Draw(Wz.UIImageLib[PlayerMark], new Vector2(Location.X + px + OffX + 2, Location.Y + py + OffY + 50), Color.White);
            }
        }

        foreach (var control in Controls)
        {
            if (control.IsVisible)
                control.DoDraw(Location);
        }
    }

}
