using System;
using KMath;

// find xchunkmin, xchunkmax, ychunkmax, ychunkmin for each rectangle
// check if chunk is air/not loaded
// then no collision if not loaded

namespace Collisions
{
    public static class RectScanTilesRef1
    {
        public static Vec2i[] RectScan(Vec2f center, Vec2f halfSize, Vec2f velocity)
        {
            var box1_xmax = (int) Math.Ceiling(center.X + halfSize.X); //round up
            var box2_xmax = (int) Math.Ceiling(center.X + halfSize.X + velocity.X); //round up

            var box1_xmin = (int) Math.Floor(center.X - halfSize.X); //round down
            var box2_xmin = (int) Math.Floor(center.X - halfSize.X + velocity.X); //round down

            var box1_ymax = (int) Math.Ceiling(center.Y + halfSize.Y); //round up
            var box2_ymax = (int) Math.Ceiling(center.Y + halfSize.Y + velocity.Y); //round up

            var box1_ymin = (int) Math.Floor(center.Y - halfSize.Y); //round down
            var box2_ymin = (int) Math.Floor(center.Y - halfSize.Y + velocity.Y); //round down
            
            // r - region
            // r1 - region1 and etc.

            int r1_xmin = velocity.X >= 0f ? box1_xmin : box2_xmin;
            int r1_xmax = velocity.X >= 0f ? box2_xmax : box1_xmax;
            int r1_ymin = velocity.Y >= 0f ? box1_ymax : box2_ymin;
            int r1_ymax = velocity.Y >= 0f ? box2_ymax : box1_ymin;
            
            int r2_xmin = velocity.X >= 0f ? box1_xmax : box2_xmin;
            int r2_xmax = velocity.X >= 0f ? box2_xmax : box1_xmin;
            int r2_ymin = velocity.Y >= 0f ? box1_ymin : box2_ymin;
            int r2_ymax = velocity.Y >= 0f ? box2_ymax : box1_ymax;

            int outputCount = (r1_xmax - r1_xmin) * (r1_ymax - r1_ymin);
            outputCount    += (r2_xmax - r2_xmin) * (r2_ymax - r2_ymin);

            var output = new Vec2i[outputCount];
            var outputIndex = 0;

            for (int x = r1_xmin; x <= r1_xmax; x++)
            {
                for (int y = r1_ymin; y <= r1_ymax; y++)
                {
                    output[outputIndex] = new Vec2i(x, y);
                    outputIndex++;
                }
            }
            
            for (int x = r2_xmin; x <= r2_xmax; x++)
            {
                for (int y = r2_ymin; y <= r2_ymax; y++)
                {
                    output[outputIndex] = new Vec2i(x, y);
                    outputIndex++;
                }
            }

            return output;
        }
    }
}
