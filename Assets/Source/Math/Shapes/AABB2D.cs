using System;

namespace KMath
{
    /*
     LeftX          RightX
     TopY           TopY
        O----------O
        |  Center  |
        |    O     |
        |          |
        O----------O
     LeftX         RightX
     BottomY       BottomY
    */
    /// <summary>
    /// Axis-aligned Bounding Box 2D
    /// </summary>
    public struct AABB2D
    {
        public Vec2f Center;
        public Vec2f HalfSize;

        public float BottomY => Center.Y - HalfSize.Y;
        public float TopY => Center.Y + HalfSize.Y;
        public float LeftX => Center.X - HalfSize.X;
        public float RightX => Center.X + HalfSize.X;

        public AABB2D(Vec2f position, Vec2f size)
        {
            HalfSize = size / 2;
            Center = position + HalfSize;
        }
        
        public AABB2D(int x, int y, Vec2f size)
        {
            HalfSize = size / 2;
            Center = new Vec2f(x, y) + HalfSize;
        }
        
        public AABB2D(int x, int y)
        {
            HalfSize = new Vec2f(1, 1) / 2;
            Center = new Vec2f(x, y) + HalfSize;
        }

        #region Intersection

        public bool Intersects(Vec2i position)
        {
            return position.X >= LeftX   && position.X < RightX &&
                   position.Y >= BottomY && position.Y < TopY;
        }
        
        public bool Intersects(int x, int y)
        {
            return x >= LeftX   && x < RightX &&
                   y >= BottomY && y < TopY;
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

