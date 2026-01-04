using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.Animation;

namespace MapleNecrocer;

public class ObjToolTip : SpriteEx
{
    public ObjToolTip(Sprite Parent) : base(Parent)
    {
    }
    public string Text;
    string Path2;
    RenderTarget2D RenderTarget;
    public void Init(string AText)
    {


        if (Wz.HasNode("UI/UIToolTip.img"))
        {
            if (Wz.HasNode("UI/UIToolTip.img/Item/Frame2/n"))
            {
                Path2 = "Frame2";
                if (!Wz.HasData("UI/UIToolTip.img/Item/Frame2/n"))
                    Wz.DumpData(Wz.GetNode("UI/UIToolTip.img/Item/Frame2"), Wz.Data, Wz.ImageLib);
            }

            if (Wz.HasNode("UI/UIToolTip.img/Item/Common/frame"))
            {
                Path2 = "Common/frame";
                if (!Wz.HasData("UI/UIToolTip.img/Item/Common/frame/n"))
                    Wz.DumpData(Wz.GetNode("UI/UIToolTip.img/Item/Common/frame"), Wz.Data, Wz.ImageLib);
            }

        }
        Text = AText;
        Engine.Canvas.DrawTarget(ref RenderTarget, 280, 80, () => RenderTargetFunc());
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        float DistanceX = Math.Abs(Game.Player.X - X);
        if (DistanceX < 150)
            Visible = true;
        else
            Visible = false;
    }

    public override void DoDraw()
    {
        Engine.Canvas.Draw(RenderTarget, (int)X - (int)Engine.Camera.X, (int)Y - (int)Engine.Camera.Y);
    }
    int Width => Map.MeasureStringX(Map.ToolTipFont, Text);
    public void RenderTargetFunc()
    {
        if (Wz.HasNode("UI/UIToolTip.img"))
        {
            if (Path2 == "Frame2")
            {
                var nw = Wz.GetImgNodeA("UI/UIToolTip.img/Item/Frame2/nw");
                Engine.Canvas.Draw(Wz.ImageLib[nw], 15 - nw.GetVector("origin").X, 15 - nw.GetVector("origin").Y);

                var ne = Wz.GetImgNodeA("UI/UIToolTip.img/Item/Frame2/ne");
                Engine.Canvas.Draw(Wz.ImageLib[ne], 15 + Width - ne.GetVector("origin").X, 15 - ne.GetVector("origin").Y);

                var n = Wz.GetImgNodeA("UI/UIToolTip.img/Item/Frame2/n");
                var s = Wz.GetImgNodeA("UI/UIToolTip.img/Item/Frame2/s");
                for (int i = 0; i < Width; i++)
                {
                    Engine.Canvas.Draw(Wz.ImageLib[n], 15 - n.GetVector("origin").X + i, 15 - n.GetVector("origin").Y);
                    Engine.Canvas.Draw(Wz.ImageLib[s], 15 - n.GetVector("origin").X + i, 15 - s.GetVector("origin").Y);
                }

                var sw = Wz.GetImgNodeA("UI/UIToolTip.img/Item/Frame2/sw");
                Engine.Canvas.Draw(Wz.ImageLib[sw], 15 - sw.GetVector("origin").X, 15 - sw.GetVector("origin").Y);

                var se = Wz.GetImgNodeA("UI/UIToolTip.img/Item/Frame2/se");
                Engine.Canvas.Draw(Wz.ImageLib[se], 15 + Width - se.GetVector("origin").X, 15 - se.GetVector("origin").Y);
                Engine.Canvas.DrawString(Map.ToolTipFont, Text, 15, 8, Microsoft.Xna.Framework.Color.White);
            }

            if (Path2 == "Common/frame")
            {
                var nw = Wz.GetNodeA("UI/UIToolTip.img/Item/" + Path2 + "/nw");
                Engine.Canvas.Draw(Wz.ImageLib[nw], 7, 7);

                var ne = Wz.GetNodeA("UI/UIToolTip.img/Item/" + Path2 + "/ne");
                Engine.Canvas.Draw(Wz.ImageLib[ne], 15 + Width - 7, 15 - 7);

                var n = Wz.GetImgNodeA("UI/UIToolTip.img/Item/" + Path2 + "/n");
                var s = Wz.GetImgNodeA("UI/UIToolTip.img/Item/" + Path2 + "/s");
                for (int i = 0; i < Width - 13; i++)
                {
                    Engine.Canvas.Draw(Wz.ImageLib[n], 21 + i, 15 - n.GetVector("origin").Y);
                    Engine.Canvas.Draw(Wz.ImageLib[s], 21 + i, 15 - s.GetVector("origin").Y + 14);
                }

                var sw = Wz.GetNodeA("UI/UIToolTip.img/Item/" + Path2 + "/sw");
                Engine.Canvas.Draw(Wz.ImageLib[sw], 7, 21);

                var se = Wz.GetNodeA("UI/UIToolTip.img/Item/" + Path2 + "/se");
                Engine.Canvas.Draw(Wz.ImageLib[se], 8 + Width, 22);

                Engine.Canvas.DrawString(Map.ToolTipFont, Text, 15, 13, Microsoft.Xna.Framework.Color.White);
            }
        }
        else
        {
            Engine.Canvas.FillRoundRect(15, 5, Width + 10, 20, new Microsoft.Xna.Framework.Color(0, 50, 150, 180));
            Engine.Canvas.DrawString(Map.ToolTipFont, Text, 20, 8, Microsoft.Xna.Framework.Color.White);
        }

    }

    public static void Create()
    {

        if (!Map.Img.HasNode("ToolTip"))
            return;
        foreach (var Iter in Map.Img.GetNodes("ToolTip"))
        {

            if (Iter.Text.Length > 4)
                continue;
            var ToolTip = new ObjToolTip(EngineFunc.SpriteEngine);

            string Title = Wz.GetStr("String/ToolTipHelp.img/Mapobject/" + Map.Img.ImgID().ToInt() + "/" + Iter.Text + "/Title");
            ToolTip.Init(Title);
            int Mid = (Iter.GetInt("x1") + Iter.GetInt("x2")) / 2 - 20;

            ToolTip.X = Mid - (ToolTip.Width / 2);
            ToolTip.Y = Iter.GetInt("y1");
            ToolTip.Z = 10000000;
        }
    }

}



