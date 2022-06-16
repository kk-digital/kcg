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

        public int IntLeft
        {
            [MethodImpl((MethodImplOptions) 256)] get => (int) LeftBottom.X;
        }
        public int IntRight
        {
            [MethodImpl((MethodImplOptions) 256)] get => (int) RightBottom.X;
        }
        public int IntTop
        {
            [MethodImpl((MethodImplOptions) 256)] get => (int) LeftTop.Y;
        }
        public int IntBottom
        {
            [MethodImpl((MethodImplOptions) 256)] get => (int) LeftBottom.Y;
        }
        
        public float Left
        {
            [MethodImpl((MethodImplOptions) 256)] get => LeftBottom.X;
        }
        public float Right
        {
            [MethodImpl((MethodImplOptions) 256)] get => RightBottom.X;
        }
        public float Top
        {
            [MethodImpl((MethodImplOptions) 256)] get => LeftTop.Y;
        }
        public float Bottom
        {
            [MethodImpl((MethodImplOptions) 256)] get => LeftBottom.Y;
        }

        #endregion
        
        public AABB2D(Vec2f position, Vec2f size)
        {
            HalfSize = size / 2f;
            Center = position + HalfSize;
        }

        public Vec2f GetClosestPoint(Vec2f point)
        {
            var closestX = Math.Min(Math.Max(point.X, Left), Right);
            var closestY = Math.Min(Math.Max(point.Y, Bottom), Top);

            return new Vec2f(closestX, closestY);
        }

        public float SqrDistanceBetween(Vec2f point)
        {
            float sqDist = 0.0f;
            
            if (point.X < Left) sqDist += (Left - point.X) * (Left - point.X);
            if (point.X > Right) sqDist += (point.X - Right) * (point.X - Right);
            
            if (point.Y < Bottom) sqDist += (Bottom - point.Y) * (Bottom - point.Y);
            if (point.Y > Top) sqDist += (point.Y - Top) * (point.Y - Top);
            
            return sqDist;
        }

        #region Intersection

        public bool Intersects(Vec2f position)
        {
            return position.X >= IntLeft && position.X <= IntRight &&
                   position.Y >= IntBottom && position.Y <= IntTop;
        }
        
        public bool Intersects(AABB2D other)
        {
            if (Math.Abs(Center.X - other.Center.X) > HalfSize.X + other.HalfSize.X) return false;
            if (Math.Abs(Center.Y - other.Center.Y) > HalfSize.Y + other.HalfSize.Y) return false;
            return true;
        }

        public bool Intersects(Sphere2D circle)
        {
            var closestPoint = GetClosestPoint(circle.Center);
            var delta = closestPoint - circle.Center;

            return Vec2f.Dot(delta, delta) <= circle.Radius * circle.Radius;
        }

        #endregion
    }
}

