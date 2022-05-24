﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

namespace Physics
{
    public struct RectangleBoundingCircleCollision
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
        public float Radius;
        public float Direction;

        // PreviousTransform     mgl32.Mat4
        // InterpolatedTransform mgl32.Mat4

        public CollisionInfo Collisions;

        // Deleted bool

        public bool IsIgnoringPlatforms;
        public bool IgnoresGravity;

        public bool IsOnGround => Collisions.Below;
    
        public bool ContainsCircle(Vector2 targetPos, float targetRadius)
        {
            var disp = Mathf.Sqrt(Mathf.Pow(targetPos.x - Pos.x, 2f) + Mathf.Pow(targetPos.y - Pos.y, 2f));
            
            return Radius > disp + targetRadius;
        }
        
        public bool Intersects(RectangleBoundingBoxCollision other)
        {
            var disp = Mathf.Sqrt(Mathf.Pow(other.Pos.x - Pos.x, 2f) + Mathf.Pow(other.Pos.y - Pos.y, 2f));
            
            return Radius > disp;
        }

        public BodyBounds Bounds(Vector2 newPos)
        {
            var left = Mathf.Round(Pos.x - Radius);
            var leftTile = (int) left;

            var right = Mathf.Round(Pos.x + Radius);
            var rightTile = (int) right;

            var top = Mathf.Round(Pos.y + Radius);
            var topTile = (int) top;

            var bottom = Mathf.Round(Pos.y - Radius);
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

            var bottom = (int) Mathf.Round(Pos.y - Radius + Eps);
            var top = (int) Mathf.Round(Pos.y + Radius - Eps);
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

            var bottom = (int) Mathf.Round(Pos.y - Radius + Eps);
            var top = (int) Mathf.Round(Pos.y + Radius - Eps);
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

            var left = (int) Mathf.Round(Pos.x - Radius + Eps);
            var right = (int) Mathf.Round(Pos.x + Radius + Eps);
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

            var left = (int) Mathf.Round(Pos.x - Radius + Eps);
            var right = (int) Mathf.Round(Pos.x + Radius + Eps);
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
                    newPos.x = Bounds(newPos).Left + 0.5f + Radius;
                }
                if (IsCollidingRight(newPos))
                {
                    Collisions.Right = true;
                    Vel.x = 0;
                    newPos.x = Bounds(newPos).Right - 0.5f - Radius;
                }
                if (IsCollidingTop(newPos))
                {
                    Collisions.Above = true;
                    Vel.y = 0;
                    newPos.y = Bounds(newPos).Top - 0.5f - Radius;
                }
                if (IsCollidingBottom(newPos))
                {
                    Collisions.Below = true;
                    Vel.y = 0;
                    newPos.y = Bounds(newPos).Bottom + 0.5f + Radius;
                }
            
                if (Collisions.Collided()) return newPos;
            }

            return newPos;
        }
    
        public float[] GetBBoxLines()
        {
            var x = Pos.x - Radius;
            var y = Pos.y - Radius;
        
            return new[]
            {
                //bottom
                x, y,
                x + Radius * 2, y,

                //right
                x + Radius * 2, y,
                x + Radius * 2, y + Radius * 2,

                //top
                x + Radius * 2, y + Radius * 2,
                x, y + Radius * 2,

                //left
                x, y + Radius * 2,
                x, y,
            };
        }
    
        public float[] GetInterpolatedBBoxLines()
        {
            // 60f is FPS
            // Time.fixedTime - TimeBetweenTicks
            float alpha = Time.fixedTime / (1.0f / 60f);

            //var vec = PrevPos.Mult(1 - alpha).Add(body.Pos.Mult(alpha)).Sub(cxmath.Vec2{body.Size.X / 2, body.Size.Y / 2});
            var vec = PrevPos * (1f - alpha) + (Pos * alpha) - new Vector2(Radius, Radius);
            var x = vec.x;
            var y = vec.y;

            return new[]
            {
                //bottom
                x, y,
                x + Radius * 2, y,

                //right
                x + Radius * 2, y,
                x + Radius * 2, y + Radius * 2,

                //top
                x + Radius * 2, y + Radius * 2,
                x, y + Radius * 2,

                //left
                x, y + Radius * 2,
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
    }
}