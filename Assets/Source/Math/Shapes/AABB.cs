using System.Runtime.CompilerServices;
using Enums;
using System;
using Utility;

namespace KMath
{
    public static class AABBExt
    {
        public static bool Intersects(this AABB aabb, Vec2f position)
        {
            return position.X >= aabb.Left && position.X <= aabb.Right &&
                   position.Y >= aabb.Bottom && position.Y <= aabb.Top;
        }

        public static CircleQuarter IntersectsAt(this AABB aabb, Circle circle)
        {
            var nearestX = Math.Max(aabb.LeftBottom.X, Math.Min(circle.Center.X, aabb.RightBottom.X));
            var nearestY = Math.Max(aabb.LeftBottom.Y, Math.Min(circle.Center.Y, aabb.RightTop.Y));

            var deltaX = circle.Center.X - nearestX;
            var deltaY = circle.Center.Y - nearestY;

            var difference = new Vec2f(-deltaX, -deltaY);
            var quarterType = Circle.GetQuarterType(difference);

            var intersects = deltaX * deltaX + deltaY * deltaY <= circle.Radius * circle.Radius;

            if (intersects)
            {
                aabb.DrawBox();
            }

            return intersects ? quarterType : CircleQuarter.Error;
        }
    }
    
    /// <summary>
    /// Axis-aligned Bounding Box
    /// </summary>
    public struct AABB
    {
        public Vec2f Center;
        public Vec2f HalfSize;

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

        public AABB(Vec2f position, Vec2f size)
        {
            HalfSize = size / 2f;
            Center = position + HalfSize;
        }
    }
}

