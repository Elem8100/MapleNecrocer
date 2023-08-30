using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;
using Microsoft.Xna.Framework;
using WzComparerR2.Animation;

namespace MapleNecrocer;

public class Reactor : SpriteEx
{
    public Reactor(Sprite Parent) : base(Parent)
    {
        IntMove = true;
    }

    string ID;
    Wz_Node Entry;
    Foothold BelowFH;
    Foothold FH;
    int Frame;
    int Delay;
    int FTime;
    Wz_Vector origin = new(0, 0);
    int OriginType;
    public static void Create(string ID)
    {
        var LEntry = Wz.GetNode("Reactor/" + ID + ".img");
        if (!LEntry.HasNode("0"))
            return;
        if (!Wz.EquipData.ContainsKey(LEntry.FullPathToFile2()))
            Wz.DumpData(LEntry, Wz.EquipData, Wz.EquipImageLib);
        var Reactor = new Reactor(EngineFunc.SpriteEngine);
        Reactor.ImageLib = Wz.EquipImageLib;
        Reactor.Entry = LEntry;
        Reactor.ImageNode = Wz.EquipData[Reactor.Entry.FullPathToFile2() + "/0/0"];
        Vector2 Pos = FootholdTree.Instance.FindBelow(new Vector2(Game.Player.X, Game.Player.Y - 50), ref Reactor.BelowFH);
        Reactor.X = Pos.X;
        Reactor.Y = Pos.Y;
        Reactor.Z = Reactor.BelowFH.Z * 100000 + 6000;
        Reactor.Width = Reactor.ImageWidth;
        Reactor.Height = Reactor.ImageHeight;
        if (Reactor.Entry.GetNode("0/1") == null)
            Reactor.OriginType = 0;
        else
            Reactor.OriginType = 1;

    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        ImageNode = Wz.EquipData[Entry.FullPathToFile2() + "/0/" + Frame];
        Delay = ImageNode.GetInt("delay", 100);
        FTime += +17;
        if (FTime > Delay)
        {
            Frame += 1;
            if (!Wz.HasDataE(Entry.FullPathToFile2() + "/0/" + Frame))
                Frame = 0;
            FTime = 0;
        }
        if (ImageNode.GetVector("origin") != null)
            origin = ImageNode.GetVector("origin");
        Offset.X = -origin.X;

        if (OriginType == 0)
            Offset.Y = -Height;
        else
            Offset.Y = -origin.Y;
    }

    public static void Remove()
    {
        foreach (var Iter in EngineFunc.SpriteEngine.SpriteList)
        {
            if (Iter is Reactor)
            {
                Iter.Dead();
                var s = Iter;
                s = null;
            }
        }
        EngineFunc.SpriteEngine.Dead();
    }
}
