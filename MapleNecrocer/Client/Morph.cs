using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;
using Keyboard = SpriteEngine.Keyboard;
using Input = Microsoft.Xna.Framework.Input.Keys;
using Spine;

namespace MapleNecrocer;

public class Morph : SpriteEx
{
    public Morph(Sprite Parent) : base(Parent)
    {

    }
    string UpPath;
    string Path;
    string State;
    int Frame;
    string MorphNum;
    int FTime;
    int Delay;
    int Flip;
    static Wz_Node Entry;
    Wz_Vector origin = new(0, 0);
    public static bool IsUse;
    public static void Create(string MorphNum)
    {
        Entry = Wz.GetNode("Morph/" + MorphNum);
        Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib);
        foreach (var Iter in Wz.EquipData[Entry.FullPathToFile2()].Nodes)
        {
            foreach (var Iter2 in Wz.EquipData[Iter.FullPathToFile2()].Nodes)
            {
                if (Char.IsNumber(Iter2.Text[0]))
                {
                    if (Iter.Text == "walk" && Iter2.Text == "0")
                    {
                        var Morph = new Morph(EngineFunc.SpriteEngine);
                        Morph.ImageLib = Wz.EquipImageLib;
                        Morph.IntMove = true;
                        Morph.Tag = 1;
                        Morph.State = Iter.Text;
                        Morph.Frame = Iter2.Text.ToInt();
                        Morph.MorphNum = Entry.Text.LeftStr(4);
                        Morph.UpPath = Entry.FullPathToFile2();
                        Morph.ImageNode = Wz.EquipData[Iter2.FullPathToFile2()];
                    }
                }
            }
        }
    }

    public static void Delete()
    {
        foreach (var Iter in EngineFunc.SpriteEngine.SpriteList)
        {
            if (Iter is Morph)
            {
                Iter.Dead();
            }
        }
        EngineFunc.SpriteEngine.Dead();
        foreach (var Iter in Wz.EquipImageLib.Keys)
        {
            if (Iter.FullPathToFile2().LeftStr(5) == "Morph")
            {
                Wz.EquipImageLib.Remove(Iter);
                Wz.EquipData.Remove(Iter.FullPathToFile2());
            }
        }
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        if (Wz.HasDataE(UpPath + "/" + State + "/" + Frame))
        {
            Path = UpPath + "/" + State + "/" + Frame;
            ImageNode = Wz.EquipData[Path];
            //Visible := True;
        }

        if (Wz.HasDataE(UpPath + "/" + State + "/" + Frame + "/delay"))
            Delay = Wz.EquipData[UpPath + "/" + State + "/" + Frame + "/delay"].ToInt();
        else
            Delay = 100;

        FTime += 17;
        if (FTime > Delay)
        {
            Frame += 1;
            if (!Wz.EquipData.ContainsKey(UpPath + "/" + State + "/" + Frame))
                Frame = 0;
            FTime = 0;
        }

        if (Wz.HasDataE(Path + "/z"))
            Z = Game.Player.Z - Wz.EquipData[Path + "/z"].ToInt();
        else
            Z = Game.Player.Z;

        if (Keyboard.KeyDown(Input.Left))
            FlipX = false;
        if (Keyboard.KeyDown(Input.Right))
            FlipX = true;

        if (Game.Player.JumpState != JumpState.jsNone)
        {
            Frame = 0;
            State = "jump";
        }

        if (Game.Player.JumpState == JumpState.jsNone)
        {
            State = "stand";
            if (Keyboard.KeyDown(Input.Left) || Keyboard.KeyDown(Input.Right))
                State = "walk";
            if (Keyboard.KeyDown(Input.Down))
                State = "prone";
            if (State == "prone" && Keyboard.KeyUp(Input.Down))
                State = "stand";
            if (Keyboard.KeyUp(Input.Left) || Keyboard.KeyUp(Input.Right))
                State = "stand";
        }

        if (Game.Player.InLadder)
        {
            switch (Game.Player.LadderType)
            {
                case LadderType.Ladder:
                    if (Keyboard.KeyDown(Input.Up) || Keyboard.KeyDown(Input.Down))
                    {
                        State = "ladder";
                    }
                    else
                    {
                        State = "ladder";
                        Frame = 0;
                    }
                    break;

                case LadderType.Rope:
                    if (Keyboard.KeyDown(Input.Up) || Keyboard.KeyDown(Input.Down))
                    {
                        State = "rope";
                    }
                    else
                    {
                        State = "rope";
                        Frame = 0;
                    }
                    break;
            }
        }

        if (ImageNode.GetNode("origin") != null)
            origin = ImageNode.GetNode("origin").ToVector();

        Y= Game.Player.Y;
        switch (FlipX)
        {
            case true:
                X = Game.Player.X + 1;
                Offset.X = origin.X - ImageWidth;
                Flip = -1;
                break;
            case false:
                X = Game.Player.X;
                Offset.X = -origin.X;
                Flip = 1;
                break;
        }
        Offset.Y = -origin.Y;
    }

}