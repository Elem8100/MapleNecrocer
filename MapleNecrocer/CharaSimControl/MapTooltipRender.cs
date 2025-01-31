using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WzComparerR2.CharaSim;
using WzComparerR2.Common;
using static WzComparerR2.CharaSimControl.RenderHelper;
using System.Windows.Forms;
using DevComponents.AdvTree;
using MapleNecrocer;
using Map=WzComparerR2.CharaSim.Map;

namespace WzComparerR2.CharaSimControl;

public class MapTooltipRenderer : TooltipRender
{
    public MapTooltipRenderer()
    {

    }

    public override object TargetItem
    {
        get { return this.MapInfo; }
        set { this.MapInfo = value as Map; }
    }

    public Map MapInfo { get; set; }
    static StringLinker StringLinker = MainForm.Instance.stringLinker;
    public override Bitmap Render()
    { 
        Bitmap bmp = null;
        Bitmap MiniMapPic = null;

        var Link = Wz.GetNode(Map.ImgNode.FullPathToFile2() + "/info/link");
        string MapID;
        if (Link == null)
            MapID = Map.ImgNode.ImgID();
        else
            MapID = Link.Value.ToString();

        var LinkImgNode = Wz.GetNode("Map/Map/Map" + MapID.LeftStr(1) + "/" + MapID + ".img");

     
        if (LinkImgNode.HasNode("miniMap"))
        {
            MiniMapPic = LinkImgNode.GetBmp("miniMap/canvas");
        }
        else
        {
            MiniMapPic = new Bitmap(150, 150);
        }

        StringResult sr;
        StringLinker.StringMap.TryGetValue(Map.ImgNode.ImgID().ToInt(), out sr);
        var font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
        if (sr==null) return null;

        int W1 = TextRenderer.MeasureText(sr["streetName"], font).Width + 10;
        int W2 = TextRenderer.MeasureText(sr["mapName"], font).Width + 10;
        int W3 = MiniMapPic.Width - 40;
        int MaxWidth = Math.Max(Math.Max(W1, W2), W3);
        
        bmp = new(MaxWidth + 50, MiniMapPic.Height + 50);
        Graphics g = Graphics.FromImage(bmp);
        GearGraphics.DrawNewTooltipBack(g, 0, 0, MaxWidth + 50, MiniMapPic.Height + 50);
        int OffX = (MaxWidth - W3) / 2;
        g.DrawImage(MiniMapPic, 5 + OffX, 45);

        TextRenderer.DrawText(g, sr["streetName"], font, new Point(50, 9), Color.White, TextFormatFlags.Left);
        TextRenderer.DrawText(g, sr["mapName"], font, new Point(50, 29), Color.White, TextFormatFlags.Left);

        var MapMarkName = Map.ImgNode.GetStr("info/mapMark");
        if (MapMarkName != "None")
        {
            var MapMarkPic = Wz.GetBmp("Map/MapHelper.img/mark/" + MapMarkName);
            g.DrawImage(MapMarkPic, 8, 8);
        }

        return bmp;
    }

}
