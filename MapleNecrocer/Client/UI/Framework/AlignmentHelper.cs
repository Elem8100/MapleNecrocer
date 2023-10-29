using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGame.UI.Forms
{
    class AlignmentHelper
    {
        public static Vector2 Align(Vector2 container, Vector2 content, ContentAlignment alignment, bool round = true)
        {
            var ret = Vector2.Zero;

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    ret = new Vector2(container.X / 2 - content.X / 2, container.Y - content.Y);
                    break;
                case ContentAlignment.BottomRight:
                    ret = new Vector2(container.X - content.X, container.Y - content.Y);
                    break;
                case ContentAlignment.BottomLeft:
                    ret = new Vector2(0, container.Y - content.Y);
                    break;
                case ContentAlignment.MiddleCenter:
                    ret = new Vector2(container.X / 2 - content.X / 2, container.Y / 2 - content.Y / 2);
                    break;
                case ContentAlignment.MiddleLeft:
                    ret = new Vector2(0, container.Y / 2 - content.Y / 2);
                    break;
                case ContentAlignment.MiddleRight:
                    ret = new Vector2(container.X - content.X, container.Y / 2 - content.Y / 2);
                    break;
                case ContentAlignment.TopCenter:
                    ret = new Vector2(container.X / 2 - content.X / 2, 0);
                    break;
                case ContentAlignment.TopLeft:
                    ret = new Vector2(0, 0);
                    break;
                case ContentAlignment.TopRight:
                    ret = new Vector2(container.X - content.X, 0);
                    break;
            }

            if(round)
                ret = new Vector2((float)Math.Round(ret.X), (float)Math.Round(ret.Y));

            return ret;
        }
    }
}
