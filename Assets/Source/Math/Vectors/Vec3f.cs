using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KMath
{
    // Vector 3D Floating-Point
    public struct Vec3f
    {
        public float X;
        public float Y;
        public float Z;

        public Vec3f(float x, float y)
        {
            X = x;
            Y = y;
            Z = 0.0f;
        }
        
        public Vec3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public float Magnitude
        {
            [MethodImpl((MethodImplOptions) 256)] get => (float) Math.Sqrt((double) this.X * (double) this.X + (double) this.Y * (double) this.Y + (double) this.Z * (double) this.Z);
        }
        
        [MethodImpl((MethodImplOptions) 256)]
        public static explicit operator Vec2f(Vec3f obj)
        {
            Vec2f output = new Vec2f(obj.X, obj.Y);
            return output;
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vec3f operator+(Vec3f a, Vec3f b) => new Vec3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec3f operator-(Vec3f a, Vec3f b) => new Vec3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec3f operator*(Vec3f a, Vec3f b) => new Vec3f(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec3f operator/(Vec3f a, Vec3f b) => new Vec3f(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec3f operator/(Vec3f a, float b) => new Vec3f(a.X / b, a.Y / b, a.Z / b);
        
    }
}

