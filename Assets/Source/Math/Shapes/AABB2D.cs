using System.Runtime.CompilerServices;
using System;

namespace KMath
{
    /// <summary>
    /// Axis-aligned Bounding Box 2D
    /// </summary>
    public struct AABB2D
    {
        public Vec2i Center;
        public Vec2i HalfSize;

        public AABB2D(Vec2i position, Vec2i size)
        {
            HalfSize = size / 2;
            Center = position + HalfSize;
        }

        #region Intersection

        public bool Intersects(Vec2i position)
        {
            return position.X >= IntLeft   && position.X < IntRight &&
                   position.Y >= IntBottom && position.Y < IntTop;
        }
        
        public bool Intersects(int x, int y)
        {
            return x >= IntLeft   && x < IntRight &&
                   y >= IntBottom && y < IntTop;
        }
        
        public bool Intersects(AABB2D other)
        {
            if (Math.Abs(Center.X - other.Center.X) > HalfSize.X + other.HalfSize.X) return false;
            if (Math.Abs(Center.Y - other.Center.Y) > HalfSize.Y + other.HalfSize.Y) return false;
            return true;
        }

        #endregion
    }
}

