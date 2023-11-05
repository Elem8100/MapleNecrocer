using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MapleNecrocer;

public class NameTag : SpriteEx
{
    public NameTag(Sprite Parent) : base(Parent)
    {
    }
    public static bool ReDraw;
    static bool CanUse;
    public static string PlayerName;
    static int NameWidth;
    static RenderTarget2D TargetTexture = null;
    public static bool IsUse = true;
    private static int OffY;
    public static void Create(string Name)
    {
        Game.Player.Name = Name;
        NameWidth = Map.MeasureStringX(Map.NpcNameTagFont, Name);
        switch (Wz.Region)
        {
            case "GMS":
            case "KMS":
                OffY = 0;
                break;
            case "TMS":
                OffY = 1;
                break;
            case "JMS":
                OffY = 2;
                break;
        }
        EngineFunc.Canvas.DrawTarget(ref TargetTexture, NameWidth + 10, 25, () =>
        {
            int NamePos = NameWidth / 2;
            if (Map.ShowPlayer)
            {
                EngineFunc.Canvas.FillRoundRect(0, 2, NameWidth + 8, 15, new Microsoft.Xna.Framework.Color(0, 0, 0, 150));
                EngineFunc.Canvas.DrawString(Map.NpcNameTagFont, Game.Player.Name, 3, 2 + OffY, Microsoft.Xna.Framework.Color.White);
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
            NameWidth = Map.MeasureStringX(Map.NpcNameTagFont, Game.Player.Name);
            EngineFunc.Canvas.DrawTarget(ref TargetTexture, NameWidth + 10, 25, () =>
            {
                int NamePos = NameWidth / 2;
                if (Map.ShowPlayer)
                {
                    Engine.Canvas.FillRoundRect(0, 2, NameWidth + 8, 15, new Microsoft.Xna.Framework.Color(0, 0, 0, 150));
                    Engine.Canvas.DrawString(Map.NpcNameTagFont, Game.Player.Name, 3, 2 + OffY, Microsoft.Xna.Framework.Color.White);
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
        if (Map.ShowPlayer)
        {
            int WX = (int)(Game.Player.X) - (int)Engine.Camera.X;
            int WY = (int)(Game.Player.Y) - (int)Engine.Camera.Y;
            int NamePos = NameWidth / 2;
            Engine.Canvas.Draw(TargetTexture, WX - NamePos - 8, WY, BlendMode.NonPremultiplied2);
        }
        if (ReDraw)
            ReDraw = false;
    }

}


public class MedalTag : SpriteEx
{
    public MedalTag(Sprite Parent) : base(Parent)
    {
        Tag=1;
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
    public Wz_Node Entry;
    public RenderTarget2D TargetTexture;
    public static MedalTag Instance;

    private static void ChangeAlpha(ref Bitmap bmp)
    {
        BitmapData bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
        IntPtr ptr = bmpData.Scan0;
        int numBytes = bmp.Width * bmp.Height * 4;
        byte[] argbValues = new byte[numBytes];
        System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, numBytes);
        for (int counter = 0; counter < argbValues.Length; counter += 4)
        {
            if (argbValues[counter + 4 - 1] >= 150)
                argbValues[counter + 4 - 1] = 255;
        }
        System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, numBytes);
        bmp.UnlockBits(bmpData);

    }

    private static Texture2D FixAlpha(Bitmap Bmp)
    {

        ChangeAlpha(ref Bmp);
        /*
        int[] imgData = new int[Bmp.Width * Bmp.Height];
        
        Texture2D Texture = new Texture2D(RenderFormDraw.Instance.GraphicsDevice, Bmp.Width, Bmp.Height);
        unsafe
        {
            System.Drawing.Imaging.BitmapData origdata =
                Bmp.LockBits(new System.Drawing.Rectangle(0, 0, Bmp.Width, Bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            uint* byteData = (uint*)origdata.Scan0;
            // Switch bgra -> rgba
            for (int i = 0; i < imgData.Length; i++)
            {
                byteData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);
            }
            System.Runtime.InteropServices.Marshal.Copy(origdata.Scan0, imgData, 0, Bmp.Width * Bmp.Height);
            byteData = null;
            Bmp.UnlockBits(origdata);
        }
        Texture.SetData(imgData);
        */

        Texture2D Texture = null;
        using (MemoryStream Stream = new MemoryStream())
        {
            Bmp.Save(Stream, System.Drawing.Imaging.ImageFormat.Bmp);
            Stream.Seek(0, SeekOrigin.Begin);
            Texture = Texture2D.FromStream(RenderFormDraw.Instance.GraphicsDevice, Stream);
            Microsoft.Xna.Framework.Color[] Buffer = new Microsoft.Xna.Framework.Color[Texture.Width * Texture.Height];
            Texture.GetData(Buffer);
            for (int i = 0; i < Buffer.Length; i++)
                Buffer[i] = Microsoft.Xna.Framework.Color.FromNonPremultiplied(Buffer[i].R, Buffer[i].G, Buffer[i].B, Buffer[i].A);
            Texture.SetData(Buffer);
        }
        return Texture;
    }

    public void RenderTargetFunc()
    {
        if (Map.ShowPlayer)
        {
            CenterLength = Map.MeasureStringX(Map.NpcNameTagFont, MedalName) + 10;
            var WestImage = Wz.EquipData[Entry.FullPathToFile2() + "/w"];
            var WestX = 150 - (CenterLength + EastWidth + WestWidth) / 2;
            Wz.EquipImageLib[WestImage] = FixAlpha(Entry.GetBmp("w"));
            Engine.Canvas.Draw(Wz.EquipImageLib[WestImage], WestX, -WestImage.GetNode("origin").ToVector().Y + 38);

            var CenterImage = Wz.EquipData[Entry.FullPathToFile2() + "/c"];
            int Count = CenterLength / CenterWidth;
            Wz.EquipImageLib[CenterImage] = FixAlpha(Entry.GetBmp("c"));
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

            var EastImage = Wz.EquipData[Entry.FullPathToFile2() + "/e"];
            Wz.EquipImageLib[EastImage] = FixAlpha(Entry.GetBmp("e"));
            Engine.Canvas.Draw(Wz.EquipImageLib[EastImage], WestX + CenterLength + WestWidth - OffX,
                -EastImage.GetNode("origin").ToVector().Y + 38);

            int OffY = 0;
            switch (Wz.Region)
            {
                case "GMS": OffY = 0; break;
                case "JMS": OffY = 2; break;
                case "TMS": OffY = 1; break;
                case "KMS": OffY = 1; break;
            }
            Engine.Canvas.DrawString(Map.NpcNameTagFont, MedalName, WestX + WestWidth + 2, 36 + OffY, new Color(R, G, B, 255));

        }
    }
    public void InitData()
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
        if (Map.ShowPlayer)
        {
            int WX = (int)(Game.Player.X) - (int)(Engine.Camera.X);
            int WY = (int)(Game.Player.Y) - (int)(Engine.Camera.Y);
            Engine.Canvas.Draw(TargetTexture, WX - 150, WY - 8, BlendMode.NonPremultiplied2);
        }
        if (IsReDraw)
            IsReDraw = false;
    }

    public static void ReDraw()
    {
        if (Instance != null)
            Instance.IsReDraw = true;
    }
    public static void Delete()
    {
        if (Instance != null)
            Instance.Dead();
        EngineFunc.SpriteEngine.Dead();

    }
}


public class NickNameTag : MedalTag
{
    public NickNameTag(Sprite Parent) : base(Parent)
    {
    }
    public static NickNameTag Instance;

    static void ReDraw()
    {
        if (Instance != null)
            Instance.IsReDraw = true;
    }

    public override void DoDraw()
    {
        if (Map.ShowPlayer)
        {
            int WX = (int)Game.Player.X - (int)Engine.Camera.X;
            int WY = (int)Game.Player.Y - (int)Engine.Camera.Y;
            Engine.Canvas.Draw(TargetTexture, WX - 150 - Game.Player.BrowPos.X + MapleChair.BodyRelMove.X, WY - 110
                - Game.Player.BrowPos.Y + MapleChair.BodyRelMove.Y);
        }
        if (IsReDraw)
            IsReDraw = false;
    }

    public static void Delete()
    {
        if (Instance != null)
        {
            Instance.Dead();
            EngineFunc.SpriteEngine.Dead();
        }
    }

    public static void Create(string ItemID)
    {
        Instance = new NickNameTag(EngineFunc.SpriteEngine);
        Instance.IntMove = true;
        int TagNum = Wz.GetNode("Item/Install/0370.img/" + ItemID + "/info/nickTag").ToInt();
        Instance.Entry = Wz.GetNode("UI/NameTag.img/nick/" + TagNum);
        Wz.DumpData(Instance.Entry, Wz.EquipData, Wz.EquipImageLib);
        Instance.MedalName = Wz.GetNode("String/Ins.img/" + ItemID.RightStr(7)).GetStr("name");
        Instance.InitData();
    }

}

public class LabelRingTag : MedalTag
{
    public LabelRingTag(Sprite Parent) : base(Parent)
    {
    }
    public static LabelRingTag Instance;

    public static void Create(string ItemID)
    {
        Instance = new LabelRingTag(EngineFunc.SpriteEngine);
        Instance.IntMove = true;
        int TagNum = Wz.GetNode("Character/Ring/" + ItemID + ".img/info/nameTag").ToInt();
        Instance.Entry = Wz.GetNode("UI/NameTag.img/" + TagNum);
        Wz.DumpData(Instance.Entry, Wz.EquipData, Wz.EquipImageLib);
        Instance.MedalName = "SuperGM";
        Instance.InitData();
    }

    public override void DoDraw()
    {
        if (Map.ShowPlayer)
        {
            int WX = (int)(Game.Player.X) - (int)(Engine.Camera.X);
            int WY = (int)(Game.Player.Y) - (int)(Engine.Camera.Y);
            Engine.Canvas.Draw(TargetTexture, WX - 150, WY - 31, BlendMode.NonPremultiplied);
        }
        if (IsReDraw)
            IsReDraw = false;
    }
    public static void ReDraw()
    {
        if (Instance != null)
            Instance.IsReDraw = true;
    }

    public static void Delete()
    {
        if (Instance != null)
        {
            Instance.Dead();
            EngineFunc.SpriteEngine.Dead();
        }
    }


}