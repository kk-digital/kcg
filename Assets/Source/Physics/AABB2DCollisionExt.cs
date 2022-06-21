using KMath;
using Utility;

namespace Physics
{
    public static class AABB2DCollisionExt
    {
        public static bool IsCollidingLeft(this AABB2D borders, Planet.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.X >= 0.0f) return false;
            
            int x = borders.IntLeft;
            for(int y = borders.IntBottom; y <= borders.IntTop; y++)
            {
                var edgePosition = new Vec2f(x, y);
                if (tileMap.Borders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        //var isColliding = tile.Borders.Intersects(edgePosition);
                        //if (isColliding)
                        //{
                            tile.Borders.DrawBox();
                            return true;
                        //}
                    }
                   }
             //   }
            }
            return false;
        }

        public static bool IsCollidingRight(this AABB2D borders, Planet.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.X <= 0.0f) return false;
            
            int x = borders.IntRight;
            for(int y = borders.IntBottom; y <= borders.IntTop; y++)
            {
                var edgePosition = new Vec2f(x, y);
                if (tileMap.Borders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        //var isColliding = tile.Borders.Intersects(edgePosition);
                        //if (isColliding)
                        //{
                            tile.Borders.DrawBox();
                            return true;
                        //}
                    }
                   }
               // }
            }
            return false;
        }
        
        public static bool IsCollidingBottom(this AABB2D borders, Planet.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.Y >= 0.0f) return false;
            
            int y = borders.IntBottom;
            for(int x = borders.IntLeft; x <= borders.IntRight; x++)
            {
                var edgePosition = new Vec2f(x, y);
                if (tileMap.Borders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        //var isColliding = tile.Borders.Intersects(edgePosition);
                        //if (isColliding)
                        //{
                            tile.Borders.DrawBox();
                            return true;
                        //}
                    }
                   }
               // }
            }
            
            return false;
        }

        public static bool IsCollidingTop(this AABB2D borders, Planet.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.Y <= 0.0f) return false;
            
            int y = borders.IntTop;
            for(int x = borders.IntLeft; x <= borders.IntRight; x++)
            {
                var edgePosition = new Vec2f(x, y);
                
                if (tileMap.Borders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        //var isColliding = tile.Borders.Intersects(edgePosition);
                        //if (isColliding)
                        //{
                            tile.Borders.DrawBox();
                            return true;
                        //}
                    }
                   }
                //}
            }
            return false;
        }
    }
}
