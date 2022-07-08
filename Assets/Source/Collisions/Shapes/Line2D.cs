using Enums.Tile;

namespace KMath
{
    public struct Line2D
    {
        public Vec2f A;
        public Vec2f B;

        public Line2D(Vec2f a, Vec2f b)
        {
            A = a;
            B = b;
        }
        
        // LINE/LINE collision check
        public bool Intersects(Line2D other)
        {
            // calculate the distance to intersection point
            var uA = ((other.B.X - other.A.X) * (A.Y - other.A.Y) - (other.B.Y - other.A.Y) * (A.X - other.A.X)) /
                     ((other.B.Y - other.A.Y) * (B.X - A.X) - (other.B.X - other.A.X) * (B.Y - A.Y));
            var uB = ((B.X - A.X) * (A.Y - other.A.Y) - (B.Y - A.Y) * (A.X - other.A.X)) /
                     ((other.B.Y - other.A.Y) * (B.X - A.X) - (other.B.X - other.A.X) * (B.Y - A.Y));

            // if uA and uB are between 0-1, lines are colliding
            return uA is >= 0 and <= 1 && uB is >= 0 and <= 1;
        }
    }
}

