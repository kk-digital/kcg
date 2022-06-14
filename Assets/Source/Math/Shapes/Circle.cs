using System.Linq;
using Enums;

namespace KMath
{
    public struct Circle
    {
        public Vec2f Center;

        public Vec2f BottomMiddle => Center + new Vec2f(0f, -1f) * Radius;
        public Vec2f LeftMiddle => Center + new Vec2f(-1f, 0f) * Radius;
        public Vec2f RightMiddle => Center + new Vec2f(1f, 0f) * Radius;
        public Vec2f TopMiddle => Center + new Vec2f(0, 1f) * Radius;

        public Vec2f BottomLeft;

        public float Radius;
        
        public Vec2f GetPointOnEdge(Vec2f newPos)
        {
            var difference = newPos - BottomLeft;
            difference.Normalize();

            return Center + difference * Radius;
        }

        #region Quarters

        public Vec2i[] GetQuarterPositions(Vec2f pointOnEdge)
        {
            var difference = pointOnEdge - Center;
            var quarter = GetQuarterType(difference);

            return quarter switch
            {
                CircleQuarter.Top or CircleQuarter.Left or CircleQuarter.Right or CircleQuarter.Bottom => new[]
                {
                    (Vec2i) pointOnEdge
                },
                /*CircleQuarter.TopRight => GetTopRightQuarter(),
                CircleQuarter.TopLeft => GetTopLeftQuarter(),
                CircleQuarter.BottomRight => GetBottomRightQuarter(),
                CircleQuarter.BottomLeft => GetBottomLeftQuarter(),*/
                _ => GetBottomLeftQuarter().Union(GetTopLeftQuarter()).Union(GetBottomRightQuarter()).Union(GetBottomLeftQuarter()).ToArray()
            };
        }        
        
        public static CircleQuarter GetQuarterType(Vec2f pointOnCircle)
        {
            pointOnCircle.Normalize();

            if (pointOnCircle.X == 0)
            {
                if (pointOnCircle.Y > 0) return CircleQuarter.Top;
                if (pointOnCircle.Y < 0) return CircleQuarter.Bottom;
            }

            if (pointOnCircle.Y == 0)
            {
                if (pointOnCircle.X > 0) return CircleQuarter.Left;
                if (pointOnCircle.X < 0) return CircleQuarter.Right;
            }


            if (pointOnCircle.X > 0 && pointOnCircle.Y > 0)
            {
                return CircleQuarter.TopRight;
            }
            
            if (pointOnCircle.X > 0 && pointOnCircle.Y < 0)
            {
                return CircleQuarter.BottomRight;
            }

            if (pointOnCircle.X < 0 && pointOnCircle.Y > 0)
            {
                return CircleQuarter.TopLeft;
            }
            
            if (pointOnCircle.X < 0 && pointOnCircle.Y < 0)
            {
                return CircleQuarter.BottomLeft;
            }

            return CircleQuarter.Error;
        }
        
        public Vec2i[] GetTopLeftQuarter()
        {
            var box = Box.Create(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = (int) TopMiddle.X - box.Left + 1;
            int yDifference = box.Up - (int) RightMiddle.Y;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = box.Left; x <= (int)TopMiddle.X; x++, index++)
            {
                positions[index] = new Vec2i(x, box.Up);
            }
            for (int y = (int)LeftMiddle.Y; y <= box.Up - 1; y++, index++)
            {
                positions[index] = new Vec2i(box.Left, y);
            }

            return positions;
        }
        public Vec2i[] GetTopRightQuarter()
        {
            var box = Box.Create(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = box.Right - (int) TopMiddle.X + 1;
            int yDifference = box.Up - (int) RightMiddle.Y;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = (int)TopMiddle.X; x <= box.Right; x++, index++)
            {
                positions[index] = new Vec2i(x, box.Up);
            }
            for (int y = (int)RightMiddle.Y; y <= box.Up - 1; y++, index++)
            {
                positions[index] = new Vec2i(box.Right, y);
            }

            return positions;
        }

        public Vec2i[] GetBottomLeftQuarter()
        {
            var box = Box.Create(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = (int) BottomMiddle.X - box.Left + 1;
            int yDifference = (int) RightMiddle.Y - box.Down;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = box.Left; x <= (int)BottomMiddle.X; x++, index++)
            {
                positions[index] = new Vec2i(x, box.Down);
            }
            for (int y = box.Down + 1; y <= (int)LeftMiddle.Y; y++, index++)
            {
                positions[index] = new Vec2i(box.Left, y);
            }

            return positions;
        }
        public Vec2i[] GetBottomRightQuarter()
        {
            var box = Box.Create(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = box.Right - (int) BottomMiddle.X + 1;
            int yDifference = (int) RightMiddle.Y - box.Down;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = (int)BottomMiddle.X; x <= box.Right; x++, index++)
            {
                positions[index] = new Vec2i(x, box.Down);
            }
            for (int y = box.Down + 1; y <= (int)RightMiddle.Y; y++, index++)
            {
                positions[index] = new Vec2i(box.Right, y);
            }

            return positions;
        }
        
        #endregion


        public static Circle Create(Vec2f position, float radius, Vec2f size)
        {
            var center = new Vec2f((position.X + position.X + size.X) / 2f, (position.Y + position.Y + size.Y) / 2f);

            return new Circle
            {
                Center = center,
                BottomLeft = position,
                Radius = radius
            };
        }
    }
}

