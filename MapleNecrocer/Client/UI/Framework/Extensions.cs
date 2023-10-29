using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace MonoGame.UI.Forms
{
    public static class Extensions
    {
        public static Color Blend(this Color color, Color blendColor)
        {
            return new Color(
                (byte)(color.R * blendColor.R / 255),
                (byte)(color.G * blendColor.G / 255),
                (byte)(color.B * blendColor.B / 255),
                (byte)(color.A * blendColor.A / 255));
        }
    }
}
