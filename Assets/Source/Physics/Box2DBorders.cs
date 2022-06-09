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
            var bottomLeft = Vector2Int.FloorToInt(position);
            var bottomRight = new Vector2Int(bottomLeft.x + (int)size.x, bottomLeft.y);
            var topLeft = new Vector2Int(bottomLeft.x, bottomLeft.y + (int)size.y);
            var topRight = new Vector2Int(bottomLeft.x + (int)size.x, bottomLeft.y + (int)size.y);

            var left = bottomLeft.x;
            var right = bottomRight.x;

            var up = topLeft.y;
            var down = bottomLeft.y;

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
        
        public Vector2Int BottomLeft, BottomRight;
        public Vector2Int TopLeft, TopRight;

        public int Left, Right;
        public int Up, Down;
    }
}


