using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;
using WzComparerR2.PluginBase;
using Spine;
using WzComparerR2.Animation;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using DevComponents.DotNetBar;
using SharpDX;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.PortableExecutable;
using static System.Net.Mime.MediaTypeNames;
using MonoGame.SpriteEngine;
using System.Security.Cryptography;
using DevComponents.DotNetBar.Controls;
using System.ComponentModel.Design;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MapleNecrocer;

public struct BalloonInfo
{
    public Wz_Node ImageNode;
    public int Width, Height;
    public Vector2 Origin;
}
public class ChatBalloon : SpriteEx
{
    public ChatBalloon(Sprite Parent) : base(Parent)
    {
        Part1 = new BalloonInfo[30];
        Part2 = new BalloonInfo[30];
        Part3 = new BalloonInfo[30];

    }
    private int Col, Row, OffH, BWidth;
    private int MaxChars;
    private BalloonInfo[] Part1, Part2, Part3;
    BalloonInfo Arrow, C, E, N, NE, NW, S, SE, SW, W;
    int Style;
    string Directory;
    Wz_Node WzNode;
    public string Msg;
    int Counter;
    Microsoft.Xna.Framework.Color Color = new Color(0);
    int R, G, B;
    int FontColor;
    public RenderTarget2D RenderTarget;
    BalloonInfo GetData(string TileName)
    {
        BalloonInfo Result = new BalloonInfo();
        if (WzNode.ParentNode.Text == "ChatBalloon.img")
        {
            if (Wz.HasNode("UI/ChatBalloon.img/" + Style + "/c"))
                Result.ImageNode = Wz.GetNode("UI/ChatBalloon.img/" + Style + "/" + TileName);
            else
                Result.ImageNode = Wz.GetNode("UI/ChatBalloon.img/" + Style + "/0/" + TileName);

        }
        else
            Result.ImageNode = Wz.GetNodeA("UI/ChatBalloon.img/" + Directory + "/" + Style + "/" + TileName);
        Result.Width = WzNode.GetNode(TileName).ExtractPng().Width;
        Result.Height = WzNode.GetNode(TileName).ExtractPng().Height;
        Result.Origin.X = WzNode.Get(TileName + "/origin").ToVector().X;
        Result.Origin.Y = WzNode.Get(TileName + "/origin").ToVector().Y;
        return Result;
    }

    public void SetStyle(int BalloonStyle, string Dir = "")
    {
        Directory = Dir;
        Style = BalloonStyle;
        if (Directory == "")
        {
            if (Wz.HasNode("UI/ChatBalloon.img/" + Style + "/c"))
                WzNode = Wz.GetNode("UI/ChatBalloon.img/" + Style);
            else

                WzNode = Wz.GetNode("UI/ChatBalloon.img/" + Style + "/0");
        }
        else
            WzNode = Wz.GetNodeA("UI/ChatBalloon.img/" + Directory + "/" + Style);
        Wz.DumpData(WzNode, Wz.Data, Wz.EquipImageLib);

        Engine.Canvas.DrawTarget(ref RenderTarget, 150, 512, () => { });
        if (WzNode.Get("clr") != null)
            FontColor = 16777216 + WzNode.Get("clr").ToInt();
        else
            FontColor = 16777215;
        string Hex = FontColor.ToString("X").PadLeft(6, '0');
        R = (byte)Convert.ToInt32(Hex.LeftStr(2), 16);
        G = (byte)Convert.ToInt32(Hex.Substring(2, 2), 16);
        B = (byte)Convert.ToInt32(Hex.RightStr(2), 16);
        if (WzNode.Get("arrow") != null)
            Arrow = GetData("arrow");
        C = GetData("c");
        E = GetData("e");
        N = GetData("n");
        NE = GetData("ne");
        NW = GetData("nw");
        S = GetData("s");
        SE = GetData("se");
        SW = GetData("sw");
        W = GetData("w");
        BWidth = 90;
        Col = (BWidth / N.Width) + 1;
        MaxChars = (N.Width * Col) / 8;
        Part1[0] = C;
        Part1[1] = NW;
        Part2[0] = C;
        Part2[1] = W;
        Part3[0] = C;
        Part3[1] = SW;
        for (int i = 2; i <= Col; i++)
        {
            Part1[i] = N;
            Part2[i] = C;
            Part3[i] = S;
        }
        Part1[Col + 1] = NE;
        Part2[Col + 1] = E;
        Part3[Col + 1] = SE;
    }

    private static String ParseText(String Text, int Width)
    {
        String Line = String.Empty;
        String ReturnString = String.Empty;
        String[] WordArray = null;
        if (Wz.Region == "GMS")
            WordArray = Text.Split(' ');
        else
            WordArray = Text.Split('=');

        foreach (String Word in WordArray)
        {
            if (Map.MeasureStringX(Map.NpcBalloonFont, Line + Word) > Width)
            {
                ReturnString = ReturnString + Line + '\n';
                Line = String.Empty;
            }
            Line = Line + Word + ' ';
        }
        return ReturnString + Line;
    }

    public override void DoMove(float Delta)
    {
        Counter += 1;
        if (Counter % 100 == 0)
        {
            if (Msg != "")
            {
                Engine.Canvas.DrawTarget(ref RenderTarget, 150, 512, () => { RenderTargetFunc(); });
            }
        }
    }

    public override void DoDraw()
    {
        if (!Map.ShowNpcChat)
            return;
        if (!Map.ShowNpc)
            return;
        if (Msg != "")
            Engine.Canvas.Draw(RenderTarget, (int)X - 70 - (int)Engine.Camera.X, (int)Y - 500 - (int)Engine.Camera.Y);
        //Engine.Canvas.Draw(RenderTarget, X - 70 - Engine.Camera.X, Y - 500 - Engine.Camera.Y);
    }
    static bool Loaded;
    public void RenderTargetFunc()
    {
        if (Msg == null) return;
        String Line = String.Empty;
        String[] WordArray = null;
        if (Wz.Region == "GMS")
            WordArray = Msg.Split(' ');
        else
            WordArray = Msg.Split('=');

        int SplitWidth = 90;
        int RowCount = 0;
        foreach (String Word in WordArray)
        {
            if (Map.MeasureStringX(Map.NpcBalloonFont, Line + Word) > SplitWidth)
            {
                RowCount += 1;
                Line = String.Empty;
            }
            Line = Line + Word + ' ';
        }

        int Row = RowCount + 1;
        int OffH = Row * C.Height + (int)C.Origin.Y + S.Height;
        int Cx1 = 0;
        int Cx2 = 0;
        int Cx3 = 0;
        int Mid = (Col * N.Width / 2);


        for (int I = 1; I <= Col + 1; I++)
        {

            Cx1 += Part1[I - 1].Width;

            if (!Loaded)
            {
                Wz.EquipImageLib[Part1[I].ImageNode] = MedalTag.FixAlpha(Part1[I].ImageNode.ExtractPng());
                Wz.EquipImageLib[Part2[I].ImageNode] = MedalTag.FixAlpha(Part2[I].ImageNode.ExtractPng());
                Wz.EquipImageLib[Part3[I].ImageNode] = MedalTag.FixAlpha(Part3[I].ImageNode.ExtractPng());
            }
            if (Style > 0)
            {
                Wz.EquipImageLib[Part1[I].ImageNode] = MedalTag.FixAlpha(Part1[I].ImageNode.ExtractPng());
                Wz.EquipImageLib[Part2[I].ImageNode] = MedalTag.FixAlpha(Part2[I].ImageNode.ExtractPng());
                Wz.EquipImageLib[Part3[I].ImageNode] = MedalTag.FixAlpha(Part3[I].ImageNode.ExtractPng());
            }

            Engine.Canvas.Draw(Wz.EquipImageLib[Part1[I].ImageNode], Cx1 - NW.Origin.X - Mid + 70, -Part1[I].Origin.Y - OffH + 500);
            Cx2 += Part2[I - 1].Width;
            for (int J = 0; J <= Row - 1; J++)
                Engine.Canvas.Draw(Wz.EquipImageLib[Part2[I].ImageNode], Cx2 - W.Origin.X - Mid + 70, -Part2[I].Origin.Y + (J * C.Height) - OffH + 500);
            Cx3 += Part3[I - 1].Width;
            Engine.Canvas.Draw(Wz.EquipImageLib[Part3[I].ImageNode], Cx3 - SW.Origin.X - Mid + 70, -Part3[I].Origin.Y + (Row * C.Height) - OffH + 500);
        }
       

        if (Arrow.ImageNode != null)
        {
            if (!Loaded)
                Wz.EquipImageLib[Arrow.ImageNode] = MedalTag.FixAlpha(Arrow.ImageNode.ExtractPng());
            if (Style > 0)
                Wz.EquipImageLib[Arrow.ImageNode] = MedalTag.FixAlpha(Arrow.ImageNode.ExtractPng());
        }
        Loaded = true;

        if (WzNode.Get("arrow") != null)
            Engine.Canvas.Draw(Wz.EquipImageLib[Arrow.ImageNode], 70, Arrow.Origin.Y + (Row * C.Height) - OffH + 500);
        if (Style == 0)
            Engine.Canvas.DrawString(Map.NpcBalloonFont, ParseText(Msg, SplitWidth), -Mid + 82, -OffH + 500, new Color(155, 0, 0, 255));
        else
            Engine.Canvas.DrawString(Map.NpcBalloonFont, ParseText(Msg, SplitWidth), -Mid + 82, -OffH + 500, new Color(R, G, B, 255));

    }

}

public class ChatRingBalloon : ChatBalloon
{
    public ChatRingBalloon(Sprite Parent) : base(Parent)
    {

    }
    public void ReDraw()
    {

        Engine.Canvas.DrawTarget(ref RenderTarget, 150, 512, () => { RenderTargetFunc(); });
    }

    public override void DoDraw()
    {

        Engine.Canvas.Draw(RenderTarget, (int)X - 70 - (int)Engine.Camera.X - Game.Player.BrowPos.X + MapleChair.BodyRelMove.X,
            (int)Y - 500 - (int)Engine.Camera.Y - Game.Player.BrowPos.Y + MapleChair.BodyRelMove.Y);
    }

    public override void DoMove(float Delta)
    {
        X = Game.Player.X - 10;
        Y = Game.Player.Y - 60;
        Z = Game.Player.Z + 100;
    }

    public static void Remove()
    {
        MapleChair.IsUse = false;
        foreach (var Iter in EngineFunc.SpriteEngine.SpriteList)
        {
            if (Iter is ChatRingBalloon)
            {
                Iter.Dead();
                var s = Iter;
                s = null;
            }
        }
        EngineFunc.SpriteEngine.Dead();
    }

}