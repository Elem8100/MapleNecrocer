using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;
using Color = Microsoft.Xna.Framework.Color;
using MapleNecrocer;


namespace MonoGame.UI.Forms
{
    public class UIForm : Control, IControls
    {
        public bool IsMovable { get; set; }
        public string Title { get; set; }
        public List<Control> Controls { get; private set; }

        public UIForm()
        {
            BackgroundColor = Color.Gray;
            Controls = new List<Control>();
            IsMovable=true;
        }
        internal override void DoDraw(Vector2 offset)
        {
            if (!IsVisible)
                return;
            ImagePath = UIPath;
            SpriteBatch.Draw(Wz.UIImageLib[Wz.UIData[ImagePath]], new Vector2(Location.X, Location.Y), Color.White);
            foreach (var control in Controls)
            {
                if (control.IsVisible)
                    control.DoDraw(Location);
            }
        }

        internal override void Update()
        {
            foreach (var control in Controls)
            {
                control.Update();
            }
        }

        Control IControls.FindControlAt(Point position)
        {
            var point = position - Location.ToPoint();
            var control = Controls.LastOrDefault(c => c.Contains(point));
            return control ?? this;
        }

        public void SetVisible()
        {
            IsVisible = !IsVisible;
            if (IsVisible)
                ZIndex = GameUI.UI.ControlManager.Controls.Last().ZIndex + 1;
        }
       
    }
}
