using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapleNecrocer;

public class TamingMob : SpriteEx
{
    public TamingMob(Sprite Parent) : base(Parent)
    {
    }




    public static bool IsUse;
    public static string CharacterAction;
    public static Vector2 Navel;
    public static bool IsChairTaming;


}
