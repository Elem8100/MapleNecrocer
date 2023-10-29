using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGame.UI.Forms
{
    internal interface IControls
    {
        List<Control> Controls { get; }

        Control FindControlAt(Microsoft.Xna.Framework.Point position);
    }
}
