using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MapleNecrocer;

public class MapleChair : SpriteEx
{

    public MapleChair(Sprite Parent) : base(Parent)
    {
    }

    public static bool CanUse;
    public static bool IsUse;
    public static bool HasSitAction;
    public static bool UseTamingNavel;
    public static Vector2 BodyRelMove;
    public static string CharacterAction;
}

