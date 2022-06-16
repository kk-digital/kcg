using System.Runtime.CompilerServices;
using Enums;
using System;
using Utility;

namespace KMath
{
    /// <summary>
    /// Axis-aligned Bounding Box 2D
    /// </summary>
    public struct AABB2D
    {
        public Vec2f Center;
        public Vec2f HalfSize;

        #region CornerGetters

        public Vec2f LeftBottom
        {
            [MethodImpl((MethodImplOptions) 256)] get => Center - HalfSize;
        }
        public Vec2f RightTop
        {
            [MethodImpl((MethodImplOptions) 256)] get => Center + HalfSize; 
        }
        public Vec2f RightBottom
        {
            [MethodImpl((MethodImplOptions) 256)] get => Center + new Vec2f(HalfSize.X, -HalfSize.Y);
        }
        public Vec2f LeftTop
        {
            [MethodImpl((MethodImplOptions) 256)]get => Center + new Vec2f(-HalfSize.X, HalfSize.Y);
        }

        public int Left
        {
            [MethodImpl((MethodImplOptions) 256)] get => (int) LeftBottom.X;
        }
        public int Right
        {
            [MethodImpl((MethodImplOptions) 256)] get => (int) RightBottom.X;
        }
        public int Top
        {
            [MethodImpl((MethodImplOptions) 256)] get => (int) LeftTop.Y;
        }
        public int Bottom
        {
            [MethodImpl((MethodImplOptions) 256)] get => (int) LeftBottom.Y;
        }

        #endregion
        
        public AABB2D(Vec2f position, Vec2f size)
        {
            HalfSize = size / 2f;
            Center = position + HalfSize;
        }

        #region Intersection

        public bool Intersects(Vec2f position)
        {
            return position.X >= Left && position.X <= Right &&
                   position.Y >= Bottom && position.Y <= Top;
        }
        
        public bool Intersects(AABB2D other)
        {
            if (Math.Abs(Center.X - other.Center.X) > HalfSize.X + other.HalfSize.X) return false;
            if (Math.Abs(Center.Y - other.Center.Y) > HalfSize.Y + other.HalfSize.Y) return false;
            return true;
        }

        public bool Intersects(Sphere2D sphere2D)
        {
            var nearestX = Math.Max(LeftBottom.X, Math.Min(sphere2D.Center.X, RightBottom.X));
            var nearestY = Math.Max(LeftBottom.Y, Math.Min(sphere2D.Center.Y, RightTop.Y));

            var deltaX = sphere2D.Center.X - nearestX;
            var deltaY = sphere2D.Center.Y - nearestY;

            return deltaX * deltaX + deltaY * deltaY <= sphere2D.Radius * sphere2D.Radius;
        }

        #endregion
    }
}

