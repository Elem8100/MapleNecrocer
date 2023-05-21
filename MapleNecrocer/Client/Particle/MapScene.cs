using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WzComparerR2.MapRender.Patches3;

namespace WzComparerR2.MapRender2
{
    public class MapScene : SceneNode
    {
        public MapScene()
        {
            this.Nodes.AddRange(new SceneNode[] {
            
                Effect = new ContainerNode()
            });

            for (int i = 0; i <= 7; i++)
            {
              //  this.Layers.Nodes.Add(new LayerNode());
            }
        }

        public ContainerNode Back { get; private set; }
        public SceneNode Layers { get; private set; }
        public FlyLayerNode Fly { get; private set; }
        public ContainerNode Front { get; private set; }
        public ContainerNode Effect { get; private set; }

       // public IEnumerable<PortalItem> Portals => this.Fly.Portal.Slots.OfType<PortalItem>();
      //  public IEnumerable<IlluminantClusterItem> IlluminantClusters => this.Fly.IlluminantCluster.Slots.OfType<IlluminantClusterItem>();
       
    }

    public class LayerNode : SceneNode
    {
        public LayerNode()
        {
            this.Nodes.AddRange(new SceneNode[]
            {
                Obj = new ContainerNode(),
                Reactor = new ContainerNode(),
                Tile = new ContainerNode(),
                Foothold = new SceneNode()
            });
        }

        public ContainerNode Obj { get; private set; }
        public ContainerNode Reactor { get; private set; }
        public ContainerNode Tile { get; private set; }
        public SceneNode Foothold { get; private set; }
    }

    public class FlyLayerNode : SceneNode
    {
        public FlyLayerNode()
        {
            this.Nodes.AddRange(new SceneNode[]
            {
                Portal = new ContainerNode(),
                LadderRope = new ContainerNode(),
                Sky = new ContainerNode(),
                SkyWhale = new ContainerNode(),
                IlluminantCluster = new ContainerNode(),
            });
        }


        public ContainerNode Portal { get; private set; }
        public ContainerNode LadderRope { get; private set; }
        /// <summary>
        /// 表示不属于任何Foothold的虚拟节点。
        /// </summary>
        public ContainerNode Sky { get; private set; }
        public ContainerNode SkyWhale { get; private set; }
        public ContainerNode IlluminantCluster { get; private set; }
    }

    public class ContainerNode : SceneNode
    {
        public ContainerNode()
        {
            this.Slots = new List<SceneItem>();
        }

        public List<SceneItem> Slots { get; private set; }
    }

    public class ContainerNode<T> : ContainerNode
    {
        public T Item { get; set; }
    }
}
