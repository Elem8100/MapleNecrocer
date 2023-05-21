namespace WzComparerR2.MapRender2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.MapRender.Patches3;
using WzComparerR2.MapRender2;
using MapleNecrocer;
using Microsoft.Xna.Framework;
public class ParticleRender
{
    public RenderEnv renderEnv;
    public MeshBatcher batcher;
    public ParticleData ParticleData;
    public void DrawParticle1(float X, float Y)
    {
        if (this.ParticleData == null)
        {
            return;
        }
        this.batcher.Begin(Matrix.CreateTranslation(X, Y, 0));
        var Item = GetMesh(ParticleData.ParticleItem1);
        this.batcher.Draw(Item);
        this.batcher.End();
    }

    public void Update1(TimeSpan elapsed)
    {
        ParticleData.ParticleItem1.View?.ParticleSystem.Update(elapsed);
    }
   
    private MeshItem GetMesh(SceneItem item)
    {
        if (item is ParticleItem)
        {
            return GetMeshParticle((ParticleItem)item);
        }
        return null;
    }

    private MeshItem GetMeshParticle(ParticleItem particle)
    {
        var pSystem = particle.View?.ParticleSystem;
        if (pSystem == null)
        {
            return null;
        }

        Vector2 position;
        position.X = renderEnv.Camera.Center.X * (100 + particle.Rx) / 100;
        position.Y = renderEnv.Camera.Center.Y * (100 + particle.Ry) / 100;

        var mesh = batcher.MeshPop();
        mesh.RenderObject = pSystem;
        mesh.Position = position;
        mesh.Z0 = particle.Z;

        return mesh;
    }


}

