using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.SpriteEngine;
using WzComparerR2.Common;
using WzComparerR2.WzLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using Keyboard = SpriteEngine.Keyboard;
using Input = Microsoft.Xna.Framework.Input.Keys;
using Microsoft.Xna.Framework.Input;
using Spine;
using static MapleNecrocer.Map;
using WzComparerR2.CharaSim;
using WzComparerR2.Animation;
using DevComponents.DotNetBar;
using WzComparerR2.Text;
using System.ComponentModel;

namespace MapleNecrocer;

public enum FaceDir { Left, Right, None }
public enum LadderType { Ladder, Rope }
public enum PartName { Head, Body, Cap, Face, Hair, Glove, FaceAcc, Glass, EarRing, Cape, Coat, Longcoat, Pants, Shield, Shoes, Weapon, CashWeapon, Chairs, SitTamingMob, WalkTamingMob, TamingMob }
public class Game
{
    public static Player Player;
    public static int Damage;

}
public class Equip
{
    public static string GetDir(string ID)
    {
        switch (Int32.Parse(ID) / 10000)
        {
            case 0: case 1: return ""; break;
            case 2: case 5: return "Face/"; break;
            case 3: case 4: case 6: return "Hair/";
            case 101: case 102: case 103: return "Accessory/";
            case 100: return "Cap/";
            case 110: return "Cape/";
            case 104: return "Coat/";
            case 105: return "Longcoat/";
            case 106: return "Pants/";
            case 107: return "Shoes/";
            case 108: return "Glove/";
            case 109: return "Shield/";
            case 170: case int i when i >= 121 && i <= 160: return "Weapon/";
            case int i when i >= 190 && i <= 199: return "TamingMob/";
        }
        return "";
    }
    public static PartName GetPart(string ID)
    {
        switch (Int32.Parse(ID) / 10000)
        {
            case 0: return PartName.Body;
            case 1: return PartName.Head;
            case 2: case 5: return PartName.Face;
            case 3: case 4: case 6: return PartName.Hair;
            case 101: return PartName.FaceAcc;
            case 102: return PartName.Glass;
            case 103: return PartName.EarRing;
            case 100: return PartName.Cap;
            case 110: return PartName.Cape;
            case 104: return PartName.Coat;
            case 105: return PartName.Longcoat;
            case 106: return PartName.Pants;
            case 107: return PartName.Shoes;
            case 108: return PartName.Glove;
            case 109: return PartName.Shield;
            case int i when i >= 121 && i <= 160: return PartName.Weapon;
            case 170: return PartName.CashWeapon;
            case int i when i >= 190 && i <= 197: case 199: return PartName.WalkTamingMob;
            case 198: return PartName.SitTamingMob;
        }
        return PartName.Body;
    }
    public static string GetAfterImageStr(string ID)
    {
        int AID = ID.ToInt();
        int Num = (AID / 10000) - 100;
        switch (Num)
        {
            case 22: case 23: case 26: case 28: case 30: case 31: case 33: case 34: case 47: return "swordOL";
            case 36: return "cane";
            case 21: case 25: case 32: case 37: case 38: case 55: case 69: return "mace";
            case 24: case 27: return "swordOS";
            case 39: case 40: return "swordTS";
            case 41: case 42: return "axe";
            case 43: return "spear";
            case 44: return "poleArm";
            case 45: return "bow";
            case 46: return "crossBow";
            case 48: case 58: return "knuckle";
            case 49: return "gun";
            case 52: return "dualBow";
            case 53: return "cannon";
            case 54: return "swordTK";
            case 56: return "swordZB";
            case 57: return "swordZL";
            case 59: return "ancientBow";
        }
        return "";
    }
    public static string GetWeaponNum(string ID)
    {
        int AID = ID.ToInt();
        return ((AID / 10000) - 100).ToString();
    }
    public static Dictionary<string, int> Data = new();
    public static Dictionary<string, string> DataS = new();
}


public class Player : JumperSprite
{
    public Player(Sprite Parent) : base(Parent)
    {
        SpriteSheetMode = SpriteSheetMode.NoneSingle;
        Z = 20000;
        Offset.Y = -79;
        Offset.X = -40;
        JumpSpeed = 0.6f;
        JumpHeight = 9.5f;
        MaxFallSpeed = 8;
        Alpha = 0;
        Tag = 1;
        JumpState = JumpState.jsFalling;
        StandType = "stand1";
        WalkType = "walk1";

        // IntMove = true;
    }
    static bool Loaded;
    public static void SpawnNew()
    {

        AvatarTargetTexture = new RenderTarget2D(EngineFunc.Canvas.GraphicsDevice, 1024, 1024, false, SurfaceFormat.Color, DepthFormat.None);
        EngineFunc.Canvas.GraphicsDevice.SetRenderTarget(AvatarTargetTexture);
        EngineFunc.Canvas.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent);
        EngineFunc.Canvas.GraphicsDevice.SetRenderTarget(null);

        Game.Player = new Player(EngineFunc.SpriteEngine);
        Game.Player.AvatarEngine = new MonoSpriteEngine(null);
        Game.Player.AvatarEngine.Canvas = EngineFunc.Canvas;
        Game.Player.AvatarEngine.Camera.X = 21 - 400;
        Game.Player.AvatarEngine.Camera.Y = 20 - 400;

        int PX = 0, PY = 0;
        foreach (var Portals in MapPortal.PortalList)
        {
            if (Portals.PortalType == 0)
            {
                PX = Portals.X;
                PY = Portals.Y;
                break;
            }
        }
        Game.Player.X = PX;
        Game.Player.Y = PY;
        Foothold BelowFH = null;
        Vector2 Below = FootholdTree.Instance.FindBelow(new Vector2(PX, PY - 2), ref BelowFH);
        Game.Player.FH = BelowFH;
        Game.Player.FaceDir = FaceDir.None;
        Game.Player.JumpState = JumpState.jsFalling;

        foreach (var Iter in Wz.GetNodeA("Base/zmap.img").Nodes)
            AvatarParts.ZMap.Add(Iter.Text);
        for (int I = 2000; I <= 2011; I++)
        {
            Equip.DataS.Add("0000" + I.ToString() + "/swingTF/0/arm", "swingTF");
            Equip.DataS.Add("0000" + I.ToString() + "/swingT2/0/arm", "swingT2");
            Equip.DataS.Add("0000" + I.ToString() + "/swingOF/0/arm", "swingOF/0");
            Equip.DataS.Add("0000" + I.ToString() + "/swingOF/1/arm", "swingOF/1");
            Equip.DataS.Add("0000" + I.ToString() + "/swingOF/2/arm", "swingOF/2");
        }

        string[] DefaultEqps = { "01302030", "00002000", "01062055", "01072054", "01040005", "00020000", "00030020", "00012000" };
        for (int I = 0; I <= 7; I++)
        {
            Game.Player.CreateEquip(DefaultEqps[I], Game.Player.AvatarEngine);
            Player.EqpList.Add(DefaultEqps[I]);
        }
        Wz.DumpData(Wz.GetNodeA("Character/00002001.img"), Wz.EquipData, Wz.EquipImageLib);

        Game.Player.AttackAction = Game.Player.AttackActions[0];
        AfterImage.Load(Game.Player.AfterImageStr, "0");
        DamageNumber.Style = "NoRed1";
        DamageNumber.Load("");

        Loaded = true;
    }


    public static List<string> EqpList = new();
    MonoSpriteEngine AvatarEngine;
    public Foothold FH;
    public bool InLadder;
    bool OnLadder;
    bool FallFlag;
    int FallCounter, OffY;
    bool OnPortal;
    public FaceDir FaceDir;
    float SpeedL, SpeedR;
    float CurrentX;
    public string Action;
    string PlayerName;
    int NameWidth;
    public bool Attack;
    bool SkillDone;
    int NameTagTargetIndex;
    PortalInfo CurrentPortal;
    PortalInfo Portal;
    public LadderType LadderType;
    public string StandType, WalkType;
    List<AvatarParts> PartSpriteList = new();
    public bool ShowHair;
    public bool DressCap;
    public int CapType;
    public string WeaponNum;
    public bool ResetAction;
    public string NewAction;
    public string AfterImageStr;
    public string AttackAction, Str;
    List<string> AttackActions = new();
    List<string> AttackOFs = new();
    bool Flip;
    public bool OtherPlayer = false;

    public float MoveX, MoveY;

    public int NewZ;
    static RenderTarget2D AvatarTargetTexture;
    //static Texture2D AvatarTargetTexture;
    static int AvatarPanelIndex;
    static List<string> EquipDumpList = new();
    public static int _NewZ;
    private static float DestX, DestY;

    private static List<string> SameNames = new();
    public Vector2 Neck, Navel, Hand, Brow, HandMove;
    public Vector2 ArmHand, ArmNavel, BodyNeck, BodyNavel, BodyHand, lHandMove, HeadBrow, HeadNeck;
    public Vector2 BrowPos, TamingNavel;


    public void Spawn(string EquipID)
    {
        CreateEquip(EquipID, AvatarEngine);
    }

    public void CreateEquip(string EquipID, MonoSpriteEngine UseEngine)
    {
        string Dir = Equip.GetDir(EquipID);
        PartName Part = Equip.GetPart(EquipID);
        Wz_Node Img = Wz.GetNodeA("Character/" + Dir + EquipID + ".img");
        string Path;
        if (!EquipDumpList.Contains(EquipID))
        {
            Wz.DumpData(Img, Wz.EquipData, Wz.EquipImageLib);
            EquipDumpList.Add(EquipID);
        }

        string LPath = "Character/Weapon/";
        switch (Part)
        {

            case PartName.Weapon:
                AfterImageStr = Equip.GetAfterImageStr(EquipID);
                AfterImage.Load(AfterImageStr, "0");
                WeaponNum = Equip.GetWeaponNum(EquipID);
                AttackActions.Clear();
                AttackOFs.Clear();
                if (Wz.HasDataE(LPath + EquipID + ".img/stand1"))
                    StandType = "stand1";
                else if (Wz.HasDataE(LPath + EquipID + ".img/stand2"))
                    StandType = "stand2";
                if (Wz.HasDataE(LPath + EquipID + ".img/walk1"))
                    WalkType = "walk1";
                else if (Wz.HasDataE(LPath + EquipID + ".img/walk2"))
                    WalkType = "walk2";
                break;

            case PartName.Cap:
                DressCap = true;
                string VSlot = Wz.GetNodeA("Character/Cap/" + EquipID + ".img/info/vslot").ToStr();
                //no Cover
                if ((VSlot == "Cp") || (VSlot == "CpH5"))
                    CapType = 0;
                //stand cover
                if (VSlot == "CpH1H5")
                    CapType = 1;
                //cover all
                if (VSlot.Length > 12)
                {
                    if (VSlot.LeftStr(6) == "CpH1H3")
                        CapType = 2;
                    else
                        CapType = 3;
                }
                break;

            case PartName.Hair:
                ShowHair = true;
                break;

            case PartName.CashWeapon:
                for (int i = 69; i >= 30; i--)
                {
                    if (Wz.HasDataE(LPath + EquipID + ".img/" + i.ToString() + "/stand1"))
                        StandType = "stand1";
                    else if (Wz.HasDataE(LPath + EquipID + ".img/" + i.ToString() + "/stand2"))
                        StandType = "stand2";
                    if (Wz.HasDataE(LPath + EquipID + ".img/" + i.ToString() + "/walk1"))
                        WalkType = "walk1";
                    else if (Wz.HasDataE(LPath + EquipID + ".img/" + i.ToString() + "/walk2"))
                        WalkType = "walk2";
                    if (Wz.HasDataE(LPath + EquipID + ".img/" + i.ToString()))
                        WeaponNum = i.ToString();
                }
                AttackActions.Clear();
                AttackOFs.Clear();
                AfterImageStr = Equip.GetAfterImageStr("01" + WeaponNum + "1234");
                foreach (var Iter in Wz.GetNode("Character/Afterimage/" + AfterImageStr + ".img/0").Nodes)
                {
                    if ((Iter.Text.LeftStr(4) == "stab") || (Iter.Text.LeftStr(5) == "swing"))
                    {
                        if ((Iter.Text.RightStr(2) != "D1") && (Iter.Text.RightStr(2) != "D2"))
                        {
                            if (Iter.Text.RightStr(1) != "F")
                                AttackActions.Add(Iter.Text);
                            else
                                AttackOFs.Add(Iter.Text);
                        }
                    }
                }
                AfterImage.Load(AfterImageStr, "0");
                Img = Wz.GetNodeA("Character/" + Dir + EquipID + ".img/" + WeaponNum);
                break;
        }

        AvatarParts Sprite;
        SameNames.Clear();
        if (Loaded)
        {
            if (Img == Wz.GetNodeA("Character/00002000.img"))
                Img = Wz.GetNodeA("Character/00002001.img");
        }

        foreach (var Iter in Img.Nodes)
        {

            switch (Part)
            {
                case PartName.Weapon:
                    if (Iter.Text != "info")
                    {
                        if ((Iter.Text.LeftStr(4) == "stab") || (Iter.Text.LeftStr(5) == "swing"))
                        {
                            if (Iter.Text.RightStr(1) != "F")
                                AttackActions.Add(Iter.Text);
                            else
                                AttackOFs.Add(Iter.Text);
                        }
                    }
                    break;
                case PartName.Body:

                    Equip.Data.AddOrReplace("body/" + Iter.Text + "/FrameCount", Iter.Nodes.Count - 1);
                    break;
                case PartName.Face:
                    Equip.Data.AddOrReplace("face/" + Iter.Text + "/FrameCount", Iter.Nodes.Count - 1);
                    break;
            }
            foreach (var Iter2 in Iter.Nodes)
            {
                if (Part == PartName.Body)
                    Equip.Data.AddOrReplace("body/" + Iter.Text + "/" + Iter2.Text + "/delay", Math.Abs(Iter2.GetInt("delay")));
                if (Part == PartName.Face)
                    Equip.Data.AddOrReplace("face/" + Iter.Text + "/" + Iter2.Text + "/delay", Iter2.GetInt("delay"));
                if ((Iter2.Nodes["action"] != null) && (Iter2.Nodes["frame"] != null))
                    Equip.DataS.AddOrReplace(Iter.Text + "/" + Iter2.Text, Iter2.GetStr("action") + "/" + Iter2.GetInt("frame"));
                if (Iter2.Text == "hairShade")
                    continue;

                foreach (var Iter3 in Iter2.Nodes)
                {
                    if ((Iter3.Text == "hairShade") || (Iter3.Text == "006"))
                        continue;
                    if (Iter3.Value is Wz_Png || Iter3.Value is Wz_Uol)
                    {
                        if (!SameNames.Contains(Iter3.Text))
                        {
                            SameNames.Add(Iter3.Text);
                            if (OtherPlayer)
                                Sprite = new AvatarPartEx(UseEngine);
                            else
                                Sprite = new AvatarParts(UseEngine);

                            if (OtherPlayer)
                            {
                                Sprite.Visible = true;
                            }
                            else
                            {
                                if ((MapleChair.IsUse) || (TamingMob.IsUse))
                                {
                                    if ((Part == PartName.CashWeapon) || (Part == PartName.Weapon))
                                        Sprite.Visible = false;
                                    else
                                        Sprite.Visible = true;

                                }
                                else
                                {
                                    Sprite.Visible = false;
                                }
                            }

                            Sprite.Owner = this;
                            Sprite.ImageLib = Wz.EquipImageLib;
                            Path = Iter3.FullPathToFile2();

                            if (EquipID.LeftStr(4) == "0000")
                            {
                                for (int i = 0; i < 9; i++)
                                    Path = Path.Replace("0000200" + i, EquipID);
                            }
                            if (Wz.EquipData.ContainsKey(Path))
                                Sprite.ImageNode = Wz.EquipData[Path];
                            //Sprite.IntMove= true;
                            Sprite.Tag = 1;
                            Sprite.Value = 1;
                            Sprite.State = "stand1";
                            Sprite.FlipX = this.FlipX;
                            Sprite.Expression = "blink";
                            Sprite.Animate = true;
                            Sprite.AnimRepeat = true;

                            if (Part == PartName.Glass || Part == PartName.CashWeapon || Part==PartName.Cape ||Part== PartName.Cap)
                                Sprite.BlendMode = MonoGame.SpriteEngine.BlendMode.NonPremultiplied;
                            string[] S = Path.Split('/');

                            if (Part != PartName.CashWeapon)
                            {
                                // Body,head
                                if (S[1].LeftStr(1) == "0")
                                {
                                    Sprite.ID = S[1].LeftStr(8);
                                    Sprite.Image = S[4];
                                }
                                else
                                {
                                    Sprite.ID = S[2].LeftStr(8);
                                    Sprite.Image = S[5];
                                }
                            }
                            else
                            {
                                Sprite.ID = S[2].LeftStr(8);
                                Sprite.Image = S[6];
                            }

                            PartSpriteList.Add(Sprite);

                        }
                    }
                }
            }
        }
        ResetAction = true;
        if (OtherPlayer)
            NewAction = StandType;
        else
        {
            if ((MapleChair.IsUse) || (TamingMob.IsUse))
                NewAction = "sit";
            else
                NewAction = StandType;
        }
        if (InLadder)
        {
            switch (LadderType)
            {
                case LadderType.Ladder:
                    NewAction = "ladder"; break;
                case LadderType.Rope:
                    NewAction = "rope"; break;
            }
        }

    }

    public void RemoveSprites()
    {
        foreach (var Iter in PartSpriteList)
        {

            Iter.Dead();
        }
        PartSpriteList.Clear();
    }
    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Keyboard.GetState();
        //  if (Map.GameMode == GameMode.Viewer)
        //   return;
        EngineFunc.Canvas.GraphicsDevice.SetRenderTarget(AvatarTargetTexture);
        EngineFunc.Canvas.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent);
        RenderTargetFunc();
        EngineFunc.Canvas.GraphicsDevice.SetRenderTarget(null);
        int X1 = FH.X1;
        int Y1 = FH.Y1;
        int X2 = FH.X2;
        int Y2 = FH.Y2;
        if (Keyboard.KeyDown(Input.LeftAlt) && !InLadder && !Attack)
        {
            DoJump = true;
        }

        LadderRope LadderRope = LadderRope.Find(new Vector2(X, Y + OffY), ref OnLadder);
        Vector2 Below = new();
        Foothold BelowFH = null, WallFH = null;
        if (Keyboard.KeyDown(Input.Up))
        {
            OffY = -3;
            if (OnLadder)
            {
                FaceDir = FaceDir.None;
                InLadder = true;
                JumpState = JumpState.jsNone;
                X = LadderRope.X;
                Y -= 1.5f;
            }
            if ((InLadder) && (Y < LadderRope.Y1))
            {
                if (LadderRope.uf == 0)
                    Y = LadderRope.Y1;
                if (LadderRope.uf == 1)
                {
                    Below = FootholdTree.Instance.FindBelow(new Vector2(X, Y - 5), ref BelowFH);
                    Y = Below.Y;
                    FH = BelowFH;
                    Z = FH.Z * 100000 + 60000;
                    InLadder = false;
                    FaceDir = FaceDir.None;
                }
            }
        }

        if (Keyboard.KeyDown(Input.Down) && JumpState == JumpState.jsNone)
        {
            OffY = 0;
            if (OnLadder)
            {
                FaceDir = FaceDir.None;
                InLadder = true;
                JumpState = JumpState.jsNone;
                X = LadderRope.X;
                Y += 1.5f;
            }
            if ((Y > LadderRope.Y2) && (InLadder))
            {
                InLadder = false;
                JumpState = JumpState.jsFalling;

            }
        }
        if (InLadder)
            Z = LadderRope.Page * 100000 + 60000;

        switch (LadderRope.L)
        {
            case 0: LadderType = LadderType.Rope; break;
            case 1: LadderType = LadderType.Ladder; break;
        }

        Portal = MapPortal.Find(new Vector2(X, Y), ref OnPortal);
        if (Keyboard.KeyDown(Input.Up) && (JumpState == JumpState.jsNone) && (OnPortal) && (!Map.FadeScreen.DoFade))
        {
            if (Portal.ToMap != "999999999")
            {
                if ((Portal.PortalType == 2) || (Portal.PortalType == 1))
                {
                    // PlaySounds('Game', 'Portal');
                    CurrentX = X;
                    CurrentPortal = Portal;
                    Map.FadeScreen.DoFade = true;
                    Map.FadeScreen.AlphaCounter = 5;
                    Map.FadeScreen.AValue = 5;
                }
            }
        }

        if (Map.FadeScreen.DoFade)
        {
            JumpState = JumpState.jsNone;
            Map.FadeScreen.AlphaCounter += Map.FadeScreen.AValue;
            if (Map.FadeScreen.AValue == 5)
                X = CurrentX;
            if (Map.FadeScreen.AlphaCounter == 255)
            {

                Map.ID = Portal.ToMap.PadLeft(9, '0');
                if (Map.ID != "")
                    Map.ReLoad = true;
                Map.FadeScreen.AValue = -6;
            }

            if (Map.FadeScreen.AlphaCounter == 249)
            {
                foreach (var NextPortal in MapPortal.PortalList)
                {
                    if (NextPortal.ToMap + CurrentPortal.ToName == NextPortal.ToMap + NextPortal.PortalName)
                    {
                        int PX = NextPortal.X;
                        int PY = NextPortal.Y;
                        Below = FootholdTree.Instance.FindBelow(new Vector2(PX, PY - 5), ref BelowFH);
                        FH = BelowFH;
                        X = PX;
                        Y = PY - 2;
                        if (Pet.Instance != null)
                        {
                            Pet.Instance.X = Game.Player.X;
                            Pet.Instance.Y = Game.Player.Y;
                            Pet.Instance.JumpState = JumpState.jsFalling;
                        }

                        /*
                        if (MonsterFamiliar.MonsterFamiliar != null)
                        {
                            MonsterFamiliar.MonsterFamiliar.X = X;
                            MonsterFamiliar.MonsterFamiliar.Y = Y - 50;
                            MonsterFamiliar.MonsterFamiliar.JumpState = jsFalling;
                        }

                        if (AndroidPlayer != null)
                        {
                            AndroidPlayer.X = Player.X;
                            AndroidPlayer.Y = Player.Y;
                            AndroidPlayer.JumpState = jsFalling;
                        }
                        */

                        Z = FH.Z * 100000 + 60000;
                        EngineFunc.SpriteEngine.Camera.X = PX - DisplaySize.X / 2;
                        EngineFunc.SpriteEngine.Camera.Y = PY - (DisplaySize.Y / 2) - 100;
                        if (EngineFunc.SpriteEngine.Camera.X > Map.Right)
                            EngineFunc.SpriteEngine.Camera.X = Map.Right;
                        if (EngineFunc.SpriteEngine.Camera.X < Map.Left)
                            EngineFunc.SpriteEngine.Camera.X = Map.Left;
                        if (EngineFunc.SpriteEngine.Camera.Y > Map.Bottom)
                            EngineFunc.SpriteEngine.Camera.Y = Map.Bottom;
                        if (EngineFunc.SpriteEngine.Camera.Y < Map.Top)
                            EngineFunc.SpriteEngine.Camera.Y = Map.Top;

                        // Reset := True;
                        break;
                    }
                }
            }
        }

        if (Map.FadeScreen.AlphaCounter <= 2)
            Map.FadeScreen.DoFade = false;
        // Alt + left
        if ((Keyboard.KeyDown(Input.LeftAlt)) && (Keyboard.KeyDown(Input.Left)) && (InLadder))
            DoJump = true;
        // Alt +right
        if ((Keyboard.KeyDown(Input.LeftAlt)) && (Keyboard.KeyDown(Input.Right)) && (InLadder))
            DoJump = true;
        if (JumpState == JumpState.jsJumping)
            InLadder = false;
        // left
        if ((Keyboard.KeyDown(Input.Left)) && (SpeedR == 0))
        {
            FaceDir = FaceDir.Left;
            if (JumpState != JumpState.jsFalling)
            {
                SpeedL += +1.5f;
                if (SpeedL > 2.5f)
                    SpeedL = 2.5f;
            }
        }
        else
        {
            SpeedL -= 0.25f;
            if (SpeedL < 0)
                SpeedL = 0;
        }
        // right
        if ((Keyboard.KeyDown(Input.Right)) && (SpeedL == 0))
        {
            FaceDir = FaceDir.Right;
            if (JumpState != JumpState.jsFalling)
            {
                SpeedR += 1.5f;
                if (SpeedR > 2.5f)
                    SpeedR = 2.5f;
            }
        }
        else
        {
            SpeedR -= 0.25f;
            if (SpeedR < 0)
                SpeedR = 0;
        }

        DestX = DisplaySize.X / 2 - X + Engine.Camera.X;
        if (Math.Abs(DestX) > 1)
            Engine.Camera.X -= DestX * (12f / 800f);

        DestY = (DisplaySize.Y / 2) + (Map.OffsetY + 60) - Y;
        if (Math.Abs(DestY + Engine.Camera.Y) > 1)
            Engine.Camera.Y -= 0.01f * (DestY + Engine.Camera.Y);

        if (Engine.Camera.X < Map.Left)
            Engine.Camera.X = Map.Left;
        if (Engine.Camera.X > Map.Right - DisplaySize.X)
            Engine.Camera.X = Map.Right - DisplaySize.X;
        if (Engine.Camera.Y < Map.Top)
            Engine.Camera.Y = Map.Top;
        if (Engine.Camera.Y > Map.Bottom - DisplaySize.Y)
            Engine.Camera.Y = Map.Bottom - DisplaySize.Y;

        if (Map.Right - Map.Left < DisplaySize.X)
            Engine.Camera.X = Map.Left - ((DisplaySize.X - Map.Info["MapWidth"]) / 2);

        int FallEdge;
        int Direction;
        if ((FaceDir == FaceDir.Left) && (SpeedR == 0) && (!InLadder))
        {
            if ((OnPortal) && (Map.FadeScreen.AValue == 5))
                return;
            if ((Keyboard.KeyDown(Input.LeftAlt)) && (!Attack))
                DoJump = true;
            if ((X < Map.Left + 20) || (Attack) && (JumpState == JumpState.jsNone))
                SpeedL = 0;

            if (!TamingMob.IsUse)
            {
                if ((Action == "prone") || (Action == "proneStab") || (!Skill.PlayEnded))
                    SpeedL = 0;
            }


            Direction = GetAngle256(X2, Y2, X1, Y1);
            if (!FH.IsWall())
            {
                X += (float)(Sin256(Direction) * SpeedL);
                Y -= (float)(Cos256(Direction) * SpeedL);
            }
            FallEdge = -999999;
            if (FH.Prev == null)
                FallEdge = FH.X1 - 10;

            // Wall down
            if ((FH.Prev != null) && (FH.Prev.IsWall()) && (FH.Prev.Y1 > Y))
                FallEdge = FH.X1;
            if ((JumpState == JumpState.jsNone) && (X < FallEdge))
                JumpState = JumpState.jsFalling;
            Below = FootholdTree.Instance.FindBelow(new Vector2(X + 10, Y - 5), ref BelowFH);
            WallFH = FootholdTree.Instance.FindWallR(new Vector2(X + 4, Y - 4));
            if ((WallFH != null) && (X <= WallFH.X1) && (BelowFH.Z == WallFH.Z))
            {
                X = WallFH.X1 + 1;
                SpeedL = 0;
            }
            // walk left
            if ((X <= FH.X1) && (FH.PrevID != 0) && (!FH.IsWall()) && (!FH.Prev.IsWall()))
            {
                if (JumpState == JumpState.jsNone)
                {
                    FH = FH.Prev;
                    X = FH.X2;
                    Y = FH.Y2;
                    Z = FH.Z * 100000 + 60000;
                }
            }
        }

        // walk right
        if ((FaceDir == FaceDir.Right) && (SpeedL == 0) && (!InLadder))
        {
            if ((OnPortal) && (Map.FadeScreen.AValue == 5))
                return;
            if (Keyboard.KeyDown(Input.LeftAlt) && (!Attack))
                DoJump = true;
            if ((X > Map.Right - 20) || (Attack) && (JumpState == JumpState.jsNone))
                SpeedR = 0;

            if (!TamingMob.IsUse)
            {
                if ((Action == "prone") || (Action == "proneStab") || (!Skill.PlayEnded))
                    SpeedR = 0;
            }

            Direction = GetAngle256(X1, Y1, X2, Y2);
            if (!FH.IsWall())
            {
                X += (float)(Sin256(Direction) * SpeedR);
                Y -= (float)(Cos256(Direction) * SpeedR);
            }

            FallEdge = 999999;
            if (FH.Next == null)
                FallEdge = FH.X2 + 5;
            // Wall down
            if ((FH.Next != null) && (FH.Next.IsWall()) && (FH.Next.Y2 > Y))
                FallEdge = FH.X2;

            if ((JumpState == JumpState.jsNone) && (X > FallEdge))
                JumpState = JumpState.jsFalling;
            Below = FootholdTree.Instance.FindBelow(new Vector2(X - 10, Y - 5), ref BelowFH);
            WallFH = FootholdTree.Instance.FindWallL(new Vector2(X - 4, Y - 4));
            if ((WallFH != null) && (X >= WallFH.X1) && (BelowFH.Z == WallFH.Z))
            {
                X = WallFH.X1 - 1;
                SpeedR = 0;
            }
            // walk right
            if ((X >= FH.X2) && (FH.NextID != 0) && (!FH.IsWall()) && (!FH.Next.IsWall()))
            {
                if (JumpState == JumpState.jsNone)
                {
                    FH = FH.Next;
                    X = FH.X1;
                    Y = FH.Y1;
                    Z = FH.Z * 100000 + 60000;
                }
            }
        }

        if ((JumpState == JumpState.jsFalling) && (FallFlag))
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

        if (Keyboard.KeyDown(Input.LeftAlt))
        {
            if ((Keyboard.KeyDown(Input.Down)) && (JumpState == JumpState.jsNone) && (!InLadder))
            {
                FallFlag = false;
                Below = FootholdTree.Instance.FindBelow(new Vector2(X, Y + 4), ref BelowFH);
                if (BelowFH.X2 > BelowFH.X1)
                {
                    if (Below.Y != 99999)
                    {
                        // Y := Y + 6;
                        JumpState = JumpState.jsFalling;
                    }
                }
            }
        }

        if (!FallFlag)
            FallCounter += 1;
        if (FallCounter > VelocityY + 3)
        {
            FallFlag = true;
            FallCounter = 0;
        }
        if (JumpState == JumpState.jsJumping)
        {
            Below = FootholdTree.Instance.FindBelow(new Vector2(X, Y - 10), ref BelowFH);
            if (BelowFH.X2 < BelowFH.X1)
                JumpState = JumpState.jsFalling;
        }

    }

    public override void DoDraw()
    {
        if (!OtherPlayer)
        {
            if (Keyboard.KeyDown(Input.LeftControl))
            {
                Random Random = new();
                switch (Random.Next(10))
                {
                    case int i when i >= 0 && i <= 8:
                        AttackAction = AttackActions[Random.Next(AttackActions.Count)];
                        break;
                    case 9:
                        if (AttackOFs.Count > 0)
                            AttackAction = AttackOFs[Random.Next(AttackOFs.Count)];
                        else
                            AttackAction = AttackActions[Random.Next(AttackActions.Count)];
                        break;
                }
            }

            int WX = (int)MoveX - (int)Engine.Camera.X;
            int WY = (int)MoveY - (int)Engine.Camera.Y;
            Engine.Canvas.Draw(AvatarTargetTexture, WX - 180 - 400, WY - 180 - 400);
        }
    }

    void RenderTargetFunc()
    {
        if (!OtherPlayer)
        {
            AvatarEngine.Draw();
            AvatarEngine.Move(1);
            AvatarEngine.Dead();
        }

    }

}

public class AvatarParts : SpriteEx
{
    public AvatarParts(Sprite Parent) : base(Parent)
    {
        SpriteSheetMode = SpriteSheetMode.NoneSingle;


    }
    int Time;
    int FaceTime;
    float AnimDelay;
    public int Value;
    public string State;
    public string Expression;
    public bool Animate;
    public bool AnimRepeat;
    public string ID;
    public string Image;
    public Player Owner;
    string WpNum;
    int Frame;
    bool DoFaceAnim;
    int FaceCount;
    int AlertCount;
    public int FaceFrame;
    int BlinkCount;
    int BlinkTime;
    int FrameCount;
    int NewFrame = 1;
    bool AnimEnd;
    bool AnimZigzag;
    public bool ChangeFrame;
    Vector2 origin;
    int Flip;
    Vector2 MoveOffset;
    int Counter;
    public static List<string> ZMap = new();

    bool IsAttack()
    {
        if ((State.LeftStr(4) == "stab") || (State.LeftStr(5) == "swing") || (State.LeftStr(5) == "shoot"))
            return true;
        else
            return false;

    }
    public void UpdateFrame()
    {
        string C = "Character/";
        float BodyDelay, FaceDelay;
        PartName Part = Equip.GetPart(ID);

        if ((State == "stand1") || (State == "stand2") || (State == "alert"))
            AnimZigzag = true;
        else
            AnimZigzag = false;

        if (/*(!AvatarForm.SaveSingleFrame) && */((Part == PartName.Weapon) || (Part == PartName.CashWeapon)) && (Time == 0))
        {
            string AfterImagePath = "Character/Afterimage/" + Owner.AfterImageStr + ".img/0/" + State + "/" + Frame + "/0";
            if (Wz.HasDataE(AfterImagePath))
            {
                //PlaySounds('Weapon', 'swordL/Attack');
                AfterImage.Create(AfterImagePath);
            }
        }

        if ((Image == "head") && (Time == 0))
            ChangeFrame = true;

        if (Wz.HasDataE("Character/00002000.img/" + State + "/" + Frame + "/move"))
        { 
            MoveOffset = WzDict.GetVectorE("Character/00002000.img/" + State + "/" + Frame + "/move");
        }
        else
        {
            if ((MapleChair.IsUse) && (!Owner.OtherPlayer))
            {
                MoveOffset.X = MapleChair.BodyRelMove.X;
                MoveOffset.Y = MapleChair.BodyRelMove.Y;
            }
            else
            {
                MoveOffset.X = 0;
                MoveOffset.Y = 0;
            }
        }

        if (FlipX)
            Owner.MoveX = Owner.X - 1 - MoveOffset.X;
        else
            Owner.MoveX = Owner.X - 1 + MoveOffset.X;
        Owner.MoveY = Owner.Y + MoveOffset.Y;
        if (!Animate)
        {
            if (NewFrame <= FrameCount)
                Frame = NewFrame;
        }
        /*
        if (AvatarForm.SaveAllFrames && (!Owner.OtherPlayer))
        {
            float WX = (Player.X - SpriteEngine.WorldX - 155) + AvatarForm.TrackBarX.Position;
            float WY = (Player.Y - SpriteEngine.WorldY - 160) + AvatarForm.TrackBarY.Position;
            Animate = false;
            Time = 0;
            BodyDelay = 0;
            Animend = false;
            TTimers.DoTick(50, 'aaa',
              procedure
                  begin
    
                Inc(AvatarForm.Frame);
            ForceDirectories(ExtractFilePath(ParamStr(0)) + 'Export');
            var FileName := ExtractFilePath(ParamStr(0)) + 'Export\' + AvatarForm.AllFrames[AvatarForm.Frame - 1] + '.png';
        AvatarForm.Label2.Caption := 'Save to:  ' + FileName;
            AvatarPanelTexture.SaveToFile(FileName, nil, 0, IntRectBDS(WX, WY, WX + AvatarForm.TrackBarW.Position, WY + AvatarForm.TrackBarH.Position));
            end);
            var S = AvatarForm.AllFrames[AvatarForm.Frame].Split(["."]);
            State = S[0];
            Frame = S[1].ToInt();
        }
        */
        /*
        if (AvatarForm.SaveSingleFrame) and(not Owner.OtherPlayer) then
  begin
    Animate= false;
    Time= 0;
    BodyDelay= 0;
    Animend= false;
        var Index := AvatarForm.AllFrameListBox.ItemIndex;
        var S := AvatarForm.AllFrameListBox.Items[Index].Split(['.']);
    State:= S[0];
    Frame:= S[1].ToInteger;
        end;

        if PlayActionForm.DoPlay then
        begin
    Animend= false;
    Time= 0;
    Frame= 0;
    State= PlayActionForm.ListBox1.Items[PlayActionForm.ListBox1.ItemIndex];
        Inc(PlayActionCounter);
        end;

        if AvatarForm.ChangeExpressionListBox then
        begin
    FaceFrame= 0;
    Expression:= AvatarForm.ExpressionListBox.Text;
        Inc(ChangeExpressionCounter);
        end;
        */

        if (Owner.ResetAction)
        {
            Frame = 1;
            State = Owner.NewAction;
            Counter += 1; ;
        }

        if (!Wz.HasDataE(C + Equip.GetDir(ID) + ID + ".img/" + WpNum + State + "/" + Frame + "/" + Image) && (!IsAttack()) && (!Equip.DataS.ContainsKey(State + "/" + Frame)))
            Frame = 0;
        FrameCount = Equip.Data["body/" + State + "/FrameCount"];
        BodyDelay = Equip.Data["body/" + State + "/" + Frame + "/delay"];

        int FaceFrameCount = Equip.Data["face/" + Expression + "/FrameCount"];
        FaceDelay = Equip.Data["face/" + Expression + "/" + FaceFrame + "/delay"];

        string Directory = Equip.GetDir(ID);
        string Path;
        if ((Image != "face") && (Equip.GetPart(ID) != PartName.FaceAcc))
        {
            if (Part == PartName.CashWeapon)
                WpNum = Owner.WeaponNum + "/";
            else
                WpNum = "";
            if (Equip.DataS.ContainsKey(State + "/" + Frame))
                Path = C + Directory + ID + ".img/" + WpNum + Equip.DataS[State + "/" + Frame] + "/" + Image;
            else
                Path = C + Directory + ID + ".img/" + WpNum + State + "/" + Frame + "/" + Image;
        }
        else
        {
            Path = C + Directory + ID + ".img/" + Expression + "/" + FaceFrame + "/" + Image;
        }


        if ((Image != "face") && (Equip.GetPart(ID) != PartName.FaceAcc))
        {
            if (Wz.HasDataE(Path))
            {
                ImageNode = Wz.EquipData[Path];
                Alpha = 255;
            }
            else
                Alpha = 0;
        }
        else if ((Image == "face") || (Part == PartName.FaceAcc))
        {
            if (Wz.HasDataE(Path))
            {
                ImageNode = Wz.EquipData[Path];
                // Visible := True;
                Alpha = 255;
            }
            else
                Alpha = 0;
        }

        if ((Image == "face") || (Part == PartName.Glass) || (Part == PartName.FaceAcc))
        {
            if ((State == "ladder") || (State == "rope") || ((State == "swingOF") && (Frame == 1)) || ((State == "swingTF") && (Frame == 0)))
                Alpha = 0;
            else
                Alpha = 255;
        }

        if (!Owner.DressCap)
        {
            if (Part == PartName.Cap)
                Visible = false;

            if (Owner.ShowHair)
            {
                //   if (Image = 'hairOverHead') or (Image = 'backHair') then
                //   Visible := True;
                if (Part == PartName.Hair)
                    Visible = true;
            }
        }




        if ((Owner.DressCap) && (Owner.ShowHair))
        {

            if (Part == PartName.Hair)
            {
                switch (Owner.CapType)
                {
                    case 0:
                        Visible = true;
                        break;
                    case 1:
                        if ((Image == "hairOverHead") || (Image == "backHair"))
                            Visible = false;
                        break;
                    case 2:
                        if ((Image == "hairOverHead") || (Image == "backHair") || (Image == "hairBelowBody") || (Image == "backHairBelowCap"))
                            Visible = false;
                        break;
                    case 3:
                        Visible = false;
                        break;
                }
            }
        }


        if ((Image == "ear") || (Image == "lefEar") || (Image == "highlefEar"))
            Alpha = 0;

        if (Wz.HasDataE(Path + "/z"))
        {
            if (!Owner.OtherPlayer)
                Z = 100 + Owner.Z - ZMap.IndexOf(Wz.EquipData[Path + "/z"].ToStr());
            else
                Z = 100 + (200 * Owner.NewZ) + Owner.Z - ZMap.IndexOf(Wz.EquipData[Path + "/z"].ToStr());
        }
        if (Animate)
            Time += 17;
        if (Time > BodyDelay)
        {
            if (Counter > 50)
            {
                Owner.ResetAction = false;
                Counter = 0;
            }
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
                    {
                        Frame = 0;
                    }
                    else
                    {
                        Frame -= 1;
                        AnimEnd = true;
                    }
                }
            }
        }

        if (Expression != "blink")
        {
            FaceCount += 1;
            if (FaceCount >= 550)
            {
                Expression = "blink";
                FaceCount = 0;
            }
        }
        BlinkCount += 1;
        if (BlinkCount >= 220)
        {
            DoFaceAnim = true;
            BlinkCount = 0;
        }

        Random Random = new Random();
        int BlinkNum = -1;
        switch (Random.Next(1500))
        {
            case 100: BlinkNum = 1; break;
            case 500: BlinkNum = 2; break;
        }
        if ((DoFaceAnim) && (Expression != "oops"))
            FaceTime += 17;
        if (FaceTime > FaceDelay)
        {
            FaceFrame += 1;
            if (FaceFrame > FaceFrameCount)
            {
                FaceFrame = 0;
                BlinkTime += 1;
                if (BlinkTime >= BlinkNum)
                {
                    DoFaceAnim = false;
                    BlinkTime = 0;
                }
            }
            FaceTime = 0;
        }
        string SkillAction;
        if (Equip.DataS.ContainsKey(State + "/" + Frame))
        {  
            SkillAction = Equip.DataS[State + "/" + Frame];
            if ((SkillAction == "hide/0") || (SkillAction == "blink/0"))
                Alpha = 0;
            if ((Image == "face") || (Part == PartName.Glass) || (Part == PartName.FaceAcc))
            {
                if ((SkillAction == "swingOF/1") || (SkillAction == "swingTF/0"))
                    Alpha = 0;
            }
        }

        if ((Part == PartName.Weapon) && (ID.LeftStr(5) == "01212"))
        {
            if ((Image == "weapon") || (Image == "weapon1") || (Image == "weapon2"))
                Visible = false;
        }

        int AdjX;
        if (Wz.HasDataE(Path + "/origin"))
        {
            switch (FlipX)
            {
                case true:
                    this.Flip = -1;
                    if (Owner.InLadder)
                        AdjX = 3;
                    else
                        AdjX = 0;
                    origin.X = Wz.EquipData[Path + "/origin"].ToVector().X - this.ImageWidth + AdjX;
                    break;
                case false:
                    this.Flip = 1;
                    origin.X = -Wz.EquipData[Path + "/origin"].ToVector().X;
                    break;
            }
            origin.Y = -Wz.EquipData[Path + "/origin"].ToVector().Y;
        }

        if (Owner.OtherPlayer)
        {
            Owner.TamingNavel.X = 0;
            Owner.TamingNavel.Y = 0;
        }
        else
        {
            Owner.TamingNavel.X = TamingMob.Navel.X;
            Owner.TamingNavel.Y = TamingMob.Navel.Y;
        }

        if (Wz.HasDataE(Path + "/map/brow"))
        {
            Owner.Brow.X = -Wz.EquipData[Path + "/map/brow"].ToVector().X * this.Flip;
            Owner.Brow.Y = -Wz.EquipData[Path + "/map/brow"].ToVector().Y;
            if (Image == "head")
                Owner.HeadBrow = Owner.Brow;
            this.Offset.X = origin.X + Owner.HeadNeck.X - Owner.BodyNeck.X - Owner.HeadBrow.X + Owner.Brow.X - Owner.TamingNavel.X;
            this.Offset.Y = origin.Y + Owner.HeadNeck.Y - Owner.BodyNeck.Y - Owner.HeadBrow.Y + Owner.Brow.Y - Owner.TamingNavel.Y;
        }

        if (Wz.HasDataE(Path + "/map/neck"))
        {
            Owner.Neck.X = -Wz.EquipData[Path + "/map/neck"].ToVector().X * this.Flip;
            Owner.Neck.Y = -Wz.EquipData[Path + "/map/neck"].ToVector().Y;
            if (Image == "body")
                Owner.BodyNeck = Owner.Neck;
            if (Image == "head")
                Owner.HeadNeck = Owner.Neck;
        }

        if (Image == "body")
            Owner.BrowPos = Owner.BodyNeck + TamingMob.Navel;
        if (Wz.HasDataE(Path + "/map/hand"))
        {
            Owner.Hand.X = -Wz.EquipData[Path + "/map/hand"].ToVector().X * this.Flip;
            Owner.Hand.Y = -Wz.EquipData[Path + "/map/hand"].ToVector().Y;
            if (Image == "arm")
                Owner.ArmHand = Owner.Hand;
            if (Image == "body")
                Owner.BodyHand = Owner.Hand;
            this.Offset.X = origin.X + Owner.Hand.X + Owner.ArmNavel.X - Owner.ArmHand.X - Owner.BodyNavel.X;
            this.Offset.Y = origin.Y + Owner.Hand.Y + Owner.ArmNavel.Y - Owner.ArmHand.Y - Owner.BodyNavel.Y;
        }

        if (Wz.HasDataE(Path + "/map/handMove"))
        {
            Owner.HandMove.X = -Wz.EquipData[Path + "/map/handMove"].ToVector().X * this.Flip;
            Owner.HandMove.Y = -Wz.EquipData[Path + "/map/handMove"].ToVector().Y;
            if (Image == "lHand")
                Owner.lHandMove = Owner.HandMove;
            this.Offset.X = origin.X + Owner.HandMove.X - Owner.lHandMove.X;
            this.Offset.Y = origin.Y + Owner.HandMove.Y - Owner.lHandMove.Y;
        }

        if (Wz.HasDataE(Path + "/map/navel"))
        {
            Owner.Navel.X = -Wz.EquipData[Path + "/map/navel"].ToVector().X * this.Flip;
            Owner.Navel.Y = -Wz.EquipData[Path + "/map/navel"].ToVector().Y;
            if (Image == "arm")
                Owner.ArmNavel = Owner.Navel;
            if (Image == "body")
                Owner.BodyNavel = Owner.Navel;
            this.Offset.X = origin.X + Owner.Navel.X - Owner.BodyNavel.X - Owner.TamingNavel.X;
            this.Offset.Y = origin.Y + Owner.Navel.Y - Owner.BodyNavel.Y - Owner.TamingNavel.Y;
        }


    }

    bool IsSkillAttack()
    {
        if (Equip.DataS.ContainsKey(Skill.ID + "/action") && (Owner.Action == Equip.DataS[Skill.ID + "/action"]))
            return true;
        else
            return false;
    }

    bool ArrowKeyDown()
    {
        if ((!Keyboard.KeyDown(Input.Left)) && (!Keyboard.KeyDown(Input.Right)) && (!Keyboard.KeyDown(Input.Up)) && (!Keyboard.KeyDown(Input.Down)))
            return false;
        else
            return true;
    }

    public override void DoMove(float Delta)
    {


        //  if (Map.GameMode == GameMode.Viewer)
        //    return;

        if (Morph.IsUse)
        {
            Owner.MoveX = -99999;
            return;
        }

        Owner.Attack = IsAttack();
        Owner.Action = State;
        PartName Part = Equip.GetPart(ID);

        if (TamingMob.IsUse)
        {
            Owner.Attack = false;
            if (State != "fly")
                Frame = 0;
            if (TamingMob.CharacterAction == "StabT2")
                TamingMob.CharacterAction = "stabT2";
            if (TamingMob.CharacterAction != "hideBody")
                State = TamingMob.CharacterAction;
            // if (Part = Weapon) or (Part = CashWeapon) then
            // Exit;
        }

        if (((Keyboard.KeyDown(Input.Left)) || (Keyboard.KeyDown(Input.Right))) && (!TamingMob.IsUse))
        {
            if ((State.LeftStr(4) != "walk") && (Owner.JumpState == JumpState.jsNone) && (!Owner.InLadder) && (!IsAttack()) && (Skill.PlayEnded))
            {
                Time = 0;
                Frame = 0;
                State = Owner.WalkType;
            }
        }

        if ((Keyboard.KeyUp(Input.Left)) || (Keyboard.KeyUp(Input.Right)))
        {
            if ((!Owner.InLadder) && (Owner.JumpState == JumpState.jsNone) && (!IsAttack()) && (Skill.PlayEnded))
            {
                Frame = 0;
                State = Owner.StandType;
            }
        }

        if ((Owner.JumpState != JumpState.jsNone) && (!IsAttack()) && (!TamingMob.IsUse))
        {
            Frame = 0;
            State = "jump";
        }
        // jump ->re stand
        if ((Owner.JumpState == JumpState.jsNone) && (State == "jump") && (!Keyboard.KeyDown(Input.LeftAlt)))
            State = Owner.StandType;
        // press jump+ left(right) key
        if ((Keyboard.KeyDown(Input.LeftAlt)) && (!IsAttack()) && (!TamingMob.IsUse))
        {
            if ((Keyboard.KeyDown(Input.Left)) || (Keyboard.KeyDown(Input.Right)))
            {
                Frame = 0;
                State = "jump";
            }
        }

        if ((!Owner.InLadder) && (Owner.JumpState == JumpState.jsNone) && (!TamingMob.IsUse))
        {
            if ((!IsAttack()) && (Skill.PlayEnded))
            {
                if ((Keyboard.KeyDown(Input.Down)) && (!Keyboard.KeyDown(Input.LeftControl)) && (State != "proneStab"))
                    State = "prone";
                if ((Keyboard.KeyDown(Input.LeftControl)) && (State != "proneStab") && (Skill.PlayEnded))
                {
                    Skill.Attacking = false;
                    AnimEnd = false;
                    Frame = 0;
                    Time = 0;
                    State = "proneStab";
                }
            }
            if ((Keyboard.KeyUp(Input.Down)) && (Skill.PlayEnded))
                State = Owner.StandType;
        }

        if (!Owner.InLadder)
        {
            if ((State == "rope") || (State == "ladder"))
            {
                Frame = 0;
                State = Owner.StandType;
            }
        }

        if (Owner.InLadder)
        {
            switch (Owner.LadderType)
            {
                case LadderType.Ladder:
                    State = "ladder";
                    break;
                case LadderType.Rope:
                    State = "rope";
                    break;
            }
        }


        if ((IsAttack()) || (State == "proneStab") || (IsSkillAttack()) /*|| (PlayActionForm.Playing)*/)
            AnimRepeat = false;
        else
            AnimRepeat = true;


        if (AnimEnd)
        {
            if ((IsSkillAttack()) || (IsAttack()) /*||(PlayActionForm.Playing) */)
            {
                Value = 1;
                Time = 0;
                Frame = 0;
                State = "alert";
                AlertCount = 0;
                Skill.Start = false;
            }
            if (State == "proneStab")
            {
                Time = 0;
                Frame = 0;
                State = "prone";
            }
        }

        AlertCount += 1;
        if ((AlertCount > 300) && (State == "alert"))
        {
            // FTime := 0;
            Frame = 1;
            State = Owner.StandType;
            AlertCount = 0;
        }

        if ((Keyboard.KeyDown(Input.LeftControl)) && (!Keyboard.KeyDown(Input.Down)) && (!IsAttack()) && (!Owner.InLadder) && (Skill.PlayEnded) && (!TamingMob.IsUse))
        {
            Skill.Attacking = false;
            AnimEnd = false;
            Frame = 0;
            Time = 0;
            State = Owner.AttackAction;
        }

        if ((Skill.Start) && (!Skill.PlayEnded))
        {
            if (Equip.DataS.ContainsKey(Skill.ID + "/action"))
            { 
                if (State != Equip.DataS[Skill.ID + "/action"])
                { 
                    AnimEnd = false;
                    Frame = 0;
                    Time = 0;
                    State = Equip.DataS[Skill.ID + "/action"];
                }
            }
        }

        if (Keyboard.KeyDown(Input.F1))
        {
            FaceFrame = 0;
            Expression = "hit";
        }
        if (Keyboard.KeyDown(Input.F2))
        {
            FaceFrame = 0;
            Expression = "smile";
        }
        if (Keyboard.KeyDown(Input.F3))
        {
            FaceFrame = 0;
            Expression = "troubled";
        }
        if (Keyboard.KeyDown(Input.F4))
        {
            FaceFrame = 0;
            Expression = "cry";
        }
        if (Keyboard.KeyDown(Input.F5))
        {
            FaceFrame = 0;
            Expression = "angry";
        }
        if (Keyboard.KeyDown(Input.F6))
        {
            FaceFrame = 0;
            Expression = "bewildered";
        }
        if (Keyboard.KeyDown(Input.F7))
        {
            FaceFrame = 0;
            Expression = "stunned";
        }

        // MirrorX := NewFlip;
        if ((!Owner.InLadder) && (!IsAttack()) && (Skill.PlayEnded))
        {
            if (Keyboard.KeyDown(Input.Left))
            {
                FlipX = false;
                Owner.FlipX = false;
            }
            if (Keyboard.KeyDown(Input.Right))
            {
                FlipX = true;
                Owner.FlipX = true;
            }
        }

        if (Owner.InLadder)
        {
            if ((Keyboard.KeyUp(Input.Up)) || (Keyboard.KeyUp(Input.Down)))
                Animate = false;
            if ((Keyboard.KeyDown(Input.Up)) || (Keyboard.KeyDown(Input.Down)))
                Animate = true;
        }
        else
        {
            Animate = true;
        }

        if ((TamingMob.IsUse) || (MapleChair.IsUse))
        {
            if ((Part == PartName.Weapon) || (Part == PartName.CashWeapon))
                Visible = false;
        }
        else
        {
            Visible = true;
        }

        //Game.Player.FlipX = FlipX;
        if ((State == "ladder") || (State == "rope"))
            MapleChair.CanUse = false;
        else
            MapleChair.CanUse = true;

        if (MapleChair.IsUse)
        {
            State = MapleChair.CharacterAction;
        }
        UpdateFrame();

    }


    public override void DoDraw()
    {
        if (ImageNode == null)
            return;
        /*
        if ((AvatarForm.SaveAllFrames) && (AvatarForm.Frame = 96))
            AvatarForm.SaveAllFrames = false;
        if (AvatarForm.ChangeExpressionListBox)
        {
            if (ChangeExpressionCounter > 5)
            {
                ChangeExpressionCounter = 0;
                AvatarForm.ChangeExpressionListBox = false;
            }
        }

        if (PlayActionForm.DoPlay)
        {
            if (PlayActionCounter > 2)
            {
                PlayActionCounter = 0;
                PlayActionForm.DoPlay = false;
            }
        }
        */

        // if (Map.GameMode == GameMode.Viewer)
        //  return;


        if (ChangeFrame)
            ChangeFrame = false;
        if (Map.ShowChar)
            base.DoDraw();
        if (Visible)
            Moved = true;


    }


}


