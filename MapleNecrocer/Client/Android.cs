using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MapleNecrocer;

public class AndroidPlayer : PlayerEx
{
    public AndroidPlayer(Sprite Parent) : base(Parent)
    {

    }
    Foothold WallFH;
    Foothold BelowFH;

    int FallEdge;
    int JumpEdge;
    int FollowDistance;
    Vector2 Distance;
    public static AndroidPlayer Instance;
    static List<string> List = new List<string>();
    public static void SpawnNew()
    {
        Player._NewZ += 1;
        Instance = new AndroidPlayer(EngineFunc.SpriteEngine);
        Instance.ImageLib = Wz.EquipImageLib;
        Instance.NewZ = Player._NewZ;
        Instance.OtherPlayer = true;
        Instance.X = Game.Player.X;
        Instance.Y = Game.Player.Y - 80;
        Foothold BelowFH = null;
        Vector2 Below = FootholdTree.Instance.FindBelow(new Vector2(Game.Player.X, Game.Player.Y - 2), ref BelowFH);
        Instance.FH = BelowFH;
        Instance.JumpSpeed = 0.6f;
        Instance.JumpHeight = 9.5f;
        Instance.MaxFallSpeed = 8;
        Instance.JumpState = JumpState.jsFalling;
        Instance.moveType = MoveType.Jump;
        Instance.FollowDistance = 100;
        Instance.MoveSpeed = 2.1f;
    }

    public void Spawn(string IDList)
    {
        RemoveSprites();
        ShowHair = false;
        DressCap = false;
        var Split = IDList.Split('-');
        List.Clear();
        for (int i = 0; i < Split.Length - 1; i++)
            List.Add(Split[i]);
        List.Sort();
        for (int i = 0; i < List.Count; i++)
            CreateEquip(List[i], EngineFunc.SpriteEngine);
    }

    public override void DoMove(float Delta)
    {
        //base.DoMove(Delta);
        UpdateJump();
        int X1 = FH.X1;
        int Y1 = FH.Y1;
        int X2 = FH.X2;
        int Y2 = FH.Y2;
        Distance.X = Math.Abs(Game.Player.X - X);
        Distance.Y = Math.Abs(Game.Player.Y - Y);

        if (Distance.X > FollowDistance)
        {
            Action = WalkType;
            if (Game.Player.X > X)
            {
                FlipX = true;
                moveDirection = MoveDirection.Right;
            }
            if (Game.Player.X < X)
            {
                FlipX = false;
                moveDirection = MoveDirection.Left;
            }
        }
        else
        {
            Action = StandType;
            moveDirection = MoveDirection.None;
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
            Vector2 Below = FootholdTree.Instance.FindBelow(new Vector2(X, Y - VelocityY - 2), ref BelowFH);
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
        if (moveDirection == MoveDirection.Left)
        {
            Direction = GetAngle256(X2, Y2, X1, Y1);
            if (!FH.IsWall())
            {
                X += (float)(Sin256(Direction) * MoveSpeed);
                Y -= (float)(Cos256(Direction) * MoveSpeed);
            }
            FallEdge = -999999;
            JumpEdge = -999999;
            if (moveType == MoveType.Move)
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
                    moveDirection = MoveDirection.Right;
                }
            }

            if (moveType == MoveType.Jump)
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
                            moveDirection = MoveDirection.Right;
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
        if (moveDirection == MoveDirection.Right)
        {
            Direction = GetAngle256(X1, Y1, X2, Y2);
            if (!FH.IsWall())
            {
                X += (float)(Sin256(Direction) * MoveSpeed);
                Y -= (float)(Cos256(Direction) * MoveSpeed);
            }
            FallEdge = 999999;
            JumpEdge = 999999;
            if (moveType == MoveType.Move)
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
                    moveDirection = MoveDirection.Left;
                }
            }

            if (moveType == MoveType.Jump)
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
                            moveDirection = MoveDirection.Left;
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
        if (JumpState != JumpState.jsNone)
            Action = "jump";

    }
}


public class AndroidNameTag : MedalTag
{
    public AndroidNameTag(Sprite Parent) : base(Parent)
    {
    }
    public static AndroidNameTag Instance;

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
        X = AndroidPlayer.Instance.X;
        Y = AndroidPlayer.Instance.Y;
        Z = AndroidPlayer.Instance.Z;
    }

    public override void DoDraw()
    {
        if (Map.ShowPlayer)
        {
            int WX = (int)(AndroidPlayer.Instance.X) - (int)(Engine.Camera.X);
            int WY = (int)(AndroidPlayer.Instance.Y) - (int)(Engine.Camera.Y);
            Engine.Canvas.Draw(TargetTexture, WX - 150, WY - 28, MonoGame.SpriteEngine.BlendMode.NonPremultiplied);
        }
        if (IsReDraw)
            IsReDraw = false;
    }

    public static void Create(string ItemID)
    {
        Instance = new AndroidNameTag(EngineFunc.SpriteEngine);
        Instance.IntMove = true;
        Instance.Tag = 1;
        int TagNum = Wz.GetInt("Etc/Android/" + ItemID + "/info/nameTag", 38);
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