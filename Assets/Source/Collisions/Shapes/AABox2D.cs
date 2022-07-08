using System;

namespace KMath
{
    /*
     xmin           xmax
     ymax           ymax
        O----------O
        |box_center|
        |    O     |
        |          |
        O----------O
     xmin         xmax
     ymin         ymin
    */
    /// <summary>
    /// Axis-aligned Bounding Box 2D
    /// </summary>
    public struct AABox2D
    {
        public Vec2f box_center;
        public Vec2f box_halfsize;

        public float ymin => box_center.Y - box_halfsize.Y; // Bottom bounding line
        public float ymax => box_center.Y + box_halfsize.Y; // Top bounding line
        public float xmin => box_center.X - box_halfsize.X; // Left bounding line
        public float xmax => box_center.X + box_halfsize.X; // Right bounding line

        public AABox2D(Vec2f position, Vec2f size)
        {
            box_halfsize = size / 2;
            box_center = position + box_halfsize;
        }
        
        public AABox2D(int x, int y, Vec2f size)
        {
            box_halfsize = size / 2;
            box_center = new Vec2f(x, y) + box_halfsize;
        }
        
        public AABox2D(int x, int y)
        {
            box_halfsize = new Vec2f(1, 1) / 2;
            box_center = new Vec2f(x, y) + box_halfsize;
        }

        #region Intersection

        public bool Intersects(Vec2i position)
        {
            return position.X >= xmin   && position.X < xmax &&
                   position.Y >= ymin && position.Y < ymax;
        }
        
        public bool Intersects(int x, int y)
        {
            return x >= xmin && x < xmax &&
                   y >= ymin && y < ymax;
        }
        
        public bool Intersects(AABox2D other)
        {
            if (Math.Abs(box_center.X - other.box_center.X) > box_halfsize.X + other.box_halfsize.X) return false;
            if (Math.Abs(box_center.Y - other.box_center.Y) > box_halfsize.Y + other.box_halfsize.Y) return false;
            return true;
        }

        #endregion
    }
}

