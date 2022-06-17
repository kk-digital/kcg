using UnityEngine;

namespace Physics
{
    public static class Box2DBordersExt
    {
        public static bool Intersects(this ref Box2DBorders borders, Vector2 position)
        {
            return position.x >= borders.Left && position.x <= borders.Right && 
                   position.y >= borders.Down && position.y <= borders.Up;
        }
        
        public static Box2DBorders CreateBoxBorders(this Vector2 position, Vector2 size)
        {
            var bottomLeft = position;
            var bottomRight = new Vector2(bottomLeft.x + size.x, bottomLeft.y);
            var topLeft = new Vector2(bottomLeft.x, bottomLeft.y + size.y);
            var topRight = new Vector2(bottomLeft.x + size.x, bottomLeft.y + size.y);

            var left = bottomLeft.x < 0 ? (int)bottomLeft.x - 1 : (int)bottomLeft.x;
            var right = bottomRight.x < 0 ? (int)bottomRight.x - 1 : (int)bottomRight.x;

            var up = (int)topLeft.y;
            var down = (int)bottomLeft.y;

            var center = new Vector2((position.x + position.x + size.x) / 2f, (position.y + position.y + size.y) / 2f);

            return new Box2DBorders
            {
                Center = center,
                
                BottomLeft = bottomLeft, BottomRight = bottomRight,
                TopLeft = topLeft, TopRight = topRight,

                Left = left, Right = right,
                Up = up, Down = down
            };
        }
        
        public static Box2DBorders CreateEntityBoxBorders(this Box2DColliderComponent colliderComponent, Vector2 position)
        {
            return position.CreateBoxBorders(colliderComponent.Size);
        }
    }
    
    public struct Box2DBorders
    {
        public Vector2 Center;
        
        public Vector2 BottomLeft, BottomRight;
        public Vector2 TopLeft, TopRight;

        public int Left, Right;
        public int Up, Down;
    }
}


