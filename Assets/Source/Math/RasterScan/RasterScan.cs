using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMath
{
    // Cell just like points, simple struct have a x,y points
    // See: https://docs.microsoft.com/en-us/dotnet/api/system.drawing.point?view=net-6.0
    public struct Cell
    {
        public int x, y;
    }

    // Raster Scaning for terrain collision.
    // See: https://en.m.wikipedia.org/wiki/Bresenham%27s_line_algorithm
    // See: https://www.cs.helsinki.fi/group/goa/mallinnus/lines/gsoft2.html
    public static class RasterScan
    {
        public static IEnumerable<Cell> LineTo(this Cell a, Cell b)
        {
            int dx = System.Math.Abs(b.x - a.x);
            int dy = System.Math.Abs(b.y - a.y);
            int x_inc = (b.x < a.x) ? -1 : 1; // increment in only one
            int y_inc = (b.y < a.y) ? -1 : 1; // direction in each axis

            if (dx == dy) // Handle perfect diagonals (my personal touch)
            {
                // I include this "optimization" for more aesthetic reasons.
                // While Bresenham's Line can handle perfect diagonals just fine, it adds
                // additional cells to the line that make it not a perfect diagonal
                // anymore. So, while this branch is ~twice as fast as the next branch,
                // the real reason it is here is for style.

                while (dx-- > 0)
                {
                    yield return a;

                    a.x += x_inc;
                    a.y += y_inc;
                }

                yield return b; // this ensures the target of the line
                yield break;    // is actually included.
            }

            // Handle all other lines (adapted from Bresenham)
            bool side_equal;
            if (a.x == b.x) side_equal = (a.y < b.y);
            else side_equal = (a.x < b.x);

            int i = dx + dy;
            int error = dx - dy;
            dx *= 2;
            dy *= 2;

            while (i-- > 0)
            {
                yield return a;

                if (error > 0 || (side_equal && error == 0))
                {
                    a.x += x_inc;
                    error -= dy;
                }
                else
                {
                    a.y += y_inc;
                    error += dx;
                }
            }

            yield return b; // this ensures the target of the line
            yield break;    // is actually included.
        } // end function LineTo
    }
}