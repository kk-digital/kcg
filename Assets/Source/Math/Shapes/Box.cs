using Enums;

namespace KMath
{
    public static class BoxExt
    {
        public static bool Intersects(this Box box, Vec2f position)
        {
            return position.X >= box.Left && position.X <= box.Right &&
                   position.Y >= box.Down && position.Y <= box.Up;
        }
    }
    
    public struct Box
    {
        public Vec2f Center;

        public Vec2f BottomLeft => Vertices.List[0];
        public Vec2f BottomRight => Vertices.List[1];
        public Vec2f TopLeft => Vertices.List[2];
        public Vec2f TopRight => Vertices.List[3];

        public int Left, Right;
        public int Up, Down;

        public Vertices Vertices;
        
        public static Box Create(Vec2f position, Vec2f size)
        {
            var vertices = new Vertices(4, VerticesType.Box);
            
            var bottomLeft = position;
            var bottomRight = new Vec2f(bottomLeft.X + size.X, bottomLeft.Y);
            var topLeft = new Vec2f(bottomLeft.X, bottomLeft.Y + size.Y);
            var topRight = new Vec2f(bottomLeft.X + size.X, bottomLeft.Y + size.Y);

            vertices.List[0] = bottomLeft;
            vertices.List[1] = bottomRight;
            vertices.List[2] = topLeft;
            vertices.List[3] = topRight;

            var left = bottomLeft.X < 0 ? (int)bottomLeft.X - 1 : (int)bottomLeft.X;
            var right = bottomRight.X < 0 ? (int)bottomRight.X - 1 : (int)bottomRight.X;

            var up = (int)topLeft.Y;
            var down = (int)bottomLeft.Y;

            var center = new Vec2f((position.X + position.X + size.X) / 2f, (position.Y + position.Y + size.Y) / 2f);

            return new Box
            {
                Center = center,

                Left = left, Right = right,
                Up = up, Down = down
            };
        }
    }
}

