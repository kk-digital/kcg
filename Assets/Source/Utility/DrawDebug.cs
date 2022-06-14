using KMath;
using UnityEngine;

namespace Utility
{
    public static class DrawDebug
    {
        public static void DrawBox(this Box box)
        {
            var bottomLeft = new Vector3(box.BottomLeft.X, box.BottomLeft.Y, 0f);
            var bottomRight = new Vector3(box.BottomRight.X, box.BottomRight.Y, 0f);
            var topLeft = new Vector3(box.TopLeft.X, box.TopLeft.Y, 0f);
            var topRight = new Vector3(box.TopRight.X, box.TopRight.Y, 0f);
            
            Debug.DrawLine(bottomLeft, bottomRight, Color.red);
            Debug.DrawLine(bottomRight, topRight, Color.red);
            Debug.DrawLine(topRight, topLeft, Color.red);
            Debug.DrawLine(topLeft, bottomLeft, Color.red);
        }
    }
}

