using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Forms.Controls;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.Animation;
using WzComparerR2.MapRender2;
using WzComparerR2.WzLib;
using SpriteEngine;
using Input = Microsoft.Xna.Framework.Input.Keys;
using System.Security.Cryptography.Xml;
using System.Security.Claims;
using WzComparerR2.CharaSim;
using WzComparerR2;

namespace MapleNecrocer;
public enum ScreenMode { Normal, Scale, FullScreen }
public class RenderFormDraw : MonoGameControl
{
    public RenderFormDraw()
    {
        Instance = this;
    }
    public static RenderFormDraw Instance;

    public static bool CanDraw;
    public static ScreenMode ScreenMode = ScreenMode.Normal;
    static RenderTarget2D ScreenRenderTarget;
    protected override void Initialize()
    {
        //if (!CanDraw)
        // return;
        base.Initialize();
        this.AlwaysEnableKeyboardInput = true;

        this.Editor.services.AddService<Random>(new Random());
        this.Editor.services.AddService<IRandom>(new ParticleRandom(Editor.services.GetService<Random>()));
        this.SetMultiSampleCount(0);
        //GMS font
        EngineFunc.AddD2DFont("Arial13", "Arial", 13f);
        EngineFunc.AddD2DFont("Arial12", "Arial", 12f);
        EngineFunc.AddD2DFont("Arial10", "Arial", 10f);
        //TMS font
        EngineFunc.AddFont(this.GraphicsDevice, "Verdana11", "Verdana", 11f);
        EngineFunc.AddFont(this.GraphicsDevice, "SimSun13", "SimSun", 13f);
        EngineFunc.AddFont(this.GraphicsDevice, "Verdana9", "Verdana", 9f);
        //JMS font
        EngineFunc.AddFont(this.GraphicsDevice, "MSGothic11", "MS Gothic", 11f);
        EngineFunc.AddFont(this.GraphicsDevice, "MSGothic12", "MS Gothic", 12f);

        ScreenRenderTarget = new RenderTarget2D(this.GraphicsDevice, 4000, 4000,
                                                   false, SurfaceFormat.Color, DepthFormat.None);

        //kms
        //EngineFunc.AddD2DFont("Arial12", "Arial12", 12f);
        //EngineFunc.AddD2DFont("Arial13", "Arial13", 12f);

    }
    private static Vector2 NewPos, CurrentPos;

    protected override void Update(GameTime gameTime)
    {

        if (Map.GameMode == GameMode.Viewer)
        {
            if (Keyboard.KeyDown(Input.Right))
                EngineFunc.SpriteEngine.Camera.X += 5;
            if (Keyboard.KeyDown(Input.Left))
                EngineFunc.SpriteEngine.Camera.X -= 5;
            if (Keyboard.KeyDown(Input.Up))
                EngineFunc.SpriteEngine.Camera.Y -= 5;
            if (Keyboard.KeyDown(Input.Down))
                EngineFunc.SpriteEngine.Camera.Y += 5;
            if (EngineFunc.SpriteEngine.Camera.X > Map.Right - Map.DisplaySize.X)
                EngineFunc.SpriteEngine.Camera.X = Map.Right - Map.DisplaySize.X;
            if (EngineFunc.SpriteEngine.Camera.X < Map.Left)
                EngineFunc.SpriteEngine.Camera.X = Map.Left;
            if (EngineFunc.SpriteEngine.Camera.Y > Map.Bottom - Map.DisplaySize.Y)
                EngineFunc.SpriteEngine.Camera.Y = Map.Bottom - Map.DisplaySize.Y;
            if (EngineFunc.SpriteEngine.Camera.Y < Map.Top)
                EngineFunc.SpriteEngine.Camera.Y = Map.Top;
        }

        if (Map.ReLoad)
        {
            Map.LoadMap(Map.ID);
            Map.ReLoad = false;
        }
        //Keyboard.GetState();

        NewPos = EngineFunc.SpriteEngine.Camera;
        Map.CameraSpeed = NewPos - CurrentPos;
        CurrentPos = EngineFunc.SpriteEngine.Camera;
        EngineFunc.SpriteEngine.Move((float)(gameTime.ElapsedGameTime.TotalMilliseconds / 16.66));
        //   EngineFunc.SpriteEngine.Camera.X+=0.2f*(float)(gameTime.ElapsedGameTime.TotalMilliseconds/16.66f);

        if (MapleChair.IsUse)
        {
            if (Keyboard.KeyPressed(Input.Left) || Keyboard.KeyPressed(Input.Right))
            {
                MapleChair.Delete();
                TamingMob.Delete();
                ItemEffect.Delete(EffectType.Chair);
                MapleChair.BodyRelMove.X = 0;
                MapleChair.BodyRelMove.Y = 0;
            }
        }


        if (ScreenMode == ScreenMode.Scale)
        {
            this.GraphicsDevice.SetRenderTarget(ScreenRenderTarget);
            EngineFunc.SpriteEngine.Draw();
            if (Map.ShowBgmName)
            {
                EngineFunc.Canvas.DrawString("Arial13", Map.BgmName, 35, 35, Microsoft.Xna.Framework.Color.Red);
            }
            if (Map.ShowFootholds)
            {
                FootholdTree.Instance.DrawFootholds();
            }
            this.GraphicsDevice.SetRenderTarget(null);
        }

        if (Sound.PlayendList.Count == 100)
        {
            for (int i = 0; i < Sound.PlayendList.Count; i++)
            {
                if (Sound.PlayendList[i].State != PlayState.Playing)
                {
                    Sound.PlayendList[i].UnLoad();
                    Sound.PlayendList.RemoveAt(i);
                }
            }
        }

    }

    //  public static float xx;
    protected override void Draw()
    {

        this.Editor.graphics.Clear(Microsoft.Xna.Framework.Color.Black);
        EngineFunc.SpriteEngine.Dead();
        // EngineFunc.SpriteEngine.Draw();

        switch (ScreenMode)
        {
            case ScreenMode.Normal:
                EngineFunc.SpriteEngine.Draw();
                if (Map.ShowBgmName)
                {
                    EngineFunc.Canvas.DrawString("Arial13", Map.BgmName, 35, 35, Microsoft.Xna.Framework.Color.Red);
                }
                if(Map.ShowFootholds)
                {
                    FootholdTree.Instance.DrawFootholds();
                }
                break;
            case ScreenMode.Scale:
                EngineFunc.Canvas.DrawStretch(ScreenRenderTarget, ScaleForm.ScaleX, ScaleForm.ScaleY, Map.DisplaySize.X, Map.DisplaySize.Y);
                break;
        }


        if (Map.ResetPos)
        {
            Map.OffsetY = (Map.DisplaySize.Y - 600) / 2;
            Back.ResetPos = true;
            EngineFunc.SpriteEngine.Move(1);
            Particle.ResetPos = true;
            Map.ResetPos = false;
        }


        // MainForm.Instance.Text = EngineFunc.SpriteEngine.Camera.X.ToString();

    }

}

