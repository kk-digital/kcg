using Enums;
using KMath;
using Utility;

namespace Physics
{
    public static class Circle2DCollisionExt
    {
        public static CircleQuarter GetTileCollisionQuarters(this Circle circle, Circle newCircle, Planet.TileMap tileMap)
        {
            var pointOnEdge = circle.GetPointOnEdge(newCircle.BottomLeft);
            var quarterPositions = newCircle.GetQuarterPositions(pointOnEdge);

            var tiles = tileMap.GetTiles(quarterPositions, Enums.Tile.MapLayerType.Front);

            if (tiles == null)
            {
                return CircleQuarter.Error;
            }

            var quarters = CircleQuarter.Error;

            int test = 0;

            foreach (var tile in tiles)
            {
                if (Flag.Set(ref quarters, tile.Borders.IntersectsAt(newCircle)))
                {
                    test++;
                }
            }

            return quarters;
        }
    }
}