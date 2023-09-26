using DevComponents.DotNetBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;
using WzComparerR2;
using ManagedBass;
using Microsoft.Xna.Framework.Audio;
using System.Windows.Forms;
using WzComparerR2.Rendering;

namespace MapleNecrocer;

internal class Sound
{
    public static Dictionary<string, BassSoundPlayer> SoundDict = new Dictionary<string, BassSoundPlayer>();
    public static List<BassSoundPlayer> PlayendList=new();
    public static void Init()
    {
        Bass.Init(-1, 44100, DeviceInitFlags.Default, IntPtr.Zero);
    }
    public static void Load(string Name)
    {
        Wz_Sound Sound = null;
        if (Wz.GetNode("Sound/" + Name) != null)
            Sound = (Wz_Sound)Wz.GetNode("Sound/" + Name).Value;
        byte[] Data = Sound.ExtractSound();

        if (Data == null || Data.Length <= 0)
        {
            return;
        }
        BassSoundPlayer SoundPlayer = new BassSoundPlayer();
        SoundPlayer.AutoPlay = false;
        SoundPlayer.PreLoad(Data);
        SoundDict.Add(Name, SoundPlayer);
    }
    public static void Play(string Path)
    {
        Wz_Node Child;
        Wz_Node WzNode = Wz.GetNode(Path);
        if (WzNode.Value is Wz_Uol)
        {
            var Entry = WzNode.ParentNode;
            Child = Entry.Get(WzNode.ToStr());
            if (Child == null)
                return;
            if (Child.Value is Wz_Uol)
                Child = Child.ParentNode.Get(Child.ToStr());
        }
        else
        { 
            Child = WzNode;
        }

        if (Child.Value is Wz_Sound)
        {
            byte[] Data = ((Wz_Sound)Child.Value).ExtractSound();
            BassSoundPlayer SoundPlayer = new BassSoundPlayer();
            SoundPlayer.AutoPlay = false;
            SoundPlayer.PreLoad(Data);
            SoundPlayer.Resume();
            PlayendList.Add(SoundPlayer);
        }
    }

    public static void DumpSounds(Wz_Node WzNode)
    {
        Wz_Node Child;
        if (WzNode.Value is Wz_Uol)
        {
            var Entry = WzNode.ParentNode;
            Child = Entry.Get(WzNode.ToStr());
            if (Child == null)
                return;
            if (Child.Value is Wz_Uol)
                Child = Child.ParentNode.Get(Child.ToStr());
            //  if (Child == null)
            //  return;
        }
        else
            Child = WzNode;

        if (Child.Value is Wz_Sound)
        {
            if (!SoundDict.ContainsKey(WzNode.FullPathToFile2()))
            {
                byte[] Data = ((Wz_Sound)Child.Value).ExtractSound();
                BassSoundPlayer SoundPlayer = new BassSoundPlayer();
                SoundPlayer.AutoPlay = false;
                SoundPlayer.PreLoad(Data);
                SoundDict.AddOrReplace(WzNode.FullPathToFile2(), SoundPlayer);
            }
        }
        foreach (var E in WzNode.Nodes)
            DumpSounds(E);
    }

    public static void Play2(string Name)
    {
         
        SoundDict[Name].Play();

    }



}

