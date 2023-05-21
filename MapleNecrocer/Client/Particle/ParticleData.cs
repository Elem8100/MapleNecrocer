using WzComparerR2.WzLib;
using WzComparerR2.Common;
using WzComparerR2.MapRender.Patches3;
using WzComparerR2.PluginBase;
using Microsoft.Xna.Framework;

namespace WzComparerR2.MapRender2
{
    public class ParticleData
    {
        public ParticleData(IRandom random)
        {
            this.Scene = new MapScene();
            this.random = random;
        }

        public MapScene Scene { get; private set; }

        private readonly IRandom random;
        public ParticleItem ParticleItem1;

        public void LoadParticleData1(Wz_Node Node, ResourceLoader resLoader)
        {
            LoadParticle1(Node);
            PreloadResource(resLoader, ParticleItem1);
        }

        private void LoadParticle1(Wz_Node Node)
        {
            ParticleItem1 = ParticleItem.LoadFromNode(Node);
            ParticleItem1.Name = Node.Text;
        }

        private void PreloadResource(ResourceLoader resLoader, ParticleItem particle)
        {
            string path = $@"Effect\particle.img\{particle.ParticleName}";
            var particleNode = PluginManager.FindWz(path);

            if (particleNode == null)
            {
                return;
            }

            var desc = resLoader.LoadParticleDesc(particleNode);

            var pSystem = new ParticleSystem(this.random);

            pSystem.LoadDescription(desc);

            for (int i = 0; i < particle.SubItems.Length; i++)
            {
                var subItem = particle.SubItems[i];

                var pGroup = pSystem.CreateGroup(i.ToString());
                pGroup.Position = new Vector2(subItem.X, subItem.Y);
                pGroup.Active();
                pSystem.Groups.Add(pGroup);

            }

            if (pSystem.Groups.Count == 0)
            {
                pSystem = new ParticleSystem(this.random);
            }

            particle.View = new ParticleItem.ItemView()
            {
                ParticleSystem = pSystem
            };
        }

    }
}
