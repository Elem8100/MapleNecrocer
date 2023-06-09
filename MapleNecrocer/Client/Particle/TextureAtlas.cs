﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle =Microsoft.Xna.Framework.Rectangle;
namespace WzComparerR2.MapRender2
{
    public struct TextureAtlas
    {
        public TextureAtlas(Texture2D texture)
        {
            this.Texture = texture;
            this.SrcRect = null;
        }

        public TextureAtlas(Texture2D texture, Rectangle rect)
        {
            this.Texture = texture;
            this.SrcRect = rect;
        }

        public Texture2D Texture { get; set; }
        public Rectangle? SrcRect { get; set; }
    }
}
