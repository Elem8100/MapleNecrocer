using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WzComparerR2.WzLib;
using WzComparerR2.PluginBase;
using Spine;
using WzComparerR2.Animation;
using System.IO;

namespace MapleNecrocer;
public class Obj : SpriteEx
{
    public Obj(Sprite Parent) : base(Parent)
    {
        IntMove=true;
    }
    string Path;
    float DeltaTime;
    int Frame;
    float Time;
    // Vector2 origin;
    float AX, AY;
    int MoveType;
    int MoveW, MoveH;
    int MoveP;
    int MoveR;
    public static void Create()
    {
        for (int Layer = 0; Layer <= 7; Layer++)
        {
            foreach (var Iter in Map.Img.GetNode(Layer.ToString() + "/obj").Nodes)
            {
                string oS = Iter.GetStr("oS");
                if (Wz.GetNode("Map/Obj/" + oS + ".img") == null)
                    continue;
                string L0 = Iter.GetStr("l0");
                string L1 = Iter.GetStr("l1");
                string L2 = Iter.GetStr("l2");
                string Path = "Map/Obj/" + oS + ".img/" + L0 + "/" + L1 + "/" + L2;
                if (!Wz.Data.ContainsKey(Path))
                    Wz.DumpData(Wz.GetNodeA(Path), Wz.Data, Wz.ImageLib);
                int Flow = Iter.GetInt("flow");
                // if (!Wz.ImageLib.ContainsKey(Wz.Data[Path + "/0"]))
                //  continue;
                if (Iter.Nodes["spineAni"] != null)
                {
                    var SpineObj = new SpineObj(EngineFunc.SpriteEngine);
                    var aniData = Map.ResLoader.LoadAnimationData(Wz.GetNodeA(Path));
                    SpineObj.SpineAnimator = new SpineAnimator((SpineAnimationData)aniData);
                    string spineAni = Iter.GetStr("spineAni");
                    if (spineAni != null)
                        SpineObj.SpineAnimator.SelectedAnimationName = spineAni;
                    SpineObj.X = Iter.GetInt("x");
                    SpineObj.Y = Iter.GetInt("y");
                    SpineObj.Z = Layer * 100000 + Iter.GetInt("z");
                    SpineObj.Width = 1000;
                    SpineObj.Height = 1000;
                    SpineObj.SkeletonRenderer = new SkeletonMeshRenderer(RenderFormDraw.Instance.GraphicsDevice);
                }
                else
                {
                    if (Flow == 0)
                    {
                        var Obj = new Obj(EngineFunc.SpriteEngine);
                        Obj.ImageLib = Wz.ImageLib;
                        Obj.Path = Path;
                        Obj.ImageNode = Wz.Data[Path + "/0"];
                        Obj.X = Iter.GetInt("x");
                        Obj.Y = Iter.GetInt("y");
                        Obj.Z = Layer * 100000 + Iter.GetInt("z");
                        Obj.AX = Obj.X;
                        Obj.AY = Obj.Y;
                        Obj.Width = Obj.ImageWidth;
                        Obj.Height = Obj.ImageHeight;
                        Obj.FlipX = Iter.GetBool("f");
                        Obj.MoveType = WzDict.GetInt(Path + "/0/moveType");
                        Obj.MoveP = WzDict.GetInt(Path + "/0/moveP");
                        Obj.MoveW = WzDict.GetInt(Path + "/0/moveW");
                        Obj.MoveH = WzDict.GetInt(Path + "/0/moveH");
                        Obj.MoveR = WzDict.GetInt(Path + "/0/moveR");
                        if (!Wz.HasData(Path + "/1"))
                            Obj.Moved = false;
                        if (Obj.MoveType > 0)
                            Obj.Moved = true;
                        if (Obj.MoveR > 0)
                            Obj.Moved = true;

                        Wz_Vector origin = WzDict.GetVector(Path + "/0/origin");
                        if (Obj.FlipX)
                            Obj.Origin.X = -origin.X + Obj.ImageWidth;
                        else
                            Obj.Origin.X = origin.X;
                        Obj.Origin.Y = origin.Y;
                        //Obj.IntMove=true;
                    }

                    if (Flow > 0)
                    {
                        var FlowObj = new FlowObj(EngineFunc.SpriteEngine);
                        FlowObj.ImageLib = Wz.ImageLib;
                        FlowObj.Path = Path;
                        FlowObj.ImageNode = Wz.Data[Path + "/0"];
                        FlowObj.MoveByEngine = true;
                        FlowObj.X = Iter.GetInt("x");
                        FlowObj.Y = Iter.GetInt("y");
                        FlowObj.Z = Layer * 1000 + Iter.GetInt("z");
                        FlowObj.Width = FlowObj.ImageWidth;
                        FlowObj.Height = FlowObj.ImageHeight;
                        FlowObj.FlipX = Iter.GetBool("f");
                        FlowObj.RX = Iter.GetInt("rx");
                        FlowObj.RY = Iter.GetInt("ry");
                        FlowObj.CX = Iter.GetInt("cx");
                        FlowObj.CY = Iter.GetInt("cy");

                        if (FlowObj.CX == 0)
                            FlowObj.Width = Map.Info["MapWidth"];
                        else
                            FlowObj.Width = FlowObj.CX;

                        if (FlowObj.CY == 0)
                            FlowObj.Height = Map.Info["MapHeight"];
                        else
                            FlowObj.Height = FlowObj.CY;

                        FlowObj.Flow = Flow;
                        FlowObj.Tiled = true;
                        if (Flow == 1)
                            FlowObj.TileMode = TileMode.Horizontal;
                        if (Flow == 2)
                            FlowObj.TileMode = TileMode.Vertical;
                        Wz_Vector origin = WzDict.GetVector(Path + "/0/origin");
                        if (FlowObj.FlipX)
                            FlowObj.Origin.X = -origin.X + FlowObj.ImageWidth;
                        else
                            FlowObj.Origin.X = origin.X;
                        FlowObj.Origin.Y = origin.Y;
                    }

                }
            }
        }
    }

    public override void DoMove(float Delta)
    {
        base.DoMove(Delta);
        string ImagePath = Path + "/" + Frame;
        ImageNode = Wz.Data[ImagePath];
        int Delay = WzDict.GetInt(ImagePath + "/delay", 100);
        int a0 = WzDict.GetInt(ImagePath + "/a0", -1);
        int a1 = WzDict.GetInt(ImagePath + "/a1", -1);
     

        Time += 16.66f * Delta;
        if (Time > Delay)
        {
            Frame += 1;
            if (!Wz.HasData(Path + '/' + Frame))
                Frame = 0;
            Time = 0;
        }
        if ((a0 != -1) && (a1 == -1))
            Alpha = (byte)WzDict.GetInt(ImagePath + "/a0", 255);
        float AniAlpha = a0 - (a0 - a1) * Time / Delay;
        if (Time > 0)
            Alpha = (byte)AniAlpha;

       
        if (MoveType.ToBool())
        {
            DeltaTime += 0.017f;
            switch (MoveType)
            {
                case 1:
                    if (MoveP.ToBool())
                        X = AX + MoveW * (float)Math.Cos(DeltaTime * 1000 * 2 * 3.14159f / MoveP);
                    else
                        X = AX + MoveW * (float)Math.Cos(DeltaTime);
                    break;
                case 2:
                    if (MoveP.ToBool())
                        Y = AY + MoveH * (float)Math.Cos(DeltaTime * 2 * 3.14159f * 1000 / MoveP);
                    else
                        Y = AY + MoveH * (float)Math.Cos(DeltaTime);
                    break;

                case 3:
                    if (MoveP.ToBool())
                    {
                        X = AX + MoveW * (float)Math.Cos(DeltaTime * 1000 * 2 * 3.14159f / MoveP);
                        Y = AY + MoveH * (float)Math.Cos(DeltaTime * 2 * 3.14159f * 1000 / MoveP);
                    }
                    else
                    {
                        X = AX + MoveW * (float)Math.Cos(DeltaTime);
                        Y = AY + MoveH * (float)Math.Cos(DeltaTime);
                    }
                    break;
            }
        }

        Wz_Vector origin = WzDict.GetVector(ImagePath + "/origin");
        if (FlipX)
            Origin.X = -origin.X + ImageWidth;
        else
            Origin.X = origin.X;
        Origin.Y = origin.Y;

        if (MoveR.ToBool())
        {
            Angle += (17f / MoveR) * 3.14159f * 2;
        }
    }
    public override void DoDraw()
    {
        if (Map.ShowObj)
            base.DoDraw();
    }

}

public class SpineObj : SpriteEx
{
    public SpineObj(Sprite Parent) : base(Parent)
    {
    }
    public SkeletonMeshRenderer SkeletonRenderer;
    public SpineAnimator SpineAnimator;

    public override void DoDraw()
    {
        if(!Map.ShowObj) 
            return;
        SpineAnimator.Update(TimeSpan.FromMilliseconds(17));
        SkeletonRenderer.Begin();
        SpineAnimator.Skeleton.X = -Engine.Camera.X + X;
        SpineAnimator.Skeleton.Y = -Engine.Camera.Y + Y;
        SkeletonRenderer.Draw(SpineAnimator.Skeleton);
        SkeletonRenderer.End();
    }

}

public class FlowObj : BackgroundSprite
{
    public FlowObj(Sprite Parent) : base(Parent)
    {
    }
    public string Path = "";
    public int Frame;
    public float Time;
    public int RX, RY;
    public int CX, CY;
    public int Flow;
    public override void DoMove(float Delta)
    {
        // base.DoMove(Delta);
        switch (Flow)
        {
            case 1:
                X -= RX * 5f / 60;
                break;
            case 2:
                Y -= RY * 5f / 60;
                break;
        }
        string ImagePath = Path + "/" + Frame;
        ImageNode = Wz.Data[ImagePath];
        int Delay = WzDict.GetInt(ImagePath + "/delay", 100);

        Time += 16.66f * Delta;
        if (Time > Delay)
        {
            Frame += 1;
            if (!Wz.HasData(Path + '/' + Frame))
                Frame = 0;
            Time = 0;
        }

        Wz_Vector origin = WzDict.GetVector(ImagePath + "/origin");
        if (FlipX)
            Origin.X = -origin.X + ImageWidth;
        else
            Origin.X = origin.X;
        Origin.Y = origin.Y;
    }

    public override void DoDraw()
    {
        if (Map.ShowObj)
            base.DoDraw();
    }
}
