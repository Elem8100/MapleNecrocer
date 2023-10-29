using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using MouseExt;

namespace MonoGame.UI.Forms
{
    public abstract class ControlManager
    {
        private SpriteBatch _spriteBatch;
        private DrawHelper _drawHelper;
        private Control _selectedControl;
        private Control _lastHoveredControl;
        private Vector2 _dragPositionOffset;
        private bool _isDragging;
        private MouseState _prevMouseState;
        private KeyboardState _prevKeyboardState;
        public List<Control> Controls { get; private set; }
        public ControlManager()
        {
            Controls = new List<Control>();
        }

        public void Update()
        {
           
            var mouseState = MouseEx.GetState();
            var hoverControl = FindControlAt(mouseState.Position, Controls);
            if (_lastHoveredControl != hoverControl)
            {
                _lastHoveredControl?.OnMouseLeave();
                hoverControl?.OnMouseEnter();
            }

            _lastHoveredControl = hoverControl;

            if (mouseState.LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton == ButtonState.Released)
            {
                if (_lastHoveredControl != null)
                {
                    _selectedControl = _lastHoveredControl;
                    if (!_selectedControl.IsVisible) 
                        return;
                    _selectedControl.OnMouseDown();

                    if (_selectedControl is UIForm)
                    {
                        var form = _selectedControl as UIForm;
                        _selectedControl.ZIndex = Controls.Last().ZIndex + 1;
                        if (form.IsMovable)
                        {
                            _dragPositionOffset = mouseState.Position.ToVector2() - _selectedControl.Location;
                            _isDragging = true;
                        }
                    }
                }
            }

            if (mouseState.LeftButton == ButtonState.Released && _prevMouseState.LeftButton == ButtonState.Pressed)
            {
                _isDragging = false;
                if (_selectedControl != null)
                {
                    _selectedControl.OnMouseUp();

                    if (_selectedControl == FindControlAt(mouseState.Position, Controls))
                        _selectedControl.OnClicked();
                }
            }

            if (_isDragging)
            {
                _selectedControl.Location = mouseState.Position.ToVector2() - _dragPositionOffset;
            }

            Controls = Controls.OrderBy(c => c.ZIndex).ToList();
            _prevMouseState = mouseState;

            foreach (var control in Controls)
            { 
            
                control.Update();
            }
        }

        public void Draw()
        {
            EngineFunc.SpriteEngine.Canvas.SpriteBatch.Begin();
            foreach (var control in Controls)
            {
                control.Draw();
            }
            EngineFunc.SpriteEngine.Canvas.SpriteBatch.End();
        }

        private Control FindControlAt(Microsoft.Xna.Framework.Point position, IEnumerable<Control> controls)
        {
            var control = controls.LastOrDefault(c => c.Contains(position));
            if (control is IControls)
                return ((IControls)control).FindControlAt(position);
            else
                return control;

        }
    }
}