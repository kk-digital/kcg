using System;
using Entitas;
using KMath;

namespace Physics
{
    public static class MovableExtenstion
    {
        public static void CheckPrecision(this ref float num)
        {
            if (Math.Abs(num - 0f) <= 0.01f)
            {
                num = 0f;
            }
        }
    }

    [Agent, Item]
    public class MovableComponent : IComponent
    {
        public float Speed;
        public Vec2f Velocity;
        public Vec2f Acceleration;
    }
}

