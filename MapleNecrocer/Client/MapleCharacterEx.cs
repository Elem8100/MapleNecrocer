using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Spine;

namespace MapleNecrocer;


public class PlayerEx : Player
{
   public enum MoveDirection { Left, Right, None, None2 }
   public enum MoveType { Stand, Move, Jump, Fly }

    public PlayerEx(Sprite Parent) : base(Parent)
    {
      
    }
  
    public static void Spawn(string IDList)
    {

        Player._NewZ += 1;
        var PlayerEx = new PlayerEx(EngineFunc.SpriteEngine);

        PlayerEx.NewZ = Player._NewZ;
        PlayerEx.OtherPlayer = true;

        PlayerEx.X = Game.Player.X;
        PlayerEx.Y = Game.Player.Y - 100;
        Foothold BelowFH = null;
        var Below = FootholdTree.Instance.FindBelow(new Vector2(Game.Player.X, Game.Player.Y - 2), ref BelowFH);
        PlayerEx.FH = BelowFH;
        PlayerEx.JumpSpeed = 0.6f;
        PlayerEx.JumpHeight = 9.5f;
        PlayerEx.MaxFallSpeed = 8;
        PlayerEx.JumpState = JumpState.jsFalling;
        PlayerEx.MoveSpeed = 1.8f;
        PlayerEx.moveType = MoveType.Jump;
        PlayerEx.moveDirection = MoveDirection.None;

        PlayerExList.Add(PlayerEx);
        string[] Split = IDList.Split("-");
        List.Clear();
        for (int i = 0; i < Split.Length - 1; i++)
            List.Add(Split[i]);
        List.Sort();
        for (int i = 0; i < List.Count; i++)
            PlayerEx.CreateEquip(List[i], EngineFunc.SpriteEngine);
    }
    static List<string> List = new List<string>();
    public static List<PlayerEx> PlayerExList = new();
    public MoveDirection moveDirection;
    public MoveType moveType;
    public float MoveSpeed;
    int JumpEdge;
    float VelocityY;
    public int JumpCount;
    public float JumpSpeed;
    public float JumpHeight;
    public float MaxFallSpeed;
    public bool DoJump;
    private JumpState jumpState;
    public JumpState JumpState
    {
        get => jumpState;
        set
        {
            if (jumpState != value)
            {
                jumpState = value;
                switch (value)
                {
                    case JumpState.jsNone:
                    case JumpState.jsFalling:
                        VelocityY = 0;
                        break;
                }
            }
        }
    }

    public void UpdateJump()
    {

        switch (jumpState)
        {
            case JumpState.jsNone:
                if (DoJump)
                {
                    jumpState = JumpState.jsJumping;
                    VelocityY = -JumpHeight;
                }
                break;

            case JumpState.jsJumping:

                Y += VelocityY * 1;
                VelocityY += JumpSpeed;
                if (VelocityY > 0)
                    jumpState = JumpState.jsFalling;
                break;

            case JumpState.jsFalling:
                Y = Y + VelocityY * 1;
                VelocityY = VelocityY + JumpSpeed;
                if (VelocityY > MaxFallSpeed)
                    VelocityY = MaxFallSpeed;
                break;
        }
        DoJump = false;

    }

    public override void DoMove(float Delta)
    {
        //base.DoMove(Delta);
        UpdateJump();
        int X1 = FH.X1;
        int Y1 = FH.Y1;
        int X2 = FH.X2;
        int Y2 = FH.Y2;
        Random Random = new Random();

        switch (Random.Next(300))
        {
            case 100:
                FlipX = false;
                moveDirection = MoveDirection.Left;
                break;
            case 150:
                FlipX = true;
                moveDirection = MoveDirection.Right;
                break;
            case 200:
                moveDirection = MoveDirection.None;
                break;
            case 290:
                DoJump = true;
                break;
        }

        if (JumpState != JumpState.jsNone)
        {
            Action = "jump";
        }
        else
        {
            switch (moveDirection)
            {
                case MoveDirection.Left:
                case MoveDirection.Right:
                    Action = WalkType;
                    break;
                case MoveDirection.None:
                    Action = StandType;
                    break;
                case MoveDirection.None2:
                    Action = "prone";
                    break;
            }
        }

        Vector2 Below = new();
        Foothold BelowFH = null, WallFH = null;
        if ((JumpState == JumpState.jsFalling))
        {
            Below = FootholdTree.Instance.FindBelow(new Vector2(X, Y - VelocityY - 6), ref BelowFH);
            if (Y >= Below.Y - 3)
            {
                Y = Below.Y;
                MaxFallSpeed = 10;
                JumpState = JumpState.jsNone;
                FH = BelowFH;
                Z = FH.Z * 100000 + 60000;
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
                if (X < Map.Left + 20)
                {
                    X = Map.Left + 20;
                    FlipX = true;
                    moveDirection = MoveDirection.Right;
                }
                // .--------.
                if (FH.Prev == null)
                    JumpEdge = FH.X1;
                // ┌--- <--
                if ((FH.Prev != null) && (FH.Prev.IsWall()) && (FH.Prev.Y1 > Y))
                    FallEdge = FH.X1;

                if (X < FallEdge)
                    JumpState = JumpState.jsFalling;
                if (X < JumpEdge)
                    DoJump = true;
                // -->  ---┐  <--
                WallFH = FootholdTree.Instance.FindWallR(new Vector2(X + 4, Y - 4));
                if ((WallFH != null) && (FH.Z == WallFH.Z))
                {
                    if (X < WallFH.X1 + 30)
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
                    Z = FH.Z * 100000 + 6000;
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
                if (X > Map.Right - 20)
                {
                    X = Map.Right - 20;
                    FlipX = false;
                    moveDirection = MoveDirection.Left;
                }
                if (FH.Next == null) // .--------.
                    JumpEdge = FH.X2;
                // -->  ----┐
                if ((FH.Next != null) && (FH.Next.IsWall()) && (FH.Next.Y2 > Y))
                    FallEdge = FH.X2;

                if (X > FallEdge)
                    JumpState = JumpState.jsFalling;
                if (X > JumpEdge)
                    DoJump = true;
                // -->  ┌.....
                WallFH = FootholdTree.Instance.FindWallL(new Vector2(X - 4, Y - 4));
                if ((WallFH != null) && (FH.Z == WallFH.Z))
                {
                    if (X > WallFH.X1 - 30)
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
                    Z = FH.Z * 100000 + 6000;
                }
            }
        }


    }


}

public class AvatarPartEx : AvatarParts
{
    public AvatarPartEx(Sprite Parent) : base(Parent)
    {

    }

    public override void DoMove(float Delta)
    {
        if (Image != "hand")
        {
            //  if ((Alpha == 0) || (Visible = false))
            //   return;
        }

        X = Owner.X;
        Y = Owner.Y;
        FlipX = Owner.FlipX;
        State = Owner.Action;
        Random Random = new Random();
        switch (Random.Next(500))
        {
            case 250:
                FaceFrame = 0;
                Expression = "smile";
                break;
            case 400:
                Expression = "blink";
                break;
        }
        UpdateFrame();

    }

    public override void DoDraw()
    {
        //   if ((Alpha == 0) || (Visible == false))
        //    return;
        base.DoDraw();
        if (ChangeFrame)
            ChangeFrame = false;
        if (Visible)
            Moved = true;
    }
}


