using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMath
{
    // Vector 3D Floating-Point
    public struct Vec3f
    {
        public float x;
        public float y;
        public float z;

        public Vec3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vec3f operator+(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vec3f operator-(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vec3f operator*(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.x * b.x, a.y * b.y, a.z * b.z);
        }
    }
}

