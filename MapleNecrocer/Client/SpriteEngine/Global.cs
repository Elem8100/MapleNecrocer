using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WzComparerR2.Rendering;
using System.Drawing;
using MapleNecrocer;

namespace MonoGame.SpriteEngine;

public class EngineFunc
{
    public static MonoSpriteEngine SpriteEngine;
    public static MonoSpriteEngine BackgroundEngine;
    public static GameCanvas Canvas;

    public static Dictionary<string, XnaFont> Fonts = new();
    public static Dictionary<string, D2DFont> D2DFonts = new();
    private static float FixedUpdateDelta = 0.016666f;
    // helper variables for the fixed update
    private static float PreviousTime = 0;
    private static float Accumulator = 0.0f;
    private static float ALPHA = 0;



    public static void FixedUpdate(GameTime gameTime, params Action[] FuncArray)
    {
        if (PreviousTime == 0)
        {
            PreviousTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
        }

        float Now = (float)gameTime.TotalGameTime.TotalMilliseconds;
        float FrameTime = Now - PreviousTime;
        if (FrameTime > 0.016666f)
        {
            FrameTime = 0.016666f;
        }

        PreviousTime = Now;
        Accumulator += FrameTime;
        while (Accumulator >= FixedUpdateDelta)
        {

            for (int i = 0; i < FuncArray.Length; i++)
                FuncArray[i]();
            Accumulator -= FixedUpdateDelta;
        }
    }

    static EngineFunc()
    {
        SpriteEngine = new MonoSpriteEngine(null);
        BackgroundEngine = new MonoSpriteEngine(null);
        Canvas = new GameCanvas(RenderFormDraw.Instance.GraphicsDevice);
        SpriteEngine.Canvas = Canvas;
        BackgroundEngine.Canvas = Canvas;

    }

    public static void AddFont(GraphicsDevice GraphicsDevice, string KeyName, string FontName, float Size)
    {
        var Font = new XnaFont(GraphicsDevice, new Font(FontName, Size, GraphicsUnit.Pixel));
       // var Font = new XnaFont(GraphicsDevice, new Font(FontName, Size));
        Fonts.Add(KeyName, Font);
    }

    public static void AddD2DFont(string KeyName, string FontName, float Size)
    {
        var Font = new D2DFont(FontName, Size);
        D2DFonts.Add(KeyName, Font);

    }
}
