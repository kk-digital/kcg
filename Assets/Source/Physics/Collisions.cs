using Enums.Tile;
using KMath;

namespace Physics
{
    public class Collisions
    { 
        public static bool IsCollidingLeft(this AABB2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.X >= 0.0f) return false;
            
            int x = borders.IntLeft;
            for (int y = borders.IntBottom; y <= borders.IntTop; y++)
            {
                ref var tile = ref tileMap.GetTileRef(x, y, MapLayerType.Front);
                if (tile.Property >= 0)
                {
                    tile.Borders.DrawBox();
                    return true;
                }
            }

            return false;
        }

        public static bool IsCollidingRight(this AABB2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.X <= 0.0f) return false;
            
            int x = borders.IntRight;
            for (int y = borders.IntBottom; y <= borders.IntTop; y++)
            {
                ref var tile = ref tileMap.GetTileRef(x, y, MapLayerType.Front);

                if (tile.TileID is not TileID.Air)
                {
                    var tileBorders = tile.CalculateBorders(x, y);
                    
                    tileBorders.DrawBox();
                    return true;
                }
            }

            return false;
        }
        
        public static bool IsCollidingBottom(this AABB2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.Y >= 0.0f) return false;
            
            int y = borders.IntBottom;
            for (int x = borders.IntLeft; x <= borders.IntRight; x++)
            {
                ref var tile = ref tileMap.GetTileRef(x, y, MapLayerType.Front);
                if (tile.Property >= 0)
                {
                    tile.Borders.DrawBox();
                    return true;
                }
            }

            return false;
        }

        public static bool IsCollidingTop(this AABB2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.Y <= 0.0f) return false;
            
            int y = borders.IntTop;
            for (int x = borders.IntLeft; x <= borders.IntRight; x++)
            {
                ref var tile = ref tileMap.GetTileRef(x, y, MapLayerType.Front);
                if (tile.Property >= 0)
                {
                    tile.Borders.DrawBox();
                    return true;
                }
            }

            return false;
        }
    }
}