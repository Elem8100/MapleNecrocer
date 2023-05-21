using MonoGame.SpriteEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.WzLib;

namespace MapleNecrocer;

public class MapleTV : SpriteEx
{
    public MapleTV(Sprite Parent) : base(Parent)
    {
    }
    float Time;
    int AD, ADCount, Frame;
    string ImagePath;
    public static void Create(int PosX, int PosY, int msgX, int msgY, int adX, int adY, int ZLayer)
    {
        if (Wz.GetNodeA("UI/MapleTV.img") == null)
            return;
        string Path = "UI/MapleTV.img/TVbasic/0";
        Wz.DumpData(Wz.GetNodeA(Path), Wz.Data, Wz.ImageLib);
        var Sprite = new SpriteEx(EngineFunc.SpriteEngine);
        Sprite.ImageLib = Wz.ImageLib;
        // Sprite.Path = Path;
        Sprite.ImageNode = Wz.Data[Path];
        if (msgX <= adX)
            Sprite.X = PosX + msgX;
        else
            Sprite.X = PosX + msgX + adX;
        Sprite.Y = PosY + msgY + Sprite.ImageHeight;
        Sprite.Z = ZLayer+1;
        Sprite.Width = Sprite.ImageWidth;
        Sprite.Height = Sprite.ImageHeight;
        Wz_Vector origin = Wz.GetVector(Path + "/origin");
        Sprite.Origin.X = origin.X;
        Sprite.Origin.Y = origin.Y;

        int ACount = 0;
        Path = "UI/MapleTV.img/TVmedia";
        foreach (var Iter in Wz.GetNodeA(Path).Nodes)
            ACount += 1;
        Wz.DumpData(Wz.GetNodeA(Path), Wz.Data, Wz.ImageLib);
        var MapleTV = new MapleTV(EngineFunc.SpriteEngine);
        MapleTV.ImageLib = Wz.ImageLib;
        MapleTV.ImageNode = Wz.Data[Path + "/0/0"];
        MapleTV.X = PosX + adX;
        MapleTV.Y = PosY + adY + MapleTV.ImageHeight;
        MapleTV.Z = ZLayer + 50;
        MapleTV.ADCount = ACount;
        MapleTV.Width = Sprite.ImageWidth;
        MapleTV.Height = Sprite.ImageHeight;
        origin = Wz.GetVector(Path + "/0/0/origin");
        MapleTV.Origin.X = origin.X;
        MapleTV.Origin.Y = origin.Y;
    }

    public override void DoMove(float Delta)
    {
        string S1 = "UI/MapleTV.img/TVmedia/";
        string ImagePath = S1 + AD + "/" + Frame;
        ImageNode = Wz.Data[ImagePath];
        int Delay = Wz.GetInt(ImagePath + "/delay", 100);
        Time += 16.66f * Delta;
        Random Random = new();
        if (Time > Delay)
        {
            Frame += 1;
            if (!Wz.HasNode(S1 + AD + "/" + Frame))
            {
                Frame = 0;
                AD = Random.Next(ADCount);
            }
            Time = 0;
        }
        Wz_Vector origin = Wz.GetVector(ImagePath + "/origin");
        Origin.X = origin.X;
        Origin.Y = origin.Y;
    }
}

