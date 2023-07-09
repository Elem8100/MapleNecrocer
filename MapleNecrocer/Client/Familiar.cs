using Microsoft.Xna.Framework.Graphics;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MapleNecrocer;

public class Familiar : JumperSprite
{
    public Familiar(Sprite Parent) : base(Parent)
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
    int FollowDistance;
    Vector2 Distance;
    bool OnLadder;
    Foothold FH;
    public static Familiar Instance;

    public static void Create(string ID)
    {
        Wz_Node Entry = null;
        if (Wz.HasNode("Mob/" + ID + ".img"))
            Entry = Wz.GetNode("Mob/" + ID + ".img");
        Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib);
        foreach (var Iter in Wz.EquipData[Entry.FullPathToFile2()].Nodes)
        {
            foreach (var Iter2 in Wz.EquipData[Iter.FullPathToFile2()].Nodes)
            {
                if (Char.IsNumber(Iter2.Text[0]))
                {
                    if ((Iter.Text == "stand" || Iter.Text == "fly") && (Iter2.Text == "0"))
                    {
                        Instance = new Familiar(EngineFunc.SpriteEngine);
                        Instance.ImageLib = Wz.EquipImageLib;
                        Instance.IntMove = true;
                        Instance.Tag = 1;
                        Instance.State = Iter.Text;
                        Instance.Frame = Iter2.Text.ToInt();
                        Instance.UpPath = Entry.FullPathToFile2();
                        Instance.ImageNode = Wz.EquipData[Iter2.FullPathToFile2()];
                        Instance.FollowDistance = 130;
                        int StartX = (int)Game.Player.X - Instance.FollowDistance;
                        if (StartX < Map.Left)
                            StartX = (int)Game.Player.X;
                        Vector2 Pos = FootholdTree.Instance.FindBelow(new Vector2(StartX, Game.Player.Y - 100), ref Instance.BelowFH);
                        Instance.MoveType = MoveType.Jump;
                        Instance.X = Pos.X;
                        Instance.Y = Pos.Y;
                        Instance.FH = Instance.BelowFH;
                        Instance.Z = Instance.FH.Z * 100000 + 50000;
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

        if (Distance.X > FollowDistance)
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

        if (Game.Player.Y < Y && !Game.Player.InLadder)
        {
            switch (Distance.Y)
            {
                case float i when i >= 100 && i <= 150:
                    if (JumpState == JumpState.jsNone && Game.Player.JumpState == JumpState.jsNone)
                    {
                        Vector2 Below = FootholdTree.Instance.FindBelow(new Vector2(X, Y - 80), ref BelowFH);
                        if (Y - Below.Y != 0)
                            DoJump = true;
                    }
                    break;
                case float i when i >= 151 && i <= 2000:
                    if (Game.Player.JumpState == JumpState.jsNone)
                    {
                        X = Game.Player.X;
                        Y = Game.Player.Y;
                        DoJump = true;
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
        if (JumpState == JumpState.jsFalling)
        {
          Vector2  Below = FootholdTree.Instance.FindBelow(new Vector2(X, Y - VelocityY - 2), ref BelowFH);
            if (Y >= Below.Y - 3)
            {
                Y = Below.Y;
                // MaxFallSpeed :=10;
                JumpState = JumpState.jsNone;
                FH = BelowFH;
                Z = FH.Z * 100000 + 50000;
            }
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
                    Z = FH.Z * 100000 + 50000;
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
                    Z = FH.Z * 100000 + 50000;
                }
            }
        }

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
                if (Iter.FullPathToFile2().LeftStr(3) == "Mob")
                {
                    Wz.EquipImageLib.Remove(Iter);
                    // EquipData.Remove(Iter.GetPath);
                }
            }
            EngineFunc.SpriteEngine.Dead();
        }
    }
}

public class FamiliarNameTag : SpriteEx
{
    public FamiliarNameTag(Sprite Parent) : base(Parent)
    {
    }
    static bool ReDraw;
    static bool CanUse;
    static string MobName;
    static int NameWidth;
    static RenderTarget2D TargetTexture = null;
    public static bool IsUse = true;
    public static FamiliarNameTag Instance;
    public static void Create(string Name)
    {
        MobName = Name;
        NameWidth = Map.MeasureStringX(Map.NpcNameTagFont, Name);
        EngineFunc.Canvas.DrawTarget(ref TargetTexture, NameWidth + 10, 25, () =>
        {
            int NamePos = NameWidth / 2;
            if (Map.ShowChar)
            {
                EngineFunc.Canvas.FillRect(0, 2, NameWidth + 8, 15, new Microsoft.Xna.Framework.Color(0, 0, 0, 150));
                EngineFunc.Canvas.DrawString(Map.NpcNameTagFont, Name, 3, 2, Microsoft.Xna.Framework.Color.White);
            }
        });

        Instance = new FamiliarNameTag(EngineFunc.SpriteEngine);
        Instance.Tag = 1;
        Instance.IntMove = true;
        //NameTag.BlendMode=BlendMode.AddtiveColor;
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        if (ReDraw)
        {
            NameWidth = Map.MeasureStringX(Map.NpcNameTagFont, MobName);
            EngineFunc.Canvas.DrawTarget(ref TargetTexture, NameWidth + 10, 25, () =>
            {
                int NamePos = NameWidth / 2;
                if (Map.ShowChar)
                {
                    Engine.Canvas.FillRect(0, 2, NameWidth + 8, 15, new Microsoft.Xna.Framework.Color(0, 0, 0, 180));
                    Engine.Canvas.DrawString(Map.NpcNameTagFont, MobName, 3, 2, Microsoft.Xna.Framework.Color.White);
                }
            });
        }
        X = Familiar.Instance.X;
        Y = Familiar.Instance.Y;
        Z = Familiar.Instance.Z;
    }

    public override void DoDraw()
    {
        if (!NameTag.IsUse)
            return;
        if (Map.ShowChar)
        {
            int WX = (int)(Familiar.Instance.X) - (int)Engine.Camera.X;
            int WY = (int)(Familiar.Instance.Y) - (int)Engine.Camera.Y;
            int NamePos = NameWidth / 2;
            Engine.Canvas.Draw(TargetTexture, WX - NamePos - 8, WY,  MonoGame.SpriteEngine.BlendMode.NonPremultiplied);
        }
        if (ReDraw)
            ReDraw = false;
    }

    public static void Remove()
    {
        if (Instance != null)
            Instance.Dead();
        EngineFunc.SpriteEngine.Dead();

    }

}