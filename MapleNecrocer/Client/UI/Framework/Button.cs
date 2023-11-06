using GameUI;
using MapleNecrocer;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Graphics;
using WzComparerR2.WzLib;


namespace MonoGame.UI.Forms
{
    public class UIButton : Control
    {
        public enum MouseAction { Enter, Leave, Down, Up };
        public string Text { get; set; }
        public ContentAlignment TextAlign { get; set; }
        public Color TextColor { get; set; }
        public string BtnLeftTexture { get; set; }
        public string BtnRightTexture { get; set; }
        public string BtnMiddleTexture { get; set; }
        public MouseAction mouseAction;
        public UIButton()
        {
            BackgroundColor = Color.Aquamarine;
            TextColor = Color.White;
            TextAlign = ContentAlignment.MiddleCenter;
            mouseAction = MouseAction.Leave;
            this.MouseEnter += (s, e) => { mouseAction = MouseAction.Enter; };
            this.MouseLeave += (s, e) => { mouseAction = MouseAction.Leave; };
            this.MouseDown += (s, e) => { mouseAction = MouseAction.Down; };
            this.MouseUp += (s, e) => { mouseAction = MouseAction.Up; };
        }

        internal override void DoDraw(Vector2 offset)
        {
            switch (mouseAction)
            {
                case MouseAction.Enter:
                    ImagePath = UIPath + "/mouseOver/0";
                    break;
                case MouseAction.Leave:
                    ImagePath = UIPath + "/normal/0";
                    break;
                case MouseAction.Down:
                    ImagePath = UIPath + "/pressed/0";
                    break;
                case MouseAction.Up:
                    if (ImagePath == UIPath + "/pressed/0")
                        ImagePath = UIPath + "/mouseOver/0";
                    break;
            }
            SpriteBatch.Draw(Wz.UIImageLib[Wz.UIData[ImagePath]], Location + offset, Color.White);
        }

        internal override void LoadContent(DrawHelper helper)
        {
            base.LoadContent(helper);

            string[] assets =
            {
                BtnLeftTexture, BtnMiddleTexture, BtnRightTexture
            };

            foreach (var asset in assets)
            {
                if (!string.IsNullOrEmpty(asset))
                    helper.LoadTexture(asset);
            }
        }
    }
}
