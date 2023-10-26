using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using DPIUtils;
using MapleNecrocer;
namespace GraphicsExtension;

public static class GraphicsExtension
{
    public static float DPIScale = 96 * (float)DPIUtil.ScaleFactor(MainForm.Instance, new Point(0, 0));

    public static void DrawImage2(this Graphics g, Image Image, int X, int Y)
    {
        Bitmap Bmp = Image as Bitmap;
        Bmp.SetResolution(DPIUtil.dpiX, DPIUtil.dpiY);
        g.DrawImage(Bmp, X, Y);
    }
    public static void DrawImage2(
       this Graphics g,
       Image Image,
       Rectangle destRect,
       int srcX,
       int srcY,
       int srcWidth,
       int srcHeight,
       GraphicsUnit srcUnit,
       ImageAttributes? imageAttr)
    {
        Bitmap Bmp = Image as Bitmap;
        Bmp.SetResolution(DPIUtil.dpiX, DPIUtil.dpiY);
        g.DrawImage(Bmp, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr, null);
    }

    public static void DrawImage2(
      this Graphics g,
      Image Image,
      int X,
      int Y,
      Rectangle srcRect,
      GraphicsUnit srcUnit
      )
    {
        Bitmap Bmp = Image as Bitmap;
        Bmp.SetResolution(DPIUtil.dpiX, DPIUtil.dpiY);
        g.DrawImage(Bmp, new Rectangle(X, Y, srcRect.Width, srcRect.Height), srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, srcUnit,  null);
    }

}
