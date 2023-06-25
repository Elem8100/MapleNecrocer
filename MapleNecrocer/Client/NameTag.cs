using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
namespace MapleNecrocer;

public class NameTag : SpriteEx
{
    public NameTag(Sprite Parent) : base(Parent)
    {
    }
    static bool ReDraw;
    static bool CanUse;
    static string PlayerName;
    static int NameWidth;
    static RenderTarget2D TargetTexture = null;
    static bool IsUse = true;
    public static void Create(string Name)
    {
        Game.Player.Name = Name;
        NameWidth = Map.MeasureStringX(Map.NpcNameTagFont, Name);
        EngineFunc.Canvas.DrawTarget(ref TargetTexture, NameWidth + 10, 25, () =>
        {
            int NamePos = NameWidth / 2;
            if (Map.ShowChar)
            {
                EngineFunc.Canvas.FillRect(0, 2, NameWidth + 8, 15, new Microsoft.Xna.Framework.Color(0, 0, 0, 150));
                EngineFunc.Canvas.DrawString(Map.NpcNameTagFont, Game.Player.Name, 3, 2, Microsoft.Xna.Framework.Color.White);
            }
        });

        var NameTag = new NameTag(EngineFunc.SpriteEngine);
        NameTag.Tag = 1;
        NameTag.IntMove = true;
        //NameTag.BlendMode=BlendMode.AddtiveColor;
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        if (ReDraw)
        {
            NameWidth = Map.MeasureStringX(Map.NpcNameTagFont, Name);
            EngineFunc.Canvas.DrawTarget(ref TargetTexture, NameWidth + 10, 25, () =>
            {
                int NamePos = NameWidth / 2;
                if (Map.ShowChar)
                {
                    Engine.Canvas.FillRect(0, 2, NameWidth + 8, 15, new Microsoft.Xna.Framework.Color(0, 0, 0, 180));
                    Engine.Canvas.DrawString(Map.NpcNameTagFont, Game.Player.Name, 3, 2, Microsoft.Xna.Framework.Color.White);
                }
            });
        }
        X = Game.Player.X;
        Y = Game.Player.Y;
        Z = Game.Player.Z;
    }

    public override void DoDraw()
    {
        if (!NameTag.IsUse)
            return;
        if (Map.ShowChar)
        {
            int WX = (int)(Game.Player.X) - (int)Engine.Camera.X;
            int WY = (int)(Game.Player.Y) - (int)Engine.Camera.Y;
            int NamePos = NameWidth / 2;
            Engine.Canvas.Draw(TargetTexture, WX - NamePos - 8, WY, BlendMode.NonPremultiplied);
        }
        if (ReDraw)
            ReDraw = false;
    }

}


public class MedalTag : SpriteEx
{
    public MedalTag(Sprite Parent) : base(Parent)
    {
    }
    int EastWidth;
    int WestWidth;
    int CenterWidth;
    int CenterLength;
    int TagWidth;
    int FontColor;
    int R, G, B;
    public string MedalName;
    public int TargetIndex;
    public bool IsReDraw;
    Wz_Node Entry;
    RenderTarget2D TargetTexture;
    public static MedalTag Instance;


    void RenderTargetFunc()
    {
        if (Map.ShowChar)
        {
            CenterLength = Map.MeasureStringX(Map.NpcNameTagFont, MedalName) + 10;
            var WestImage = Wz.EquipData[Entry.FullPathToFile2() + "/w"];
            var WestX = 150 - (CenterLength + EastWidth + WestWidth) / 2;
            //FixAlphaChannel(EquipImages[WestImage]);
            Engine.Canvas.Draw(Wz.EquipImageLib[WestImage], WestX, -WestImage.GetNode("origin").ToVector().Y + 38);

            var CenterImage = Wz.EquipData[Entry.FullPathToFile2() + "/c"];
            int Count = CenterLength / CenterWidth;
            //FixAlphaChannel(EquipImages[WestImage]);
            for (int i = 1; i <= Count; i++)
            {
                Engine.Canvas.Draw(Wz.EquipImageLib[CenterImage], WestX + ((i - 1) * CenterWidth) + WestWidth, -
                    CenterImage.GetNode("origin").ToVector().Y + 38);
            }
            int OffX = 0;
            switch (CenterWidth)
            {
                case 1:
                    OffX = 0;
                    break;
                case 2:
                    OffX = 1;
                    break;
                case 3:
                case 4:
                case 5:
                    OffX = 4; break;
                case int i when i >= 6 && i <= 13:
                    OffX = 5;
                    break;
                case 14:
                    OffX = 12;
                    break;
                case 20:
                    OffX = 18;
                    break;
            }

            var EastImage = Wz.EquipData[Entry.GetPath + "/e"];
            //FixAlphaChannel(EquipImages[EastImage]);
            Engine.Canvas.Draw(Wz.EquipImageLib[EastImage], WestX + CenterLength + WestWidth - OffX,
                -EastImage.GetNode("origin").ToVector().Y + 38);
            Engine.Canvas.DrawString(Map.NpcNameTagFont, MedalName, WestX + WestWidth + 2, 36, new Color(R, G, B, 255));

        }
    }
    void InitData()
    {
        EastWidth = Entry.GetNode("e").ExtractPng().Width;
        WestWidth = Entry.GetNode("w").ExtractPng().Width;
        CenterWidth = Entry.GetNode("c").ExtractPng().Width;

        TagWidth = CenterLength + EastWidth + WestWidth + 30;

        var TagHeight = Entry.GetNode("w").ExtractPng().Height + 30;
        if (Entry.GetNode("clr") != null)
            FontColor = 16777216 + Entry.GetNode("clr").ToInt();
        else
            FontColor = 16777215;
        string Hex = FontColor.ToString("X").PadLeft(6, '0');
        R = (byte)Convert.ToInt32(Hex.LeftStr(2), 16);
        G = (byte)Convert.ToInt32(Hex.Substring(2, 2), 16);
        B = (byte)Convert.ToInt32(Hex.RightStr(2), 16);
        Engine.Canvas.DrawTarget(ref TargetTexture, 300, 100, () => { RenderTargetFunc(); });
    }

    public static void Create(string ItemID)
    {
        Instance = new MedalTag(EngineFunc.SpriteEngine);
        Instance.IntMove = true;
        int TagNum = Wz.GetNode("Character/Accessory/" + ItemID + ".img/info/medalTag").ToInt();
        Instance.Entry = Wz.GetNode("UI/NameTag.img/medal/" + TagNum);
        Wz.DumpData(Instance.Entry, Wz.EquipData, Wz.EquipImageLib);
        Instance.MedalName = Wz.GetNode("String/Eqp.img/Eqp/Accessory/" + ItemID.RightStr(7)).GetStr("name");
        Instance.InitData();
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        if (IsReDraw)
        {
            Engine.Canvas.DrawTarget(ref TargetTexture, 300, 100, () => { RenderTargetFunc(); });
        }
        X = Game.Player.X;
        Y = Game.Player.Y;
        Z = Game.Player.Z;
    }

    public override void DoDraw()
    {
        if (Map.ShowChar)
        {
            int WX = (int)(Game.Player.X) - (int)(Engine.Camera.X);
            int WY = (int)(Game.Player.Y) - (int)(Engine.Camera.Y);
            Engine.Canvas.Draw(TargetTexture, WX - 150, WY  -8,BlendMode.NonPremultiplied);
        }
        if (IsReDraw)
            IsReDraw = false;
    }

    public static void ReDraw()
    {
        if (Instance != null)
            Instance.IsReDraw = true;
    }

}