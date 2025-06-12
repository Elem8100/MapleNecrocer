using Microsoft.Xna.Framework;
using MonoGame.Forms.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using DevComponents.DotNetBar;

using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using WzComparerR2.CharaSim;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MapleNecrocer;

public class FrameListDraw : MonoGameControl
{
    public FrameListDraw()
    {
        Instance = this;
    }
    public static FrameListDraw Instance;
    public static RenderTarget2D AvatarPanelTexture;
    private static RenderTarget2D CheckBoardTexture;

    protected override void Initialize()
    {

        base.Initialize();

        EngineFunc.Canvas.DrawTarget(ref CheckBoardTexture, 512, 512, () =>
        {
            for (int J = 0; J < 512; J++)
            {
                for (int I = 0; I < 512; I++)
                {
                    if ((I == 0) || (J == 0) || (I == 511) || (J == 511))
                        EngineFunc.Canvas.Pixel(I, J, new Color(0, 0, 0));
                    else if (((I / 8) + (J / 8)) % 2 == 0)  // put checkboard pattern
                        EngineFunc.Canvas.Pixel(I, J, new Color(205, 205, 205));
                    else
                        EngineFunc.Canvas.Pixel(I, J, new Color(255, 255, 255));
                }
            }
        });

        EngineFunc.Canvas.DrawTarget(ref AvatarPanelTexture, 4096, 4096, () => { });
        this.SetMultiSampleCount(0);
    }
    int xx;
    protected override void Update(GameTime gameTime)
    {
        EngineFunc.Canvas.GraphicsDevice.SetRenderTarget(AvatarPanelTexture);
        EngineFunc.Canvas.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent);
        EngineFunc.SpriteEngine.DrawEx("Player", "ItemEffect", "SetEffect");
        EngineFunc.Canvas.GraphicsDevice.SetRenderTarget(null);
       
    }

    protected override void Draw()
    {
        EngineFunc.Canvas.Draw(CheckBoardTexture, 0, 0);
        // Editor.graphics.Clear(Color.Aqua);
        int WX = (int)(Game.Player.X - EngineFunc.SpriteEngine.Camera.X - 155);
        int WY = (int)(Game.Player.Y - EngineFunc.SpriteEngine.Camera.Y - 160);
        EngineFunc.Canvas.DrawCropArea(AvatarPanelTexture, 100, 150, new Microsoft.Xna.Framework.Rectangle(WX, WY, WX + 250, WY + 230), 0, 0, 1, 1, 0, false, false, 255, 255, 255, 255, false, BlendMode.NonPremultiplied2);
        EngineFunc.Canvas.DrawRectangle(100 + AvatarForm.AdjustX, 150 + AvatarForm.AdjustY, AvatarForm.AdjustW, AvatarForm.AdjustH,
              new Color(255,0, 0));
    }

}

