using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KMath
{
    // Vector 2D Integer
    public struct Vec2i
    {
        private static readonly Vec2i zeroVector = new(0, 0);
        
        public int X;
        public int Y;
        
        public Vec2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static explicit operator Vec2f(Vec2i obj)
        {
            Vec2f output = new Vec2f(obj.X, obj.Y);
            return output;
        }
        
        #region Properties

        /// <summary>
        ///   <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public int sqrMagnitude
        {
            [MethodImpl((MethodImplOptions) 256)] get => X * X + Y * Y;
        }
        
        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public float magnitude
        {
            [MethodImpl((MethodImplOptions) 256)] get => Mathf.Sqrt(X * X + Y * Y);
        }

        /// <summary>
        ///   <para>Shorthand for writing Vec2f(0, 0).</para>
        /// </summary>
        public static Vec2i zero
        {
            [MethodImpl((MethodImplOptions) 256)] get => zeroVector;
        }

        #endregion

        #region Operators
        
        [MethodImpl((MethodImplOptions) 256)]
        public static explicit operator Vec2i(Vec2f obj)
        {
            Vec2i output = new Vec2i((int)obj.X, (int)obj.Y);
            return output;
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator *(Vec2i a, int d) => new(a.X * d, a.Y * d);
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator *(int d, Vec2i a) => new(a.X * d, a.Y * d);
        
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator /(Vec2i a, int d) => new(a.X / d, a.Y / d);
        
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator -(Vec2i a, Vec2i b) => new(a.X - b.X, a.Y - b.Y);
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator +(Vec2i a, Vec2i b) => new(a.X + b.X, a.Y + b.Y);

        #endregion

    }
}

