using UnityEngine;


namespace Utility
{

    public static class Math
    {

        public static float LengthSquared(Vector2 vector)
        {
            return (vector.x * vector.x) + (vector.y * vector.y);
        }
    }
}