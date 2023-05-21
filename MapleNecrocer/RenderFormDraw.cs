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

namespace MapleNecrocer;

public class RenderFormDraw : MonoGameControl
{
    public RenderFormDraw()
    {
        Instance = this;
    }
    public static RenderFormDraw Instance;

    public static bool CanDraw;
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

        //kms
        //EngineFunc.AddD2DFont("Arial12", "Arial12", 12f);
        //EngineFunc.AddD2DFont("Arial13", "Arial13", 12f);

    }
    private static Vector2 NewPos, CurrentPos;

    protected override void Update(GameTime gameTime)
    {

      
        /*
        Keyboard.GetState();
        if (Keyboard.KeyDown(Input.Right))
            EngineFunc.SpriteEngine.Camera.X += 5.7f;

        if (Keyboard.KeyDown(Input.Left))
            EngineFunc.SpriteEngine.Camera.X -= 5.3f;
        if (Keyboard.KeyDown(Input.Up))
            EngineFunc.SpriteEngine.Camera.Y -= 5.4f;
        if (Keyboard.KeyDown(Input.Down))
            EngineFunc.SpriteEngine.Camera.Y += 5.2f;
        */

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


    }

    //  public static float xx;
    protected override void Draw()
    {
        // if (!CanDraw)
        //  return;
       

        this.Editor.graphics.Clear(Microsoft.Xna.Framework.Color.Black);
        EngineFunc.SpriteEngine.Dead();
        EngineFunc.SpriteEngine.Draw();

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

