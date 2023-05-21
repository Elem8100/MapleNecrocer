using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonoGame.SpriteEngine;
public struct Rect
{
    public int Left, Top, Right, Bottom;
}

public static class SpriteUtils
{   
   
    public static Rect Rect(int Left,int Top,int Right,int Bottom)
    {
        Rect Rect= new Rect();
        Rect.Left= Left;
        Rect.Top= Top;
        Rect.Right= Right;
        Rect.Bottom= Bottom;
        return Rect;
    }
    public static  float PIConv256 = -40.743665431f;
    public static bool OverLapRect(Rect Rect1, Rect Rect2)
    { 
        return (Rect1.Left < Rect2.Right) && (Rect1.Right > Rect2.Left) && (Rect1.Top < Rect2.Bottom) && (Rect1.Bottom > Rect2.Top);

    }
 
    public static bool PointInRect(Vector2 Point, Rect Rect)
    {
        return (Point.X >= Rect.Left) && (Point.X <= Rect.Right) && (Point.Y >= Rect.Top) && (Point.Y <= Rect.Bottom);

    }
    public static int GetAngle256(int X, int Y)
    {
        return (int)(Math.Atan2(X, Y) * PIConv256) + 128;
    }
    public static int GetAngle256(int SrcX, int SrcY, int DestX, int DestY)
    {
        return (int)(Math.Atan2(DestX - SrcX, DestY - SrcY) * PIConv256) + 128;
    }

    public static float Angle2(Vector2 v)
    {
        return (float)Math.Atan2(v.X, v.Y);
    }
    public static float AngleDiff(float SrcAngle, float DestAngle)
    {
        float Diff = DestAngle - SrcAngle;
        if (SrcAngle > DestAngle)
        {
            if ((SrcAngle > 128) && (DestAngle < 128))
            {
                if (Diff < 128.0)
                    Diff = Diff + 256;
            }
            if (Diff > 128.0)
                Diff = Diff - 256;
        }
        else
        {
            if (Diff > 128.0)
                Diff = Diff - 256;
        }
        return Diff;
    }



}
