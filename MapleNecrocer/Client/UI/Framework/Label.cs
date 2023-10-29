using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoGame.UI.Forms
{
    public class Label : Control
    {
        public string Text { get; set; }
        public Color TextColor { get; set; }

        public Label()
        {
            BackgroundColor = Color.Transparent;
            TextColor = Color.Black;
        }

        internal override void DoDraw(Vector2 offset)
        {
           // var txtSize = helper.MeasureString(FontName, Text);
           // var rectangle = new Rectangle((int)Location.X, (int)Location.Y, (int)txtSize.X, (int)txtSize.Y);
           // rectangle.Offset(offset);

           // if(BackgroundColor != Color.Transparent)
              //  helper.DrawRectangle(rectangle, BackgroundColor);
          // helper.DrawString(this, Location + offset, Text, TextColor);
        }
    }
}
