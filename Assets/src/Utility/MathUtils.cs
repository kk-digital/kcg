using UnityEngine;


namespace Utility
{

    public static class MathUtils
    {

        public static float LengthSquared(Vector2 vector)
        {
            return (vector.x * vector.x) + (vector.y * vector.y);
        }
    }
}