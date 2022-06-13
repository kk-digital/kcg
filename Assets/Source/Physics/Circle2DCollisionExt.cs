using KMath;

namespace Physics
{
    public static class Circle2DCollisionExt
    {
        public static bool IsColliding(this Circle circle, Planet.TileMap tileMap, Vec2f newPos)
        {
            var pointOnEdge = circle.PointOnEdge(newPos);
            var tilePos = (Vec2i) pointOnEdge;
            var tilePosF = (Vec2f) tilePos;
            

            if (tileMap.Borders.Intersects(tilePosF))
            {
                ref var tile = ref tileMap.GetTileRef(tilePos.X, tilePos.Y, Enums.Tile.MapLayerType.Front);
                
                if (tile.Type >= 0)
                {
                    return tile.Borders.Intersects(tilePosF);
                }
            }

            return false;
        }
    }
}