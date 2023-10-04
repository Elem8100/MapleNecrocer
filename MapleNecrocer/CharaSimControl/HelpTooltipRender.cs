using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Resource = CharaSimResource.Resource;
using WzComparerR2.PluginBase;
using WzComparerR2.WzLib;
using WzComparerR2.Common;
using WzComparerR2.CharaSim;

namespace WzComparerR2.CharaSimControl
{
    public class HelpTooltipRender : TooltipRender
    {
        public HelpTooltipRender()
        {
        }

        public TooltipHelp Pair { get; set; }

        public override object TargetItem
        {
            get { return this.Pair; }
            set { this.Pair = value as TooltipHelp; }
        }

        public override Bitmap Render()
        {
            int picHeight;
            Bitmap originBmp = RenderHelp(out picHeight);
            Bitmap tooltip = new Bitmap(originBmp.Width, picHeight);
            Graphics g = Graphics.FromImage(tooltip);

            //绘制背景区域
            GearGraphics.DrawNewTooltipBack(g, 0, 0, tooltip.Width, tooltip.Height);

            //复制图像
            g.DrawImage(originBmp, 0, 0, new Rectangle(0, 0, tooltip.Width, picHeight), GraphicsUnit.Pixel);

            if (originBmp != null)
                originBmp.Dispose();

            g.Dispose();
            return tooltip;
        }

        private Bitmap RenderHelp(out int picH)
        {
            Bitmap helpBitmap = new Bitmap(270, DefaultPicHeight);
            Graphics g = Graphics.FromImage(helpBitmap);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;

            picH = 10;
            TextRenderer.DrawText(g, Pair.Title, GearGraphics.ItemNameFont2, new Point(helpBitmap.Width, 10), Color.White, TextFormatFlags.HorizontalCenter);
            picH += 22;

            GearGraphics.DrawString(g, string.Format(Pair.Desc, 0), GearGraphics.ItemDetailFont2, 10, 252, ref picH, 16);

            picH += 4;
            format.Dispose();
            g.Dispose();
            return helpBitmap;
        }
    }
}
