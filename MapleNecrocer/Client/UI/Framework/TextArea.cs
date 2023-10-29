using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoGame.UI.Forms
{
    public class TextArea : Control
    {
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public float LineSpacing { get; set; }

        public TextArea()
        {
            BackgroundColor = Color.Transparent;
            TextColor = Color.Black;
            LineSpacing = 0f;
        }

        internal override void DoDraw(Vector2 offset)
        {
            string[] lines = Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            float maxWidth = 0;
            float height = 0;

            foreach (var line in lines)
            {
              //  var txtSize = helper.MeasureString(FontName, line);
             //   maxWidth = txtSize.X > maxWidth ? txtSize.X : maxWidth;
              //  height += txtSize.Y;
            }

            var rectangle = new Rectangle((int)Location.X, (int)Location.Y, (int)maxWidth, (int)height);
          //  rectangle.Offset(offset);
          /*
            if (BackgroundColor != Color.Transparent)
                helper.DrawRectangle(rectangle, BackgroundColor);

            height = 0;
            foreach (var line in lines)
            {
                var txtSize = helper.MeasureString(FontName, line);
                helper.DrawString(this, Location + offset + new Vector2(0, height), line, TextColor);
                height += txtSize.Y + LineSpacing;
            }

            */
        }
    }
}
