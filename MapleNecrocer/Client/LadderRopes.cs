using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MapleNecrocer;

public struct LadderRope
{
    public int X, Y1, Y2, Page, uf, L;
    public static List<LadderRope> LadderRopeList;
    public static LadderRope Find(Vector2 P, ref bool OnLadder)
    {
        OnLadder = false;
        foreach (var L in LadderRopeList)
        {
            if ((P.X > L.X - 10) && (P.X < L.X + 10) && (P.Y < L.Y2 + 12) && (P.Y > L.Y1 - 12))
            {
                OnLadder = true;
                return L;
            }
        }
        return new LadderRope();
    }

    public static void Create()
    {
        if (LadderRopeList == null)
            LadderRopeList = new();
        else
            LadderRopeList.Clear();
        LadderRope LadderRope = new ();
        foreach (var Iter in Map.Img.Nodes["ladderRope"].Nodes)
        {
            LadderRope.X = Iter.GetInt("x");
            LadderRope.Y1 = Iter.GetInt("y1");
            LadderRope.Y2 = Iter.GetInt("y2");
            LadderRope.L = Iter.GetInt("l");
            LadderRope.Page = Iter.GetInt("page");
            LadderRope.uf = Iter.GetInt("uf");
            LadderRopeList.Add(LadderRope);
        }
    }

}

