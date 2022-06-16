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
        public Vec2f GetPointOnEdge(Vec2f newPos)
        {
            return Center + (newPos - Center).Normalized * Radius;
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
    }
}

