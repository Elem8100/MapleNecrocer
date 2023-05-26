using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WzComparerR2.WzLib;

namespace MapleNecrocer;

enum EffectType { Cash, Chair, Equip, Consume, Totem, Soul, Ring }
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
    static void LoadList()
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

    static void Delete(string ID)
    {
        if (SetEffect.AllList.ContainsKey(ID))
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
                foreach(var Iter3 in Iter2.Nodes)
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

}

