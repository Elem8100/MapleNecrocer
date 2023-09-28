using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using WzComparerR2.WzLib;
using System.Drawing;
using Spine;

namespace MapleNecrocer;

public class Pet : JumperSprite
{
    public Pet(Sprite Parent) : base(Parent)
    {
        SpriteSheetMode = SpriteSheetMode.NoneSingle;
    }

    public int FTime;
    Foothold WallFH;
    Foothold BelowFH;
    MoveDirection MoveDirection;
    float MoveSpeed;
    string PetName;
    int NameWidth;
    int IDWidth;
    MoveType MoveType;
    int FallEdge;
    int JumpEdge;
    public Wz_Vector origin = new(0, 0);
    public string Path;
    public string UpPath;
    public string State;
    public int Frame;
    public int Delay;
    Vector2 Distance;
    bool OnLadder;
    Foothold FH;
    public static Pet Instance;

    public static void Create(string ID)
    {
        Wz_Node Entry = Wz.GetNode("Item/Pet/" + ID + ".img");
        Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib);
        foreach (var Iter in Wz.EquipData[Entry.FullPathToFile2()].Nodes)
        {
            foreach (var Iter2 in Wz.EquipData[Iter.FullPathToFile2()].Nodes)
            {
                if (Char.IsNumber(Iter2.Text[0]))
                {
                    if (Iter.Text == "stand0" && Iter2.Text == "0")
                    {
                        Instance = new Pet(EngineFunc.SpriteEngine);
                        Instance.ImageLib = Wz.EquipImageLib;
                        Instance.IntMove = true;
                        Instance.Tag = 1;
                        Instance.State = Iter.Text;
                        Instance.Frame = Iter2.Text.ToInt();
                        Instance.UpPath = Entry.FullPathToFile2();
                        Instance.ImageNode = Wz.EquipData[Iter2.FullPathToFile2()];
                        var StartX = Game.Player.X - 60;
                        if (StartX < Map.Left)
                            StartX = Game.Player.X;
                        var Pos = FootholdTree.Instance.FindBelow(new Vector2(StartX, Game.Player.Y - 3), ref Instance.BelowFH);
                        Instance.MoveType = MoveType.Jump;
                        Instance.X = Pos.X;
                        Instance.Y = Pos.Y;
                        Instance.FH = Instance.BelowFH;
                        Instance.Z = Game.Player.Z;
                        Instance.JumpSpeed = 0.6f;
                        Instance.JumpHeight = 9;
                        Instance.MaxFallSpeed = 8;
                        Instance.MoveDirection = MoveDirection.None;
                        Instance.MoveSpeed = 2.5f;
                    }
                }
            }
        }
    }


    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        int X1 = FH.X1;
        int Y1 = FH.Y1;
        int X2 = FH.X2;
        int Y2 = FH.Y2;

        if (Wz.HasDataE(UpPath + "/" + State + "/" + Frame))
        {
            Path = UpPath + "/" + State + "/" + Frame;
            ImageNode = Wz.EquipData[Path];
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

        Distance.X = Math.Abs(Game.Player.X - X);
        Distance.Y = Math.Abs(Game.Player.Y - Y);

        if (Distance.X > 70)
        {
            State = "move";
            if (Game.Player.X > X)
            {
                FlipX = true;
                MoveDirection = MoveDirection.Right;
            }
            if (Game.Player.X < X)
            {
                FlipX = false;
                MoveDirection = MoveDirection.Left;
            }
        }
        else
        {
            State = "stand0";
            MoveDirection = MoveDirection.None;
        }

        if (Game.Player.Y < Y)
        {
            switch (Distance.Y)
            {
                case float i when i >= 100 && i <= 150:
                    if (JumpState == JumpState.jsNone)
                    {
                        //  Below := TFootholdTree.This.FindBelow(Point(Round(X), Round(Y - 70)), BelowFH);
                        //  if Y - Below.Y <> 0 then
                        DoJump = true;
                    }
                    break;

                case float i when i >= 151 && i <= 2000:
                    if (Game.Player.JumpState == JumpState.jsNone)
                    {
                        X = Game.Player.X;
                        Y = Game.Player.Y;
                    }
                    break;
            }
        }

        if (Game.Player.Y > Y)
        {
            if (Distance.Y >= 200 && Distance.Y <= 2000)
            {

                Y += 5;
                JumpState = JumpState.jsFalling;
            }
        }

        Vector2 Below;
        if (Game.Player.InLadder)
        {
            LadderRope ladderRope = LadderRope.Find(new Vector2(Game.Player.X, Game.Player.Y), ref OnLadder);
            State = "hang";
            X = Game.Player.X;
            Y = Game.Player.Y + 20;
            Z = 7 * 100000 + 60000;
            if (Y > ladderRope.Y2 - 10)
                JumpState = JumpState.jsFalling;
            if (Y < ladderRope.Y1 + 30)
            {
                Below = FootholdTree.Instance.FindBelow(new Vector2(Game.Player.X, Game.Player.Y - 100), ref BelowFH);
                Y = Below.Y;
                FH = BelowFH;
            }
        }

        if (JumpState == JumpState.jsFalling)
        {
            Below = FootholdTree.Instance.FindBelow(new Vector2(X, Y - VelocityY - 2), ref BelowFH);
            if (Y >= Below.Y - 3)
            {
                Y = Below.Y;
                // MaxFallSpeed :=10;
                JumpState = JumpState.jsNone;
                FH = BelowFH;
                Z = FH.Z * 100000 + 70000;
            };
        }

        int FallEdge;
        int Direction;
        if (MoveDirection == MoveDirection.Left)
        {
            Direction = GetAngle256(X2, Y2, X1, Y1);
            if (!FH.IsWall())
            {
                X += (float)(Sin256(Direction) * MoveSpeed);
                Y -= (float)(Cos256(Direction) * MoveSpeed);
            }
            FallEdge = -999999;
            JumpEdge = -999999;
            if (MoveType == MoveType.Move)
            {
                // no fh
                if (FH.Prev == null)
                    FallEdge = FH.X1;
                // Wall's edge down
                if ((FH.Prev != null) && (FH.Prev.IsWall()))
                    FallEdge = FH.X1;

                if (X < FallEdge)
                {
                    X = FallEdge;
                    FlipX = true;
                    MoveDirection = MoveDirection.Right;
                }
            }

            if (MoveType == MoveType.Jump)
            {
                if (X < Map.Left + 20)
                {
                    X = Map.Left + 20;
                    FlipX = true;
                    MoveDirection = MoveDirection.Right;
                }
                // .--------.
                if (FH.Prev == null)
                    JumpEdge = FH.X1;
                // ┌--- <--
                if ((FH.Prev != null) && (FH.Prev.IsWall()) && (FH.Prev.Y1 > Y))
                    FallEdge = FH.X1;

                if (X < FallEdge)
                {
                    if (Game.Player.Y <= Y)
                        DoJump = true;
                    if (Game.Player.Y > Y && JumpState == JumpState.jsNone)
                        JumpState = JumpState.jsFalling;
                }
                if (X < JumpEdge)
                    DoJump = true;
                // -->  ---┐  <--
                WallFH = FootholdTree.Instance.FindWallR(new Vector2(X + 4, Y - 4));
                if ((WallFH != null) && (FH.Z == WallFH.Z))
                {
                    if (X < WallFH.X1 + 30 && Game.Player.Y <= Y)
                        DoJump = true;
                    if (X <= WallFH.X1)
                    {
                        X = WallFH.X1 + MoveSpeed;
                        if (JumpState == JumpState.jsNone)
                        {
                            FlipX = true;
                            MoveDirection = MoveDirection.Right;
                        }
                    }
                }
            }
            // walk left
            if ((X <= FH.X1) && (FH.PrevID != 0) && (!FH.IsWall()) && (!FH.Prev.IsWall()))
            {
                if (JumpState == JumpState.jsNone)
                {
                    FH = FH.Prev;
                    X = FH.X2;
                    Y = FH.Y2;
                    Z = FH.Z * 100000 + 70000;
                }
            }
        }

        // walk right
        if (MoveDirection == MoveDirection.Right)
        {

            Direction = GetAngle256(X1, Y1, X2, Y2);
            if (!FH.IsWall())
            {
                X += (float)(Sin256(Direction) * MoveSpeed);
                Y -= (float)(Cos256(Direction) * MoveSpeed);
            }

            FallEdge = 999999;
            JumpEdge = 999999;
            if (MoveType == MoveType.Move)
            {
                if (FH.Next == null)
                    FallEdge = FH.X2 + 5;
                // Wall down
                if ((FH.Next != null) && (FH.Next.IsWall()))

                    FallEdge = FH.X2;
                if (X > FallEdge)
                {
                    X = FallEdge;
                    FlipX = false;
                    MoveDirection = MoveDirection.Left;
                }
            }

            if (MoveType == MoveType.Jump)
            {
                if (X > Map.Right - 20)
                {
                    X = Map.Right - 20;
                    FlipX = false;
                    MoveDirection = MoveDirection.Left;
                }
                if (FH.Next == null) // .--------.
                    JumpEdge = FH.X2;
                // -->  ----┐
                if ((FH.Next != null) && (FH.Next.IsWall()) && (FH.Next.Y2 > Y))
                    FallEdge = FH.X2;

                if (X > FallEdge)
                {
                    if (Game.Player.Y <= Y)
                        DoJump = true;
                    if (Game.Player.Y > Y && JumpState == JumpState.jsNone)
                        JumpState = JumpState.jsFalling;
                }
                if (X > JumpEdge)
                    DoJump = true;
                // -->  ┌.....
                WallFH = FootholdTree.Instance.FindWallL(new Vector2(X - 4, Y - 4));
                if ((WallFH != null) && (FH.Z == WallFH.Z))
                {
                    if (X > WallFH.X1 - 30 && Game.Player.Y <= Y)
                        DoJump = true;
                    if (X >= WallFH.X1)
                    {
                        X = WallFH.X2 - MoveSpeed;
                        if (JumpState == JumpState.jsNone)
                        {
                            FlipX = false;
                            MoveDirection = MoveDirection.Left;
                        }
                    }
                }
            }

            // walk right
            if ((X >= FH.X2) && (FH.NextID != 0) && (!FH.IsWall()) && (!FH.Next.IsWall()))
            {
                if (JumpState == JumpState.jsNone)
                {
                    FH = FH.Next;
                    X = FH.X1;
                    Y = FH.Y1;
                    Z = FH.Z * 100000 + 70000;
                }
            }
        }

        //if (MoveDirection ==  MoveDirection.None)
        // X = (X);

        if (ImageNode.GetNode("origin") != null)
            origin = ImageNode.GetNode("origin").ToVector();
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

    public static void Remove()
    {
        if (Instance != null)
        {
            Instance.Dead();
            Instance = null;
            foreach (var Iter in Wz.EquipImageLib.Keys)
            {
                if (Iter.FullPathToFile2().LeftStr(8) == "Item/Pet")
                {
                    Wz.EquipImageLib.Remove(Iter);
                    // EquipData.Remove(Iter.GetPath);
                }
            }
            EngineFunc.SpriteEngine.Dead();
        }
    }
}

public class PetNameTag : MedalTag
{
    public PetNameTag(Sprite Parent) : base(Parent)
    {
    }
    public static PetNameTag Instance;

    static void ReDraw()
    {
        if (Instance != null)
            Instance.IsReDraw = true;
    }

    public static void Remove()
    {
        if (Instance != null)
        {
            Instance.Dead();
            EngineFunc.SpriteEngine.Dead();
        }
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        if (IsReDraw)
        {
            Engine.Canvas.DrawTarget(ref TargetTexture, 300, 100, () => { RenderTargetFunc(); });
        }
        X = Pet.Instance.X;
        Y = Pet.Instance.Y;
        Z = Pet.Instance.Z;
    }

    public override void DoDraw()
    {
        if (Map.ShowPlayer)
        {
            int WX = (int)(Pet.Instance.X) - (int)(Engine.Camera.X);
            int WY = (int)(Pet.Instance.Y) - (int)(Engine.Camera.Y);
            Engine.Canvas.Draw(TargetTexture, WX - 150, WY - 28, MonoGame.SpriteEngine.BlendMode.NonPremultiplied);
        }
        if (IsReDraw)
            IsReDraw = false;
    }

    public static void Create(string ItemID)
    {
        Instance = new PetNameTag(EngineFunc.SpriteEngine); 
        Instance.IntMove = true;
        Instance.Tag = 1; 
        int TagNum = Wz.GetInt("Item/Pet/" + ItemID + ".img/info/nameTag", 3);
        Instance.Entry = Wz.GetNode("UI/NameTag.img/pet/" + TagNum);
        if (Instance.Entry == null)
            Instance.Entry = Wz.GetNode("UI/NameTag.img/pet/38");
        if (Instance.Entry.HasNode("c/_inlink"))
        {
            string Data = Instance.Entry.GetStr("c/_inlink");
            Data = Data.Replace("/c", "");
            Data = Data.Replace("pet/", "");
            Instance.Entry = Wz.GetNode("UI/NameTag.img/pet/" + Data);
        }
        Wz.DumpData(Instance.Entry, Wz.EquipData, Wz.EquipImageLib);
     
    }


}

public class PetEquip : Pet
{
    public PetEquip(Sprite Parent) : base(Parent)
    {

    }
    public static PetEquip Instance;

    public static void Create(string ID)
    {
        Wz_Node Entry = Wz.GetNode("Character/PetEquip/" + ID + ".img");
        Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib);
        Instance = new PetEquip(EngineFunc.SpriteEngine);
        Instance.ImageLib = Wz.EquipImageLib;
        Instance.IntMove = true;
        Instance.Tag = 1;
        Instance.State = Pet.Instance.State;
        Instance.Frame = Pet.Instance.Frame;
        Instance.UpPath = Entry.FullPathToFile2();
        Instance.ImageNode = Wz.EquipData[Instance.UpPath + "/" + PetForm.PetID + "/" + Instance.State + "/" + Instance.Frame];
        Instance.X = Pet.Instance.X;
        Instance.Y = Pet.Instance.Y;
        Instance.Z = Pet.Instance.Z + 100;
    }


    public override void DoMove(float Delta)
    {
        if (Wz.HasDataE(UpPath + "/" + PetForm.PetID + "/" + State + "/" + Frame))
        {
            Path = UpPath + "/" + PetForm.PetID + "/" + State + "/" + Frame;
            ImageNode = Wz.EquipData[Path];
        }

        if (Wz.HasDataE(UpPath + "/" + PetForm.PetID + "/" + State + "/" + Frame + "/delay"))
            Delay = Wz.EquipData[UpPath + "/" + PetForm.PetID + "/" + State + "/" + Frame + "/delay"].ToInt();
        else
            Delay = 100;


        FTime += 17;
        if (FTime > Delay)
        {
            Frame += 1;
            if (!Wz.EquipData.ContainsKey(UpPath + "/" + PetForm.PetID + "/" + State + "/" + Frame))
                Frame = 0;
            FTime = 0;
        }

        State = Pet.Instance.State;
        Frame = Pet.Instance.Frame;
        X = Pet.Instance.X;
        Y = Pet.Instance.Y;
        Z = Pet.Instance.Z + 100;
        FlipX = Pet.Instance.FlipX;

        if (ImageNode.GetNode("origin") != null)
            origin = ImageNode.GetNode("origin").ToVector();
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

    public static void Remove()
    {
        if (Instance != null)
        {
            Instance.Dead();
            Instance = null;
            foreach (var Iter in Wz.EquipImageLib.Keys)
            {
                if (Iter.FullPathToFile2().LeftStr(18) == "Character/PetEquip")
                {
                    Wz.EquipImageLib.Remove(Iter);
                    // EquipData.Remove(Iter.GetPath);
                }
            }
            EngineFunc.SpriteEngine.Dead();
        }
    }
}