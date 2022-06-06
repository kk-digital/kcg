using Components;
using TileMap;
using UnityEngine;

namespace Agent
{
    public struct BoxColliderComponent
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
        
        public Vector2 Size;

        public CollisionInfo CollisionInfo;
        public bool IsIgnoringPlatforms;
        public bool IsOnGround => CollisionInfo.Below;

        public BoxColliderComponent(Vector2 size) : this()
        {
            Size = size;
        }
    
        /*public bool Intersects(Vector2 targetPos, Vector2 targetSize)
        {
            // TODO: Implement
            return false;
        }*/

        public static RectangleBounds Bounds(Vector2 newPos, Vector2 size)
        {
            var left = newPos.x - size.x;
            var leftTile = Mathf.CeilToInt(left);

            var right = newPos.x + size.x;
            var rightTile = Mathf.CeilToInt(right);

            var top = newPos.y + size.y;
            var topTile = Mathf.CeilToInt(top);

            var bottom = newPos.y - size.y;
            var bottomTile = Mathf.CeilToInt(bottom);

            return new RectangleBounds
            {
                Left = left, Right = right, Top = top, Bottom = bottom,
                LeftTile = leftTile, RightTile = rightTile,
                TopTile = topTile, BottomTile = bottomTile
            };
        }
    
        public bool IsCollidingLeft(ref TileMap.Component tileMap, Vector2 newPos)
        {
            var bounds = Bounds(newPos, Size);
            // TODO: don't bother checking if not moving right
            //if (Vel.x >= 0f) return false;
            
            for (int y = bounds.BottomTile; y <= bounds.TopTile; y++) 
            {
                var tile = ManagerSystem.Instance.GetTile(ref tileMap, bounds.LeftTile, y);
                int tilePropertiesIndex = tile.BackTilePropertiesId;

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.CreationAPISystem.GetTileProperties(tilePropertiesIndex);
                    return true;
                    //TODO: Use IsSolid and etc then return true if isSolid
                }
            }

            return false;
        }
    
        public bool IsCollidingRight(ref TileMap.Component tileMap, Vector2 newPos)
        {
            var bounds = Bounds(newPos, Size);
            // TODO: don't bother checking if not moving Left
            //if (Vel.x <= 0f) return false;
            
            for (int y = bounds.BottomTile; y <= bounds.TopTile; y++) 
            {
                var tile = ManagerSystem.Instance.GetTile(ref tileMap, bounds.RightTile, y);
                int tilePropertiesIndex = tile.BackTilePropertiesId;

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.CreationAPISystem.GetTileProperties(tilePropertiesIndex);
                    return true;
                    //TODO: Use IsSolid and etc then return true if isSolid
                }
            }

            return false;
        }
    
        public bool IsCollidingTop(ref TileMap.Component tileMap, Vector2 newPos)
        {
            var bounds = Bounds(newPos, Size);
            // TODO: don't bother checking if not moving Down
            //if (Vel.y <= 0f) return false;
            
            for (int x = bounds.LeftTile; x <= bounds.RightTile; x++) 
            {
                var tile = ManagerSystem.Instance.GetTile(ref tileMap, x, bounds.TopTile);
                int tilePropertiesIndex = tile.BackTilePropertiesId;

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.CreationAPISystem.GetTileProperties(tilePropertiesIndex);
                    return true;
                    //TODO: Use IsSolid and etc then return true if isSolid
                }
            }

            return false;
        }

        public bool IsCollidingBottom(ref TileMap.Component tileMap, Vector2 newPos)
        {
            var bounds = Bounds(newPos, Size);
            // TODO: don't bother checking if not moving Up
            //if (Vel.y >= 0f) return false;
            
            for (int x = bounds.LeftTile; x <= bounds.RightTile; x++)
            {
                var tile = ManagerSystem.Instance.GetTile(ref tileMap, x, bounds.BottomTile);
                int tilePropertiesIndex = tile.BackTilePropertiesId;

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.CreationAPISystem.GetTileProperties(tilePropertiesIndex);
                    return true;
                    //TODO: Use IsSolid and etc then return true if isSolid
                }
            }

            return false;
        }
    
        /*public Vector2[] DiscretizeDisplacement(Vector2 totalDisplacement)
        {
            // TODO: Reimplement
            
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
        }*/

        public Vector2 ResolveCollisions(ref TileMap.Component component, Vector2 newPos)
        {
            //var partialDisplacements = DiscretizeDisplacement(totalDisplacement);
            /*Vector2 newPos = default;
            foreach (var partialDisplacement in partialDisplacements)
            {
                newPos = Pos + partialDisplacement;

                if (IsCollidingLeft(newPos))
                {
                    CollisionInfo.Left = true;
                    Vel.x = 0;
                    newPos.x = Bounds(newPos).Left + 0.5f + Radius;
                }
                if (IsCollidingRight(newPos))
                {
                    CollisionInfo.Right = true;
                    Vel.x = 0;
                    newPos.x = Bounds(newPos).Right - 0.5f - Radius;
                }
                if (IsCollidingTop(newPos))
                {
                    CollisionInfo.Above = true;
                    Vel.y = 0;
                    newPos.y = Bounds(newPos).Top;
                }
                if (IsCollidingBottom(newPos))
                {
                    CollisionInfo.Below = true;
                    Vel.y = 0;
                    newPos.y = Bounds(newPos).Bottom + 0.5f + Radius;
                }
            
                if (CollisionInfo.Collided()) return newPos;
            }*/

            return newPos;
        }

        /*public float[] GetBBoxLines()
        {
            // TODO: Reimplement
            
            var x = Pos.x - Size.x;
            var y = Pos.y - Size.y;
        
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
        }*/
    
        /*public float[] GetInterpolatedBBoxLines()
        {
            // TODO: Reimplement
            
            // 60f is FPS
            // Time.fixedTime - TimeBetweenTicks
            float alpha = Time.fixedTime / (1.0f / 60f);

            //var vec = PrevPos.Mult(1 - alpha).Add(body.Pos.Mult(alpha)).Sub(cxmath.Vec2{body.Size.X / 2, body.Size.Y / 2});
            var vec = PrevPos * (1f - alpha) + (Pos * alpha) - new Vector2(Size.x, Size.y);
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
            // TODO: Reimplement
            
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
        }*/
    
        /*public float[] GetInterpolatedCollidingLines() 
        {
            // TODO: Reimplement
            
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
        }*/
    }
}
