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

        #region Methods

        /// <summary>
        /// Make X and Y positive
        /// </summary>
        [MethodImpl((MethodImplOptions) 256)]
        public void Abs()
        {
            X = Math.Abs(X);
            Y = Math.Abs(Y);
        }

        #endregion

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
        public static explicit operator Vec2f(Vec2i obj)
        {
            var output = new Vec2f(obj.X, obj.Y);
            return output;
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator *(Vec2i a, int d) => new(a.X * d, a.Y * d);
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator *(int d, Vec2i a) => new(a.X * d, a.Y * d);
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator <<(Vec2i a, int d) => new(a.X << d, a.Y << d);
        
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator /(Vec2i a, int d) => new(a.X / d, a.Y / d);
        
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator -(Vec2i a, Vec2i b) => new(a.X - b.X, a.Y - b.Y);
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator -(Vec2i a, int b) => new(a.X - b, a.Y - b);
        [MethodImpl((MethodImplOptions) 256)]
        public static Vec2i operator +(Vec2i a, Vec2i b) => new(a.X + b.X, a.Y + b.Y);
        [MethodImpl((MethodImplOptions)256)]
        public static bool operator ==(Vec2i lhs, Vec2i rhs)
        {
            int num1 = lhs.X - rhs.X;
            int num2 = lhs.Y - rhs.Y;
            return num1 * (double)num1 + num2 * (double)num2 < 9.99999943962493E-11;
        }
        [MethodImpl((MethodImplOptions)256)]
        public static bool operator !=(Vec2i lhs, Vec2i rhs) => !(lhs == rhs);

        #endregion

    }
}

