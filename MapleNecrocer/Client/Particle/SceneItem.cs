﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WzComparerR2.MapRender.Patches3
{
    public class SceneItem
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public string Tag { get; set; }

        public override string ToString()
        {
            return $"{Name} {GetType().Name}";
        }
    }
}
