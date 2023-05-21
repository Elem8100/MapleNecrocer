using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ButtonState=Microsoft.Xna.Framework.Input.ButtonState;

namespace SpriteEngine;

public class Keyboard
{
    static KeyboardState currentKeyState;
    static KeyboardState previousKeyState;

    public static KeyboardState GetState()
    {
        previousKeyState = currentKeyState;
        currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        return currentKeyState;
    }

    public static bool KeyDown(Microsoft.Xna.Framework.Input.Keys key)
    {
        return currentKeyState.IsKeyDown(key);
    }
    public static bool KeyUp(Microsoft.Xna.Framework.Input.Keys key)
    {
        return !previousKeyState.IsKeyUp(key) && currentKeyState.IsKeyUp(key);
    }

    public static bool KeyPressed(Microsoft.Xna.Framework.Input.Keys key)
    {
        return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
    }

}
public class Mouse
{ 
    static MouseState lastMouseState;
    static MouseState currentMouseState;
   
    public static MouseState GetState()
    {
        lastMouseState = currentMouseState;
        currentMouseState =  Microsoft.Xna.Framework.Input.Mouse.GetState();
        return currentMouseState;
       
    }
    public static MouseState State
    {
        get=> currentMouseState;
    }
    public static bool LeftClick
    {
        get=> currentMouseState.LeftButton == ButtonState.Pressed &&
            lastMouseState.LeftButton == ButtonState.Released;
      
    }

    public static bool RightClick()
    {
        return currentMouseState.RightButton == ButtonState.Pressed &&
            lastMouseState.RightButton == ButtonState.Released;
    }
    public static bool RightPressed()
    {
        return currentMouseState.RightButton == ButtonState.Pressed;
    }
}
