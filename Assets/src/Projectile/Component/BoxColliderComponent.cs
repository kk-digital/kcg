using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Components;

namespace Projectile
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

        public struct RectangleBounds
        {
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
                Left = left,
                Right = right,
                Top = top,
                Bottom = bottom,
                LeftTile = leftTile,
                RightTile = rightTile,
                TopTile = topTile,
                BottomTile = bottomTile
            };
        }

        public bool IsCollidingLeft(ref PlanetTileMap.PlanetTileMap map, Vector2 newPos)
        {
            var bounds = Bounds(newPos, Size);

            for (int y = bounds.BottomTile; y <= bounds.TopTile; y++)
            {
                var tile = map.getTile(bounds.LeftTile, y);
                int tilePropertiesIndex = tile.BackTilePropertiesId;

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);
                    return true;
                }
            }

            return false;
        }

        public bool IsCollidingRight(ref PlanetTileMap.PlanetTileMap map, Vector2 newPos)
        {
            var bounds = Bounds(newPos, Size);

            for (int y = bounds.BottomTile; y <= bounds.TopTile; y++)
            {
                var tile = map.getTile(bounds.RightTile, y);
                int tilePropertiesIndex = tile.BackTilePropertiesId;

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);
                    return true;
                }
            }

            return false;
        }

        public bool IsCollidingTop(ref PlanetTileMap.PlanetTileMap map, Vector2 newPos)
        {
            var bounds = Bounds(newPos, Size);

            for (int x = bounds.LeftTile; x <= bounds.RightTile; x++)
            {
                var tile = map.getTile(x, bounds.TopTile);
                int tilePropertiesIndex = tile.BackTilePropertiesId;

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);
                    return true;
                }
            }

            return false;
        }

        public bool IsCollidingBottom(ref PlanetTileMap.PlanetTileMap map, Vector2 newPos)
        {
            var bounds = Bounds(newPos, Size);

            for (int x = bounds.LeftTile; x <= bounds.RightTile; x++)
            {
                var tile = map.getTile(x, bounds.BottomTile);
                int tilePropertiesIndex = tile.BackTilePropertiesId;

                if (tilePropertiesIndex >= 0)
                {
                    var tileProperties = GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);
                    return true;
                }
            }

            return false;
        }
        public Vector2 ResolveCollisions(ref PlanetTileMap.PlanetTileMap map, Vector2 newPos)
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
    }
}
