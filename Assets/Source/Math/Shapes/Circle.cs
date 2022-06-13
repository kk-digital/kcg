using System;
using UnityEngine;

namespace KMath
{
    public static class CircleExt
    {
        public static Vec2f PointOnEdge(this Circle circle, Vec2f newPos)
        {
            var difference = newPos - circle.BottomLeft;
            difference.Normalize();

            return circle.BottomLeft + difference * circle.Radius;
        }
    }
    
    public struct Circle
    {
        public Vec2f Center;

        public Vec2f BottomLeft;

        public float Radius;

        public static Circle Create(Vec2f position, float radius)
        {
            var center = new Vec2f((position.X + position.X + radius * 2) / 2f, (position.Y + position.Y + radius * 2) / 2f);

            return new Circle
            {
                Center = center,
                BottomLeft = position,
                Radius = radius
            };
        }
    }
}

