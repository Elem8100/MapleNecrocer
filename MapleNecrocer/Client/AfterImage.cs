using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using MonoGame.SpriteEngine;
using WzComparerR2.WzLib;
namespace MapleNecrocer;

public class AfterImage : SpriteEx
{
    public AfterImage(Sprite Parent) : base(Parent)
    {
        CollideMode = CollideMode.Rect;
        CanCollision = true;
    }

    int FTime;
    int Frame;
    string ImagePath;
    string Path;
    Wz_Vector LT, RB;
    int Left, Top, Right, Bottom;
    public static int ColorInt;
    public static void Load(string AfterImageStr, string ColorType)
    {
        var Entry = Wz.GetNodeA("Character/Afterimage/" + AfterImageStr + ".img/" + ColorType);
        if (!Wz.EquipData.ContainsKey(Entry.FullPathToFile2()))
            Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib);
    }
    public static void Create(string PathName)
    {
        var AfterImage = new AfterImage(EngineFunc.SpriteEngine);
        AfterImage.ImageLib = Wz.EquipImageLib;
        AfterImage.ImageNode = Wz.EquipData[PathName];
        AfterImage.Visible = true;
        AfterImage.Path = PathName.LeftStr(PathName.Length - 1);
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Visible = true;
        X = Game.Player.X - 10;
        Y = Game.Player.Y;
        Z = 150 + Game.Player.Z;
        ImagePath = Path + Frame;
        if (!Wz.EquipData.ContainsKey(ImagePath))
            return;
        ImageNode = Wz.EquipData[ImagePath];
        int Delay = ImageNode.GetInt("delay", 100);
        int a1 = ImageNode.GetInt("a1", -1);
        FlipX = Game.Player.FlipX;
        string c = "Character/Afterimage/";
        if (Wz.EquipData.ContainsKey(c + Game.Player.AfterImageStr + ".img/0/" + Game.Player.Action + "/lt"))
        {
            LT = Wz.EquipData[c + Game.Player.AfterImageStr + ".img/0/" + Game.Player.Action + "/lt"].ToVector();
            RB = Wz.EquipData[c + Game.Player.AfterImageStr + ".img/0/" + Game.Player.Action + "/rb"].ToVector();
            switch (Game.Player.FlipX)
            {
                case true:
                    Right = (int)X - LT.X + 18;
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
        CollideRect = CollideRect = SpriteUtils.Rect(Left, Top, Right, Bottom);
        Collision();

        FTime += 17;
        if (FTime > Delay)
        {
            Frame += 1;
            if (!Wz.EquipData.ContainsKey(Path + Frame))
                Dead();
            FTime = 0;
        }

        if (a1 != -1)
        {
            float AniAlpha = 255 - (255 - a1) * FTime / Delay;
            if ((AniAlpha < 255) && (AniAlpha > 0))
                Alpha = (byte)AniAlpha;
            if (Alpha <= 20)
                Dead();
        }
        switch (FlipX)
        {
            case true:
                Offset.X = ImageNode.GetVector("origin").X - ImageWidth + 18;
                break;
            case false:
                Offset.X = -ImageNode.GetVector("origin").X;
                break;
        }
        Offset.Y = -ImageNode.GetVector("origin").Y;
    }

    public override void OnCollision(Sprite sprite)
    {
        if ((FTime == 0) && (Frame == 0))
        {
            if (sprite is Mob)
            {
                var Mob = (Mob)sprite;
                if (Mob.HP > 0)
                {
                    Mob.Hit = true;
                    Random Random = new Random();
                    Game.Damage = 50000 + Random.Next(700000);
                    Mob.HP -= Game.Damage;

                    if (Wz.GetNode("Sound/Mob.img/" + Mob.ID + "/Damage") != null)
                        Sound.Play("Sound/Mob.img/" + Mob.ID + "/Damage");
                    else if (Wz.GetNode("Sound/Mob.img/" + Mob.ID + "/Hit1") != null)
                        Sound.Play("Sound/Mob.img/" + Mob.ID + "/Hit1");
                   
                    if (Wz.Data.ContainsKey("Mob/" + Mob.ID + ".img/hit1"))
                    {
                        Mob.GetHit1 = true;
                    }
                }
                if ((Mob.HP <= 0) && (!Mob.Die))
                {
                    Sound.Play("Sound/Mob.img/" + Mob.ID + "/Die");
                    Mob.Die = true;
                    Mob.CanCollision = false;
                    // Dead;
                }
                CanCollision = false;
            }

        }
    }
}