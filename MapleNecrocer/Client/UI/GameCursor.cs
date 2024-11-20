﻿using WzComparerR2.WzLib;
using MapleNecrocer;
using MouseExt;
using Microsoft.Xna.Framework.Input;
using System.Security.Cryptography.X509Certificates;
using ButtonState=Microsoft.Xna.Framework.Input.ButtonState;
namespace GameUI;

public class GameCursor
{
    public static bool IsDataWz;
    static int Frame;
    static string CursorNumber="0";
    static int FTime;
    static Wz_Node ImageNode;
    static bool HasAnim;
    //static Wz_Node ImagEntry;
    public static void LoadRes(string CursorNum)
    {
        if (Wz.HasNode("UI/Basic.img/Cursor/" + CursorNum))
        {
            Wz.DumpData(Wz.GetNode("UI/Basic.img/Cursor/" + CursorNum), Wz.UIData, Wz.UIImageLib);
        } else if(Wz.HasNode("UI/Basic.img/Cursor/arrow/" + CursorNum)) {
            // Fallback for older clients, with arrow/0 cursor available only.
            Wz.DumpData(Wz.GetNode("UI/Basic.img/Cursor/arrow/" + CursorNum), Wz.UIData, Wz.UIImageLib);

        } else {
            // Fallback for REALLY old clients
            Wz.DumpData(Wz.GetNode("UI/Basic.img/Cursor/arrow"), Wz.UIData, Wz.UIImageLib);
        }
    }

    public static void Draw()
    {
        var MouseState = MouseEx.GetState();
        if(MouseState.LeftButton== ButtonState.Pressed)
        { 
           if(!IsDataWz)
            Change("12");
        }
        
        if (MouseState.LeftButton == ButtonState.Released)
        { 
            Change("0");
        }

        if (Wz.UIData.ContainsKey("UI/Basic.img/Cursor/" + CursorNumber + "/1"))
        {
            ImageNode = Wz.UIData["UI/Basic.img/Cursor/" + CursorNumber + '/' + Frame];
            int Delay = ImageNode.GetInt("delay", 100);
            FTime += 17;
            if (FTime > Delay)
            {
                Frame += 1;
                if (!Wz.UIData.ContainsKey("UI/Basic.img/Cursor/" + CursorNumber + '/' + Frame))
                    Frame = 0;
                FTime = 0;
            }
            Wz_Vector Origin = ImageNode.GetVector("origin");
            int OffsetX = -Origin.X + 3;
            int OffsetY = -Origin.Y + 3;
            EngineFunc.Canvas.Draw(Wz.UIImageLib[ImageNode], MouseState.X + OffsetX, MouseState.Y + OffsetY);
        }
        else
        {
            if (Wz.UIData.ContainsKey("UI/Basic.img/Cursor/" + CursorNumber + "/0"))
            {
                ImageNode = Wz.UIData["UI/Basic.img/Cursor/" + CursorNumber + "/0"];
            } else if (Wz.UIData.ContainsKey("UI/Basic.img/Cursor/arrow/" + CursorNumber)) {
                ImageNode = Wz.UIData["UI/Basic.img/Cursor/arrow/" + CursorNumber];
            } else {
                // It's most likely the /arrow
                ImageNode = Wz.UIData["UI/Basic.img/Cursor/arrow"];
            }
            Wz_Vector Origin = ImageNode.GetVector("origin");
            int OffsetX = -Origin.X + 3;
            int OffsetY = -Origin.Y + 3;
            EngineFunc.Canvas.Draw(Wz.UIImageLib[ImageNode], MouseState.X + OffsetX, MouseState.Y + OffsetY);
        }
    }

    public static void Change(string Number)
    {
        if (Frame != 0)
            Frame = 0;
        CursorNumber = Number;
    }

}
