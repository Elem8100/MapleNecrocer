using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Point;
namespace MapleNecrocer;

public class Foothold
{
    public Foothold(Vector2 P1, Vector2 P2, int ID)
    {
        p1 = P1;
        p2 = P2;
        id = ID;
    }
    private Vector2 p1, p2;
    private int id;
    public int ID { get => id; set => id = value; }
    public int PlatformID;
    public int Z;
    public int NextID;
    public int PrevID;
    public int X1 { get => (int)p1.X; }
    public int X2 { get => (int)p2.X; }
    public int Y1 { get => (int)p1.Y; }
    public int Y2 { get => (int)p2.Y; }
    public bool IsWall()
    {
        if (p1.X == p2.X)
            return true;
        return false;
    }
    public Foothold Prev;
    public Foothold Next;
}

public class FootholdTree
{
    private List<Foothold> footholds = new();
    private Vector2 P1, P2;
    public static FootholdTree Instance;
    public static List<int> MinX1, MaxX2;
    public List<Foothold> Footholds { get => footholds; }
    public FootholdTree(Vector2 P1, Vector2 P2)
    {
        this.P1 = P1;
        this.P2 = P2;
    }
    public Foothold GetPrev(Foothold FH)
    {
        Foothold Result = null;
        foreach (var F in footholds)
        {
            if (F.ID == FH.PrevID)
                Result = F;
        }
        return Result;
    }
    public Foothold GetNext(Foothold FH)
    {
        Foothold Result = null;
        foreach (var F in footholds)
        {
            if (F.ID == FH.NextID)
                Result = F;
        }
        return Result;
    }

    public Vector2 FindBelow(Vector2 Pos, ref Foothold FH)
    {
        bool First = true;
        double MaxY = 0, CMax = 0;
        Vector2 Result = new Vector2(0, 0);
        int X = (int)Pos.X;
        int Y = (int)Pos.Y;

        foreach (var F in footholds)
        {
            if (X >= F.X1 && X <= F.X2 || X >= F.X2 && X <= F.X1)
            {

                if (First)
                {
                    if (F.X1 == F.X2)
                        continue;
                    MaxY = (float)(F.Y1 - F.Y2) / (F.X1 - F.X2) * (X - F.X1) + F.Y1;
                    FH = F;
                    if (MaxY >= Y)
                        First = false;
                }
                else
                {
                    if (F.X1 == F.X2)
                        continue;
                    CMax = (float)(F.Y1 - F.Y2) / (F.X1 - F.X2) * (X - F.X1) + F.Y1;
                    if (CMax < MaxY && CMax >= Y)
                    {
                        FH = F;
                        MaxY = CMax;
                    }
                }
            }
        }

        if (!First)
        {
            Result.X = X;
            Result.Y = (float)MaxY;

        }
        else
        {
            Result = new Vector2(99999, 99999);
        }
        return Result;
    }

    public Foothold FindWallR(Vector2 P)
    {
        Foothold Result = null;
        bool First = true;
        double MaxX = 0, CMax = 0;
        int X = (int)P.X;
        int Y = (int)P.Y;
        foreach (var F in footholds)
        {
            if (F.IsWall() && F.X1 <= P.X && F.Y1 <= P.Y && F.Y2 >= P.Y)
            {
                if (First)
                {
                    MaxX = F.X1;
                    Result = F;
                    if (MaxX <= X)
                        First = false;
                }
                else
                {
                    CMax = F.X1;
                    if (CMax > MaxX && CMax <= X)
                    {
                        MaxX = CMax;
                        Result = F;
                    }
                }
            }
        }
        return Result;
    }

    public Foothold FindWallL(Vector2 P)
    {
        Foothold Result = null;
        bool First = true;
        double MaxX = 0, CMax = 0;
        int X = (int)P.X;
        int Y = (int)P.Y;
        foreach (var F in footholds)
        {
            if (F.IsWall() && F.X1 >= P.X && F.Y1 >= P.Y && F.Y2 <= P.Y)
            {
                if (First)
                {
                    MaxX = F.X1;
                    Result = F;
                    if (MaxX >= X)
                        First = false;
                }
                else
                {
                    CMax = F.X1;
                    if (CMax < MaxX && CMax >= X)
                    {
                        MaxX = CMax;
                        Result = F;
                    }
                }
            }
        }
        return Result;
    }
    public bool ClosePlatform(Foothold FH)
    {
        int Count = 0;
        bool Result = false;
        foreach (var F in footholds)
        {
            if (F.PlatformID == FH.PlatformID && F.IsWall())
                Count += 1;
        }
        if (Count == 2)
            Result = true;
        return Result;
    }
    public void Insert(Foothold F)
    {
        footholds.Add(F);
    }

    public void DrawFootholds()
    {
        int WX = (int)EngineFunc.SpriteEngine.Camera.X;
        int WY = (int)EngineFunc.SpriteEngine.Camera.Y;
        foreach (var FH in Footholds)
        {
            EngineFunc.Canvas.DrawLine(new Point(FH.X1 - WX, FH.Y1 - WY), new Point(FH.X2 - WX, FH.Y2 - WY), 1, Microsoft.Xna.Framework.Color.Red);
            if (FH.X1 != FH.X2)
                EngineFunc.Canvas.DrawLine(new Point(FH.X1 - WX, FH.Y1 - WY + 1), new Point(FH.X2 - WX, FH.Y2 - WY + 1), 1, Microsoft.Xna.Framework.Color.Red);
            else
                EngineFunc.Canvas.DrawLine(new Point(FH.X1 - WX + 1, FH.Y1 - WY), new Point(FH.X2 - WX + 1, FH.Y2 - WY),1, Microsoft.Xna.Framework.Color.Red);
        }
    }
    public static void CreateFootholds()
    {
        if (Instance == null)
        {
            Instance = new FootholdTree(new Vector2(100, 10), new Vector2(-100, -100));
            MinX1 = new();
            MaxX2 = new();
        }
        else
        {
            Instance.Footholds.Clear();
            MinX1.Clear();
            MaxX2.Clear();
        }


        Foothold FH;
        foreach (var Iter in Map.Img.Nodes["foothold"].Nodes)
        {
            foreach (var Iter2 in Iter.Nodes)
            {
                foreach (var Iter3 in Iter2.Nodes)
                {
                    int X1 = Iter3.GetInt("x1");
                    int Y1 = Iter3.GetInt("y1");
                    int X2 = Iter3.GetInt("x2");
                    int Y2 = Iter3.GetInt("y2");
                    FH = new Foothold(new Vector2(X1, Y1), new Vector2(X2, Y2), 0);
                    FH.NextID = Iter3.GetInt("next");
                    FH.PrevID = Iter3.GetInt("prev");
                    FH.PlatformID = Iter2.Text.ToInt();
                    FH.ID = Iter3.Text.ToInt();
                    FH.Z = Iter.Text.ToInt();
                    Instance.Insert(FH);
                    MinX1.Add(X1);
                    MaxX2.Add(X2);
                }
            }
        }


        List<Foothold> FHs;
        MinX1.Sort();
        MaxX2.Sort();
        FHs = Instance.Footholds;
        for (int i = 0; i < FHs.Count; i++)
        {
            for (int j = 0; j < FHs.Count; j++)
            {
                if (i == j)
                    continue;
                if (FHs[j].ID == FHs[i].PrevID)
                    FHs[i].Prev = FHs[j];
                if (FHs[j].ID == FHs[i].NextID)
                    FHs[i].Next = FHs[j];
            }
        }
    }

}
