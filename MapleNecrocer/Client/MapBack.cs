using WzComparerR2.WzLib;
using WzComparerR2.PluginBase;
using Spine;
using WzComparerR2.Animation;
using Microsoft.Xna.Framework;
using System.Drawing;
using Microsoft.Xna.Framework.Graphics;

namespace MapleNecrocer;

public class Back : BackgroundSprite
{
    public Back(Sprite Parent) : base(Parent)
    {
        BlendMode = MonoGame.SpriteEngine.BlendMode.NonPremultiplied;
    }
    string Path;
    int Frame;
    int RX, RY;
    Vector2 Pos;
    int BackType;
    float Time;
    bool Flip;
    bool Front;
    float AX, AY;
    int FlowX, FlowY;
    int MoveType;
    int MoveW, MoveH;
    int MoveP;
    int MoveR;
    float DeltaTime;
    bool HasAnim;
    public static bool ResetPos;

    public static void Create()
    {
        foreach (var Iter in Map.Img.GetNode("back").Nodes)
        {
            string bS = Iter.GetStr("bS");
            if (bS == "") continue;
            string No = Iter.GetStr("no");
            int Ani = Iter.GetInt("ani");
            int PosX = Iter.GetInt("x");
            int PosY = Iter.GetInt("y");
            int RX = Iter.GetInt("rx");
            int RY = Iter.GetInt("ry");
            int CX = Iter.GetInt("cx");
            int CY = Iter.GetInt("cy");
            int FlowX = Iter.GetInt("flowX");
            int FlowY = Iter.GetInt("flowY");
            int ZLayer = int.Parse(Iter.Text);
            bool Front = Iter.GetBool("front");
            int BackType = Iter.GetInt("type");
            string Path = "";
            bool Tiled = false;
            TileMode TileMode = TileMode.Horizontal;
            if (!Wz.HasNode("UI/UIWindow4.img"))
            {
                if (bS == "dryRock" && No == "1")
                    BackType = 1;
            }

            switch (BackType)
            {
                case 0:
                    Tiled = false;
                    break;
                case 1:
                    Tiled = true;
                    TileMode = TileMode.Horizontal;
                    break;
                case 2:
                    Tiled = true;
                    TileMode = TileMode.Vertical;
                    break;
                case 3:
                    Tiled = true;
                    TileMode = TileMode.Full;
                    break;
                case 4:
                    Tiled = true;
                    TileMode = TileMode.Horizontal;
                    break;
                case 5:
                    Tiled = true;
                    TileMode = TileMode.Vertical;
                    break;
                case 6:
                    Tiled = true;
                    TileMode = TileMode.Full;
                    break;
                case 7:
                    Tiled = true;
                    TileMode = TileMode.Full;
                    break;
            }

            if (Ani == 2)
            {
                Path = "Map/Back/" + bS + ".img/spine/" + No;
                var SpineBack = new SpineBack(EngineFunc.SpriteEngine);
                var aniData = Map.ResLoader.LoadAnimationData(Wz.GetNodeA(Path));

                if (((ISpineAnimationData)aniData).SpineVersion == SpineVersion.V2)
                {
                    SpineBack.SpineAnimatorV2 = new SpineAnimatorV2((SpineAnimationDataV2)aniData);
                    SpineBack.Version = 2;
                }
                else
                {
                    SpineBack.SpineAnimatorV4 = new SpineAnimatorV4((SpineAnimationDataV4)aniData);
                    SpineBack.Version = 4;
                }

                string spineAni = Iter.GetStr("spineAni");
                if (SpineBack.Version == 2)
                {
                    if (spineAni != "")
                        SpineBack.SpineAnimatorV2.SelectedAnimationName = spineAni;
                    else
                        SpineBack.SpineAnimatorV2.SelectedAnimationIndex = 0;
                }

                if (SpineBack.Version == 4)
                {
                    if (spineAni != "")
                        SpineBack.SpineAnimatorV4.SelectedAnimationName = spineAni;
                    else
                        SpineBack.SpineAnimatorV4.SelectedAnimationIndex = 0;
                }


                SpineBack.Pos.X = PosX;
                SpineBack.Pos.Y = PosY;
                SpineBack.RX = RX;
                SpineBack.RY = RY;
                Bitmap Png = null;
                if (Wz.GetNodeA(Path + "/0") != null)
                {
                    Png = Wz.GetNode(Path + "/0").ExtractPng();
                }
                else
                {
                    foreach (var i in Wz.GetNodeA(Path).Nodes)
                    {
                        if (i.Text.RightStr(3) == "png")
                        {
                            Png = i.ExtractPng();
                            break;
                        }
                    }
                }
                if (CX == 0)
                    SpineBack.Width = Png.Width;
                else
                    SpineBack.Width = CX;
                if (CY == 0)
                    SpineBack.Height = Png.Height;
                else
                    SpineBack.Height = CY;
                if (bS == "downtown" && No == "3")
                    SpineBack.Width = 800;

                SpineBack.BackType = BackType;
                SpineBack.Tiled = Tiled;
                SpineBack.TileMode = TileMode;
                float WX = EngineFunc.SpriteEngine.Camera.X;
                float WY = EngineFunc.SpriteEngine.Camera.Y;
                SpineBack.X = -PosX - (100f + RX) / 100f * (WX + Map.DisplaySize.X / 2) + WX;
                SpineBack.Y = -PosY - (100f + RY) / 100f * (WY + Map.DisplaySize.Y / 2) + WY;
                if (Front)
                    SpineBack.Z = ZLayer + 1000000;
                else
                    SpineBack.Z = ZLayer - 1000;
                SpineBack.SkeletonRenderer = new Spine.SkeletonRenderer(RenderFormDraw.Instance.GraphicsDevice);
            }
            else
            {
                var Back = new Back(EngineFunc.SpriteEngine);
                Back.ImageLib = Wz.ImageLib;
                Wz_Vector origin = new Wz_Vector(0, 0);
                string ImagePath = "";

                if (Ani == 0)
                {
                    Path = "Map/Back/" + bS + ".img/back/" + No;
                    ImagePath = Path;
                    if (Wz.GetNodeA(Path) == null)
                        continue;
                    if (!Wz.Data.ContainsKey(Path))
                        Wz.DumpData(Wz.GetNodeA(Path), Wz.Data, Wz.ImageLib);
                    Back.Path = Path;
                    Back.ImageNode = Wz.Data[Path];
                    origin = WzDict.GetVector(Path + "/origin");
                    if (WzDict.GetBool(Path + "/blend"))
                        Back.BlendMode = MonoGame.SpriteEngine.BlendMode.AddtiveColor;
                }

                if (Ani == 1)
                {
                    Path = "Map/Back/" + bS + ".img/ani/" + No;
                    ImagePath = Path + "/0";
                    if (Wz.GetNodeA(Path) == null)
                        continue;
                    if (!Wz.Data.ContainsKey(Path))
                        Wz.DumpData(Wz.GetNodeA(Path), Wz.Data, Wz.ImageLib);
                    Back.Path = Path;
                    Back.ImageNode = Wz.Data[Path + "/0"];
                    origin = WzDict.GetVector(Path + "/0/origin");
                    Back.HasAnim = true;
                }
                Back.FlipX = Iter.GetBool("f");
                if (Back.FlipX)
                    Back.Origin.X = -origin.X + Back.ImageWidth;
                else
                    Back.Origin.X = origin.X;
                Back.Origin.Y = origin.Y;
                Back.Width = Back.ImageWidth;
                Back.Height = Back.ImageHeight;
                Back.Pos.X = PosX;
                Back.Pos.Y = PosY;
                Back.RX = RX;
                Back.RY = RY;
                Back.FlowX = FlowX;
                Back.FlowY = FlowY;

                Back.MoveType = WzDict.GetInt(ImagePath + "/moveType");
                Back.MoveP = WzDict.GetInt(ImagePath + "/moveP");
                Back.MoveW = WzDict.GetInt(ImagePath + "/moveW");
                Back.MoveH = WzDict.GetInt(ImagePath + "/moveH");
                Back.MoveR = WzDict.GetInt(ImagePath + "/moveR");

                if (CX == 0)
                    Back.Width = Back.ImageWidth;
                else
                    Back.Width = CX;
                if (CY == 0)
                    Back.Height = Back.ImageHeight;
                else
                    Back.Height = CY;

                float WX = EngineFunc.SpriteEngine.Camera.X;
                float WY = EngineFunc.SpriteEngine.Camera.Y;
                Back.X = -PosX - (100f + RX) / 100f * (WX + Map.DisplaySize.X / 2) + WX;
                Back.Y = -PosY - (100f + RY) / 100f * (WY + Map.DisplaySize.Y / 2) + WY;

                if (Front)
                    Back.Z = ZLayer + 1000000;
                else
                    Back.Z = ZLayer - 1000;
                Back.Front = Front;
                Back.AX = Back.X;
                Back.AY = Back.Y;
                Back.BackType = BackType;
                Back.Tiled = Tiled;
                Back.TileMode = TileMode;
            }
        }
        ResetPos = true;
    }

    public override void DoMove(float Delta)
    {
        switch (BackType)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                if (Map.CameraSpeed.X != 0)
                    X -= RX * Map.CameraSpeed.X / 100f;
                if (Map.CameraSpeed.Y != 0)
                    Y -= RY * Map.CameraSpeed.Y / 100f;
                break;

            case 4:
            case 6:
                if (Map.CameraSpeed.X != 0)
                    X += Map.CameraSpeed.X;
                if (Map.CameraSpeed.Y != 0)
                    Y -= RY * Map.CameraSpeed.Y / 100f;
                X -= RX * 5f / 60;
                break;

            case 5:
            case 7:
                if (Map.CameraSpeed.X != 0)
                    X -= RX * Map.CameraSpeed.X / 100f;
                if (Map.CameraSpeed.Y != 0)
                    Y += Map.CameraSpeed.Y;
                Y -= RY * 5f / 60f;
                break;
        }

        if (FlowX.ToBool())
        {
            X -= FlowX * 5f / 60;
        }
        if (FlowY.ToBool())
        {
            Y -= FlowY * 5f / 60;
        }


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
                    // case 3:
                    //Angle += (17f / MoveR) * 3.14159f * 2;
                    //  break;
            }
        }

        if (MoveR.ToBool())
        {
            Angle += (17f / MoveR) * 3.14159f * 2;
        }
        if (HasAnim)
        {
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

            Wz_Vector origin = WzDict.GetVector(ImagePath + "/origin");
            if (FlipX)
                Origin.X = -origin.X + ImageWidth;
            else
                Origin.X = origin.X;
            Origin.Y = origin.Y;
        }

        if (ResetPos)
        {
            X = -Pos.X - (100f + RX) / 100f * (Engine.Camera.X + Map.DisplaySize.X / 2) + Engine.Camera.X;
            Y = -Pos.Y - (100f + RY) / 100f * (Engine.Camera.Y + Map.DisplaySize.Y / 2 + Map.OffsetY) + Engine.Camera.Y;
        }

        if (Map.SaveMap)
        {
            X = -Pos.X - (100 + RX) / 100 * (Engine.Camera.X + 1366 / 2) + Engine.Camera.X;
            if (Front)
            {
                if (Map.Info.ContainsKey("VRLeft"))
                    Y = -Pos.Y - (100f + RY) / 100f * (Map.Bottom - 600 + (600 / 2)) + Map.Top;
                else
                    Y = -Pos.Y - (100f + RY) / 100f * (Map.SaveMapBottom - 600 + (600 / 2) - 100) + Map.Top;
            }
            else
            {
                if (Map.Info.ContainsKey("VRLeft"))
                    Y = -Pos.Y - (100f + RY * (float)Convert.ToDouble(SaveMapForm.Instance.comboBox2.Text)) / 100f *
                      (Map.Bottom - 600 + (600 / 2)) + Map.Top - SaveMapForm.Instance.comboBox1.Text.ToInt();
                else
                    Y = -Pos.Y - (100f + RY * (float)Convert.ToDouble(SaveMapForm.Instance.comboBox2.Text)) / 100 *
                      (Map.SaveMapBottom - 600 + (600 / 2) - 100) + Map.Top - SaveMapForm.Instance.comboBox1.Text.ToInt();
            }
        }

        if (Front)
        {
            if (Map.ShowFront)
                Visible = true;
            else
                Visible = false;
        }
        else
        {
            if (Map.ShowBack)
                Visible = true;
            else
                Visible = false;
        }

    }
    public override void DoDraw()
    {
        base.DoDraw();

        if (ResetPos)
            ResetPos = false;
    }


}

public class SpineBack : SpriteEx
{
    public SpineBack(Sprite Parent) : base(Parent)
    {
    }
    public Vector2 Pos;
    public int RX, RY;
    public int BackType;
    public TileMode TileMode;
    public bool Tiled;
    public Spine.SkeletonRenderer SkeletonRenderer;
    public SpineAnimatorV2 SpineAnimatorV2;
    public SpineAnimatorV4 SpineAnimatorV4;
    public int Version;
    private Matrix? matrix;
    public override void DoMove(float Delta)
    {
        X = -Pos.X - (100f + RX) / 100f * (Engine.Camera.X + Map.DisplaySize.X / 2) + Engine.Camera.X;
        Y = -Pos.Y - (100f + RY) / 100f * (Engine.Camera.Y + Map.DisplaySize.Y / 2 + Map.OffsetY) + Engine.Camera.Y;
        if (Z > 0)
        {
            if (Map.ShowFront)
                Visible = true;
            else
                Visible = false;
        }
        else
        {
            if (Map.ShowBack)
                Visible = true;
            else
                Visible = false;
        }
    }
    public override void Draw()
    {
        if (Visible)
        {
            if (Engine != null)
            {
                DoDraw();
                Engine.DrawCount++;
            }

            if (DrawList != null)
            {
                for (int i = 0; i < DrawList.Count; i++)
                    ((SpineBack)DrawList[i]).Draw();
            }
        }
    }
    public override void DoDraw()
    {
        int ChipWidth = this.Width;
        int ChipHeight = this.Height;
        int dWidth = (Engine.VisibleWidth + ChipWidth) / ChipWidth + 1;
        int dHeight = (Engine.VisibleHeight + ChipHeight) / ChipHeight + 1;
        float _x;
        float _y;
        _x = -X;
        _y = -Y;

        float OfsX = _x % ChipWidth;
        float OfsY = _y % ChipHeight;
        int StartX = (int)_x / ChipWidth;
        int StartX_ = 0;
        if (StartX < 0)
        {
            StartX_ = -StartX;
            StartX = 0;
        }
        int StartY = (int)_y / ChipHeight;
        int StartY_ = 0;
        if (StartY < 0)
        {
            StartY_ = -StartY;
            StartY = 0;
        }

        int EndX = Math.Min(StartX + 1 - StartX_, dWidth);
        int EndY = Math.Min(StartY + 1 - StartY_, dHeight);

        switch (TileMode)
        {
            case TileMode.Horizontal:
                dWidth = (Engine.VisibleWidth + ChipWidth) / ChipWidth + 1;
                dHeight = -1;
                break;
            case TileMode.Vertical:
                dWidth = -1;
                dHeight = (Engine.VisibleHeight + ChipHeight) / ChipHeight + 1;
                break;
            case TileMode.Full:
                dWidth = (Engine.VisibleWidth + ChipWidth) / ChipWidth + 1;
                dHeight = (Engine.VisibleHeight + ChipHeight) / ChipHeight + 1;
                break;
        }



        if (Version == 2)
        {
            SpineAnimatorV2.Update(TimeSpan.FromMilliseconds(17));
        }
        else if (Version == 4)
        {
            SpineAnimatorV4.Update(TimeSpan.FromMilliseconds(17));
        }
        if (SkeletonRenderer.Effect is BasicEffect basicEff)
        {
            basicEff.World = matrix ?? Matrix.Identity;
            basicEff.Projection = Matrix.CreateOrthographicOffCenter(0, Map.DisplaySize.X, Map.DisplaySize.Y, 0, 1, 0);
        }

        SkeletonRenderer.Begin();
        if (Tiled)
        {
            for (int cy = -1; cy <= dHeight; cy++)
            {
                for (int cx = -1; cx <= dWidth; cx++)
                {
                    switch (TileMode)
                    {
                        case TileMode.Horizontal:
                            if (Version == 2)
                            {
                                SpineAnimatorV2.Skeleton.X = cx * ChipWidth + OfsX - Offset.X;
                                SpineAnimatorV2.Skeleton.Y = _y - Offset.Y;
                            }
                            else if (Version == 4)
                            {
                                SpineAnimatorV4.Skeleton.X = cx * ChipWidth + OfsX - Offset.X;
                                SpineAnimatorV4.Skeleton.Y = _y - Offset.Y;
                            }
                            break;

                        case TileMode.Vertical:
                            if (Version == 2)
                            {
                                SpineAnimatorV2.Skeleton.X = _x - Offset.X;
                                SpineAnimatorV2.Skeleton.Y = cy * ChipHeight + OfsY - Offset.Y;
                            }
                            else if (Version == 4)
                            {
                                SpineAnimatorV4.Skeleton.X = _x - Offset.X;
                                SpineAnimatorV4.Skeleton.Y = cy * ChipHeight + OfsY - Offset.Y;

                            }
                            break;

                        case TileMode.Full:
                            if (Version == 2)
                            {
                                SpineAnimatorV2.Skeleton.X = cx * ChipWidth + OfsX - Offset.X;
                                SpineAnimatorV2.Skeleton.Y = cy * ChipHeight + OfsY - Offset.Y;
                            }
                            if (Version == 4)
                            {
                                SpineAnimatorV4.Skeleton.X = cx * ChipWidth + OfsX - Offset.X;
                                SpineAnimatorV4.Skeleton.Y = cy * ChipHeight + OfsY - Offset.Y;
                            }
                            break;
                    }

                    if (Version == 2)
                    {
                        SkeletonRenderer.Draw(SpineAnimatorV2.Skeleton);
                    }
                    else if (Version == 4)
                    {
                        SkeletonRenderer.Draw(SpineAnimatorV4.Skeleton);
                    }

                }
            }
        }
        else
        {
            for (int cy = StartY; cy < EndY; cy++)
            {
                for (int cx = StartX; cx < EndX; cx++)
                {
                    if (Version == 2)
                    {
                        SpineAnimatorV2.Skeleton.X = cx * ChipWidth + OfsX - Offset.X;
                        SpineAnimatorV2.Skeleton.Y = cy * ChipHeight + OfsY - Offset.Y;
                        SkeletonRenderer.Draw(SpineAnimatorV2.Skeleton);
                    }
                    else if (Version == 4)
                    {
                        SpineAnimatorV4.Skeleton.X = cx * ChipWidth + OfsX - Offset.X;
                        SpineAnimatorV4.Skeleton.Y = cy * ChipHeight + OfsY - Offset.Y;
                        SkeletonRenderer.Draw(SpineAnimatorV4.Skeleton);
                    }
                }
            }
        }
        SkeletonRenderer.End();

    }
}




