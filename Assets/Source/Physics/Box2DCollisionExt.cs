using UnityEngine;

namespace Physics
{
    public static class Box2DCollisionExt
    {
        public static bool IsCollidingLeft(this ref Box2DBorders borders, Planet.TileMap tileMap, Vector2 velocity)
        {
            if (velocity.x >= 0.0f) return false;
            
            int x = borders.Left;
            for(int y = borders.Down; y <= borders.Up; y++)
            {
                var edgePosition = new Vector2(x, y);
                if (tileMap.BoxBorders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        return tile.BoxBorders.Intersects(edgePosition);
                    }
                }
            }
            return false;
        }

        public static bool IsCollidingRight(this ref Box2DBorders borders, Planet.TileMap tileMap, Vector2 velocity)
        {
            if (velocity.x <= 0.0f) return false;
            
            int x = borders.Right;
            for(int y = borders.Down; y <= borders.Up; y++)
            {
                var edgePosition = new Vector2(x, y);
                if (tileMap.BoxBorders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        return tile.BoxBorders.Intersects(edgePosition);
                    }
                }
            }
            return false;
        }
        
        public static bool IsCollidingBottom(this ref Box2DBorders borders, Planet.TileMap tileMap, Vector2 velocity)
        {
            if (velocity.y >= 0.0f) return false;
            
            int y = borders.Down;
            for(int x = borders.Left; x <= borders.Right; x++)
            {
                var edgePosition = new Vector2(x, y);
                if (tileMap.BoxBorders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        return tile.BoxBorders.Intersects(edgePosition);
                    }
                }
            }
            
            return false;
        }

        public static bool IsCollidingTop(this ref Box2DBorders borders, Planet.TileMap tileMap, Vector2 velocity)
        {
            if (velocity.y <= 0.0f) return false;
            
            int y = borders.Up;
            for(int x = borders.Left; x <= borders.Right; x++)
            {
                var edgePosition = new Vector2(x, y);
                
                if (tileMap.BoxBorders.Intersects(edgePosition))
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        return tile.BoxBorders.Intersects(edgePosition);
                    }
                }
            }
            return false;
        }
    }
}