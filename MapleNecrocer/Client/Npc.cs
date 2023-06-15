using WzComparerR2.WzLib;
using WzComparerR2.PluginBase;
using Spine;
using WzComparerR2.Animation;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Xml.Linq;
using WzComparerR2.CharaSim;
using System.Drawing;
using SharpDX.Direct3D11;

namespace MapleNecrocer;

public class Npc : SpriteEx
{
    public Npc(Sprite Parent) : base(Parent)
    {
        IntMove=true;
    }
    string SpriteID;
    string Action;
    float Time;
    int Frame;
    int RandMsg;
    int Counter;
    string ImagePath;
    Foothold FH;
    List<string> Actions = new();
    List<string> Msgs = new();
    public static List<string> SummonedList = new();
    public string LocalID;
    public ChatBalloon Balloon;

    public static void Create()
    {
        foreach (var Iter in Map.Img.Nodes["life"].Nodes)
        {
            if (Iter.GetBool("hide"))
                continue;
            if (Iter.Nodes["type"] != null)
            {
                if (Iter.GetStr("type") == "n")
                    Npc.Spawn(Iter.GetStr("id"), Iter.GetInt("x"), Iter.GetInt("cy"), Iter.GetInt("f"));
            }
            else
            {
                foreach (var Iter2 in Iter.Nodes)
                {
                    if (Iter2.GetStr("type") == "n")
                        Npc.Spawn(Iter2.GetStr("id"), Iter2.GetInt("x"), Iter2.GetInt("cy"), Iter2.GetInt("f"));
                }
            }
        }
    }

    private static string ReString(string Str)
    {
        for (int i = 0; i <= Str.Length / 8; i++)
            Str = Str.Insert(i * 8, "=");
        Str = Str.Remove(0, 1);
        return Str;
    }
    public static void Spawn(string ID, int PosX, int PosY, int FlipX)
    {
        if (Wz.GetNodeA("String/Npc.img/" + ID.IntID()) == null)
            return;
        switch (ID.ToInt())
        {
            case 9062010:
            case 9050000:
            case 9050009:
            case 9300018:
            case 9010088:
                return;
        }
        var Link = Wz.GetNodeA("Npc/" + ID + ".img/info/link");
        var NpcImg = Wz.GetNodeA("Npc/" + ID + ".img");
        if (!Wz.Data.ContainsKey("Npc/" + ID + ".img"))
        {
            Wz.DumpData(NpcImg, Wz.Data, Wz.ImageLib);
            if (Link != null)
                Wz.DumpData(Wz.GetNodeA("Npc/" + Link.ToStr() + ".img"), Wz.Data, Wz.ImageLib);
        }

        var Npc = new Npc(EngineFunc.SpriteEngine);
        Npc.ImageLib = Wz.ImageLib;
        Npc.LocalID = ID;
        if (Link != null)
            Npc.SpriteID = Link.ToStr();
        else
            Npc.SpriteID = ID;
        Npc.Frame = 0;
        foreach (var Iter in Wz.GetNodeA("Npc/" + Npc.SpriteID + ".img").Nodes)
        {
            if ((Iter.Text != "info") && (Iter.Text.LeftStr(9) != "condition") && (Iter.Nodes["0"] != null))
            {
                Npc.Actions.Add(Iter.Text);
            }
        }

        if (Npc.Actions.Count == 0)
            return;
        Npc.Action = Npc.Actions[0];
        if (FlipX.ToBool())
            Npc.FlipX = true;

        string Path = "Npc/" + Npc.SpriteID + ".img/" + Npc.Action;
        Npc.ImageNode = Wz.Data[Path + "/0"];
        Foothold BelowFH = null;
        Vector2 Pos = FootholdTree.Instance.FindBelow(new Vector2(PosX, PosY - 3), ref BelowFH);
        Npc.X = Pos.X;
        Npc.Y = Pos.Y;
        Npc.FH = BelowFH;
        Npc.Z = BelowFH.Z * 100000 + 7000;
        Npc.Width = Npc.ImageWidth;
        Npc.Height = Npc.ImageHeight;

        foreach (var Iter2 in Wz.GetNode("String/Npc.img/" + Npc.SpriteID.IntID()).Nodes)
        {
            if (Iter2.Text.Length == 2)
            {
                string Str1 = Iter2.ToStr();

                if (Wz.Country == "GMS")
                {
                    Npc.Msgs.Add(Str1);
                }
                else
                {
                    Str1 = Str1.Replace(" ", "");
                    Npc.Msgs.Add(ReString(Str1));
                }
            }
        }

        if (Npc.Msgs.Count > 0)
        {
            Npc.Balloon = new ChatBalloon(EngineFunc.SpriteEngine);
            Npc.Balloon.SetStyle(0);
            Npc.Balloon.X = Npc.X;
            Npc.Balloon.Y = Npc.Y - Npc.Height - 20;
            Npc.Balloon.Z = Npc.Z + 100000000;
        }

        Random Random = new();
        Npc.Counter = Random.Next(1000);

        Wz_Vector origin = Wz.GetVector(Path + "/0/origin");
        if (Npc.FlipX)
            Npc.Origin.X = -origin.X + Npc.ImageWidth;
        else
            Npc.Origin.X = origin.X;
        Npc.Origin.Y = origin.Y;

        //
        if (Wz.GetNodeA("Npc/" + Npc.SpriteID + ".img/info/MapleTV").ToBool())
        {
            string Path2 = "Npc/" + Npc.SpriteID + ".img/info";
            int msgX = Wz.GetInt(Path2 + "/MapleTVmsgX");
            int msgY = Wz.GetInt(Path2 + "/MapleTVmsgY");
            int adX = Wz.GetInt(Path2 + "/MapleTVadX");
            int adY = Wz.GetInt(Path2 + "/MapleTVadY");
            MapleTV.Create((int)Npc.X, (int)Npc.Y, msgX, msgY, adX, adY, Npc.Z);
        }
        //
        var NpcText = new NpcText(EngineFunc.SpriteEngine);
        NpcText.NpcName = Wz.GetNodeA("String/Npc.img/" + Npc.LocalID.IntID()).GetStr("name");
        NpcText.NpcFunc = Wz.GetNodeA("String/Npc.img/" + Npc.LocalID.IntID()).GetStr("func");
        if (NpcText.NpcFunc != "")
            NpcText.HasFunc = true;
        NpcText.X = Pos.X;
        NpcText.Y = Pos.Y;
        NpcText.Z = 1000000 + 100000;
        NpcText.Width = Npc.ImageWidth;
        NpcText.Height = Npc.ImageHeight;
        NpcText.NameWidth = Map.MeasureStringX(Map.NpcNameTagFont, NpcText.NpcName);
        NpcText.FuncWidth = Map.MeasureStringX(Map.NpcNameTagFont, NpcText.NpcFunc);
        NpcText.ID = "ID:" + ID;
        NpcText.IDWidth = Map.MeasureStringX(Map.NpcNameTagFont, NpcText.ID);
        NpcText.HideName = Wz.GetBool("Npc/" + ID + ".img/info/hideName");
        NpcText.Moved = false;
       

    }

    public override void DoMove(float Delta)
    {
        ImagePath = "Npc/" + SpriteID + ".img/" + Action + "/" + Frame;
        ImageNode = Wz.Data[ImagePath];
        int Delay = Wz.GetInt(ImagePath + "/delay", 100);
        Random random = new Random();
        Time += 16.66f * Delta;
        if (Time > Delay)
        {
            Frame += 1;
            if (!Wz.HasData("Npc/" + SpriteID + ".img/" + Action + "/" + Frame))
            {
                Frame = 0;
                if (Actions.Count > 1)
                {
                    if (random.Next(2) == 0)
                        Action = Actions[random.Next(Actions.Count)];
                }
            }
            Time = 0;
        }

        Wz_Vector origin = Wz.GetVector(ImagePath + "/origin");
        if (FlipX)
            Origin.X = -origin.X + ImageWidth;
        else
            Origin.X = origin.X;
        Origin.Y = origin.Y;
    }

    public override void DoDraw()
    {
        base.DoDraw();
        Random Random = new();
        Counter += 1;

        if (Counter > 1000)
        {
            Counter = 0;
            RandMsg = Random.Next(Msgs.Count);
        }
        if (Msgs.Count > 0)
        {
            if ((Counter > 0) && (Counter < 500))
                Balloon.Msg = Msgs[RandMsg];
            else
                Balloon.Msg = "";

        }
    }

}

public class NpcText : SpriteEx
{
    public NpcText(Sprite Parent) : base(Parent)
    {

    }
    public string NpcName;
    public string NpcFunc;
    public bool HasFunc;
    public int NameWidth;
    public int FuncWidth;
    public int IDWidth;
    public string ID;
    public bool HideName;
  
    public override void DoDraw()
    {
         
       
        int  WX = (int)X - (int)Engine.Camera.X;
        int  WY = (int)Y - (int)Engine.Camera.Y;
       

        if (Map.ShowID)
        {
            float IDPos = WX - IDWidth / 2;
            Engine.Canvas.FillRect((int)IDPos - 2, (int)WY + 36, IDWidth + 5, 15, new Microsoft.Xna.Framework.Color(0, 0, 0, 150));
            Engine.Canvas.DrawString(Map.NpcNameTagFont, ID, IDPos, WY + 37, Microsoft.Xna.Framework.Color.Cyan);
        }

        if (Map.ShowNpcName)
        {
            if (HideName)
                return;
            int OffsetY = 0;
            switch (Wz.Country)
            {
                case "GMS": OffsetY = 0; break;
                case "KMS": case "TMS": OffsetY = 1; break;
                case "JMS": OffsetY = 2; break;
            }
            int NamePos = WX - NameWidth / 2;
            Engine.Canvas.FillRect((int)NamePos - 2, (int)WY + 3, NameWidth + 5, 16, new Microsoft.Xna.Framework.Color(0, 0, 0, 150));
            Engine.Canvas.DrawString(Map.NpcNameTagFont, NpcName, NamePos, WY + 4 + OffsetY, Microsoft.Xna.Framework.Color.Yellow);

            if (HasFunc)
            {
                float FuncPos = WX - FuncWidth / 2;
                Engine.Canvas.FillRect((int)FuncPos - 2, (int)WY + 21, FuncWidth + 5, 16, new Microsoft.Xna.Framework.Color(0, 0, 0, 150));
                Engine.Canvas.DrawString(Map.NpcNameTagFont, NpcFunc, FuncPos, WY + 22 + OffsetY, Microsoft.Xna.Framework.Color.Yellow);
            }
        }


    }

}


