using Microsoft.Xna.Framework;
using SharpDX;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGame.SpriteEngine;
public enum FittingCurveType { ConstantParameter, ConstantSpeed }
public enum NDrawMode { None, Curve, CurveCP, CurveCPHull }
public class NURBSCurve
{
    public NURBSCurve()
    {
        OrderOfCurve = 4;
        CPCount = 0;
        Array.Resize(ref ControlPoints, CPCount);
        Segments = 3000;
        FittingCurveType = FittingCurveType.ConstantSpeed;
        FittingCurveReady = false;
        UpdateKnots();
    }

    private int KnotsCount;
    private int OrderOfCurve;
    public float ParameterStart;
    public float ParameterEnd;
    private int segments;
    public FittingCurveType FittingCurveType;
    public bool FittingCurveReady;
    public Vector2[] ControlPoints;
    public float[] KnotsVector;
    public Vector2[] FittingCurve;
    public int CPCount;

    public int Segments
    {
        get => segments;
        set
        {
            segments = value;
            Array.Resize(ref FittingCurve, segments);
        }
    }
    private float BSFunction(int KnotIndex, int OrderOfCurve, float Parameter)
    {
        if ((Parameter >= KnotsVector[KnotIndex + OrderOfCurve]) || (Parameter < KnotsVector[KnotIndex]))
        {
            return 0;
        }
        else if (OrderOfCurve == 1)
        {
            if ((Parameter <= KnotsVector[KnotIndex + 1]) && (Parameter >= KnotsVector[KnotIndex]))
                return 1;
            else
                return 0;
        }
        else
        {
            float FirstItem, SecondItem, numerator;
            float denominator = KnotsVector[KnotIndex + OrderOfCurve - 1] - KnotsVector[KnotIndex];
            if (denominator == 0)
            {
                FirstItem = 0;
            }
            else
            {
                numerator = Parameter - KnotsVector[KnotIndex];
                if (numerator == 0)
                    FirstItem = 0;
                else
                    FirstItem = (numerator / denominator) * BSFunction(KnotIndex, (OrderOfCurve - 1), Parameter);

            }
            // { SecondItem}
            denominator = KnotsVector[KnotIndex + OrderOfCurve] - KnotsVector[KnotIndex + 1];
            if (denominator == 0)
            {
                SecondItem = 0;
            }
            else
            {
                numerator = KnotsVector[KnotIndex + OrderOfCurve] - Parameter;
                if (numerator == 0)
                    SecondItem = 0;
                else
                    SecondItem = (numerator / denominator) * BSFunction((KnotIndex + 1), (OrderOfCurve - 1), Parameter);

            }
            return FirstItem + SecondItem;
        }

    }

    private Vector2 CalcXY(float Parameter)
    {
        Vector2 Value = new Vector2(0, 0);
        float CheckSumm = 0;
        float N;
        for (int i = 0; i < CPCount; i++)
        {
            N = BSFunction(i, OrderOfCurve, Parameter);
            Value += ControlPoints[i] * N;
            CheckSumm += N;
        }
        Vector2 Result = new Vector2(Value.X / CheckSumm, Value.Y / CheckSumm);
        if (Parameter >= ParameterEnd)
        {
            Result.X = ControlPoints[CPCount - 1].X;
            Result.Y = ControlPoints[CPCount - 1].Y;
        }
        return Result;
    }
    static double Hypot(float x, float y)
    {
        return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
    }
    float Length2(Vector2 v)
    {
        return (float)Hypot(v.X, v.Y);
    }
    private float GetSegmentLength(int Index1, int Index2)
    {
        if (!FittingCurveReady)
            SetFittingCurve();
        float Result = 0;
        for (int i = Index1; i < Index2 - 1; i++)
            Result += Length2(new Vector2(FittingCurve[i].X, FittingCurve[i].Y) -
                              new Vector2(FittingCurve[i + 1].X, FittingCurve[i + 1].Y));
        return Result;
    }
    private float GetCurveLength()
    {
        if (!FittingCurveReady)
            SetFittingCurve();
        return GetSegmentLength(1, Segments - 1);
    }

    public float CurveLength
    {
        get
        {
            if (!FittingCurveReady)
                SetFittingCurve();

            return GetSegmentLength(1, Segments - 1) * 3000;
        }
    }

    public void SetFittingCurve()
    {
        float Parameter2, SegmentLength, Parameter2Delta, temp;
        float[] Parameter = new float[0];
        switch (FittingCurveType)
        {
            case FittingCurveType.ConstantParameter:
                Array.Resize(ref Parameter, Segments);
                Parameter[0] = ParameterStart;
                for (int i = 1; i < Segments; i++)
                    Parameter[i] = Parameter[i - 1] + (ParameterEnd - ParameterStart) / (Segments - 1);
                for (int i = 0; i < Segments; i++)
                    FittingCurve[i] = CalcXY(Parameter[i]);
                FittingCurve[Segments - 1].X = ControlPoints[CPCount - 1].X;
                FittingCurve[Segments - 1].Y = ControlPoints[CPCount - 1].Y;
                break;
            case FittingCurveType.ConstantSpeed:
                Parameter2 = ParameterStart;
                // getting CurveLength
                FittingCurveType = FittingCurveType.ConstantParameter;
                SetFittingCurve();
                FittingCurveType = FittingCurveType.ConstantSpeed;
                SegmentLength = CurveLength / (Segments - 1);
                Parameter2Delta = SegmentLength / Segments / 100;
                FittingCurve[0].X = ControlPoints[0].X;
                FittingCurve[0].Y = ControlPoints[0].Y;

                for (int i = 0; i < Segments - 1; i++)
                {
                    temp = 0;
                    do
                    {
                        FittingCurve[i] = CalcXY(Parameter2);
                        Parameter2 = Parameter2 + Parameter2Delta;
                        if (Parameter2 >= ParameterEnd)
                            break;
                        temp = GetSegmentLength(i - 1, i);
                    } while (temp >= SegmentLength);
                }

                FittingCurve[Segments - 1].X = ControlPoints[CPCount - 1].X;
                FittingCurve[Segments - 1].Y = ControlPoints[CPCount - 1].Y;
                break;
        }
        FittingCurveReady = true;
        Parameter = null;
    }
    public void UpdateKnots()
    {
        KnotsCount = CPCount + OrderOfCurve;
        Array.Resize(ref KnotsVector, KnotsCount);
        for (int i = 0; i < CPCount; i++)
        {
            KnotsVector[i + OrderOfCurve - 1] = i;
        }
        for (int i = KnotsCount - OrderOfCurve; i < KnotsCount; i++)
        {
            KnotsVector[i] = KnotsVector[KnotsCount - OrderOfCurve];
        }
        ParameterStart = KnotsVector[0];
        ParameterEnd = KnotsVector[KnotsCount - 1];
    }

    static float Angle2(Vector2 v)
    {
        return (float)Math.Atan2(v.X, v.Y);
    }
    public float GetTangent(float Parameter)
    {
        int Index;
        float ParameterDetla = 1000, Result = 0;
        Vector2 p1, p2;
        switch (FittingCurveType)
        {
            case FittingCurveType.ConstantParameter:

                if (Parameter >= 1)
                    Parameter = Parameter - ParameterDetla;
                p1 = CalcXY(Parameter * ParameterEnd);
                p2 = CalcXY(Parameter * ParameterEnd + ParameterDetla);
                Result = Angle2(p2 - p1);
                break;
            case FittingCurveType.ConstantSpeed:
                Index = (int)(Parameter * Segments) * 3000;
                if (Index >= Segments - 1)
                    Index = Segments - 2;
                Result = Angle2(FittingCurve[Index + 1] - FittingCurve[Index]);
                break;
        }
        return Result;
    }

    public Vector2 GetXY(float Parameter)
    {
        int Index;
        float k, Residue, Span;
        Vector2 Result = new Vector2();

        switch (FittingCurveType)
        {
            case FittingCurveType.ConstantParameter:
                Result = CalcXY(Parameter * ParameterEnd);

                break;

            case FittingCurveType.ConstantSpeed:
                Index = (int)(Parameter * Segments);
                if (Index > Segments - 2)
                {
                    Result.X = FittingCurve[Segments - 1].X;
                    Result.Y = FittingCurve[Segments - 1].Y;
                    break;
                }
                Residue = Parameter * Segments - Index;
                k = FittingCurve[Index + 1].Y - FittingCurve[Index].Y;
                k = k / (FittingCurve[Index + 1].X - FittingCurve[Index].X);
                Span = GetSegmentLength(Index, Index + 1);
                Span = Span * Residue;
                Result.X = Span * (float)Math.Cos(Math.Atan2(k, 1));
                Result.Y = Span * (float)Math.Sin(Math.Atan2(k, 1));

                if (FittingCurve[Index + 1].X > FittingCurve[Index].X)
                {
                    Result.X = FittingCurve[Index].X + Result.X;
                    Result.Y = FittingCurve[Index].Y + Result.Y;
                }
                else
                {
                    Result.X = FittingCurve[Index].X - Result.X;
                    Result.Y = FittingCurve[Index].Y - Result.Y;
                }

                break;
        }
        return Result;

    }
}

public class NURBSCurveEx : NURBSCurve
{
    public NURBSCurveEx() : base()
    {
        CPRadius = 10;
    }
    public int CreateCP(int X, int Y)
    {
        CPCount += 1;
        Array.Resize(ref ControlPoints, CPCount);
        ControlPoints[CPCount - 1].X = X;
        ControlPoints[CPCount - 1].Y = Y;
        UpdateKnots();
        return CPCount - 1;
    }

    public int CPRadius;

    public void DeleteCp()
    {
        if (CPCount == 1)
            return;
        CPCount -= 1;
        Array.Resize(ref ControlPoints, CPCount);
        UpdateKnots();
    }
    public void Draw()
    {
    }

    public int GetCP(int X, int Y)
    {
        for (int i = 0; i < CPCount; i++)
        {
            if (CPRadius >= Math.Sqrt((ControlPoints[i].X - X) * (ControlPoints[i].X - X) + (ControlPoints
               [i].Y - Y) * (ControlPoints[i].Y - Y)))
            {
                return i;

            }
        }
        return -1;
    }
    public void LoadCurve(string FileName)
    {
        string AllText = System.IO.File.ReadAllText(FileName);
        string[] Section = AllText.Split('/');
        int Length = Section.Length;
        CPCount = Length;
        Array.Resize(ref ControlPoints, CPCount);
        int x = 0, y = 0;
        for (int i = 0; i < CPCount - 1; i++)
        {
            var Str = Section[i].Split(',');
            x = int.Parse(Regex.Replace(Str[0], @"\D", ""));
            y = int.Parse(Regex.Replace(Str[1], @"\D", ""));
            ControlPoints[i].X = x;
            ControlPoints[i].Y = y;
        }
        FittingCurveReady = false;
        UpdateKnots();
    }

    public void Updata()
    {
        Vector2[] Points = new Vector2[0];
        if (CPCount > 4)
        {
            Array.Resize(ref Points, CPCount);
            for (int i = 0; i < CPCount; i++)
            {
                Points[i].X = ControlPoints[i].X;
                Points[i].Y = ControlPoints[i].Y;
            }
            SetFittingCurve();
        }
        Points = null;
    }



}