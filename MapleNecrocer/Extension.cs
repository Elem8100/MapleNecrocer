using DevComponents.AdvTree;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;

namespace MapleNecrocer;

internal static class Extension
{
    public static Wz_Node AsWzNode(this Node node)
    {
        return (node?.Tag as WeakReference)?.Target as Wz_Node;
    }

    public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        dict[key] = value;
        return dict;
    }
    public static string LeftStr(this string s, int count)
    {
        if (count > s.Length)
            count = s.Length;
        return s.Substring(0, count);
    }

    public static string RightStr(this string param, int length)
    {
        return param.Substring(param.Length - length, length);
    }

    public static bool ToBool(this int Value)
    {
        return Convert.ToBoolean(Value);
    }

    public static Int32 ToInt(this String number)
    {
        return Int32.Parse(number);
        
    }

    public static string IntID(this String Number)
    {
        return Number.ToInt().ToString();
    }
}
