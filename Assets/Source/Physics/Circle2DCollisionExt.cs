using KMath;
using UnityEngine;
using Utility;

namespace Physics
{
    public static class Circle2DCollisionExt
    {
        public static CircleIntersectionPoint GetTileIntersectionPoint(this Circle circle, Planet.TileMap tileMap, Vec2f newPos)
        {
            var pointOnEdge = circle.GetPointOnEdge(newPos);
            var quarterPositions = circle.GetQuarterPositions(pointOnEdge);

            Debug.DrawLine(new Vector3(pointOnEdge.X, pointOnEdge.Y, 0.0f),
                new Vector3(pointOnEdge.X + 0.1f, pointOnEdge.Y, 0.0f), Color.red);
            Debug.DrawLine(new Vector3(pointOnEdge.X + 0.1f, pointOnEdge.Y, 0.0f),
                new Vector3(pointOnEdge.X + 0.1f, pointOnEdge.Y + 0.1f, 0.0f), Color.red);

            var tiles = tileMap.GetTiles(quarterPositions, Enums.Tile.MapLayerType.Front);

            foreach (var tile in tiles)
            {
                var intersectionPoint = tile.Borders.GetIntersectionPointAt(circle);
                if (intersectionPoint.IsCollided)
                {
                    return intersectionPoint;
                }
            }

            return new CircleIntersectionPoint
            {
                IsCollided = false
            };
        }
    }
}