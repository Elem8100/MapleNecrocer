using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spine;
using WzComparerR2.WzLib;
using Keyboard = SpriteEngine.Keyboard;
using Input = Microsoft.Xna.Framework.Input.Keys;
using System.Security.Cryptography;


namespace MapleNecrocer;

public class TamingMob : SpriteEx
{
    public TamingMob(Sprite Parent) : base(Parent)
    {
    }

    string UpPath;
    string Path, ID;
    string State;
    int Frame;
    string ImageNum;
    int FTime;
    int Delay;
    int Flip;
    Wz_Vector origin = new(0, 0);
    bool FixedImageNum;
    bool IsSaddle;
    string PartIndex;
    public static Wz_Node Entry;
    public static bool IsUse;
    public static string CharacterAction;
    public static Vector2 Navel = new(0, 0);
    public static bool IsChairTaming;
    static Dictionary<string, string> SaddleList = new();
    static Dictionary<string, string> ImageNumList = new();
    static Dictionary<string, Wz_Node> Data = new();
    public static void LoadSaddleList()
    {
        foreach (var Img in Wz.GetNode("Character/TamingMob").Nodes)
        {
            if (Img.Text.LeftStr(4) == "0191")
                foreach (var Iter in Wz.GetNode("Character/TamingMob/" + Img.Text).Nodes)
                    if (Char.IsNumber(Iter.Text[0]))
                        SaddleList.AddOrReplace("0" + Iter.Text, Img.Text.LeftStr(8));
        }

    }

    public static void Remove()
    {
        TamingMob.IsUse = false;
        foreach (var Iter in EngineFunc.SpriteEngine.SpriteList)
        {
            if (Iter is TamingMob)
                Iter.Dead();
        }
        EngineFunc.SpriteEngine.Dead();
        foreach (var Iter in Wz.EquipImageLib.Keys)
        {
            if (Iter.FullPathToFile2().LeftStr(23) == "Character/TamingMob/019")
            {
                Wz.EquipImageLib.Remove(Iter);
                Wz.EquipData.Remove(Iter.FullPathToFile2());
            }
        }
        TamingMob.Navel.X = 0;
        TamingMob.Navel.Y = 0;
    }

    public static void CreateSprites()
    {
        Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib);
        string _State;
        string _Frame;

        if (IsChairTaming)
        {
            _State = "sit";
            _Frame = "0";
        }
        else
        {
            _State = "walk1";
            _Frame = "1";
        }


        foreach (var Iter in Wz.EquipData[Entry.FullPathToFile2()].Nodes)
        {
            foreach (var Iter2 in Wz.EquipData[Iter.FullPathToFile2()].Nodes)
            {
                if (Char.IsNumber(Iter2.Text[0]))
                {
                    int Index = -1;
                    foreach (var Iter3 in Wz.EquipData[Iter2.FullPathToFile2()].Nodes)
                    {
                        if (/*Char.IsNumber(Iter3.Text[0]) && */((Iter3.Value is Wz_Png) || (Iter3.Value is Wz_Uol)))
                        {
                            Index += 1;
                            ImageNumList.AddOrReplace(Entry.FullPathToFile2() + "/" + Iter.Text + "/" + Iter2.Text + "/" + Index.ToString(), Iter3.Text);
                            if ((Iter.Text == _State) && (Iter2.Text == _Frame))
                            {
                                var TamingMob = new TamingMob(EngineFunc.SpriteEngine);
                                TamingMob.ImageLib = Wz.EquipImageLib;
                                TamingMob.IntMove = true;
                                TamingMob.Tag = 1;
                                TamingMob.PartIndex = Index.ToString();
                                TamingMob.State = Iter.Text;
                                TamingMob.Frame = Iter2.Text.ToInt();
                                TamingMob.ImageNum = Iter3.Text;
                                TamingMob.ID = Entry.Text.LeftStr(8);
                                TamingMob.UpPath = Entry.FullPathToFile2();
                                TamingMob.ImageNode = Wz.EquipData[Iter3.FullPathToFile2()];

                                if (Entry.ParentNode != null)
                                {
                                    if (Entry.ParentNode.Text.LeftStr(4) == "0191")
                                        TamingMob.IsSaddle = true;
                                }
                                if ((Iter.Text == "walk1") && (Iter2.Text == "1") && (Iter2.GetNode("0") == null))
                                    TamingMob.FixedImageNum = true;
                                if (Iter3.Text.Length >= 3)
                                    TamingMob.FixedImageNum = true;
                            }
                        }
                    }
                }
            }
        }

    }

    public static void CreateSaddle(string ID)
    {
        Data.Clear();
        if (SaddleList.ContainsKey(ID))
            Entry = Wz.GetNode("Character/TamingMob/" + SaddleList[ID] + ".img/" + ID.IntID());
        else
            return;

        //add saddle delay
        foreach (var Iter in Wz.GetNode("Character/TamingMob/" + ID + ".img").Nodes)
        {
            if (Iter.Text != "info")
            {
                foreach (var Iter2 in Iter.Nodes)
                    Data.AddOrReplace(Entry.FullPathToFile2() + "/" + Iter.Text + "/" + Iter2.Text, Iter2.GetNode("delay"));
            }
        }
        CreateSprites();
    }

    public static void CreateTaming(string ID)
    {
        Entry = Wz.GetNode("Character/TamingMob/" + ID + ".img");
        CreateSprites();
    }

    public static void Create(string ID)
    {
        TamingMob.CharacterAction = "sit";
        ImageNumList.Clear();
        CreateSaddle(ID);
        CreateTaming(ID);
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        if (FixedImageNum)
        {
            if (ImageNumList.ContainsKey(UpPath + "/" + State + "/" + Frame + "/" + PartIndex))
                ImageNum = ImageNumList[UpPath + "/" + State + "/" + Frame + "/" + PartIndex];
        }

        if (Wz.HasDataE(UpPath + "/" + State + "/" + Frame + "/" + ImageNum))
        {
            Path = UpPath + "/" + State + "/" + Frame + "/" + ImageNum;
            ImageNode = Wz.EquipData[Path];
            Visible = true;
        }
        else
        {
            if ((State == "rope") || (State == "ladder"))
                Visible = false;
        }

        if (Wz.HasDataE(UpPath + "/" + State + "/" + Frame + "/delay"))
            Delay = Wz.EquipData[UpPath + "/" + State + "/" + Frame + "/delay"].ToInt();
        else
            Delay = 100;

        if (IsSaddle)
        {
            if (Data.ContainsKey(UpPath + "/" + State + "/" + Frame))
                Delay = Data[UpPath + "/" + State + "/" + Frame].ToInt();
        }

        FTime += 17;
        if (FTime > Delay)
        {
            Frame += 1;
            if (!Wz.HasDataE(UpPath + "/" + State + "/" + Frame))
                Frame = 0;
            FTime = 0;
        }

        if (AvatarParts.ZMap.Contains(Wz.EquipData[Path + "/z"].ToStr()))
        {
            Z = 100 + Game.Player.Z - AvatarParts.ZMap.IndexOf(Wz.EquipData[Path + "/z"].ToStr());
        }
        else
        {
            string ZName = Wz.EquipData[Path + "/z"].ToStr();
            if (ZName == "tamingMobBack")
            {
                Z = Game.Player.Z - 100;
            }
            else
            {
                if (ImageNum == "0")
                    Z = Game.Player.Z - 1;
                else
                    Z = Game.Player.Z;
                string[] List = { "01932524", "01932422", "01932454" };
                foreach (var i in List)
                {
                    if (ID == i)
                        Z = Game.Player.Z;
                }
            }
        }

        if (Game.Player.JumpState != JumpState.jsNone)
        {
            Frame = 0;
            State = "jump";
        }

        if (Game.Player.JumpState == JumpState.jsNone)
        {
            State = "stand1";
            if (Keyboard.KeyDown(Input.Left) || Keyboard.KeyDown(Input.Right))
                State = "walk1";
            if (Keyboard.KeyDown(Input.Down))
                State = "prone";
            if (State == "prone" && Keyboard.KeyUp(Input.Down))
                State = "stand1";
            if (Keyboard.KeyUp(Input.Left) || Keyboard.KeyUp(Input.Right))
                State = "stand1";
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

        if (Wz.HasDataE(UpPath + "/characterAction"))
        {
            if (Wz.HasDataE(UpPath + "/characterAction/" + State))
                CharacterAction = Wz.EquipData[UpPath + "/characterAction/" + State].ToStr();
            else if (Wz.HasDataE(UpPath + "/characterAction/walk1"))
                CharacterAction = Wz.EquipData[UpPath + "/characterAction/walk1"].ToStr();
        }
        else
        {
            CharacterAction = "sit";
        }
        if (IsChairTaming)
            State = "sit";

        FlipX = Game.Player.FlipX;
        if (ImageNode.GetNode("origin") != null)
            origin = ImageNode.GetNode("origin").ToVector();

        Y = Game.Player.Y;
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

        if (Wz.HasDataE(Path + "/map/navel"))
        {
            Vector2 _Offset;
            if (CharacterAction == "prone")
            {
                if (FlipX)
                    _Offset.X = -3;
                else
                    _Offset.X = 3;
                _Offset.Y = 3;
            }
            else if (CharacterAction == "fly")
            {
                if (FlipX)
                    _Offset.X = -3;
                else
                    _Offset.X = 3;
                _Offset.Y = 27;
            }
            else
            {
                _Offset.X = 0;
                _Offset.Y = 17;
            }

            if (FlipX)
                Navel.X = -Wz.EquipData[Path + "/map/navel"].ToVector().X * Flip - _Offset.X;
            else
                Navel.X = -Wz.EquipData[Path + "/map/navel"].ToVector().X * Flip - 4 - _Offset.X;
            Navel.Y = -Wz.EquipData[Path + "/map/navel"].ToVector().Y - _Offset.Y;
        }

        string[] FixedIDs = { "01932377", "01902002", "1902002", "1902007", "01902007", "01932123", "01932181", "01932116", "01932081", "01992000", "01932418", "01932461", "01932507", "01932504", "01932505" };
        foreach (var I in FixedIDs)
        {
            if (ID == I)
            {
                switch (FlipX)
                {
                    case true:
                        Offset.X = origin.X - ImageWidth + Navel.X;
                        break;
                    case false:
                        Offset.X = -origin.X + Navel.X;
                        break;
                }
                Offset.Y = -origin.Y + Navel.Y - 50;
                Navel.X = 0;
                Navel.Y = 50;
            }
        }

    }

}
