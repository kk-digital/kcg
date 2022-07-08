using KMath;

// Line Width = 1
// Functions for getting int Line Coordinates
// Used for example for a bullet checking

namespace Collisions
{
    public static class PlotLine
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

        public static Vec2i[] GetLineCoordinates(Vec2i start, Vec2i end)
        {
            if (System.Math.Abs(end.Y - start.Y) < System.Math.Abs(end.X - start.X))
            {
                return start.X > end.X ? PlotLineLow(end, start) : PlotLineLow(start, end);
            }

            return start.Y > end.Y ? PlotLineHigh(end, start) : PlotLineHigh(start, end);
        }
    }
}