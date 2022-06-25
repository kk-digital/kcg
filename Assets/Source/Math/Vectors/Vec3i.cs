using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KMath
{
    // Vector 3D Integer
    public struct Vec3i
    {
        public int x;
        public int y;
        public int z;

        public Vec3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vec3i operator+(Vec3i a, Vec3i b)
        {
            return new Vec3i(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vec3i operator-(Vec3i a, Vec3i b)
        {
            return new Vec3i(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vec3i operator*(Vec3i a, Vec3i b)
        {
            return new Vec3i(a.x * b.x, a.y * b.y, a.z * b.z);
        }
    }
}

