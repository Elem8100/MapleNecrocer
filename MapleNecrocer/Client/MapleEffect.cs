using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WzComparerR2.CharaSim;
using WzComparerR2.WzLib;
using Vector2 = Microsoft.Xna.Framework.Vector2;
namespace MapleNecrocer;

public enum EffectType { Cash, Chair, Equip, Consume, Totem, Soul, Ring }
public class SetEffect : SpriteEx
{
    public SetEffect(Sprite Parent) : base(Parent)
    {
    }
    string Path;
    int Frame;
    int FTime;
    int Delay;
    int Default;
    bool DoWalk;
    public static Dictionary<string, string> AllList = new();
    public static Dictionary<string, SetEffect> UseList = new();
    public static void LoadList()
    {
        foreach (var Iter in Wz.GetNodeA("Effect/SetEff.img").Nodes)
        {
            foreach (var Iter2 in Iter.Nodes)
            {
                if (Iter2.Text == "info")
                {
                    foreach (var Iter3 in Iter2.Nodes)
                    {
                        foreach (var Iter4 in Iter3.Nodes)
                        {
                            SetEffect.AllList.AddOrReplace("0" + Iter4.ToInt(), Iter.Text);
                        }
                    }
                }
            }
        }
    }

    public static void Delete(string ID)
    {
        if (SetEffect.UseList.ContainsKey(ID))
        {
           SetEffect.UseList[ID].Dead();
           SetEffect.UseList.Remove(ID);
        }
    }

    public static void Create(string ID)
    {
        var Entry = Wz.GetNodeA("Effect/SetEff.img/" + AllList[ID].IntID());
        Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib);
        var SetEffect = new SetEffect(EngineFunc.SpriteEngine);
        SetEffect.ImageLib = Wz.EquipImageLib;
        SetEffect.IntMove = true;
        SetEffect.Tag = 1;
        foreach (var Iter in Entry.Nodes)
        {
            foreach (var Iter2 in Iter.Nodes)
            {
                if (Iter2.Text == "walk1")
                    SetEffect.DoWalk = true;
                if ((Char.IsNumber(Iter2.Text[0])) && (Iter2.Value is Wz_Png))
                {
                    SetEffect.Path = Iter2.ParentNode.FullPathToFile2();
                    SetEffect.ImageNode = Wz.EquipData[Iter2.FullPathToFile2()];
                }
                foreach (var Iter3 in Iter2.Nodes)
                {
                    if ((Char.IsNumber(Iter3.Text[0])) && (Iter3.Value is Wz_Png))
                    {
                        SetEffect.Path = Iter3.ParentNode.FullPathToFile2();
                        SetEffect.ImageNode = Wz.EquipData[Iter3.FullPathToFile2()];
                    }
                }
            }
        }
        UseList.AddOrReplace(ID, SetEffect);
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        if (Wz.HasDataE(Path + "/" + Game.Player.Action + "/" + Frame))
        {
            ImageNode = Wz.EquipData[Path + "/" + Game.Player.Action + "/" + Frame];
            Default = 1;
            Visible = true;
        }
        else if (Wz.HasDataE(Path + "/" + Frame))
        {
            ImageNode = Wz.EquipData[Path + "/" + Frame];
            Default = 0;
            Visible = true;
        }
        else
            Visible = false;

        if (DoWalk)
        {
            if ((Game.Player.Action == "walk1") || (Game.Player.Action == "walk2"))
                Visible = true;
            else
                Visible = false;
        }

        Delay = ImageNode.GetInt("delay", 100);
        FTime += 17;
        if (FTime > Delay)
        {
            Frame += 1;
            switch (Default)
            {
                case 1:
                    if (!Wz.HasDataE(Path + "/" + Game.Player.Action + "/" + Frame))
                        Frame = 0;
                    break;
                case 0:
                    if (!Wz.HasDataE(Path + "/" + Frame))
                        Frame = 0;
                    break;
            }
            FTime = 0;
        }
        X = Game.Player.X - 10;
        int Pos = ImageNode.ParentNode.GetInt("pos", -1);

        if (Pos == 1)
            Y = Game.Player.Y - 50;
        else
            Y = Game.Player.Y;
        Z = Game.Player.Z + ImageNode.ParentNode.GetInt("z", -1);

        FlipX = Game.Player.FlipX;

        Wz_Vector origin = ImageNode.GetVector("origin");
        Vector2 BrowPos = Game.Player.BrowPos;
        Wz_Vector BodyRelMove = MapleChair.BodyRelMove;
        int OffY = 30;
        switch (FlipX)
        {
            case true:
                Origin.X = (int)(origin.X - ImageWidth - BrowPos.X + BodyRelMove.X + 3);
                break;
            case false:
                Origin.X = (int)(-origin.X - BrowPos.X + 12 + BodyRelMove.X);
                break;
        }
        Origin.Y = (int)(-origin.Y - BrowPos.Y + OffY + BodyRelMove.Y);
    }

}

public class ItemEffect : SpriteEx
{
    public ItemEffect(Sprite Parent) : base(Parent)
    {
    }
    string Path;
    int Frame;
    int FTime;
    int Delay;
    int Default;
    bool IsCash;
    EffectType EffType;
    public static List<string> AllList = new();
    static Wz_Node Entry;
    public static Dictionary<string, ItemEffect> UseList = new();
    public static void LoadList()
    {
        foreach (var Iter in Wz.GetNodeA("Effect/ItemEff.img").Nodes)
            ItemEffect.AllList.Add("0" + Iter.Text);
    }
    public static void Delete(string ID)
    {
        if (ItemEffect.UseList.ContainsKey(ID))
        {
            ItemEffect.UseList[ID].Dead();
            ItemEffect.UseList.Remove(ID);
        }
    }

    public static void Delete(EffectType EffType)
    {
        foreach (var Iter in EngineFunc.SpriteEngine.SpriteList)
        {
            if ((Iter is ItemEffect) && ((ItemEffect)Iter).EffType == EffType)
            {
                Iter.Dead();
                var s = Iter;
                s = null;
            }
        }
        EngineFunc.SpriteEngine.Dead();
    }
    public static void Create(string ID, EffectType EffectType)
    {
        if (ID == "01048000") return;
        if (ID == "01049000") return;
        switch (EffectType)
        {
            case EffectType.Cash:
                Entry = Wz.GetNodeA("Item/Cash/0501.img/" + ID);
                break;
            case EffectType.Chair:
            case EffectType.Equip:
            case EffectType.Consume:
            case EffectType.Totem:
            case EffectType.Ring:
                Entry = Wz.GetNodeA("Effect/ItemEff.img/" + ID.IntID());
                break;
            case EffectType.Soul:
                Entry = Wz.GetNodeA("Effect/BasicEff.img/SoulSkillReadied/Repeat/" + ID);
                break;
        }

        Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib);

        if (ID.LeftStr(4) == "0111" || ID.LeftStr(4) == "0301")
        {
            if (ID.LeftStr(6) == "011129" || ID.LeftStr(6) == "011132")
            {
                var ItemEffect = new ItemEffect(EngineFunc.SpriteEngine);
                ItemEffect.EffType = EffectType.Ring;
                ItemEffect.ImageLib = Wz.EquipImageLib;
                ItemEffect.IntMove = true;
                ItemEffect.Tag = 1;
                ItemEffect.Path = Entry.FullPathToFile2() + "/effect";
                foreach (var Iter in Entry.GetNode("effect").Nodes)
                {
                    if (Iter.GetNode("effect/0") != null)
                    {
                        ItemEffect.ImageNode = Wz.EquipData[Iter.GetPath + "/0"];
                        break;
                    }
                }
            }

            foreach (var Iter in Entry.Nodes)
            {
                if (Char.IsNumber(Iter.Text[0]))
                {
                    var ItemEffect = new ItemEffect(EngineFunc.SpriteEngine);
                    if (ID.LeftStr(4) == "0111")
                        ItemEffect.EffType = EffectType.Ring;
                    else
                        ItemEffect.EffType = EffectType.Chair;
                    ItemEffect.ImageLib = Wz.EquipImageLib;
                    ItemEffect.IntMove = true;
                    ItemEffect.Tag = 1;

                    foreach (var Iter2 in Iter.Nodes)
                    {
                        if (Iter2.Value is Wz_Png)
                        {
                            ItemEffect.Path = Iter2.ParentNode.FullPathToFile2();
                            ItemEffect.ImageNode = Wz.EquipData[Iter2.FullPathToFile2()];
                        }
                        foreach (var Iter3 in Iter2.Nodes)
                        {
                            if (Iter3.Value is Wz_Png)
                            {
                                ItemEffect.Path = Iter3.ParentNode.FullPathToFile2();
                                ItemEffect.ImageNode = Wz.EquipData[Iter3.FullPathToFile2()];
                            }
                        }
                    }
                }
            }
        }
        else if (ID.LeftStr(3) == "010" || ID.LeftStr(4) == "0110" || ID.LeftStr(4) == "0501")
        {

            var ItemEffect = new ItemEffect(EngineFunc.SpriteEngine);
            if (ID.LeftStr(4) == "0501")
                ItemEffect.EffType = EffectType.Cash;
            else
                ItemEffect.EffType = EffectType.Equip;
            ItemEffect.ImageLib = Wz.EquipImageLib;
            ItemEffect.Path = Entry.FullPathToFile2() + "/effect";
            foreach (var Iter in Entry.GetNode("effect").Nodes)
            {
                if (Iter.GetNode("0") != null)
                {
                    ItemEffect.ImageNode = Wz.EquipData[Iter.FullPathToFile2() + "/0"];
                    break;
                }
            }
            ItemEffect.IntMove = true;
            ItemEffect.Tag = 1;
            if (EffectType == EffectType.Equip)
                UseList.Add(ID, ItemEffect);
        }
        else
        {
            var ItemEffect = new ItemEffect(EngineFunc.SpriteEngine);
            if (ID.LeftStr(3) == "012")
            {
                ItemEffect.EffType = EffectType.Totem;
                switch (ID.ToInt())
                {
                    case 1202215:
                    case 1202216:
                    case 1202217:
                    case 1202160:
                        ItemEffect.Path = Entry.FullPathToFile2() + "/effect/default";
                        break;
                    default:
                        ItemEffect.Path = Entry.FullPathToFile2();
                        break;
                }
            }
            else if (ID.LeftStr(1) == "8")
            {
                ItemEffect.EffType = EffectType.Soul;
                ItemEffect.Path = Entry.FullPathToFile2();
            }
            else
            {
                ItemEffect.EffType = EffectType.Consume;
                ItemEffect.Path = Entry.FullPathToFile2();
            }
            ItemEffect.ImageLib = Wz.EquipImageLib;
            ItemEffect.IntMove = true;
            ItemEffect.Tag = 1;
        }
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);

        if (Wz.HasDataE(Path + "/" + Game.Player.Action + "/" + Frame))
        {
            ImageNode = Wz.EquipData[Path + "/" + Game.Player.Action + "/" + Frame];
            Default = 1;
            Visible = true;
        }
        else if (Wz.HasDataE(Path + "/default/" + Frame))
        {
            ImageNode = Wz.EquipData[Path + "/default/" + Frame];
            Default = 0;
            Visible = true;

        }
        else if (Wz.HasDataE(Path + "/0/" + Frame))
        {
            ImageNode = Wz.EquipData[Path + "/0/" + Frame];
            Default = 2;
            Visible = true;
        }
        else if (Wz.HasDataE(Path + "/" + Frame))
        {
            ImageNode = Wz.EquipData[Path + "/" + Frame];
            Default = 3;
            Visible = true;
        }
        else
        {

            Visible = false;
        }

        if (ImageNode == null)
            return;

        Delay = ImageNode.GetInt("delay", 100);
        FTime += 17;
        if (FTime > Delay)
        {
            Frame += 1;
            switch (Default)
            {
                case 1:
                    if (!Wz.HasDataE(Path + "/" + Game.Player.Action + "/" + Frame))
                        Frame = 0;
                    break;
                case 0:
                    if (!Wz.HasDataE(Path + "/default/" + Frame))
                        Frame = 0;
                    break;
                case 2:
                    if (!Wz.HasDataE(Path + "/0/" + Frame))
                        Frame = 0;
                    break;
                case 3:
                    if (!Wz.HasDataE(Path + "/" + Frame))
                        Frame = 0;
                    break;
            }
            FTime = 0;
        }
        FlipX = Game.Player.FlipX;
        X = Game.Player.X - 10;
        int Pos = ImageNode.ParentNode.GetInt("pos", -1);

        if (Pos == 0 || Pos == 1)
        {
            X = Game.Player.X - 10;
        }
        else
        {
            if (FlipX)
            {
                if (Game.Player.InLadder)
                    X = Game.Player.X - 12;
                else
                    X = Game.Player.X - 19;
            }
            else
            {
                if (Game.Player.InLadder)
                    X = Game.Player.X + 5;
                else
                    X = Game.Player.X;
            }
        }

        if (EffType != EffectType.Totem)
        {
            if (Pos == 1)
                Y = Game.Player.Y - 50;
            else
                Y = Game.Player.Y;
        }
        else
        {
            Y = Game.Player.Y - 60;
        }

        Z = Game.Player.Z + ImageNode.ParentNode.GetInt("z", 0);
        if (EffType == EffectType.Chair)
            Z = Game.Player.Z + ImageNode.GetInt("z", 0) - 1;

        Wz_Vector origin = ImageNode.GetVector("origin");
        Vector2 BrowPos;
        Wz_Vector BodyRelMove=new(0,0);
        int OffY = 0;
        if (EffType == EffectType.Chair)
        {
            BrowPos.X = 0;
            BrowPos.Y = 0;
            BodyRelMove.X = 0;
            BodyRelMove.Y = 0;
            OffY = 0;
        }
        else
        {
            BrowPos = Game.Player.BrowPos;
            BodyRelMove = MapleChair.BodyRelMove;
            OffY = 30;
        }

        switch (FlipX)
        {
            case true:
                Origin.X = (int)(-origin.X + ImageWidth - 12 + BrowPos.X - BodyRelMove.X);
                break;
            case false:
                Origin.X = (int)(origin.X + BrowPos.X - 2 - BodyRelMove.X);
                break;
        }
        Origin.Y = (int)(origin.Y + BrowPos.Y - OffY + BodyRelMove.Y);

    }
}


