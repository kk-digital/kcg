using System.Linq;
using Enums;
using Utility;

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
            return GetBottomLeftQuarter().Union(GetTopLeftQuarter()).Union(GetBottomRightQuarter())
                .Union(GetTopRightQuarter()).ToArray();
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
            var box = new AABB(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = (int) TopMiddle.X - box.Left + 1;
            int yDifference = box.Top - (int) RightMiddle.Y;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = box.Left; x <= (int)TopMiddle.X; x++, index++)
            {
                positions[index] = new Vec2i(x, box.Top);
            }
            for (int y = (int)LeftMiddle.Y; y <= box.Top - 1; y++, index++)
            {
                positions[index] = new Vec2i(box.Left, y);
            }

            return positions;
        }
        public Vec2i[] GetTopRightQuarter()
        {
            var box = new AABB(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = box.Right - (int) TopMiddle.X + 1;
            int yDifference = box.Top - (int) RightMiddle.Y;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = (int)TopMiddle.X; x <= box.Right; x++, index++)
            {
                positions[index] = new Vec2i(x, box.Top);
            }
            for (int y = (int)RightMiddle.Y; y <= box.Top - 1; y++, index++)
            {
                positions[index] = new Vec2i(box.Right, y);
            }

            return positions;
        }

        public Vec2i[] GetBottomLeftQuarter()
        {
            var box = new AABB(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = (int) BottomMiddle.X - box.Left + 1;
            int yDifference = (int) RightMiddle.Y - box.Bottom;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = box.Left; x <= (int)BottomMiddle.X; x++, index++)
            {
                positions[index] = new Vec2i(x, box.Bottom);
            }
            for (int y = box.Bottom + 1; y <= (int)LeftMiddle.Y; y++, index++)
            {
                positions[index] = new Vec2i(box.Left, y);
            }

            return positions;
        }
        public Vec2i[] GetBottomRightQuarter()
        {
            var box = new AABB(Center - Radius, new Vec2f(Radius * 2, Radius * 2));

            int xDifference = box.Right - (int) BottomMiddle.X + 1;
            int yDifference = (int) RightMiddle.Y - box.Bottom;

            var positions = new Vec2i[xDifference + yDifference];
            var index = 0;
                
            for (int x = (int)BottomMiddle.X; x <= box.Right; x++, index++)
            {
                positions[index] = new Vec2i(x, box.Bottom);
            }
            for (int y = box.Bottom + 1; y <= (int)RightMiddle.Y; y++, index++)
            {
                positions[index] = new Vec2i(box.Right, y);
            }

            return positions;
        }
        
        #endregion


        public static Circle Create(Vec2f position, float radius, Vec2f spriteSize)
        {
            var center = new Vec2f((position.X + position.X + spriteSize.X) / 2f, (position.Y + position.Y + spriteSize.Y) / 2f);

            return new Circle
            {
                Center = center,
                BottomLeft = position,
                Radius = radius
            };
        }
    }
}

