using System;
using System.Collections.Generic;
using System.Text;

namespace Teroll.Objects
{
    [Flags]
    public enum CollisionSide
    {
        None = 0,

        Top = 1,
        Bottom = 2,
        Left = 4,
        Right = 8,

        All = Top | Bottom | Left | Right
    }
}
