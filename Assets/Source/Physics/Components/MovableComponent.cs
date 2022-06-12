using Entitas;
using UnityEngine;

namespace Physics
{
    public static class MovableExtenstion
    {
        public static void CheckPrecision(this ref float num)
        {
            if (Mathf.Abs(num - 0f) <= 0.01f)
            {
                num = 0f;
            }
        }
    }
    public struct MovableComponent : IComponent
    {
        public float Speed;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float AccelerationTime;
    }
}

