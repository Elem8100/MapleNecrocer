using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapleNecrocer;

public class Skill : SpriteEx
{
    public Skill(Sprite Parent) : base(Parent)
    {
    }
    public static bool PlayEnded;
    public static string ID;
    public static bool Attacking;
    public static bool Start;
}

