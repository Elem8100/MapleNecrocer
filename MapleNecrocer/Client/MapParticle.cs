using WzComparerR2.WzLib;
using WzComparerR2.PluginBase;
using Spine;
using WzComparerR2.Animation;
using Microsoft.Xna.Framework;
using WzComparerR2.MapRender2;
namespace MapleNecrocer;

public class Particle : SpriteEx
{
    int RX, RY;
    Vector2 Pos;
    public static bool ResetPos;
    public ParticleRender ParticleRender;
    public Particle(Sprite Parent) : base(Parent)
    {
    }

    public static void Create()
    {
     
        if (Map.Img.Nodes["particle"] == null)
            return;
        foreach (var Iter in Map.Img.Nodes["particle"].Nodes)
        {
            var Particle = new Particle(EngineFunc.SpriteEngine);
            Particle.ParticleRender = new ParticleRender();
            Particle.ParticleRender.ParticleData = new ParticleData(RenderFormDraw.Instance.Editor.services.GetService<IRandom>());
            Particle.ParticleRender.batcher = new WzComparerR2.MapRender2.MeshBatcher(RenderFormDraw.Instance.GraphicsDevice) { CullingEnabled = true };
            Particle.ParticleRender.renderEnv = new RenderEnv(RenderFormDraw.Instance.GraphicsDevice);
            Particle.ParticleRender.ParticleData.LoadParticleData1(Iter, Map.ResLoader);
            int Z = 0;
            if (Iter.Nodes["rx"] != null)
            {
                Particle.RX = Iter.GetInt("rx");
                Particle.RY = Iter.GetInt("ry");
                Z = Iter.GetInt("z");
            }
            else if (Iter.GetNode("0/rx") != null)
            {
                Particle.RX = Iter.GetInt("0/rx");
                Particle.RY = Iter.GetInt("0/ry");
                if (Particle.RX == 0)
                    Particle.RX = -100;
                if (Particle.RY == 0)
                    Particle.RY = -100;
                Z = Iter.GetInt("0/z");
            }
            else
            {
                Particle.RX = -100;
                Particle.RY = -100;
            }
            // if (Z < 0)
            //  Z = 1000000 - 400000;
            // if (Z == 0)
            Z = 10000000;

            Particle.Z = Z;
        }
        ResetPos = true;
    }

    public override void DoMove(float Delta)
    {
        if (Map.CameraSpeed.X != 0)
            X += RX * Map.CameraSpeed.X / 100f;
        if (Map.CameraSpeed.Y != 0)
            Y += RY * Map.CameraSpeed.Y / 100f;

        if (ResetPos)
        {
            X = (100f + RX) / 100f * (Engine.Camera.X + Map.DisplaySize.X / 2) - Engine.Camera.X;
            Y = (100f + RY) / 100f * (Engine.Camera.Y + Map.DisplaySize.Y / 2 + Map.OffsetY) - Engine.Camera.Y;
        }
    }
    public override void DoDraw()
    {
        if (ParticleRender != null)
        {
            ParticleRender.Update1(TimeSpan.FromMilliseconds(17));
            ParticleRender.DrawParticle1(X, Y);
        }
        if (ResetPos) ResetPos = false;
    }

    public override void Draw()
    {
        if (Visible)
        {
            if (Engine != null)
            {
                DoDraw();
                Engine.DrawCount++;
            }

            if (DrawList != null)
            {
                for (int i = 0; i < DrawList.Count; i++)
                    ((Particle)DrawList[i]).Draw();
            }
        }
    }
    public override void Free()
    {
        base.Free();
        if (ParticleRender != null)
        {
            ParticleRender.ParticleData = null;
            ParticleRender.batcher.Dispose();
            ParticleRender.renderEnv.Dispose();
            ParticleRender = null;
        }
    }

}