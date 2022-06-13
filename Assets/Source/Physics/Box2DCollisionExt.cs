using KMath;

namespace Physics
{
    public static class Box2DCollisionExt
    {
        public static bool IsCollidingLeft(this Box borders, Planet.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.X >= 0.0f) return false;
            
            int x = borders.Left;
            for(int y = borders.Down; y <= borders.Up; y++)
            {
                var edgePosition = new Vec2f(x, y);
                if (tileMap.Borders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        return tile.Borders.Intersects(edgePosition);
                    }
                   }
             //   }
            }
            return false;
        }

        public static bool IsCollidingRight(this Box borders, Planet.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.X <= 0.0f) return false;
            
            int x = borders.Right;
            for(int y = borders.Down; y <= borders.Up; y++)
            {
                var edgePosition = new Vec2f(x, y);
                if (tileMap.Borders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        return tile.Borders.Intersects(edgePosition);
                    }
                   }
               // }
            }
            return false;
        }
        
        public static bool IsCollidingBottom(this Box borders, Planet.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.Y >= 0.0f) return false;
            
            int y = borders.Down;
            for(int x = borders.Left; x <= borders.Right; x++)
            {
                var edgePosition = new Vec2f(x, y);
                if (tileMap.Borders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        return tile.Borders.Intersects(edgePosition);
                    }
                   }
               // }
            }
            
            return false;
        }

        public static bool IsCollidingTop(this Box borders, Planet.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.Y <= 0.0f) return false;
            
            int y = borders.Up;
            for(int x = borders.Left; x <= borders.Right; x++)
            {
                var edgePosition = new Vec2f(x, y);
                
                if (tileMap.Borders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        return tile.Borders.Intersects(edgePosition);
                    }
                   }
                //}
            }
            return false;
        }
    }
}
