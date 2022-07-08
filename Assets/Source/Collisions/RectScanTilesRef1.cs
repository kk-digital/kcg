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
            var previous_box_xmax = (int) Math.Ceiling(center.X + halfSize.X); //round up
            var current_box_xmax = (int) Math.Ceiling(center.X + halfSize.X + velocity.X); //round up

            var previous_box_xmin = (int) Math.Floor(center.X - halfSize.X); //round down
            var current_box_xmin = (int) Math.Floor(center.X - halfSize.X + velocity.X); //round down

            var previous_box_ymax = (int) Math.Ceiling(center.Y + halfSize.Y); //round up
            var current_box_ymax = (int) Math.Ceiling(center.Y + halfSize.Y + velocity.Y); //round up

            var previous_box_ymin = (int) Math.Floor(center.Y - halfSize.Y); //round down
            var current_box_ymin = (int) Math.Floor(center.Y - halfSize.Y + velocity.Y); //round down

            //verify, that no tile is checked twice
            //do a count to verify
            //NOTE: Do asserts, that min<=max and swap if not

            int xmin_1 = velocity.X >= 0f ? previous_box_xmin : current_box_xmin;
            int xmax_1 = velocity.X >= 0f ? current_box_xmax  : previous_box_xmax;
            int ymin_1 = velocity.Y >= 0f ? previous_box_ymax : current_box_ymin;
            int ymax_1 = velocity.Y >= 0f ? current_box_ymax  : previous_box_ymin;
            
            int xmin_2 = velocity.X >= 0f ? previous_box_xmax : current_box_xmin;
            int xmax_2 = velocity.X >= 0f ? current_box_xmax  : previous_box_xmin;
            int ymin_2 = velocity.Y >= 0f ? previous_box_ymin : current_box_ymin;
            int ymax_2 = velocity.Y >= 0f ? current_box_ymax : previous_box_ymax;

            int outputCount = (xmin_1 - xmax_1) * (ymin_1 - ymax_1);
            outputCount += (xmin_2 - xmax_2) * (ymin_2 - ymax_2);

            var output = new Vec2i[outputCount];
            var outputIndex = 0;

            for (int x = xmin_1; x <= xmax_1; x++)
            {
                for (int y = ymin_1; y <= ymax_1; y++)
                {
                    output[outputIndex] = new Vec2i(x, y);
                    outputIndex++;
                }
            }
            

            
            for (int y = xmin_2; y <= xmax_2; y++)
            {
                for (int x = ymin_2; x <= ymax_2; x++)
                {
                    output[outputIndex] = new Vec2i(x, y);
                    outputIndex++;
                }
            }

            return output;
        }
    }
}
