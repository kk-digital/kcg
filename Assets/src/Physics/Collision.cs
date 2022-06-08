using Enums;
using UnityEngine;

namespace Physics
{
    public static class BoxCollision
    {

        public static bool IsCollidingBottom(Planet.TileMap.Model tileMap, Vector2 position,
                                                            Vector2 size)
        {
            Vector2Int bottomLeft = new Vector2Int((int)position.x, (int)position.y);
            Vector2Int bottomRight = new Vector2Int((int)(position.x + size.x), (int)position.y);


            int y = bottomLeft.y;
            for(int x = bottomLeft.x; x <= bottomRight.x; x++)
            {
                if (x >= 0 && x < tileMap.MapSize.x && 
                   y >= 0 && y < tileMap.MapSize.y)
                {
                    ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                    if (tile.Type >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public static bool IsCollidingTop(Planet.TileMap.Model tileMap, Vector2 position,
                                                            Vector2 size)
        {
            var topLeft = new Vector2Int((int)position.x, (int)(position.y + size.y));
            var topRight = new Vector2Int((int)(position.x + size.x), (int)(position.y + size.y));


            int y = topLeft.y;
            for(int x = topLeft.x; x <= topRight.x; x++)
            {
                if (x >= 0 && x < tileMap.MapSize.x && 
                   y >= 0 && y < tileMap.MapSize.y)
                   {
                        ref var tile = ref tileMap.GetTileRef(x, y, Enums.Tile.MapLayerType.Front);
                        if (tile.Type >= 0)
                        {
                            return true;
                        }
                   }
            }
            return false;
        }
    }
}