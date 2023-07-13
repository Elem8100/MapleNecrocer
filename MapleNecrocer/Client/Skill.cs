using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Input = Microsoft.Xna.Framework.Input.Keys;
using WzComparerR2.WzLib;
using WzComparerR2.CharaSim;
using System.Drawing;
using static MapleNecrocer.Skill;
using Microsoft.Xna.Framework;

namespace MapleNecrocer;

public class Skill : SpriteEx
{
    public Skill(Sprite Parent) : base(Parent)
    {
    }
    public static Dictionary<Input, string> HotKeyList = new();
    public static bool PlayEnded;
    public static string ID;
    public static bool Attacking;
    public static bool Start;
    public static Dictionary<string, string> AllIDs = new();
    public static List<string> LoadedList = new();
    //  if(Keyboard.KeyDown((Input) Enum.Parse(typeof(Input), "X", true) )  )
    public static bool MultiStrike;
    public static int DamageWaitTime;
    public static int TotalTime;
    static Wz_Node Entry;
    static string GetJobImg(string ID)
    {
        if (ID.LeftStr(3) == "800" && ID.Length == 8)
            return (ID.ToInt() / 100).ToString();
        else
            return (ID.ToInt() / 10000).ToString();
    }
    public static void Load(string ID)
    {
        Wz_Node Entry = null;
        if (Wz.HasNode("Skill/" + GetJobImg(ID) + ".img"))
            Entry = Wz.GetNode("Skill/" + GetJobImg(ID) + ".img/skill/" + ID);
        Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib);
        foreach (var Iter in Entry.Nodes)
        {
            if (Iter.Text == "action")
                Equip.DataS.AddOrReplace(ID + "/action", Iter.GetNode("0").ToStr("alert"));
            if (Iter.Text == "tile")
                Equip.Data.AddOrReplace(ID + "/tileCount", Iter.Nodes.Count - 1);
        }
    }

    static int GetDamageWaitTime(string ID)
    {
        switch (ID.ToInt())
        {
            case 1311012:
            case 4341052:
            case 5101004:
            case 65121008:
            case 400040006:
                return 5;
                break;
            case 61111101:
            case 142121031:
            case 400041024:
            case 5221026:
            case 101120202:
                return 10;
                break;
            case 1321012:
            case 2211010:
            case 2301005:
            case 15111022:
            case 25121005:
            case 101100100:
            case 101110202:
            case 101110203:
            case 155111211:
            case 400051042:
                return 15;
                break;
            case 3121015:
            case 11101008:
            case 31221002:
            case 41121017:
                return 20;
                break;
            case 15121002:
            case 1121008:
            case 1001005:
            case 1121015:
            case 2211002:
            case 5121017:
            case 61121104:
            case 61121105:
            case 65111100:
            case 41121018:
            case 65121100:
                return 25;
                break;
            case 5321000:
            case 25121007:
            case 400041021:
                return 30;
                break;
            case 2221007:
            case 65121002:
                return 35;

            case 5121016:
            case 5121052:
            case 15121052:
            case 101120102:
            case 36121011:
            case 400021002:
                return 40;
                break;
            case 5221052:
            case 27111101:
            case 32121004:
                return 45;

            case 27111303:
            case 27121202:
                return 50;
                break;
            case 31221052:
                return 55;

            case 2321008:
            case 2121007:
            case 41121052:
            case 400001014:
                return 70;

            case 61121052:
                return 85;

            case 5121013:
                return 95;

            case 4341011:
                return 120;

            case 36121052:
                return 160;
            default: return 15;
        }
    }

    public static void Create(string ID)
    {
        string[] Effects = {"effect", "effect0", "effect1", "effect2","effect3",
                            "screen", "screen0", "ball", "keydown", "keydown0", "keydowned"};
        Skill.ID = ID;
        Skill.PlayEnded = false;
        //PlaySounds('Skill', ID + '/Use');
        Wz_Node Entry = null;
        if (Wz.HasNode("Skill/" + GetJobImg(ID) + ".img"))
            Entry = Wz.GetNode("Skill/" + GetJobImg(ID) + ".img/skill/" + ID);
        Skill.Entry = Entry;

        switch (ID)
        {
            case "1121008":
            case "1001005":
            case "1311012":
                Skill.MultiStrike = false;
                break;
            default:
                Skill.MultiStrike = true;
                break;
        }
        int Count = 0;
        DamageWaitTime = GetDamageWaitTime(ID);
        //連打6次
        if (Skill.MultiStrike)
        {
            Count = 1;
            TotalTime = DamageWaitTime + 6 * 7;
            foreach (var Iter in EngineFunc.SpriteEngine.SpriteList)
            {
                if (Iter is Mob)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        ((Mob)Iter).MobCollision[i] = new MobCollision(EngineFunc.SpriteEngine);
                        ((Mob)Iter).MobCollision[i].ImageLib = Wz.EquipImageLib;
                        ((Mob)Iter).MobCollision[i].Owner = (Mob)Iter;
                        ((Mob)Iter).MobCollision[i].StartTime = DamageWaitTime + i * 7;
                        ((Mob)Iter).MobCollision[i].Index = i - 1;
                        ((Mob)Iter).MobCollision[i].CanCollision = true;
                    }
                }
            }
        }
        else
        {
            Count = 6;
        }

        //順序打
        for (int i = 1; i <= Count; i++)
        {
            var SkillCollision = new SkillCollision(EngineFunc.SpriteEngine);
            SkillCollision.ImageLib = Wz.EquipImageLib;
            SkillCollision.CanCollision = true;
            //ID := ID;
            SkillCollision.IDEntry = Entry;
            SkillCollision.StartTime = DamageWaitTime + i * 6;
        }

        foreach (var Iter in EngineFunc.SpriteEngine.SpriteList)
        {
            if (Iter is Mob)
                Iter.CanCollision = true;
        }

        for (int i = 0; i <= 10; i++)
        {
            if (!Entry.HasNode("Effect/0"))
                return;
            if (Entry.HasNode(Effects[i]))
            {
                var SkillSprite = new SkillSprite(EngineFunc.SpriteEngine);
                SkillSprite.ParentPath = Entry.GetNode(Effects[i]).FullPathToFile2();
                if (Entry.GetNode(Effects[i] + "/0/0") != null)
                    SkillSprite.ParentPath = Entry.GetNode(Effects[i] + "/0").FullPathToFile2();
                SkillSprite.ImageLib = Wz.EquipImageLib;
                SkillSprite.ImageNode = Wz.EquipData[SkillSprite.ParentPath + "/0"];
                SkillSprite.X = Game.Player.X;
                SkillSprite.Y = Game.Player.Y;


            }


        }

    }


}

public class SkillSprite : SpriteEx
{
    public SkillSprite(Sprite Parent) : base(Parent)
    {
    }
    int FTime;
    int Frame;
    Wz_Vector origin = new(0, 0);
    string ID;
    float BallSpeed;
    string EffectName;
    public string ParentPath;
    int Counter;
    bool AnimRepeat, AnimEnd;
    int Left, Top, Right, Bottom;
    bool MoveWithPlayer;

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        Visible = true;
        if ((FTime == 0) && (Frame == 0) && (!Skill.PlayEnded))
            Skill.Start = true;
        ImageNode = Wz.EquipData[ParentPath + "/" + Frame];
        int AnimDelay = ImageNode.GetInt("delay", 100);
        FlipX = Game.Player.FlipX;
        if ((MoveWithPlayer) && (EffectName != "ball"))
        {
            X = Game.Player.X;
            Y = Game.Player.Y;
        }
        FTime += 17;
        if (FTime > AnimDelay)
        {
            FTime = 0;
            Frame += 1;
            AnimEnd = false;
            if (!Wz.EquipData.ContainsKey(ParentPath + "/" + Frame))
            {
                // if Frame> FrameCount then
                // Frame := 0;
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

        if (EffectName != "ball")
        {
            Skill.PlayEnded = AnimEnd;
            if (AnimEnd)
                Dead();
        }

        if (EffectName == "ball")
        {
            Counter += 1;
            if (Counter > 15)
                X += BallSpeed;
            if (Counter > 180)
                Dead();
        }

    }
}

public class SkillCollision : SpriteEx
{
    public SkillCollision(Sprite Parent) : base(Parent)
    {
    }
    public int StartTime;
    public Wz_Node IDEntry;
}