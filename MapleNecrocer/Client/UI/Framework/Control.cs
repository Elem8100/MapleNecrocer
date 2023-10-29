using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
//using MonoGame.UI.Forms.Effects;
using MonoGame.SpriteEngine;
using WzComparerR2.WzLib;
using WzComparerR2.Rendering;

namespace MonoGame.UI.Forms
{
    public abstract class Control
    {
        public bool Enabled { get; set; }
        public bool IsVisible { get; set; }
        public SpriteBatchEx SpriteBatch => EngineFunc.SpriteEngine.Canvas.SpriteBatch;
        public string UIPath;
        public string ImagePath;

        public Vector2 Location { get; set; }
        public Vector2 Size { get; set; }
        public Microsoft.Xna.Framework.Color BackgroundColor { get; set; }
        public string FontName { get; set; }
        public Microsoft.Xna.Framework.Rectangle HitBox => new Microsoft.Xna.Framework.Rectangle((int)Location.X, (int)Location.Y, (int)Size.X, (int)Size.Y);
        public int ZIndex { get; set; }

        //public Effect HoverEffect { get; set; }

        public event EventHandler Clicked;
        public event EventHandler MouseDown;
        public event EventHandler MouseUp;
        public event EventHandler MouseLeave;
        public event EventHandler MouseEnter;

        protected bool IsPressed;
        protected bool IsHovering;
        protected float Zoom = 1.0f;

        private bool _wasHovering;

        protected Control()
        {
            FontName = "defaultFont";
            IsVisible = true;
        }

        public virtual bool Contains(Microsoft.Xna.Framework.Point point)
        {
            return HitBox.Contains(point);
        }

        internal abstract void DoDraw(Vector2 offset);

        internal virtual void Draw()
        {
            if (IsVisible)
            {
                DoDraw(Vector2.Zero);
            }
            else
            {
                ZIndex = -100000;
            }
          
        }

        internal virtual void Update()
        {
          
            Zoom = 1.0f;

            //  if (IsHovering)
            {
                // if(!_wasHovering)
                //   HoverEffect.Reset();
                // HoverEffect.Update(gameTime);
                //Zoom = HoverEffect.Zoom;
            }

            //  _wasHovering = IsHovering;
        }

        internal virtual void LoadContent(DrawHelper helper)
        {
            if (!string.IsNullOrEmpty(FontName))
                helper.LoadFont(FontName);
        }

        internal virtual void OnMouseDown()
        {
            IsPressed = true;
            MouseDown?.Invoke(this, new EventArgs());
        }

        internal virtual void OnMouseUp()
        {
            IsPressed = false;
            MouseUp?.Invoke(this, new EventArgs());
        }

        internal virtual void OnMouseEnter()
        {
            IsHovering = true;
            MouseEnter?.Invoke(this, new EventArgs());
        }

        internal virtual void OnMouseLeave()
        {
            IsHovering = false;
            MouseLeave?.Invoke(this, new EventArgs());
        }

        internal virtual void OnClicked()
        {
            Clicked?.Invoke(this, new EventArgs());
        }
    }
}
