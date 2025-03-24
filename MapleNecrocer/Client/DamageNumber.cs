using DevComponents.AdvTree;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.CharaSim;
using WzComparerR2.WzLib;

namespace MapleNecrocer;

public class DamageNumber : SpriteEx
{
    public DamageNumber(Sprite Parent) : base(Parent)
    {

    }
    int Number;
    int Counter;
    byte Alpha;
    string LargeNumber, SmallNumber;
    public static string Style;
    public static bool UseNewDamage;
    static bool Loaded;
    public static void Load(string Num)
    {
        string[] StyleList = { "NoBlue0", "NoBlue1", "NoCri0", "NoCri1", "NoRed0", "NoRed1", "NoViolet0", "NoViolet1" };
        var Entry = Wz.GetNode("Effect/BasicEff.img");
        if (UseNewDamage)
        {
            //num=1/NoRed1
            if (Wz.GetNode("Effect/DamageSkin.img/" + Num) != null)
            {
                var Split = Num.Split("/");
                Wz.DumpData(Wz.GetNode("Effect/DamageSkin.img/" + Split[0]), Wz.EquipData, Wz.EquipImageLib);
            }
            else if (Wz.GetNode("Etc/DamageSkin.img/" + Num) != null)
            {
                var Split = Num.Split("/");
                Wz.DumpData(Wz.GetNode("Etc/DamageSkin.img/" + Split[0]), Wz.EquipData, Wz.EquipImageLib);
            }
            else
            {
                Wz.DumpData(Entry.GetNode("damageSkin/" + Num), Wz.EquipData, Wz.EquipImageLib);
            }
        }
        else
        {
            for (int I = 0; I <= 7; I++)
            {
                if (Wz.HasNode("Effect/BasicEff.img/" + StyleList[I]))
                    Wz.DumpData(Entry.Nodes[StyleList[I]], Wz.EquipData, Wz.EquipImageLib);
            }
        }

        if (!Loaded)
        {
            if (!Wz.HasNode("Effect/BasicEff.img"))
            {
                DamageNumber.UseNewDamage = true;
                if (Wz.HasNode("Effect/DamageSkin.img/16"))
                {
                    DamageNumber.Style = "16/NoCri1";
                    Wz.DumpData(Wz.GetNode("Effect/DamageSkin.img/16/NoCri0"), Wz.EquipData, Wz.EquipImageLib);
                    Wz.DumpData(Wz.GetNode("Effect/DamageSkin.img/16/NoCri1"), Wz.EquipData, Wz.EquipImageLib);
                }
                if (Wz.HasNode("Etc/DamageSkin.img/16"))
                {
                    DamageNumber.Style = "16/effect/NoCri1";
                    Wz.DumpData(Wz.GetNode("Etc/DamageSkin.img/16/effect/NoCri0"), Wz.EquipData, Wz.EquipImageLib);
                    Wz.DumpData(Wz.GetNode("Etc/DamageSkin.img/16/effect/NoCri1"), Wz.EquipData, Wz.EquipImageLib);
                }
            }
            Loaded = true;
        }

    }

    public static void Create(int ANumber, int AX, int AY)
    {
        int Len = ANumber.ToString().Length;
        int Mid = (Len * 28) / 2;
        var DamageNumber = new DamageNumber(EngineFunc.SpriteEngine);

        if (UseNewDamage)
        {
            var Len2 = Style.Length - 1;
            DamageNumber.LargeNumber = Style.LeftStr(Len2) + "1";
            DamageNumber.SmallNumber = Style.LeftStr(Len2) + "0";
        }
        else
        {
            DamageNumber.LargeNumber = Style.LeftStr(5) + "1";
            DamageNumber.SmallNumber = Style.LeftStr(5) + "0";
        }
        DamageNumber.Number = ANumber;
        DamageNumber.X = AX - Mid / 2;
        DamageNumber.Y = AY;
        DamageNumber.Z = Game.Player.Z;
        DamageNumber.Alpha = 255;
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Y -= 0.5f;
        Counter += 1;
        if (Counter > 50)
            Alpha -= 6;
        if (Alpha < 10)
            Dead();
    }

    public override void DoDraw()
    {
        int W, OffY;
        for (int I = 0; I < Number.ToString().Length; I++)
        {
            var Char = Number.ToString().Substring(I, 1);
            if (UseNewDamage)
            {
                //style='1/NoRed1' or
                //style=1/effect/Nored1/
                W = 29;
                if (Wz.EquipData.ContainsKey("Effect/DamageSkin.img/" + Style + "/" + Char))
                {

                    if (I == 0)
                        ImageNode = Wz.EquipData["Effect/DamageSkin.img/" + LargeNumber + "/" + Char];
                    else
                        ImageNode = Wz.EquipData["Effect/DamageSkin.img/" + SmallNumber + "/" + Char];
                }
                else if (Wz.EquipData.ContainsKey("Etc/DamageSkin.img/" + Style + "/" + Char))
                {

                    if (I == 0)
                        ImageNode = Wz.EquipData["Etc/DamageSkin.img/" + LargeNumber + "/" + Char];
                    else
                        ImageNode = Wz.EquipData["Etc/DamageSkin.img/" + SmallNumber + "/" + Char];
                }
                else
                {
                    if (I == 0)
                        ImageNode = Wz.EquipData["Effect/BasicEff.img/damageSkin/" + LargeNumber + "/" + Char];
                    else
                        ImageNode = Wz.EquipData["Effect/BasicEff.img/damageSkin/" + SmallNumber + "/" + Char];
                }
            }
            else
            {
                W = 20;
                if (I == 0)
                    ImageNode = Wz.EquipData["Effect/BasicEff.img/" + LargeNumber + "/" + Char];
                else
                    ImageNode = Wz.EquipData["Effect/BasicEff.img/" + SmallNumber + "/" + Char];
            }
            if ((I % 2) == 0)
                OffY = 0;
            else
                OffY = -5;
            EngineFunc.Canvas.DrawColor(Wz.EquipImageLib[ImageNode], (int)X + I * W - ImageNode.Nodes["origin"].ToVector().X
              - (int)Engine.Camera.X, (int)Y - ImageNode.Nodes["origin"].ToVector().Y - (int)Engine.Camera.Y + OffY, 255, 255, 255, Alpha);

        }
    }
}

