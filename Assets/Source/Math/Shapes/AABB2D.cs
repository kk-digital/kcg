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
            [MethodImpl((MethodImplOptions) 256)] get => LeftBottom.X >= 0f ? (int)LeftBottom.X : (int)LeftBottom.X - 1;
        }
        public int IntRight
        {
            [MethodImpl((MethodImplOptions) 256)] get => RightBottom.X >= 0f ? (int)RightBottom.X : (int)RightBottom.X - 1;
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
        
        public Vec2f GetCornerByBitMask(int index)
        {
            Vec2f p;
            p.X = Convert.ToBoolean(index & 1) ? Right : Left;
            p.Y = Convert.ToBoolean(index & 1) ? Top : Bottom;
            return p;
        }

        #region Intersection

        public bool Intersects(Vec2f position)
        {
            return position.X >= IntLeft && position.X < IntRight &&
                   position.Y >= IntBottom && position.Y < IntTop;
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

        /// <summary>
        /// Intersect ray R(t) = p + t*d against AABB.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="d">direction</param>
        /// <param name="intersectionPoint"></param>
        /// <returns>Intersection distance tmin and intersectionPoint</returns>
        public bool IntersectsRay(Vec2f p, Vec2f d, out float tMin, out Vec2f intersectionPoint)
        {
            tMin = 0.0f; // set to -FLT_MAX to get first hit on line
            var tMax = float.MaxValue; // set to max distance ray can travel (for segment)

            var direction = new[] {d.X, d.Y};
            var point = new[] {p.X, p.Y};
            var boxMin = new[] {LeftBottom.X, LeftBottom.Y};
            var boxMax = new[] {RightTop.X, RightTop.Y};
            
            // For all two slabs
            for (int axes = 0; axes < 2; axes++)
            {
                if (Math.Abs(direction[axes]) < float.Epsilon)
                {
                    // Ray is parallel to slab. No hit if origin not within slab
                    if (point[axes] < boxMin[axes] || point[axes] > boxMax[axes])
                    {
                        intersectionPoint = Vec2f.Zero;
                        return false;
                    }
                }
                else
                {
                    // Compute intersection t value of ray with near and far plane of slab
                    var ood = 1.0f / direction[axes];
                    var t1 = (boxMin[axes] - point[axes]) * ood;
                    var t2 = (boxMax[axes] - point[axes]) * ood;
                    // Make t1 be intersection with near plane, t2 with far plane
                    if (t1 > t2) (t1, t2) = (t2, t1);
                    // Compute the intersection of slab intersection intervals
                    if (t1 > tMin) tMin = t1;
                    if (t2 > tMax) tMax = t2;
                    // Exit with no collision as soon as slab intersection becomes empty
                    if (tMin > tMax)
                    {
                        intersectionPoint = Vec2f.Zero;
                        return false;
                    }
                }
            }

            // Ray intersects all 2 slabs. Return point (q) and intersection t value (tmin)
            intersectionPoint = p + d * tMin;
            return true;
        }

        #endregion
    }
}

