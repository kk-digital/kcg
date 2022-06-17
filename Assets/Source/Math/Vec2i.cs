using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMath
{
    // Vector 2D Integer
    public struct Vec2i
    {
        public int x;
        public int y;

        public Vec2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vec2i operator+(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.x + b.x, a.y + b.y);
        }

        public static Vec2i operator-(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.x - b.x, a.y - b.y);
        }

        public static Vec2i operator*(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.x * b.x, a.y * b.y);
        }
    }
}

