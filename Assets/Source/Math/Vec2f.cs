using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMath
{
    // Vector 2D Floating-Point
    public struct Vec2f
    {
        public float x;
        public float y;

        public Vec2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vec2f operator+(Vec2f a, Vec2f b)
        {
            return new Vec2f(a.x + b.x, a.y + b.y);
        }

        public static Vec2f operator-(Vec2f a, Vec2f b)
        {
            return new Vec2f(a.x - b.x, a.y - b.y);
        }

        public static Vec2f operator*(Vec2f a, Vec2f b)
        {
            return new Vec2f(a.x * b.x, a.y * b.y);
        }
    }
}

