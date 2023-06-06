using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WzComparerR2.Text;
using WzComparerR2.WzLib;
namespace MapleNecrocer;

public class MapleChair : SpriteEx
{
    public MapleChair(Sprite Parent) : base(Parent)
    {
    }
    string Path;
    int Frame;
    int FTime;
    int Delay;
    Wz_Vector origin=new(0,0);
    public static bool CanUse;
    public static bool IsUse;
    public static bool HasSitAction;
    public static bool UseTamingNavel;
    public static Wz_Vector BodyRelMove=new(0,0);
    public static string CharacterAction;

    public static void Create(string ID)
    {
        // BodyRelMove.X = 0;
        // BodyRelMove.Y = 0;
        BodyRelMove = new(0, 0);
        HasSitAction = false;
        UseTamingNavel = false;

        Wz_Node Entry = null;
        if (Wz.GetNode("Item/Install/0301.img") != null)
        {
            Entry = Wz.GetNode("Item/Install/0301.img");
        }
        else
        {
            if (ID.LeftStr(4) == "0302")
            {
                Entry = Wz.GetNode("Item/Install/0302.img");
            }
            else
            {
                switch (ID[4])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        Entry = Wz.GetNode("Item/Install/0301" + ID[4] + ".img");
                        break;
                    case '5':
                        if (Char.IsNumber(ID[5]))
                            Entry = Wz.GetNode("Item/Install/03015" + ID[5] + ".img");
                        break;
                }
            }
        }

        CharacterAction = "sit";
        if (Entry.HasNode(ID + "/info/tamingMob"))
        {
            string TamingMobID = null;
            if (Entry.HasNode(ID + "/info/tamingMob/0"))
                TamingMobID = Entry.GetInt(ID + "/info/tamingMob/0").ToString();
            else
                TamingMobID = Entry.GetInt(ID + "/info/tamingMob").ToString();


            if (Wz.GetNode("Character/TamingMob/" + "0" + TamingMobID + ".img/sit/0") != null)
            {

                if (Wz.GetNode("Character/TamingMob/" + "0" + TamingMobID + ".img/sit/0/0") != null)
                    if (Wz.GetNode("Character/TamingMob/" + "0" + TamingMobID + ".img/sit/0/0").ExtractPng().Width == 4)
                        UseTamingNavel = true;
                TamingMob.IsChairTaming = true;

                TamingMob.Create("0" + TamingMobID);
                if (Wz.GetNode("Character/TamingMob/" + "0" + TamingMobID + ".img/characterAction/sit") != null)
                {
                    HasSitAction = true;
                    CharacterAction = Wz.GetNode("Character/TamingMob/" + "0" + TamingMobID + ".img/characterAction/sit").ToStr();
                }
                else
                {
                    CharacterAction = "sit";
                }
            }
        }

        if ((Entry.GetNode(ID + "/effect") == null) && (Entry.GetNode(ID + "/effect2") == null))
            return;

        if (Entry.GetNode(ID + "/effect/0") != null)
            if (Entry.GetNode(ID + "/info/customChair/randomChairInfo/0") == null)
                if ((Entry.GetNode(ID + "/effect/0").ExtractPng().Width == 1) && (Entry.GetNode(ID + "/effect/0/_inlink") == null) &&
                  (Entry.GetNode(ID + "/effect/0/_outlink") == null))
                    if ((Entry.GetNode(ID + "/effect/1") == null) && (Entry.GetNode(ID + "/effect2") == null))
                        return;

        Wz.DumpData(Entry.GetNode(ID), Wz.EquipData, Wz.EquipImageLib);

        if (Entry.HasNode(ID + "/info/bodyRelMove"))
        {
            BodyRelMove = Entry.GetNode(ID + "/info/bodyRelMove").ToVector();
        }

        if (Entry.HasNode(ID + "/info/sitAction"))
        {
            HasSitAction = true;
            CharacterAction = Entry.GetNode(ID + "/info/sitAction").ToStr();
        }
        else
        {
            CharacterAction = "sit";
        }
        bool HasSitEmotion;
        if (Entry.HasNode(ID + "/info/SitEmotion"))
            HasSitEmotion = true;

        Wz_Node Entry2;
        if (Entry.HasNode(ID + "/info/customChair/randomChairInfo/0"))
            Entry2 = Entry.GetNode(ID + "/info/customChair/randomChairInfo/0");
        else
            Entry2 = Entry.GetNode(ID);


        foreach (var Iter in Entry2.Nodes)
        {
            if (Iter.Text.Length < 6) continue;
            if (Iter.Text.LeftStr(6) == "effect" || Iter.Text.RightStr(6) == "effect")
            {
                if (Iter.GetNode("0") == null)
                    continue;

                var MapleChair = new MapleChair(EngineFunc.SpriteEngine);
                MapleChair.ImageLib = Wz.EquipImageLib;
                MapleChair.Path = Iter.FullPathToFile2();
                MapleChair.ImageNode = Wz.EquipData[MapleChair.Path + "/0"];
                Foothold BelowFH = null;
                Vector2 Below = FootholdTree.Instance.FindBelow(new Vector2(Game.Player.X, Game.Player.Y - 50), ref BelowFH);
                MapleChair.FlipX = Game.Player.FlipX;
                MapleChair.IntMove = true;

                int BodyRelMoveX;
                if (MapleChair.FlipX)
                    BodyRelMoveX = -(int)BodyRelMove.X;
                else
                    BodyRelMoveX = (int)BodyRelMove.X;

                int Pos = Iter.GetInt("pos", -1);
                switch (Pos)
                {
                    case -1: //no pos data
                        if (HasSitAction)
                        {
                            MapleChair.X = Game.Player.X;
                            MapleChair.Y = Below.Y;
                        }
                        else
                        {
                            MapleChair.X = Game.Player.X + BodyRelMoveX;
                            MapleChair.Y = Below.Y + BodyRelMove.Y;
                        }
                        break;
                    case 0:
                        UseTamingNavel = true;
                        if (HasSitAction)
                        {
                            MapleChair.X = Game.Player.X;
                            MapleChair.Y = Below.Y;
                        }
                        else
                        {
                            MapleChair.X = Game.Player.X + BodyRelMoveX;
                            MapleChair.Y = Below.Y + BodyRelMove.Y;
                        }
                        break;
                    case 1:
                        if (HasSitAction)
                        {

                            UseTamingNavel = false;
                            MapleChair.X = Game.Player.X;
                            MapleChair.Y = Below.Y;
                        }
                        else
                        {
                            MapleChair.X = Game.Player.X + BodyRelMoveX;
                            MapleChair.Y = Below.Y - 50 + BodyRelMove.Y;
                        }
                        break;
                    case 2:
                    case 3:
                        UseTamingNavel = false;
                        MapleChair.X = Game.Player.X;
                        MapleChair.Y = Below.Y;
                        break;
                }

                if (Entry.HasNode(ID + "/info/customChair/randomChairInfo/0"))
                {
                    MapleChair.X = Game.Player.X;
                    MapleChair.Y = Below.Y;
                }

                if (Iter.HasNode("z"))
                {
                    if (Iter.GetNode("z").Value is int)
                        MapleChair.Z = Game.Player.Z + Iter.Get("z").ToInt();
                    else
                    {
                        if (AvatarParts.ZMap.Contains(Iter.Get("z").ToStr()))
                            MapleChair.Z = Game.Player.Z + AvatarParts.ZMap.IndexOf(Iter.Get("z").ToStr());
                        else
                            MapleChair.Z = Game.Player.Z - 1;
                    }
                }
                else
                    MapleChair.Z = Game.Player.Z - 1;

            }
        }
    }

    public static void Delete()
    {
        MapleChair.IsUse = false;
        foreach (var Iter in EngineFunc.SpriteEngine.SpriteList)
        {
            if (Iter is MapleChair)
            {
                Iter.Dead();
                var s = Iter;
                s = null;
            }
        }
        EngineFunc.SpriteEngine.Dead();
        foreach (var Iter in Wz.EquipImageLib.Keys)
        {
            if (Iter.FullPathToFile2().LeftStr(17) == "Item/Install/0301")
            {
                Wz.EquipImageLib.Remove(Iter);
                Wz.EquipData.Remove(Iter.FullPathToFile2());
            }
        }

        foreach (var Iter in Wz.EquipImageLib.Keys)
        {
            if (Iter.FullPathToFile2().LeftStr(24) == "Character/TamingMob/0198")
            {
                Wz.EquipImageLib.Remove(Iter);
                Wz.EquipData.Remove(Iter.FullPathToFile2());
            }
        }
        //  BodyRelMove.X = 0;
        //  BodyRelMove.Y = 0;
    }

    public override void DoMove(float Delta)
    {  
        base.DoMove(Delta);
        ImageNode = Wz.EquipData[Path + "/" + Frame];
        Delay = ImageNode.GetInt("delay", 100);
        FTime += 17;
        if (FTime > Delay)
        {
            Frame += 1;
            if (!Wz.EquipData.ContainsKey(Path + "/" + Frame))
                Frame = 0;
            FTime = 0;
        }
        if (ImageNode.HasNode("origin"))
            origin = ImageNode.GetVector("origin");
    
        
        if (UseTamingNavel)
        {
            switch (FlipX)
            {
                case true:
                    Offset.X = origin.X - ImageWidth - TamingMob.Navel.X;
                    break;
                case false:
                    Offset.X = -origin.X - TamingMob.Navel.X;
                    break;
            }
            Offset.Y = -origin.Y - TamingMob.Navel.Y;
        }
        else
        {
            switch (FlipX)
            {
                case true:
                    Offset.X = origin.X - ImageWidth;
                    break;
                case false:
                    Offset.X = -origin.X;
                    break;
            }
            Offset.Y = -origin.Y;
        }

    }
}

