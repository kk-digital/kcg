using Enums;
using UnityEngine;

namespace Physics
{
    public static class BoxCollision
    {

        public static bool IsCollidingBottom(PlanetTileMap.PlanetTileMap tileMap, Vector2 position,
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
                        ref TileProperties.PlanetTile tile = ref tileMap.GetTileRef(x, y, PlanetTileMap.Layer.Front);
                        if (tile.TileType >= 0)
                        {
                            return true;
                        }
                   }
            }
            return false;
        }


        public static bool IsCollidingTop(PlanetTileMap.PlanetTileMap tileMap, Vector2 position,
                                                            Vector2 size)
        {
            Vector2Int TopLeft = new Vector2Int((int)position.x, (int)(position.y + size.y));
            Vector2Int TopRight = new Vector2Int((int)(position.x + size.x), (int)(position.y + size.y));


            int y = TopLeft.y;
            for(int x = TopLeft.x; x <= TopRight.x; x++)
            {
                if (x >= 0 && x < tileMap.Size.x && 
                   y >= 0 && y < tileMap.Size.y)
                   {
                        ref TileProperties.PlanetTile tile = ref tileMap.GetTileRef(x, y, PlanetTileMap.Layer.Front);
                        if (tile.TileType >= 0)
                        {
                            return true;
                        }
                   }
            }
            return false;
        }
    }
}