using UnityEngine;

namespace Physics
{
    public static class BoxCollision
    {

        public static bool IsCollidingBottom(PlanetTileMap.PlanetTileMap tileMap, Vector2 position,
                                                            Vector2 size)
        {
            Vector2 point1 = position;
            Vector2 point2 = position + new Vector2(size.x, 0.0f);

            Vector2Int begin = new Vector2Int((int)point1.x, (int)point1.y);
            Vector2Int end = new Vector2Int((int)point2.x, (int)point2.y);


            int y = begin.y;
            for(int x = begin.x; x <= end.x; x++)
            {
                if (x >= 0 && x < tileMap.Size.x && 
                   y >= 0 && y < tileMap.Size.y)
                   {
                        ref TileProperties.PlanetTile tile = ref tileMap.GetTileRef(x, y, PlanetTileMap.Layer.Front);
                        if (tile.PropertiesId >= 0)
                        {
                            return true;
                        }
                   }
            }
            return false;
        }
    }
}