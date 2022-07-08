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
            var previousRightX = (int) Math.Ceiling(center.X + halfSize.X); //round up
            var currentRightX = (int) Math.Ceiling(center.X + halfSize.X + velocity.X); //round up

            var previousLeftX = (int) Math.Floor(center.X - halfSize.X); //round down
            var currentLeftX = (int) Math.Floor(center.X - halfSize.X + velocity.X); //round down

            var previousTopY = (int) Math.Ceiling(center.Y + halfSize.Y); //round up
            var currentTopY = (int) Math.Ceiling(center.Y + halfSize.Y + velocity.Y); //round up

            var previousBottomY = (int) Math.Floor(center.Y - halfSize.Y); //round down
            var currentBottomY = (int) Math.Floor(center.Y - halfSize.Y + velocity.Y); //round down

            //verify, that no tile is checked twice
            //do a count to verify
            //NOTE: Do asserts, that min<=max and swap if not

            int xmin_1 = velocity.X >= 0f ? previousRightX : currentLeftX;
            int xmax_1 = velocity.X >= 0f ? currentRightX : previousLeftX;
            int ymin_1 = velocity.Y >= 0f ? previousTopY : currentBottomY;
            int ymax_1 = velocity.Y >= 0f ? currentTopY : previousBottomY;
            
            int xmin_2 = Math.Min(currentLeftX, previousLeftX);
            int xmax_2 = Math.Max(previousRightX, currentRightX);
            int ymin_2 = Math.Min(previousBottomY, currentBottomY);
            int ymax_2 = Math.Max(previousTopY, currentTopY);

            int outputCount = (currentTopY - currentBottomY) * (xmax_1 - xmin_1);
            outputCount += (ymax_1 - ymin_1) * (currentRightX - currentLeftX);
            outputCount += (currentTopY - currentBottomY) * (xmax_2 - xmin_2);
            outputCount += (ymax_2 - ymin_1) * (currentRightX - currentLeftX);

            var output = new Vec2i[outputCount];
            var outputIndex = 0;

            for (int y = currentBottomY; y <= currentTopY; y++)
            {
                for (int x = xmin_1; x <= xmax_1; x++)
                {
                    output[outputIndex] = new Vec2i(x, y);
                    outputIndex++;
                }
            }
            
            for (int y = ymin_1; y <= ymax_1; y++)
            {
                for (int x = currentLeftX; x <= currentRightX; x++)
                {
                    output[outputIndex] = new Vec2i(x, y);
                    outputIndex++;
                }
            }
            
            for (int y = currentBottomY; y <= currentTopY; y++)
            {
                for (int x = xmin_2; x <= xmax_2; x++)
                {
                    output[outputIndex] = new Vec2i(x, y);
                    outputIndex++;
                }
            }
            
            for (int y = ymin_2; y <= ymax_2; y++)
            {
                for (int x = currentLeftX; x <= currentRightX; x++)
                {
                    output[outputIndex] = new Vec2i(x, y);
                    outputIndex++;
                }
            }

            return output;
        }
    }
}
