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
using System.Security.Cryptography;
using System.Numerics;
using System.Threading;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Xml.Linq;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using WzComparerR2.CharaSim;
using WzComparerR2.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Reflection.Metadata;
using static MapleNecrocer.Skill;
using Microsoft.Xna.Framework.Graphics;

namespace MapleNecrocer;
enum MoveDirection { Left, Right, None }
enum MoveType { Stand, Move, Jump, Fly }

public class Mob : JumperSprite
{
    public Mob(Sprite Parent) : base(Parent)
    {
        SpriteSheetMode = SpriteSheetMode.NoneSingle;
        CollideMode = CollideMode.Rect;
        IntMove = true;
       
    }
    int Frame;
    float Time;
    public string ID;
    string Action;
    Foothold FH, WallFH;
    Foothold BelowFH;
    int RX0, RX1;
    MoveDirection MoveDirection;
    float MoveSpeed, FlySpeed;
    string MobName;
    int Level;
    int NameWidth, IDWidth, LevelWidth;
    int FrameCount;
    bool NoFlip;
    MoveType MoveType;
    int FallEdge, JumpEdge;
    int CosY, SrcY;
    public bool GetHit1;
    public bool Hit;
    bool AnimEnd;
    int Value;
    bool AnimRepeat;
    bool AnimZigzag;
    Wz_Vector LT, RB;
    public int Left, Top, Right, Bottom;
    //Vector2 Head;
    //Vector2 Origin;
    public Int64 HP;
    public bool Die;
    string DieActionName;
    // int HeadX;
    public Vector2 Head;
    public int HitIndex;
    public int HeadX;
    public string LocalID;
    static Dictionary<string, int> FrameData = new();
    public static List<string> MobList = new();
    public static List<string> SummonedList = new();
    public MobCollision[] MobCollision = new MobCollision[7];
    int NameTagWidth;
    public RenderTarget2D RenderTarget;
    public RenderTarget2D IDRenderTarget;
    public static void Create()
    {
        Mob.MobList.Clear();
        Mob.FrameData.Clear();
        foreach (var Iter in Map.Img.Nodes["life"].Nodes)
        {
            if (Iter.Nodes["type"] != null)
            {
                if (Iter.GetStr("type") == "m")
                    Mob.Spawn(Iter.GetStr("id"), Iter.GetInt("x"), Iter.GetInt("cy"), Iter.GetInt("rx0"), Iter.GetInt("rx1"));
            }
            else
            {
                foreach (var Iter2 in Iter.Nodes)
                {
                    if (Iter2.GetStr("type") == "n")
                        Mob.Spawn(Iter2.GetStr("id"), Iter2.GetInt("x"), Iter2.GetInt("cy"), Iter2.GetInt("rx0"), Iter2.GetInt("rx1"));
                }
            }
        }
    }

    public static void Spawn(string ID, int PosX, int PosY, int RX0 = 0, int RX1 = 0)
    {
        if (!MobList.Contains(ID))
        {
            Wz.DumpData(Wz.GetNode("Mob/" + ID + ".img"), Wz.Data, Wz.ImageLib);
       
            MobList.Add(ID);
        }
        var Link = Wz.GetNode("Mob/" + ID + ".img/info/link");
        string TestID;
        if (Link != null)
        {
            var LinkID = Link.ToStr();
            TestID = LinkID;
            if (!Wz.Data.ContainsKey("'Mob/" + LinkID + ".img"))
                Wz.DumpData(Wz.GetNode("Mob/" + LinkID + ".img"), Wz.Data, Wz.ImageLib);
        }
        else
        {
            TestID = ID;
        }
        if ((Wz.GetNodeA("Mob/" + TestID + ".img/stand/0") == null) && (Wz.GetNodeA("Mob/" + TestID + ".img/fly/0") == null))
            return;
        //Create Mob
        var Mob = new Mob(EngineFunc.SpriteEngine);

        Mob.LocalID = ID;

        if (Link != null)
            Mob.ID = Link.ToStr();
        else
            Mob.ID = ID;
        Mob.Value = 1;

        foreach (var Iter in Wz.GetNode("Mob/" + Mob.ID + ".img").Nodes)
        {
            int c = 0;
            foreach (var Iter2 in Iter.Nodes)
            {
                if (Char.IsNumber(Iter2.Text[0]))
                    c += 1;
            }
            FrameData.AddOrReplace(Mob.ID + Iter.Text + "/FrameCount", c - 1);
        }

        var InfoNode = Wz.GetNode("Mob/" + Mob.LocalID + ".img/info");
        if (InfoNode.Get("speed") != null)
            Mob.MoveSpeed = (1 + (float)InfoNode.GetInt("speed") / 100) * 2;
        else
            Mob.MoveSpeed = 2;
        if (InfoNode.Get("flySpeed") != null)
            Mob.FlySpeed = (1 + InfoNode.GetInt("flySpeed") / 100) * 2;
        else
            Mob.FlySpeed = 2;

        // int Hp=InfoNode.GetInt("maxHP");
        Mob.HP = 2000000;
        Mob.Level = InfoNode.GetInt("level", 1);

        Mob.MoveType = MoveType.Move;
        Mob.Action = "stand";

        Random Random = new Random();
        var ImgNode = Wz.GetNode("Mob/" + Mob.ID + ".img");
        if (ImgNode.Nodes["move"] == null)
        {
            Mob.MoveSpeed = 0;
            Mob.MoveType = MoveType.Stand;
        }
        if (ImgNode.Nodes["jump"] != null)
            Mob.MoveType = MoveType.Jump;
        if (ImgNode.Nodes["fly"] != null)
        {
            Mob.MoveType = MoveType.Fly;
            Mob.Action = "fly";
            Mob.CosY = Random.Next(256);
        }
        if (ImgNode.Nodes["die1"] != null)
            Mob.DieActionName = "die1";
        else
            Mob.DieActionName = "die";

        Mob.ImageLib = Wz.ImageLib;
        Mob.ImageNode = Wz.Data["Mob/" + Mob.ID + ".img/" + Mob.Action + "/0"];
        Vector2 Pos = FootholdTree.Instance.FindBelow(new Vector2(PosX, PosY - 3), ref Mob.BelowFH);
        Mob.X = Pos.X;
        Mob.Y = Pos.Y;
        Mob.SrcY = (int)Mob.Y;
        Mob.FH = Mob.BelowFH;
        Mob.Z = Mob.FH.Z * 100000 + 6000;
        Mob.JumpSpeed = 0.6f;
        Mob.JumpHeight = 9;
        Mob.MaxFallSpeed = 8;
        Mob.RX0 = RX0;
        Mob.RX1 = RX1;
        Mob.Width = Mob.ImageWidth;
        Mob.Height = Mob.ImageHeight;
        Mob.FlipX = Random.Next(2).ToBool();
        Mob.MoveDirection = MoveDirection.None;
        Mob.CanCollision = true;
        Mob.AnimRepeat = true;
        Mob.DoAnimate = false;
        if (ImgNode.GetInt("info/noFlip").ToBool())
            Mob.FlipX = false;
        Mob.LevelWidth = Map.MeasureStringX(Map.MobLvFont, "Lv." + Mob.Level);
        Mob.MobName = Wz.GetNodeA("String/Mob.img/" + Mob.LocalID.IntID()).GetStr("name");
        Mob.NameWidth = Map.MeasureStringX(Map.NpcNameTagFont, Mob.MobName);
        Mob.IDWidth = Map.MeasureStringX(Map.NpcNameTagFont, "ID:" + Mob.LocalID);
        Mob.NameTagWidth = 10 + Mob.LevelWidth + Mob.NameWidth;
        Mob.Engine.Canvas.DrawTarget(ref Mob.RenderTarget, Mob.NameTagWidth, 25, () =>
        {
            Mob.RenderTargetFunc();
        });

        Mob.Engine.Canvas.DrawTarget(ref Mob.IDRenderTarget, Mob.IDWidth+10, 25, () =>
        {
            Mob.IDRenderTargetFunc();
        });

        Wz_Vector origin = WzDict.GetVector("Mob/" + Mob.ID + ".img/" + Mob.Action + "/0/origin");
        if (Mob.FlipX)
            Mob.Origin.X = -origin.X + Mob.ImageWidth;
        else
            Mob.Origin.X = origin.X;
        Mob.Origin.Y = origin.Y;
    }

    void RenderTargetFunc()
    {
        float LvPosX = LevelWidth - (NameWidth / 2) + 4;
        Engine.Canvas.FillRoundRect(0, 5, LevelWidth + 5, 11, new Microsoft.Xna.Framework.Color(0, 0, 0, 150));
        Engine.Canvas.DrawString(Map.MobLvFont, "Lv." + Level, 2, 5, Microsoft.Xna.Framework.Color.White);

        float NamePosX = LevelWidth + 6;
        Engine.Canvas.FillRoundRect((int)NamePosX, 3, NameWidth + 4, 15, new Microsoft.Xna.Framework.Color(0, 0, 0, 150));
        Engine.Canvas.DrawString(Map.NpcNameTagFont, MobName, NamePosX + 2, 4, Microsoft.Xna.Framework.Color.White);
    }

    void IDRenderTargetFunc()
    {
        
        Engine.Canvas.FillRoundRect(0, 1, IDWidth + 5, 15, new Microsoft.Xna.Framework.Color(0, 0, 0, 150));
        Engine.Canvas.DrawString(Map.NpcNameTagFont, "ID:" + ID, 2, 2, Microsoft.Xna.Framework.Color.Cyan);
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);

        int X1 = FH.X1;
        int Y1 = FH.Y1;
        int X2 = FH.X2;
        int Y2 = FH.Y2;
        string NewAction = "";
        if (FrameData.ContainsKey(ID + Action + "/FrameCount"))
            FrameCount = FrameData[ID + Action + "/FrameCount"];
        string ImagePath = "Mob/" + ID + ".img/" + Action + "/" + Frame;
        ImageNode = Wz.Data[ImagePath];
        int AnimDelay = WzDict.GetInt(ImagePath + "/delay", 100);
        int a1 = WzDict.GetInt(ImagePath + "/a1", 255);

        if (Wz.HasData(ImagePath + "/lt"))
        {
            LT = WzDict.GetVector(ImagePath + "/lt");
            RB = WzDict.GetVector(ImagePath + "/rb");
            switch (FlipX)
            {
                case true:
                    Right = (int)X - LT.X;
                    Left = (int)X - RB.X;
                    break;
                case false:
                    Left = (int)X + LT.X;
                    Right = (int)X + RB.X;
                    break;
            }
            Top = (int)Y + LT.Y;
            Bottom = (int)Y + RB.Y;
        }
        CollideRect = SpriteUtils.Rect(Left, Top, Right, Bottom);

        if (Wz.HasData(ImagePath + "/head"))
        {
            Wz_Vector head = WzDict.GetVector(ImagePath + "/head");
            if ((FlipX != Game.Player.FlipX) || (!GetHit1))
            {
                if (FlipX)
                    Head.X = (int)X - head.X - 20;
                else
                    Head.X = (int)X + head.X - 20;
            }
            else
            {
                if (FlipX)
                    Head.X = (int)X - head.X - 20;
                else
                    Head.X = (int)X + head.X - 20;
            }
            Head.Y = (int)Y + head.Y;
        }

        if ((Action == "hit1") || (Action == DieActionName))
            AnimRepeat = false;
        else
            AnimRepeat = true;
        if (Wz.HasData("Mob/" + ID + ".img/" + Action + "/zigzag"))
            AnimZigzag = true;
        else
            AnimZigzag = false;

        Time += 17;
        if (Time > AnimDelay)
        {
            Time = 0;
            if (AnimZigzag)
            {
                Frame += Value;
                if ((Frame >= FrameCount) || (Frame <= 0))
                    Value = -Value;
            }
            else
            {
                Frame += 1;
                AnimEnd = false;
                if (Frame > FrameCount)
                {
                    if (AnimRepeat)
                        Frame = 0;
                    else
                    {
                        Frame -= 1;
                        AnimEnd = true;
                    }
                }
            }
        }

        if (!Die)
        {
            Random Random = new Random();
            if ((MoveType != MoveType.Stand) && (MoveType != MoveType.Fly))
            {
                switch (Random.Next(200))
                {
                    case 50:
                        FlipX = false;
                        MoveDirection = MoveDirection.Left;
                        break;
                    case 100:
                        FlipX = true;
                        MoveDirection = MoveDirection.Right;
                        break;
                    case 150:
                        MoveDirection = MoveDirection.None;
                        break;
                    case 199:
                        if (!GetHit1)
                        {
                            if (MoveType == MoveType.Jump)
                            {
                                DoJump = true;
                            }
                        }
                        break;
                }
            }
        }


        if ((!GetHit1) && (!Die))
        {
            if (JumpState != JumpState.jsNone)
            {
                NewAction = "jump";
                Frame = 0;
            }
            else
            {
                switch (MoveDirection)
                {
                    case MoveDirection.Left:
                    case MoveDirection.Right:
                        NewAction = "move";
                        break;
                    case MoveDirection.None:
                        NewAction = "stand";
                        break;
                }
            }
        }

        if ((AnimEnd) && (Action == "hit1"))
        {
            Time = 0;
            Frame = 0;
            GetHit1 = false;
        }

        if (Hit)
        {
            if (GetHit1)
            {
                if ((Action == "hit1") || (Action == DieActionName))
                {
                    if (Frame == 0)
                    {

                        if (Skill.Attacking)
                        {
                            if (Skill.MultiStrike)
                                DamageNumber.Create(Game.Damage, (int)HeadX, (int)Head.Y + HitIndex * (-30));
                            else
                                DamageNumber.Create(Game.Damage, (int)Head.X, (int)Head.Y);
                            SkillHitEffect.Create(this);
                        }
                        else
                        {
                            DamageNumber.Create(Game.Damage, (int)Head.X, (int)Head.Y);
                        }
                    }
                    Hit = false;
                }
            }

            // no push
            if (!GetHit1)
            {
                DamageNumber.Create(Game.Damage, (int)Head.X, (int)Head.Y);
                Hit = false;
            }
        }

        if ((GetHit1) && (!Die))
        {
            if (!Wz.HasData("Mob/" + ID + ".img/hit1/0"))
            {
                GetHit1 = false;
            }
            if (Action != "hit1")
            {
                AnimEnd = false;
                Frame = 0;
                Time = 0;
                NewAction = "hit1";
            }
            // -->player   -->mob
            if ((FlipX == Game.Player.FlipX) && (Time == 0))
            {
                FlipX = !FlipX;
                switch (MoveDirection)
                {
                    case MoveDirection.Left:
                        MoveDirection = MoveDirection.Right;
                        break;
                    case MoveDirection.Right:
                        MoveDirection = MoveDirection.Left;
                        break;
                    case MoveDirection.None:
                        if (FlipX)
                            MoveDirection = MoveDirection.Right;
                        else
                            MoveDirection = MoveDirection.Left;
                        break;
                }
            }
            // player-->  <--Mob
            if ((FlipX != Game.Player.FlipX) && (Time == 0))
            {
                if (MoveDirection == MoveDirection.None)
                {
                    if (FlipX)
                        MoveDirection = MoveDirection.Right;
                    else
                        MoveDirection = MoveDirection.Left;
                }
            }
        }

        if (Die)
        {
            if (Action != "die1")
            {
                AnimEnd = false;
                Frame = 0;
                if (!Wz.HasData("Mob/" + ID + ".img/" + DieActionName + "/0"))
                    Frame = 1;
                if ((FrameCount > 3) && (!Wz.HasData("Mob/" + ID + ".img/" + DieActionName + "/3")))
                    Frame = 4;
                Time = 0;
                NewAction = DieActionName;
            }

            if (!Wz.HasData("Mob/" + ID + ".img/" + DieActionName + "/" + Frame))
                Dead();
        }

        if (a1 != -1)
        {
            float AniAlpha = 255 - (255 - a1) * Time / AnimDelay;
            if ((AniAlpha < 255) && (AniAlpha > 0))
                Alpha = (byte)AniAlpha;
            // if Alpha <=10 then Dead;
        }

        if ((AnimEnd) && (Action == DieActionName))
        {
            //TMobDrop.Drop(Trunc(X), Trunc(Y), Random(1), DropList);
            Dead();
        }

        if (Wz.HasData("Mob/" + ID + ".img/" + NewAction + "/" + Frame))
        {
            Action = NewAction;
        }
        else
        {
            if (Action != DieActionName)
            {
                if (Wz.HasData("Mob/" + ID + ".img/" + NewAction + "/" + Frame + 1))
                    Action = "Mob/" + ID + ".img/" + NewAction + "/" + Frame + 1;
                else
                    Frame = 0;
            }
            if (AnimZigzag)
                Frame = 0;
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
                Z = FH.Z * 100000 + 6000;
            }
        }
        int Direction;
        switch (MoveDirection)
        {
            case MoveDirection.Left:
                Direction = GetAngle256(X2, Y2, X1, Y1);
                if ((!FH.IsWall()) && (!GetHit1) && (!Die))
                {
                    X += (float)(Sin256(Direction) * MoveSpeed);
                    Y -= (float)(Cos256(Direction) * MoveSpeed);
                }
                if ((GetHit1) && (!Die) && (Time < 300))
                {
                    X += (float)(Sin256(Direction) * -1.3f);
                    Y -= (float)(Cos256(Direction) * -1.3f);
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
                    if (X <= RX0)
                    {
                        X = RX0;
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
                        Z = FH.Z * 100000 + 6000;
                    }
                }
                break;

            case MoveDirection.Right:
                Direction = GetAngle256(X1, Y1, X2, Y2);
                if ((!FH.IsWall()) && (!GetHit1) && (!Die))
                {
                    X += (float)(Sin256(Direction) * MoveSpeed);
                    Y -= (float)(Cos256(Direction) * MoveSpeed);
                }
                if ((GetHit1) && (!Die) && (Time < 300))
                {
                    X += (float)(Sin256(Direction) * -1.3f);
                    Y -= (float)(Cos256(Direction) * -1.3f);
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
                    if (X >= RX1)
                    {
                        X = RX1;
                        FlipX = false;
                        MoveDirection = MoveDirection.Left;
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
                        Z = FH.Z * 100000 + 6000;
                    }
                }
                break;
        }

        if (MoveType == MoveType.Fly)
        {
            Random Random = new();
            switch (Random.Next(250))
            {
                case 50:
                    FlipX = true;
                    break;
                case 200:
                    FlipX = false;
                    break;
            }
            if (X <= RX0)
                FlipX = true;
            if (X >= RX1)
                FlipX = false;

            switch (FlipX)
            {
                case true:
                    X += 1.5f * FlySpeed;
                    break;
                case false:
                    X -= 1.5f * FlySpeed;
                    break;
            }
            CosY += 7;
            Y = SrcY - (float)(Cos256(CosY) * 16);
        }

        Wz_Vector origin = WzDict.GetVector(ImagePath + "/origin");
        if (FlipX)
            Origin.X = -origin.X + ImageWidth;
        else
            Origin.X = origin.X;
        Origin.Y = origin.Y;
    }

    public override void DoDraw()
    {
        if (!Map.ShowMob)
            return;
        base.DoDraw();
        if (Map.ShowMobName)
        {
            int WX = (int)X - (int)Engine.Camera.X;
            int WY = (int)Y - (int)Engine.Camera.Y;
            Engine.Canvas.Draw(RenderTarget, WX - NameTagWidth / 2, WY, MonoGame.SpriteEngine.BlendMode.NonPremultiplied2);
        }

        if(Map.ShowID)
        {
            int WX = (int)X - (int)Engine.Camera.X;
            int WY = (int)Y - (int)Engine.Camera.Y;
            Engine.Canvas.Draw(IDRenderTarget, WX - IDWidth / 2, WY+20, MonoGame.SpriteEngine.BlendMode.NonPremultiplied2);
        }
    }
}

public class MobCollision : SpriteEx
{
    public MobCollision(Sprite Parent) : base(Parent)
    {
        CollideMode = CollideMode.Rect;
        CanCollision = true;
    }

    public Mob Owner;
    // int Left, Top, Right, Bottom;
    int Counter;
    public int Index;
    public int StartTime;
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        CollideRect = SpriteUtils.Rect(Owner.Left, Owner.Top, Owner.Right, Owner.Bottom);
        Counter += 1;
        if (Counter >= StartTime)
        {
            Collision();
            Dead();
        }
    }

    public override void OnCollision(Sprite sprite)
    {
        if (sprite is SkillCollision)
        {
            if (Owner.HP > 0)
            {
                Owner.HitIndex = this.Index;
                if (this.Index == 0)
                    Owner.HeadX = (int)Owner.Head.X;
                Owner.Hit = true;
                Random Random = new Random();
                Game.Damage = 50000 + Random.Next(550000);
                Owner.HP -= Game.Damage;
               
                if (Wz.GetNode("Sound/Mob.img/" + Owner.ID + "/Damage") != null)
                    Sound.Play("Sound/Mob.img/" + Owner.ID + "/Damage");
                else if (Wz.GetNode("Sound/Mob.img/" + Owner.ID + "/Hit1") != null)
                    Sound.Play("Sound/Mob.img/" + Owner.ID + "/Hit1");
                if (Wz.Data.ContainsKey("Mob/" + Owner.ID + ".img/hit1"))
                {
                    Owner.GetHit1 = true;
                }
            }

            if (Owner.HP <= 0 && !Owner.Die)
            {
                Sound.Play("Sound/Mob.img/"+ Owner.ID + "/Die");
                Owner.Die = true;
                //  Collisioned := False;
                // Dead;
            }
        }
    }

    public override void DoDraw()
    {
        if (ImageNode == null) return;
    }

}


