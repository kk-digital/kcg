using UnityEngine;

namespace Physics
{
    public static class BoxCollision
    {

        public static bool IsCollidingBottom(PlanetTileMap.PlanetTileMap tileMap, Vector2 position,
                                                            Vector2 size)
        {
            Vector2 point1 = position;
            Vector2 point2 = position + new Vector2(0.0f, size.x);

            Vector2Int begin = new Vector2Int((int)point1.x, (int)point1.y);
            Vector2Int end = new Vector2Int((int)point2.x, (int)point2.y);


            return false;
        }
    }
}