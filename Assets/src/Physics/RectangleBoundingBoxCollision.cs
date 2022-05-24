using System.Collections.Generic;
using Physics;
using UnityEngine;

namespace Physics
{
    public struct RectangleBoundingBoxCollision
    {
        // epsilon parameter for values that are "close enough"
        public const float Eps = 0.05f;
        // maximum allowable speed.
        // This cap is important for performance reasons 
        // because we discretize the displacements.
        public const float SafeSpeed = 20f;
        public const float MaxPartialDisplacement = 0.5f;
    
        public struct BodyBounds {
            public float Left, Right, Top, Bottom;
            public int LeftTile, RightTile, TopTile, BottomTile;
        }
    
        public Vector2 Pos;
        public Vector2 PrevPos;
        public Vector2 Vel;
        public Vector2 Size;
        public float Direction;

        // PreviousTransform     mgl32.Mat4
        // InterpolatedTransform mgl32.Mat4

        public CollisionInfo Collisions;

        // Deleted bool

        public bool IsIgnoringPlatforms;
        public bool IgnoresGravity;

        public bool IsOnGround => Collisions.Below;
    
        public bool Contains(Vector2 targetPos, Vector2 targetSize)
        {
            var disp = targetPos - Pos;
            // add 0.5 to account for offset to center of point
            var contains = Mathf.Abs(disp.x) < Size.x / 2 + targetSize.x &&
                           Mathf.Abs(disp.y) < Size.y / 2 + targetSize.y;
            return contains;
        }

        public BodyBounds Bounds(Vector2 newPos)
        {
            var left = Mathf.Round(newPos.x - Size.x / 2);
            var leftTile = (int) left;

            var right = Mathf.Round(newPos.x + Size.x / 2);
            var rightTile = (int) right;

            var top = Mathf.Round(newPos.y + Size.y / 2);
            var topTile = (int) top;

            var bottom = Mathf.Round(newPos.y - Size.y / 2);
            var bottomTile = (int) bottom;

            return new BodyBounds
            {
                Left = left, Right = right, Top = top, Bottom = bottom,
                LeftTile = leftTile, RightTile = rightTile,
                TopTile = topTile, BottomTile = bottomTile
            };
        }
    
        public bool IsCollidingLeft(Vector2 newPos)
        {
            var bounds = Bounds(newPos);
            // don't bother checking if not moving left
            if (Vel.x >= 0f) return false;

            var bottom = (int) Mathf.Round(Pos.y - Size.y / 2 + Eps);
            var top = (int) Mathf.Round(Pos.y + Size.y / 2 - Eps);
            for (int y = bottom; y <= top; y++) 
            {
                if (GameState.TileCreationApi.GetTile(bounds.LeftTile, y).IsSolid) return true;
            }

            return false;
        }
    
        public bool IsCollidingRight(Vector2 newPos)
        {
            var bounds = Bounds(newPos);
            // don't bother checking if not moving left
            if (Vel.x <= 0f) return false;

            var bottom = (int) Mathf.Round(Pos.y - Size.y / 2 + Eps);
            var top = (int) Mathf.Round(Pos.y + Size.y / 2 - Eps);
            for (int y = bottom; y <= top; y++) 
            {
                if (GameState.TileCreationApi.GetTile(bounds.RightTile, y).IsSolid) return true;
            }

            return false;
        }
    
        public bool IsCollidingTop(Vector2 newPos)
        {
            var bounds = Bounds(newPos);
            // don't bother checking if not moving left
            if (Vel.y <= 0f) return false;

            var left = (int) Mathf.Round(Pos.x - Size.x / 2 + Eps);
            var right = (int) Mathf.Round(Pos.x + Size.x / 2 - Eps);
            for (int x = left; x <= right; x++) 
            {
               if (GameState.TileCreationApi.GetTile(x, bounds.TopTile).IsSolid) return true;
            }

            return false;
        }
    
        public bool IsCollidingBottom(Vector2 newPos)
        {
            var bounds = Bounds(newPos);
            // don't bother checking if not moving left
            if (Vel.y >= 0f) return false;

            var left = (int) Mathf.Round(Pos.x - Size.x / 2 + Eps);
            var right = (int) Mathf.Round(Pos.x + Size.x / 2 - Eps);
            for (int x = left; x <= right; x++) 
            {
                if (GameState.TileCreationApi.GetTile(x, bounds.BottomTile).IsSolid) return true;
            }

            return false;
        }
    
        public Vector2[] DiscretizeDisplacement(Vector2 totalDisplacement)
        {
            var n = totalDisplacement.magnitude;
            var normalized = totalDisplacement.normalized * MaxPartialDisplacement;

            var partialDisplacements = new List<Vector2>();
            for (int i = 0; i < (int)(n / MaxPartialDisplacement); i++)
            {
                var partialDisplacement = normalized * (i + 0.5f);
                partialDisplacements.Add(partialDisplacement);
            }
            partialDisplacements.Add(totalDisplacement);
        
            return partialDisplacements.ToArray();
        }

        public Vector2 CheckForCollisions(Vector2 totalDisplacement)
        {
            var partialDisplacements = DiscretizeDisplacement(totalDisplacement);
            Vector2 newPos = default;
            foreach (var partialDisplacement in partialDisplacements)
            {
                newPos = Pos + partialDisplacement;

                if (IsCollidingLeft(newPos))
                {
                    Collisions.Left = true;
                    Vel.x = 0;
                    newPos.x = Bounds(newPos).Left + 0.5f + Size.x / 2f;
                }
                if (IsCollidingRight(newPos))
                {
                    Collisions.Right = true;
                    Vel.x = 0;
                    newPos.x = Bounds(newPos).Right - 0.5f - Size.x / 2f;
                }
                if (IsCollidingTop(newPos))
                {
                    Collisions.Above = true;
                    Vel.y = 0;
                    newPos.y = Bounds(newPos).Top - 0.5f - Size.y / 2f;
                }
                if (IsCollidingBottom(newPos))
                {
                    Collisions.Below = true;
                    Vel.y = 0;
                    newPos.y = Bounds(newPos).Bottom + 0.5f + Size.y / 2f;
                }
            
                if (Collisions.Collided()) return newPos;
            }

            return newPos;
        }
    
        public float[] GetBBoxLines()
        {
            var x = Pos.x - Size.x / 2f;
            var y = Pos.y - Size.y / 2f;
        
            return new[]
            {
                //bottom
                x, y,
                x + Size.x, y,

                //right
                x + Size.x, y,
                x + Size.x, y + Size.y,

                //top
                x + Size.x, y + Size.y,
                x, y + Size.y,

                //left
                x, y + Size.y,
                x, y,
            };
        }
    
        public float[] GetInterpolatedBBoxLines()
        {
            // 60f is FPS
            // Time.fixedTime - TimeBetweenTicks
            float alpha = Time.fixedTime / (1.0f / 60f);

            //var vec = PrevPos.Mult(1 - alpha).Add(body.Pos.Mult(alpha)).Sub(cxmath.Vec2{body.Size.X / 2, body.Size.Y / 2});
            var vec = PrevPos * (1f - alpha) + (Pos * alpha) - new Vector2(Size.x / 2, Size.y / 2);
            var x = vec.x;
            var y = vec.y;

            return new[]
            {
                //bottom
                x, y,
                x + Size.x, y,

                //right
                x + Size.x, y,
                x + Size.x, y + Size.y,

                //top
                x + Size.x, y + Size.y,
                x, y + Size.y,

                //left
                x, y + Size.y,
                x, y,
            };
        }
    
        public float[] GetCollidingLines()
        {
            var collidingLines = new List<float>(16);
            var bboxLines = GetBBoxLines();
            if (Collisions.Below) 
            {
                collidingLines.Add(bboxLines[0]);
                collidingLines.Add(bboxLines[1]);
                collidingLines.Add(bboxLines[2]);
                collidingLines.Add(bboxLines[3]);
            }
            if (Collisions.Right) 
            {
                collidingLines.Add(bboxLines[4]);
                collidingLines.Add(bboxLines[5]);
                collidingLines.Add(bboxLines[6]);
                collidingLines.Add(bboxLines[7]);
            }
            if (Collisions.Above) {
                collidingLines.Add(bboxLines[8]);
                collidingLines.Add(bboxLines[9]);
                collidingLines.Add(bboxLines[10]);
                collidingLines.Add(bboxLines[11]);
            }
            if (Collisions.Left) {
                collidingLines.Add(bboxLines[12]);
                collidingLines.Add(bboxLines[13]);
                collidingLines.Add(bboxLines[14]);
                collidingLines.Add(bboxLines[15]);
            }

            return collidingLines.ToArray();
        }
    
        public float[] GetInterpolatedCollidingLines() 
        {
            var collidingLines = new List<float>(16);
            var bboxLines = GetInterpolatedBBoxLines();
        
            if (Collisions.Below) 
            {
                collidingLines.Add(bboxLines[0]);
                collidingLines.Add(bboxLines[1]);
                collidingLines.Add(bboxLines[2]);
                collidingLines.Add(bboxLines[3]);
            }
            if (Collisions.Right) 
            {
                collidingLines.Add(bboxLines[4]);
                collidingLines.Add(bboxLines[5]);
                collidingLines.Add(bboxLines[6]);
                collidingLines.Add(bboxLines[7]);
            }
            if (Collisions.Above) {
                collidingLines.Add(bboxLines[8]);
                collidingLines.Add(bboxLines[9]);
                collidingLines.Add(bboxLines[10]);
                collidingLines.Add(bboxLines[11]);
            }
            if (Collisions.Left) {
                collidingLines.Add(bboxLines[12]);
                collidingLines.Add(bboxLines[13]);
                collidingLines.Add(bboxLines[14]);
                collidingLines.Add(bboxLines[15]);
            }

            return collidingLines.ToArray();
        }
    
        public bool Intersects(RectangleBoundingBoxCollision other)
        {
            var disp = Pos - other.Pos;
            var intersects =
                Mathf.Abs(disp.x) < Size.x / 2 &&
                Mathf.Abs(disp.y) < Size.y / 2;
        
            return intersects;
        }
    }
}
