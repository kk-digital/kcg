using UnityEngine;


namespace KMath
{

    public static class KMath
    {
        public static float LengthSquared(Vector2 vector)
        {
            return (vector.x * vector.x) + (vector.y * vector.y);
        }

        public static float MakePositive(float num)
        {
            if (num < 0)
            {
                num *= -1f;
            }

            return num;
        }
    }
}