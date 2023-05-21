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

namespace MapleNecrocer;

public class AvatarFormDraw : MonoGameControl
{
    public AvatarFormDraw()
    {
        Instance = this;
    }
    public static AvatarFormDraw Instance;
    public static RenderTarget2D AvatarPanelTexture;
    private static RenderTarget2D CheckBoardTexture;

    protected override void Initialize()
    {
        
        base.Initialize();
        this.AlwaysEnableKeyboardInput = true;
        EngineFunc.Canvas.DrawTarget(ref CheckBoardTexture, 260, 200, () =>
        {
            for (int J = 0; J < 200; J++)
            {
                for (int I = 0; I < 260; I++)
                {
                    if ((I == 0) || (J == 0) || (I == 259) || (J == 199))
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


    protected override void Update(GameTime gameTime)
    {
        

        EngineFunc.Canvas.GraphicsDevice.SetRenderTarget(AvatarPanelTexture);
        EngineFunc.Canvas.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent);
        EngineFunc.SpriteEngine.DrawEx("Player");
        EngineFunc.Canvas.GraphicsDevice.SetRenderTarget(null);
    }

    //  public static float xx;
    protected override void Draw()
    {
        this.Editor.graphics.Clear(Microsoft.Xna.Framework.Color.Black);

        EngineFunc.Canvas.Draw(CheckBoardTexture, 0, 0);
        int WX = (int)(Player.Instance.X - EngineFunc.SpriteEngine.Camera.X - 130 + MapleChair.BodyRelMove.X - TamingMob.Navel.X);
        int WY = (int)(Player.Instance.Y - EngineFunc.SpriteEngine.Camera.Y - 160 + MapleChair.BodyRelMove.Y - TamingMob.Navel.Y);
        EngineFunc.Canvas.DrawCropArea(AvatarPanelTexture, 0, 0, new Microsoft.Xna.Framework.Rectangle(WX, WY, WX + 280, WY + 200), 0, 0, 1, 1, 0, false, false, 255, 255, 255, 255, false);
    }

}

