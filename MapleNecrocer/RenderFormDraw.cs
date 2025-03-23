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
using GameUI;
using MouseExt;
using System.Runtime.InteropServices;

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
    static float TimeDelta;

    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hdc);
    [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
    static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hdc);
    [DllImport("gdi32.dll")]
    static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    protected override void Initialize()
    {
        //if (!CanDraw)
        // return;
        base.Initialize();
        this.AlwaysEnableKeyboardInput = true;

        this.Editor.services.AddService<Random>(new Random());
        this.Editor.services.AddService<IRandom>(new ParticleRandom(Editor.services.GetService<Random>()));
        this.SetMultiSampleCount(0);

        EngineFunc.AddFont(this.GraphicsDevice, "Arial13", "Arial", 13f);
        //GMS font
        EngineFunc.AddD2DFont("Arial14", "Arial", 14f);
        EngineFunc.AddD2DFont("Arial13", "Arial", 13f);
        EngineFunc.AddD2DFont("Arial12", "Arial", 12f);
        EngineFunc.AddD2DFont("Arial10", "Arial", 10f);
        //TMS font
        EngineFunc.AddFont(this.GraphicsDevice, "Verdana11", "Verdana", 11f);
        EngineFunc.AddFont(this.GraphicsDevice, "SimSun13", "SimSun", 13f);
        EngineFunc.AddFont(this.GraphicsDevice, "SimSun14", "SimSun", 14f);
        EngineFunc.AddFont(this.GraphicsDevice, "Verdana9", "Verdana", 9f);
        //JMS font
        EngineFunc.AddFont(this.GraphicsDevice, "MSGothic11", "MS Gothic", 11f);
        EngineFunc.AddFont(this.GraphicsDevice, "MSGothic12", "MS Gothic", 12f);
        EngineFunc.AddFont(this.GraphicsDevice, "MSGothic14", "MS Gothic", 14f);
        ScreenRenderTarget = new RenderTarget2D(this.GraphicsDevice, 4000, 4000,
                                                   false, SurfaceFormat.Color, DepthFormat.None);
        //kms
        //EngineFunc.AddD2DFont("Arial12", "Arial12", 12f);
        //EngineFunc.AddD2DFont("Arial13", "Arial13", 12f);
        MouseEx.PlatformSetWindowHandle(RenderFormDraw.Instance.Handle);
        IntPtr hdc = GetDC(IntPtr.Zero);
        TimeDelta = (float)1 / GetDeviceCaps(hdc, 116);
        ReleaseDC(IntPtr.Zero, hdc);

    }
    private static Vector2 NewPos, CurrentPos;

    void UpdateGame()
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
        EngineFunc.SpriteEngine.Move(1);
        //   EngineFunc.SpriteEngine.Camera.X+=0.2f*(float)(gameTime.ElapsedGameTime.TotalMilliseconds/16.66f);

        if (MapleChair.IsUse)
        {
            if (Keyboard.KeyPressed(Input.Left) || Keyboard.KeyPressed(Input.Right))
            {
                MapleChair.Remove();
                TamingMob.Remove();
                ItemEffect.Remove(EffectType.Chair);
                MapleChair.BodyRelMove.X = 0;
                MapleChair.BodyRelMove.Y = 0;
            }
        }

        if (ScreenMode == ScreenMode.Scale || ScreenMode == ScreenMode.FullScreen)
        {
            this.GraphicsDevice.SetRenderTarget(ScreenRenderTarget);
            EngineFunc.SpriteEngine.Draw();
            if (Map.FadeScreen.DoFade)
            {
                EngineFunc.Canvas.FillRoundRect(0, 0, Map.DisplaySize.X, Map.DisplaySize.Y,
                    new Microsoft.Xna.Framework.Color(0, 0, 0, Map.FadeScreen.AlphaCounter));
            }
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

        if (ScreenMode == ScreenMode.FullScreen && Keyboard.KeyPressed(Input.Escape))
        {
            RenderForm.Instance.TopLevel = false;
            RenderForm.Instance.Parent = MainForm.Instance;
            MainForm.SetScreenNormal();
            System.Drawing.Rectangle Rect = new(MainForm.Instance.Left + 257, MainForm.Instance.Top + 93,
              Map.DisplaySize.X, Map.DisplaySize.Y);
            if (Rect.Contains(new System.Drawing.Point(Cursor.Position.X, Cursor.Position.Y)))
                Cursor.Show();
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

    private static float FixedUpdateDelta = 0.016666668f;
    private static float PreviousTime = 0;
    private static float Accumulator = 0.0f;
    protected override void Update(GameTime gameTime)
    {
        if (PreviousTime == 0)
        {
            PreviousTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
        }

        float Now = (float)gameTime.TotalGameTime.TotalMilliseconds;
        float FrameTime = Now - PreviousTime;

        if (FrameTime > TimeDelta)
        {
            FrameTime = TimeDelta;
        }

        PreviousTime = Now;
        Accumulator += FrameTime;
        while (Accumulator >= FixedUpdateDelta)
        {
            UpdateGame();
            Accumulator -= FixedUpdateDelta;
        }

    }

    protected override void Draw()
    {
        this.Editor.graphics.Clear(Microsoft.Xna.Framework.Color.Black);
        EngineFunc.SpriteEngine.Dead();
        // EngineFunc.SpriteEngine.Draw();

        switch (ScreenMode)
        {
            case ScreenMode.Normal:
                EngineFunc.SpriteEngine.Draw();
                if (Map.FadeScreen.DoFade)
                {
                    EngineFunc.Canvas.FillRoundRect(0, 0, Map.DisplaySize.X, Map.DisplaySize.Y,
                        new Microsoft.Xna.Framework.Color(0, 0, 0, Map.FadeScreen.AlphaCounter));
                }
                if (Map.ShowBgmName)
                {
                    EngineFunc.Canvas.DrawString("Arial13", Map.BgmName, 35, 35, Microsoft.Xna.Framework.Color.Red);
                }
                if (Map.ShowFootholds)
                {
                    FootholdTree.Instance.DrawFootholds();
                }
                break;
            case ScreenMode.Scale:
                EngineFunc.Canvas.DrawStretch(ScreenRenderTarget, ScaleForm.ScaleX, ScaleForm.ScaleY, Map.DisplaySize.X, Map.DisplaySize.Y, MonoGame.SpriteEngine.BlendMode.NonPremultiplied2);
                if (ScaleForm.UseScanline)
                    EngineFunc.Canvas.Draw(ScaleForm.ScanlineTexture4096, 0, 0, MonoGame.SpriteEngine.BlendMode.Multiply);
                break;
            case ScreenMode.FullScreen:
                EngineFunc.Canvas.DrawStretch(ScreenRenderTarget, Map.ScreenWidth, Map.ScreenHeight, Map.DisplaySize.X, Map.DisplaySize.Y, MonoGame.SpriteEngine.BlendMode.NonPremultiplied2);
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

        if (UI.ControlManager != null)
        {
            // var m = MouseEx.GetState();
            UI.ControlManager.Update();
            UI.ControlManager.Draw();
            //  EngineFunc.SpriteEngine.Canvas.Draw(Wz.EquipImageLib[Wz.GetNode("UI/UIWindow.img/Shop/backgrnd")],
            //  m.X, m.Y);
        }

        if (Map.FirstLoaded)
        {
            GameCursor.Draw();
        }

    }
    protected override void OnMouseEnter(EventArgs e)
    {
        if (Map.FirstLoaded)
            Cursor.Hide();
        MainForm.Instance.ToolTipView.Visible = false;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        if (Map.FirstLoaded)
            Cursor.Show();
    }


}

