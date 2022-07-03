using Enums.Tile;
using KMath;
using Utility;

namespace Physics
{
    public static class Collisions
    { 
        public static bool IsCollidingLeft(this AABB2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.X >= 0.0f) return false;
            
            int x = borders.LeftX < 0 ? (int) borders.LeftX - 1 : (int)borders.LeftX;
            
            if (x >= 0 && x < tileMap.MapSize.X)
            {
                for (int y = (int)borders.BottomY; y <= (int)borders.TopY; y++)
                {
                    if (y >= 0 && y < tileMap.MapSize.Y)
                    {
                        ref var tile = ref tileMap.GetTileRef(x, y, MapLayerType.Front);
                        if (tile.ID != TileID.Air)
                        {
                            var tileBorders = new AABB2D(x, y);
                            tileBorders.DrawBox();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool IsCollidingRight(this AABB2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.X <= 0.0f) return false;
            
            int x = borders.RightX < 0 ? (int) borders.RightX - 1 : (int)borders.RightX;
            
            if (x >= 0 && x < tileMap.MapSize.X)
            {
                for (int y = (int)borders.BottomY; y <= (int)borders.TopY; y++)
                {
                    if (y >= 0 && y < tileMap.MapSize.Y)
                    {
                        ref var tile = ref tileMap.GetTileRef(x, y, MapLayerType.Front);

                        if (tile.ID != TileID.Air)
                        {
                            var tileBorders = new AABB2D(x, y);
                            tileBorders.DrawBox();
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        
        public static bool IsCollidingBottom(this AABB2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.Y >= 0.0f) return false;
            
            // LeftBottom.X >= 0f ? (int)LeftBottom.X : (int)LeftBottom.X - 1;
            
            int y = (int)borders.BottomY;
            int leftX = borders.LeftX < 0 ? (int) borders.LeftX - 1 : (int)borders.LeftX;
            int rightX = borders.RightX < 0 ? (int) borders.RightX - 1 : (int)borders.RightX;
            
            if (y >= 0 && y < tileMap.MapSize.Y)
            {
                for (int x = leftX; x <= rightX; x++)
                {
                    if (x >= 0 && x < tileMap.MapSize.X)
                    {
                        ref var tile = ref tileMap.GetTileRef(x, y, MapLayerType.Front);
                        if (tile.ID != TileID.Air)
                        {
                            var tileBorders = new AABB2D(x, y);
                            tileBorders.DrawBox();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool IsCollidingTop(this AABB2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.Y <= 0.0f) return false;
            
            int y = (int)borders.TopY;
            int leftX = borders.LeftX < 0 ? (int) borders.LeftX - 1 : (int)borders.LeftX;
            int rightX = borders.RightX < 0 ? (int) borders.RightX - 1 : (int)borders.RightX;
            
            if (y >= 0 && y < tileMap.MapSize.Y)
            {
                for (int x = leftX; x <= rightX; x++)
                {
                    if (x >= 0 && x < tileMap.MapSize.X)
                    {
                        ref var tile = ref tileMap.GetTileRef(x, y, MapLayerType.Front);
                        if (tile.ID != TileID.Air)
                        {
                            var tileBorders = new AABB2D(x, y);
                            tileBorders.DrawBox();
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}