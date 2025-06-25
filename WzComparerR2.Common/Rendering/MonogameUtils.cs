﻿using System;
using System.Runtime.InteropServices;
using GdipColor = System.Drawing.Color;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D11;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using WzComparerR2.WzLib;
using Color =Microsoft.Xna.Framework.Color;

namespace WzComparerR2.Rendering
{
    public static class MonogameUtils
    {
        public static Color ToXnaColor(this GdipColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        public static Color ToXnaColor(int argbPackedValue)
        {
            var bgra = BitConverter.GetBytes(argbPackedValue);
            return new Color(bgra[2], bgra[1], bgra[0], bgra[3]);
        }

        public static Color GetXnaColor(this Wz_Node node)
        {
            var argbColor = node.GetValueEx<int>(0);
            return ToXnaColor(argbColor);
        }

        public static Texture2D CreateMosaic(GraphicsDevice device, Color c0, Color c1, int blockSize)
        {
            var t2d = new Texture2D(device, blockSize * 2, blockSize * 2, false, SurfaceFormat.Color);
            Color[] colorData = new Color[blockSize * blockSize * 4];
            int offset = blockSize * blockSize * 2;
            for (int i = 0; i < blockSize; i++)
            {
                colorData[i] = c0;
                colorData[blockSize + i] = c1;
                colorData[offset + i] = c1;
                colorData[offset + blockSize + i] = c0;
            }
            for (int i = 1; i < blockSize; i++)
            {
                Array.Copy(colorData, 0, colorData, blockSize * 2 * i, blockSize * 2);
                Array.Copy(colorData, offset, colorData, offset + blockSize * 2 * i, blockSize * 2);
            }
            t2d.SetData(colorData);
            return t2d;
        }

        public static Texture2D ToTexture(this System.Drawing.Bitmap bitmap, GraphicsDevice device)
        {
            var t2d = new Texture2D(device, bitmap.Width, bitmap.Height, false, SurfaceFormat.Bgra32);
            bitmap.ToTexture(t2d,Microsoft.Xna.Framework.Point.Zero);
            return t2d;
        }

        public static void ToTexture(this System.Drawing.Bitmap bitmap, Texture2D texture,Microsoft.Xna.Framework.Point origin)
        {
            var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            byte[] buffer = new byte[bmpData.Stride * bmpData.Height];
            Marshal.Copy(bmpData.Scan0, buffer, 0, buffer.Length);
            bitmap.UnlockBits(bmpData);

            texture.SetData(0, 0, new Microsoft.Xna.Framework.Rectangle(origin.X, origin.Y, rect.Width, rect.Height), buffer, 0, buffer.Length);
        }

        public static Device _d3dDevice(this GraphicsDevice device)
        {
            return (Device)device.Handle;
        }

        public static DeviceContext _d3dContext(this GraphicsDevice device)
        {
            var d3dContextField = typeof(GraphicsDevice).GetField("_d3dContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return (DeviceContext)d3dContextField.GetValue(device);
        }

        public static SharpDX.DXGI.SwapChain _swapChain(this GraphicsDevice device)
        {
            var _swapChainField = typeof(GraphicsDevice).GetField("_swapChain", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return (SharpDX.DXGI.SwapChain)_swapChainField.GetValue(device);
        }

        public static Resource GetTexture(this Texture texture)
        {
            var _getTextureFunc = typeof(Texture).GetMethod("GetTexture", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return (Resource)_getTextureFunc.Invoke(texture, Array.Empty<object>());
        }

        public static void CopyBackBuffer(this GraphicsDevice graphicsDevice, Texture2D destTexture)
        {
            var pp = graphicsDevice.PresentationParameters;
            if (pp.BackBufferWidth != destTexture.Width || pp.BackBufferHeight != destTexture.Height || pp.BackBufferFormat != destTexture.Format)
            {
                throw new Exception("Destination texture size or format does not compatible with back buffer.");
            }

            var d3dContext = graphicsDevice._d3dContext();
            var swapChain = graphicsDevice._swapChain();
            var dest = destTexture.GetTexture();
            using SharpDX.Direct3D11.Texture2D source = SharpDX.Direct3D11.Resource.FromSwapChain<SharpDX.Direct3D11.Texture2D>(swapChain, 0);
            lock (d3dContext)
            {
                d3dContext.CopyResource(source, dest);
            }
        }

        public static bool IsSupportFormat(this GraphicsDevice device, SharpDX.DXGI.Format format)
        {
            var d3dDevice = device._d3dDevice();
            var fmtSupport = d3dDevice.CheckFormatSupport(format);
            return (fmtSupport & SharpDX.Direct3D11.FormatSupport.Texture2D) != 0;
        }

        public static bool IsSupportBgra4444(this GraphicsDevice device)
        {
            return device.IsSupportFormat(SharpDX.DXGI.Format.B4G4R4A4_UNorm);
        }

        public static bool IsSupportBgr565(this GraphicsDevice device)
        {
            return device.IsSupportFormat(SharpDX.DXGI.Format.B5G6R5_UNorm);
        }

        public static bool IsSupportBgra5551(this GraphicsDevice device)
        {
            return device.IsSupportFormat(SharpDX.DXGI.Format.B5G5R5A1_UNorm);
        }
    }
}
