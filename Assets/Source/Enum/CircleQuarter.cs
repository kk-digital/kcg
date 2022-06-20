using System;

namespace Enums
{
    [Flags]
    public enum CircleQuarter
    {
        Error = 0,

        Top,
        Left,
        Right = 4,
        Bottom = 8,
        
        RightTop = 16,
        LeftTop = 32,
        RightBottom = 64,
        LeftBottom = 128,
    }
}

