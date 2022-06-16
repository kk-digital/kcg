using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Enums;
using Utility;

namespace KMath
{
    public struct Sphere2D
    {
        public Vec2f Center;
        public float Radius;

        public Vec2f Bottom
        {
            [MethodImpl((MethodImplOptions) 256)] get => Center + new Vec2f(0f, -1f) * Radius;
        }
        public Vec2f Left
        {
            [MethodImpl((MethodImplOptions) 256)] get => Center + new Vec2f(-1f, 0f) * Radius;
        }
        public Vec2f Right
        {
            [MethodImpl((MethodImplOptions) 256)] get => Center + new Vec2f(1f, 0f) * Radius;
        }
        public Vec2f Top
        {
            [MethodImpl((MethodImplOptions) 256)] get => Center + new Vec2f(0, 1f) * Radius;
        }

        public Sphere2D(Vec2f position, float radius, Vec2f spriteSize)
        {
            var center = position + (spriteSize / 2f);

            Center = center;
            Radius = radius;
        }

        [MethodImpl((MethodImplOptions) 256)]
        public Vec2f GetPointOnEdge(Vec2f otherCenter)
        {
            return Center + (otherCenter - Center).Normalized * Radius;
        }
        
        [MethodImpl((MethodImplOptions) 256)]
        public Vec2f GetDirection(Vec2f otherCenter)
        {
            return (otherCenter - Center).Normalized;
        }
        
        #region Quarters

        public Vec2i[] GetAllQuarters(Vec2f pointOnEdge)
        {
            return GetBottomLeftQuarter().Union(GetTopLeftQuarter()).Union(GetBottomRightQuarter()).Union(GetTopRightQuarter()).ToArray();
        }
        public static CircleQuarter GetQuarterType(Vec2f pointOnCircle)
        {
            pointOnCircle.Normalize();

            var type = CircleQuarter.Error;

            if (pointOnCircle.X == 0f)
            {
                switch (pointOnCircle.Y)
                {
                    case > 0f:
                        Flag.Set(ref type, CircleQuarter.Top);
                        break;
                    case < 0f:
                        Flag.Set(ref type, CircleQuarter.Bottom);
                        break;
                }
            }
            else if (pointOnCircle.Y == 0f)
            {
                switch (pointOnCircle.X)
                {
                    case > 0f:
                        Flag.Set(ref type, CircleQuarter.Right);
                        break;
                    case < 0f:
                        Flag.Set(ref type, CircleQuarter.Left);
                        break;
                }
            }

            switch (pointOnCircle.X)
            {
                case > 0:
                    switch (pointOnCircle.Y)
                    {
                        case > 0f:
                            Flag.Set(ref type, CircleQuarter.RightTop);
                            Flag.UnsetFlag(ref type, CircleQuarter.Right | CircleQuarter.Top);
                            break;
                        case < 0f:
                            Flag.Set(ref type, CircleQuarter.RightBottom);
                            Flag.UnsetFlag(ref type, CircleQuarter.Right | CircleQuarter.Bottom);
                            break;
                    }

                    break;
                case < 0:
                    switch (pointOnCircle.Y)
                    {
                        case > 0f:
                            Flag.Set(ref type, CircleQuarter.LeftTop);
                            Flag.UnsetFlag(ref type, CircleQuarter.Left | CircleQuarter.Top);
                            break;
                        case < 0f:
                            Flag.Set(ref type, CircleQuarter.LeftBottom);
                            Flag.UnsetFlag(ref type, CircleQuarter.Left | CircleQuarter.Bottom);
                            break;
                    }

                    break;
            }
            

            return type;
        }
        
        public Vec2i[] GetTopLeftQuarter()
        {
            var box = new AABB2D(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = (int) Top.X - box.IntLeft + 1;
            int yDifference = box.IntTop - (int) Right.Y;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = box.IntLeft; x <= (int)Top.X; x++, index++)
            {
                positions[index] = new Vec2i(x, box.IntTop);
            }
            for (int y = (int)Left.Y; y <= box.IntTop - 1; y++, index++)
            {
                positions[index] = new Vec2i(box.IntLeft, y);
            }

            return positions;
        }
        public Vec2i[] GetTopRightQuarter()
        {
            var box = new AABB2D(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = box.IntRight - (int) Top.X + 1;
            int yDifference = box.IntTop - (int) Right.Y;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = (int)Top.X; x <= box.IntRight; x++, index++)
            {
                positions[index] = new Vec2i(x, box.IntTop);
            }
            for (int y = (int)Right.Y; y <= box.IntTop - 1; y++, index++)
            {
                positions[index] = new Vec2i(box.IntRight, y);
            }

            return positions;
        }
        public Vec2i[] GetBottomLeftQuarter()
        {
            var box = new AABB2D(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = (int) Bottom.X - box.IntLeft + 1;
            int yDifference = (int) Right.Y - box.IntBottom;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = box.IntLeft; x <= (int)Bottom.X; x++, index++)
            {
                positions[index] = new Vec2i(x, box.IntBottom);
            }
            for (int y = box.IntBottom + 1; y <= (int)Left.Y; y++, index++)
            {
                positions[index] = new Vec2i(box.IntLeft, y);
            }

            return positions;
        }
        public Vec2i[] GetBottomRightQuarter()
        {
            var box = new AABB2D(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = box.IntRight - (int) Bottom.X + 1;
            int yDifference = (int) Right.Y - box.IntBottom;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = (int)Bottom.X; x <= box.IntRight; x++, index++)
            {
                positions[index] = new Vec2i(x, box.IntBottom);
            }
            for (int y = box.IntBottom + 1; y <= (int)Right.Y; y++, index++)
            {
                positions[index] = new Vec2i(box.IntRight, y);
            }

            return positions;
        }
        
        #endregion

        #region Intersection

        public bool Intersects(Sphere2D other)
        {
            var delta = Center - other.Center;
            var distance = Vec2f.Dot(delta, delta);

            float radiusSum = Radius + other.Radius;

            return distance <= radiusSum * radiusSum;
        }
        
        public bool Intersects(AABB2D box)
        {
            var closestPoint = box.GetClosestPoint(Center);
            var delta = closestPoint - Center;

            return Vec2f.Dot(delta, delta) <= Radius * Radius;
        }

        #endregion

        #region MovingIntersection

        // TODO: FIX Capsule Intersection
        public bool MovingIntersects(AABB2D box, Vec2f direction, out float t)
        {
            // Compute the AABB resulting from expanding b by sphere radius r
            AABB2D newBox = box;
            newBox.HalfSize += Radius;
            // Intersect ray against expanded AABB newBox. Exit with no intersection if ray
            // misses newBox, else get intersection point p and time t as result
            if (!newBox.IntersectsRay(Center, direction, out t, out var intersectionPoint) || t > 1.0f)
            {
                return false;
            }
            // Compute which min and max faces of b the intersection point p lies
            // outside of. Note, u and v cannot have the same bits set and
            // they must have at least one bit set among them
            int bit1 = 0, bit2 = 0;
            if (intersectionPoint.X < box.Left)   bit1 |= 1;
            if (intersectionPoint.X > box.Right)  bit2 |= 1;
            if (intersectionPoint.Y < box.Bottom) bit1 |= 2;
            if (intersectionPoint.Y > box.Top)    bit2 |= 2;
            // ‘Or’ all set bits together into a bit mask (note: here u + v == u | v)
            int bitCount = bit1 + bit2;
            // Define line segment [Center, Center + direction] specified by the sphere movement
            var lineSegment = new Line2D(Center, Center + direction);
            // If all 2 bits set (m == 3) then p is in a vertex region
            if (bitCount == 3)
            {
                // Must now intersect segment [c, c+d] against the capsules of the three
                // edges meeting at the vertex and return the best time, if one or more hit
                float tMin = float.MaxValue;
                // TODO: NOT WORKING
                if (new Cylinder2D(Radius, box.GetCornerByBitMask(bit2), box.GetCornerByBitMask(bit2 ^ 1)).Intersects(lineSegment, ref t))
                    tMin = Math.Min(t, tMin);
                // TODO: NOT WORKING
                if (new Cylinder2D(Radius, box.GetCornerByBitMask(bit2), box.GetCornerByBitMask(bit2 ^ 2)).Intersects(lineSegment, ref t))
                    tMin = Math.Min(t, tMin);
                
                if (tMin == float.MaxValue) return false; // No intersection
                
                t = tMin;
                return true; // Intersection at time t == tmin
            }
            // If only one bit set in m, then p is in a face region
            if ((bitCount & (bitCount - 1)) == 0) {
                // Do nothing. Time t from intersection with
                // expanded box is correct intersection time
                return true;
            }
            // point is in an edge region. Intersect against the capsule at the edge
            return new Cylinder2D(Radius, box.GetCornerByBitMask(bit1 ^ 3), box.GetCornerByBitMask(bit2)).Intersects(lineSegment, ref t);
        }

        #endregion
    }
}

