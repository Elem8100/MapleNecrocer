using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitmap = System.Drawing.Bitmap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.SpriteEngine;
namespace Parallax_Scrolling;

internal class ImageFilter
{
    public static void HSL(ref System.Drawing.Bitmap img, int Hue, int Saturation = 0, int Lightness = 0)
    {
        const double c1o60 = 1 / (double)60;
        const double c1o255 = 1 / (double)255;
        Bitmap result = new Bitmap(img);
        result.SetResolution(img.HorizontalResolution, img.VerticalResolution);
        BitmapData bmpData = result.LockBits(new System.Drawing.Rectangle(0, 0, result.Width, result.Height), ImageLockMode.ReadWrite, img.PixelFormat);
        int pixelBytes = System.Drawing.Image.GetPixelFormatSize(img.PixelFormat) / 8;
        // Get the address of the first line.
        IntPtr ptr = bmpData.Scan0;
        int size = bmpData.Stride * result.Height;
        byte[] pixels = new byte[size - 1 + 1];
        int index;
        double R, G, B;
        double H, S, L, H1;
        double min, max, dif, sum;
        double f1, f2;
        double v1, v2, v3;
        double sat = 127 * Saturation / 100;
        double lum = 127 * Lightness / 100;
        // Copy the RGB values into the array.
        System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, size);
        // Main loop.
        for (int row = 0; row <= result.Height - 1; row++)
        {
            for (int col = 0; col <= result.Width - 1; col++)
            {
                index = (row * bmpData.Stride) + (col * pixelBytes);
                R = pixels[index + 2];
                G = pixels[index + 1];
                B = pixels[index + 0];
                // Conversion to HSL space.
                min = R;
                if ((G < min))
                    min = G;
                if ((B < min))
                    min = B;
                max = R; f1 = 0.0; f2 = G - B;
                if ((G > max))
                {
                    max = G; f1 = 120.0; f2 = B - R;
                }
                if ((B > max))
                {
                    max = B; f1 = 240.0; f2 = R - G;
                }
                dif = max - min;
                sum = max + min;
                L = 0.5 * sum;
                if ((dif == 0))
                {
                    H = 0.0; S = 0.0;
                }
                else
                {
                    if ((L < 127.5))
                        S = 255.0 * dif / sum;
                    else
                        S = 255.0 * dif / (510.0 - sum);
                    H = (f1 + 60.0 * f2 / dif);
                    if (H < 0.0)
                        H += 360.0;
                    if (H >= 360.0)
                        H -= 360.0;
                }
                // Apply transformation.
                H = H + Hue;
                if (H >= 360.0)
                    H = H - 360.0;
                S = S + sat;
                if (S < 0.0)
                    S = 0.0;
                if (S > 255.0)
                    S = 255.0;
                L = L + lum;
                if (L < 0.0)
                    L = 0.0;
                if (L > 255.0)
                    L = 255.0;
                // Conversion back to RGB space.
                if ((S == 0))
                {
                    R = L; G = L; B = L;
                }
                else
                {
                    if ((L < 127.5))
                        v2 = c1o255 * L * (255 + S);
                    else
                        v2 = L + S - c1o255 * S * L;
                    v1 = 2 * L - v2;
                    v3 = v2 - v1;
                    H1 = H + 120.0;
                    if ((H1 >= 360.0))
                        H1 -= 360.0;
                    if ((H1 < 60.0))
                        R = v1 + v3 * H1 * c1o60;
                    else if ((H1 < 180.0))
                        R = v2;
                    else if ((H1 < 240.0))
                        R = v1 + v3 * (4 - H1 * c1o60);
                    else
                        R = v1;
                    H1 = H;
                    if ((H1 < 60.0))
                        G = v1 + v3 * H1 * c1o60;
                    else if ((H1 < 180.0))
                        G = v2;
                    else if ((H1 < 240.0))
                        G = v1 + v3 * (4 - H1 * c1o60);
                    else
                        G = v1;
                    H1 = H - 120.0;
                    if ((H1 < 0.0))
                        H1 += 360.0;
                    if ((H1 < 60.0))
                        B = v1 + v3 * H1 * c1o60;
                    else if ((H1 < 180.0))
                        B = v2;
                    else if ((H1 < 240.0))
                        B = v1 + v3 * (4 - H1 * c1o60);
                    else
                        B = v1;
                }
                // Save new values.
                pixels[index + 2] = System.Convert.ToByte(R);
                pixels[index + 1] = System.Convert.ToByte(G);
                pixels[index + 0] = System.Convert.ToByte(B);
            }
        }
        // Copy the RGB values back to the bitmap
        System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, size);
        // Unlock the bits.
        result.UnlockBits(bmpData);
        img = result;
        //  return result;

    }

    public static Texture2D GetHSL(GraphicsDevice dev,Bitmap Bmp, int Hue, int Saturation = 0, int Lightness = 0)
    {
        HSL(ref Bmp, Hue, Saturation, Lightness);
        int[] imgData = new int[Bmp.Width * Bmp.Height];
        Texture2D Texture = new Texture2D(dev, Bmp.Width, Bmp.Height);
        unsafe
        {
            // lock bitmap
            System.Drawing.Imaging.BitmapData origdata =
                Bmp.LockBits(new System.Drawing.Rectangle(0, 0, Bmp.Width, Bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, Bmp.PixelFormat);
            uint* byteData = (uint*)origdata.Scan0;
            // Switch bgra -> rgba
            for (int i = 0; i < imgData.Length; i++)
            {
                byteData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);
            }
            // copy data
            System.Runtime.InteropServices.Marshal.Copy(origdata.Scan0, imgData, 0, Bmp.Width * Bmp.Height);
            byteData = null;
            // unlock bitmap
            Bmp.UnlockBits(origdata);
        }
        Texture.SetData(imgData);
        return Texture;

    }

}
