using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Color=Microsoft.Xna.Framework.Color;
using Rectangle=Microsoft.Xna.Framework.Rectangle; 

namespace MonoGame.UI.Forms
{
    internal class DrawHelper
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly ContentManager _manager;
        private Dictionary<string, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
        private Dictionary<string, SpriteFont> _fontCache = new Dictionary<string, SpriteFont>();

        private Texture2D whiteTexture;
        private Texture2D circleTexture;

        public DrawHelper(SpriteBatch spriteBatch, ContentManager manager, GraphicsDevice device)
        {
            _spriteBatch = spriteBatch;
            _manager = manager;

            var size = 100;
            whiteTexture = new Texture2D(device, size, size);
            Color[] data = new Color[size * size];

            for (int i = 0; i < size*size; i++)
                data[i] = Color.White;

            whiteTexture.SetData(data);

            // create circle texture

            size = 512;
            circleTexture = new Texture2D(device, size, size);

            data = new Color[size * size];
            DrawFilledCircle(size / 2, size/2, size/2 - 1, Color.White, size, size, data);
            circleTexture.SetData<Color>(data);
        }

        public void ReloadResources(IEnumerable<Control> controls)
        {
            foreach (var control in controls)
            {
                control.LoadContent(this);
            }
        }

        #region Public Draw Methods

        public void DrawCircle(Vector2 center, float radius, Color color)
        {
            float scale = radius / 256.0f;
            _spriteBatch.Draw(circleTexture, center, null, color, 0f, new Vector2(256, 256), scale, SpriteEffects.None, 0f);
        }

        public void DrawLine(Vector2 pos1, Vector2 pos2, Color color, int width = 1)
        {
            Vector2 diff = (pos1 - pos2);
            float length = diff.Length();
            float angle = (float)Math.Atan2(diff.Y, diff.X) + MathHelper.Pi;

            _spriteBatch.Draw(whiteTexture, new Rectangle((int)(pos1.X), (int)(pos1.Y), (int)length, width),
                null, color, angle, new Vector2(0, whiteTexture.Width / 2.0f), SpriteEffects.None, 0f);
        }

        public void DrawString(Control control, Vector2 position, string text, Color color, float zoom = 1.0f)
        {
            var font = LoadFont(control.FontName);
            var size = font.MeasureString(text);
            _spriteBatch.DrawString(font, text, position, color, 0f, Vector2.Zero, zoom, SpriteEffects.None, 0);
        }

        public void DrawRectangle(Rectangle rectangle, Color color)
        {
            _spriteBatch.Draw(whiteTexture, rectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0);
        }

        public Vector2 DrawTextureWithOffset(Vector2 position, string asset, AlignOffset align = AlignOffset.TopLeft)
        {
            return DrawTextureWithOffset(position, asset, Color.White, align);
        }

        public Vector2 DrawTextureWithOffset(Vector2 position, string asset, Color blend, AlignOffset align = AlignOffset.TopLeft)
        {
            var texture = LoadTexture(asset);
            var offset = Vector2.Zero;

            switch (align)
            {
                case AlignOffset.TopRight: offset = new Vector2(-texture.Width, 0); break;
                case AlignOffset.BottomLeft: offset = new Vector2(0, -texture.Height); break;
                case AlignOffset.BottomRight: offset = new Vector2(-texture.Width, -texture.Height); break;
            }

            _spriteBatch.Draw(texture, position + offset, blend);
            return new Vector2(texture.Width, texture.Height);
        }

        public void DrawTextureRepeat(Vector2 start, Vector2 stop, string asset)
        {
            DrawTextureRepeat(start, stop, asset, Color.White);
        }

        public void DrawTextureRepeat(Vector2 start, Vector2 stop, string asset, Color blend)
        {
            var texture = LoadTexture(asset);

            int width = (int) (stop.X - start.X);
            int height = (int) (stop.Y - start.Y);

            while (height > 0)
            {
                var startX = start;
                while (width > 0)
                {
                    var srcRect = new Rectangle(0, 0,
                        width >= texture.Width ? texture.Width : width,
                        height >= texture.Height ? texture.Height : height);
                    _spriteBatch.Draw(texture, start, srcRect, blend);
                    width -= texture.Width;
                    start += new Vector2(texture.Width, 0);
                }
                start = startX;
                width = (int)(stop.X - start.X);

                height -= texture.Height;
                start += new Vector2(0, texture.Height);
            }
        }

        #endregion

        public Vector2 MeasureString(string asset, string text)
        {
            var font = LoadFont(asset);
            return font.MeasureString(text);
        }

        public SpriteFont LoadFont(string asset)
        {
            if (_fontCache.ContainsKey(asset))
                return _fontCache[asset];

            var font = _manager.Load<SpriteFont>(asset);
            _fontCache.Add(asset, font);
            return font;
        }

        public Texture2D LoadTexture(string asset)
        {
            if (_textureCache.ContainsKey(asset))
                return _textureCache[asset];

            var texture = _manager.Load<Texture2D>(asset);
            _textureCache.Add(asset, texture);
            return texture;
        }

        internal enum AlignOffset
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center
        }

        #region Private Methods

        private void DrawFilledCircle(int x, int y, int radius, Color color, int width, int height, Color[] data)
        {
            int sx = -radius;
            int sy = 0;
            int error = 2 - 2 * radius;

            while (sx <= 0)
            {
                for (int i = sy; i >= 0; i--)
                {
                    data[(x - sx) + (y - i) * width] = color;
                    data[(x - sx) + (y + i) * width] = color;
                    data[(x + sx) + (y + i) * width] = color;
                    data[(x + sx) + (y - i) * width] = color;
                }

                int r = error;
                if (r <= sy)
                    error += ++sy * 2 + 1;
                if (r > sx || error > y)
                    error += ++sx * 2 + 1;
            }
        }

        #endregion
    }
}
