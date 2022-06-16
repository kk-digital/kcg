using KMath;
using UnityEngine;

namespace Utility
{
    public static class DrawDebug
    {
        public static void DrawBox(this AABB aabb)
        {
            var bottomLeft = new Vector3(aabb.LeftBottom.X, aabb.LeftBottom.Y, 0f);
            var bottomRight = new Vector3(aabb.RightBottom.X, aabb.RightBottom.Y, 0f);
            var topLeft = new Vector3(aabb.LeftTop.X, aabb.LeftTop.Y, 0f);
            var topRight = new Vector3(aabb.RightTop.X, aabb.RightTop.Y, 0f);
            
            Debug.DrawLine(bottomLeft, bottomRight, Color.red);
            Debug.DrawLine(bottomRight, topRight, Color.red);
            Debug.DrawLine(topRight, topLeft, Color.red);
            Debug.DrawLine(topLeft, bottomLeft, Color.red);
        }

        public static void DrawPoint(this Vec2f point)
        {
            Debug.DrawLine(new Vector3(point.X, point.Y, 0.0f),
                new Vector3(point.X + 0.1f, point.Y, 0.0f), Color.red);
            Debug.DrawLine(new Vector3(point.X + 0.1f, point.Y, 0.0f),
                new Vector3(point.X + 0.1f, point.Y + 0.1f, 0.0f), Color.red);
        }
    }
}

