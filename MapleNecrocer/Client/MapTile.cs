using System.IO;
using WzComparerR2.WzLib;

namespace MapleNecrocer;
public class MapTile : SpriteEx
{
    public MapTile(Sprite Parent) : base(Parent)
    {
        IntMove = true;
    }

    public static void Create()
    {
        for (int Layer = 0; Layer <= 7; Layer++)
        {
            string tS = Map.Img.GetNode(Layer.ToString() + "/info").GetValue2("tS", "");

            foreach (var Iter in Map.Img.GetNode(Layer.ToString() + "/tile").Nodes)
            {
                string u = Iter.GetValue2("u", "");
                string no = Iter.GetValue2("no", "");
                string Path = "Map/Tile/" + tS + ".img/" + u + "/" + no;
                if (!Wz.Data.ContainsKey(Path))
                {
                    Wz.DumpData(Wz.GetNodeA(Path), Wz.Data, Wz.ImageLib);
                }
                var MapTile = new MapTile(EngineFunc.SpriteEngine);
                MapTile.ImageLib = Wz.ImageLib;
                MapTile.ImageNode = Wz.Data[Path];
                MapTile.Moved = false;
                MapTile.X = Iter.GetValue2("x", 0);
                MapTile.Y = Iter.GetValue2("y", 0);
                MapTile.Z = (Layer * 100000) + Wz.GetInt(Path + "/z") + 1000;
                MapTile.Width = MapTile.ImageWidth;
                MapTile.Height = MapTile.ImageHeight;
                MapTile.Origin = Wz.GetVector(Path + "/origin");
               
            }
        }
    }
    public override void DoDraw()
    {
        if (Map.ShowTile)
            base.DoDraw();
        /*
                if(Map.CameraSpeed.X==0)
                    IntMove=true;
                else
                    IntMove=false;
        */
    }

}
