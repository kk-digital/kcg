using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    
        public struct RectangleBounds {
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

        public RectangleBoundingBoxCollision(Vector2 pos, Vector2 size) : this()
        {
            Pos = pos;
            Size = size;
        }
    
        public bool ContainsBox(Vector2 targetPos, Vector2 targetSize)
        {
            var disp = targetPos - Pos;
            disp.Abs();
            // add 0.5 to account for offset to center of point
            var contains = 
                                disp.x < Size.x / 2 + targetSize.x &&
                                disp.y < Size.y / 2 + targetSize.y;
            return contains;
        }
        
        public bool Intersects(RectangleBoundingBoxCollision other)
        {
            var disp = Pos - other.Pos;
            disp.Abs();
            
            var intersects = 
                                disp.x < Size.x / 2 && 
                                disp.y < Size.y / 2;
        
            return intersects;
        }

        public RectangleBounds Bounds(Vector2 newPos)
        {
            var left = Mathf.Round(newPos.x - Size.x);
            var leftTile = (int) left;

            var right = Mathf.Round(newPos.x + Size.x);
            var rightTile = (int) right;

            var top = Mathf.Round(newPos.y + Size.y);
            var topTile = (int) top;

            var bottom = Mathf.Round(newPos.y - Size.y);
            var bottomTile = (int) bottom;

            return new RectangleBounds
            {
                Left = left, Right = right, Top = top, Bottom = bottom,
                LeftTile = leftTile, RightTile = rightTile,
                TopTile = topTile, BottomTile = bottomTile
            };
        }
    
        public bool IsCollidingLeft(ref PlanetTileMap.PlanetTileMap map, Vector2 newPos)
        {
            var bounds = Bounds(newPos);
            // TODO: don't bother checking if not moving right
            //if (Vel.x >= 0f) return false;
            
            for (int y = bounds.BottomTile; y <= bounds.TopTile; y++) 
            {
                var tile = map.getTile(bounds.LeftTile, y);
                int tilePropertiesIndex = tile.TileIdPerLayer[0];

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);
                    return true;
                    //TODO: Use IsSolid and etc then return true if isSolid
                }
            }

            return false;
        }
    
        public bool IsCollidingRight(ref PlanetTileMap.PlanetTileMap map, Vector2 newPos)
        {
            var bounds = Bounds(newPos);
            // TODO: don't bother checking if not moving Left
            //if (Vel.x <= 0f) return false;
            
            for (int y = bounds.BottomTile; y <= bounds.TopTile; y++) 
            {
                var tile = map.getTile(bounds.RightTile, y);
                int tilePropertiesIndex = tile.TileIdPerLayer[0];

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);
                    return true;
                    //TODO: Use IsSolid and etc then return true if isSolid
                }
            }

            return false;
        }
    
        public bool IsCollidingTop(ref PlanetTileMap.PlanetTileMap map, Vector2 newPos)
        {
            var bounds = Bounds(newPos);
            // TODO: don't bother checking if not moving Down
            //if (Vel.y <= 0f) return false;
            
            for (int x = bounds.LeftTile; x <= bounds.RightTile; x++) 
            {
                var tile = map.getTile(x, bounds.TopTile);
                int tilePropertiesIndex = tile.TileIdPerLayer[0];

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);
                    return true;
                    //TODO: Use IsSolid and etc then return true if isSolid
                }
            }

            return false;
        }
    
        public bool IsCollidingBottom(ref PlanetTileMap.PlanetTileMap map, Vector2 newPos)
        {
            var bounds = Bounds(newPos);
            // TODO: don't bother checking if not moving Up
            //if (Vel.y >= 0f) return false;
            
            for (int x = bounds.LeftTile; x <= bounds.RightTile; x++)
            {
                var tile = map.getTile(x, bounds.BottomTile);
                int tilePropertiesIndex = tile.TileIdPerLayer[0];

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);
                    return true;
                    //TODO: Use IsSolid and etc then return true if isSolid
                }
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

        public Vector2 CheckForCollisions(ref PlanetTileMap.PlanetTileMap map, Vector2 totalDisplacement)
        {
            var partialDisplacements = DiscretizeDisplacement(totalDisplacement);
            Vector2 newPos = default;
            foreach (var partialDisplacement in partialDisplacements)
            {
                newPos = Pos + partialDisplacement;

                if (IsCollidingLeft(ref map, newPos))
                {
                    Collisions.Left = true;
                    Vel.x = 0;
                    newPos.x = Bounds(newPos).Left + 0.5f + Size.x / 2f;
                }
                if (IsCollidingRight(ref map, newPos))
                {
                    Collisions.Right = true;
                    Vel.x = 0;
                    newPos.x = Bounds(newPos).Right - 0.5f - Size.x / 2f;
                }
                if (IsCollidingTop(ref map, newPos))
                {
                    Collisions.Above = true;
                    Vel.y = 0;
                    newPos.y = Bounds(newPos).Top - 0.5f - Size.y / 2f;
                }
                if (IsCollidingBottom(ref map, newPos))
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
            var collidingLines = new float[16];
            var bboxLines = GetBBoxLines();
            if (Collisions.Below)
            {
                Array.Copy(bboxLines, 0, collidingLines, 0, 4);
            }
            if (Collisions.Right) 
            {
                Array.Copy(bboxLines, 4, collidingLines, 4, 4);
            }
            if (Collisions.Above) {
                Array.Copy(bboxLines, 8, collidingLines, 8, 4);
            }
            if (Collisions.Left) {
                Array.Copy(bboxLines, 12, collidingLines, 12, 4);
            }

            return collidingLines;
        }
    
        public float[] GetInterpolatedCollidingLines() 
        {
            var collidingLines = new float[16];
            var bboxLines = GetInterpolatedBBoxLines();
        
            if (Collisions.Below)
            {
                Array.Copy(bboxLines, 0, collidingLines, 0, 4);
            }
            if (Collisions.Right) 
            {
                Array.Copy(bboxLines, 4, collidingLines, 4, 4);
            }
            if (Collisions.Above) {
                Array.Copy(bboxLines, 8, collidingLines, 8, 4);
            }
            if (Collisions.Left) {
                Array.Copy(bboxLines, 12, collidingLines, 12, 4);
            }

            return collidingLines;
        }
    }
}
