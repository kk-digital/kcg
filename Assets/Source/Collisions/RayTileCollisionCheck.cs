using KMath;

// Bresenham's line algorithm
// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm

// Line Width = 1 pixel
// Functions for getting int coordinates of Line
// Used for example for a bullet checking
// Ray from start position to the End position
// One returns all tiles
// One has radius 0
// One keeps going from start to finish

//TODO: Function that returns all tiles on ray
//TODO: Function for Raycast Tile Grid for variable width line
//TODO: Function for ray casting tile grid, that only returns the first

namespace Collisions
{
    public static class RayTileCollisionCheck
    {
        private static Vec2i[] PlotLineLow(Vec2i start, Vec2i end)
        {
            Vec2i delta = end - start;

            int y_inc = 1;

            if (delta.Y < 0)
            {
                y_inc = -1;

                delta.Y = -delta.Y;
            }

            var D = 2 * delta.Y - delta.X;
            var y = start.Y;
            
            var outputCount = end.X - start.X;
            var output = new Vec2i[outputCount];
            var outputIndex = 0;

            for (int x = start.X; x <= end.X; x++)
            {
                // plot(x, y); Output for the position at Line
                output[outputIndex] = new Vec2i(x, y);
                outputIndex++;
                if (D > 0)
                {
                    y += y_inc;
                    D += 2 * (delta.Y - delta.X);
                }
                else
                {
                    D += 2 * delta.Y;
                }
            }

            return output;
        }

        private static Vec2i[] PlotLineHigh(Vec2i start, Vec2i end)
        {
            Vec2i delta = end - start;

            int x_inc = 1;
            if (delta.X < 0)
            {
                x_inc = -1;
                delta.X = -delta.X;
            }

            var D = 2 * delta.X - delta.Y;
            var x = start.X;

            var outputCount = end.Y - start.Y;
            var output = new Vec2i[outputCount];
            var outputIndex = 0;

            for (int y = start.Y; y <= end.Y; y++)
            {
                // plot(x, y); Output for the position at Line
                output[outputIndex] = new Vec2i(x, y);
                outputIndex++;
                if (D > 0)
                {
                    x += x_inc;
                    D += 2 * (delta.X - delta.Y);

                }
                else
                {
                    D += 2 * delta.X;
                }
            }

            return output;
        }

        public static Vec2i[] GetRayCoordinates(Vec2i start, Vec2i end)
        {
            if (System.Math.Abs(end.Y - start.Y) < System.Math.Abs(end.X - start.X))
            {
                return start.X > end.X ? PlotLineLow(end, start) : PlotLineLow(start, end);
            }

            return start.Y > end.Y ? PlotLineHigh(end, start) : PlotLineHigh(start, end);
        }
    }
}