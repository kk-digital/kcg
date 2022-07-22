using Enums.Tile;
using KMath;
using Utility;
using UnityEngine;
using System;

namespace Collisions
{
    public static class Collisions
    {
        public static bool IsCollidingLeft(this AABox2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.X >= 0.0f) return false;
            
            int x = borders.xmin < 0 ? (int) borders.xmin - 1 : (int)borders.xmin;
            
            if (x >= 0 && x < tileMap.MapSize.X)
            {
                for (int y = (int)borders.ymin; y <= (int)borders.ymax; y++)
                {
                    if (y >= 0 && y < tileMap.MapSize.Y)
                    {
                        ref var tile = ref tileMap.GetFrontTile(x, y);
                        if (tile.MaterialType != PlanetTileMap.TileMaterialType.Air)
                        {
                            if (IsAPlatform(tile))
                            {
                                return false;
                            }
                            var tileBorders = new AABox2D(x, y);
                            tileBorders.DrawBox();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool IsCollidingRight(this AABox2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.X <= 0.0f) return false;
            
            int x = borders.xmax < 0 ? (int) borders.xmax - 1 : (int)borders.xmax;
            
            if (x >= 0 && x < tileMap.MapSize.X)
            {
                for (int y = (int)borders.ymin; y <= (int)borders.ymax; y++)
                {
                    if (y >= 0 && y < tileMap.MapSize.Y)
                    {
                        ref var tile = ref tileMap.GetFrontTile(x, y);

                        if (tile.MaterialType != PlanetTileMap.TileMaterialType.Air)
                        {
                            if (IsAPlatform(tile))
                            {
                                return false;
                            }
                            var tileBorders = new AABox2D(x, y);
                            tileBorders.DrawBox();
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        
        public static bool IsCollidingBottom(this AABox2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.Y >= 0.0f) return false;
            
            // LeftBottom.X >= 0f ? (int)LeftBottom.X : (int)LeftBottom.X - 1;
            
            int y = (int)borders.ymin;
            int leftX = borders.xmin < 0 ? (int) borders.xmin - 1 : (int)borders.xmin;
            int rightX = borders.xmax < 0 ? (int) borders.xmax - 1 : (int)borders.xmax;
            
            if (y >= 0 && y < tileMap.MapSize.Y)
            {
                for (int x = leftX; x <= rightX; x++)
                {
                    if (x >= 0 && x < tileMap.MapSize.X)
                    {
                        ref var tile = ref tileMap.GetFrontTile(x, y);
                        if (tile.MaterialType != PlanetTileMap.TileMaterialType.Air)
                        {
                            var tileBorders = new AABox2D(x, y);
                            if(Math.Abs(borders.ymin - tileBorders.ymax) > 0.1f && tile.MaterialType == PlanetTileMap.TileMaterialType.Platform)
                            {
                                return false;
                            }
                            tileBorders.DrawBox();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool IsCollidingTop(this AABox2D borders, PlanetTileMap.TileMap tileMap, Vec2f velocity)
        {
            if (velocity.Y <= 0.0f) return false;
            
            int y = (int)borders.ymax;
            int leftX = borders.xmin < 0 ? (int) borders.xmin - 1 : (int)borders.xmin;
            int rightX = borders.xmax < 0 ? (int) borders.xmax - 1 : (int)borders.xmax;
            
            if (y >= 0 && y < tileMap.MapSize.Y)
            {
                for (int x = leftX; x <= rightX; x++)
                {
                    if (x >= 0 && x < tileMap.MapSize.X)
                    {
                        ref var tile = ref tileMap.GetFrontTile(x, y);
                        
                        if (tile.MaterialType != PlanetTileMap.TileMaterialType.Air)
                        {
                            if (IsAPlatform(tile))
                            {
                                return false;
                            }
                            var tileBorders = new AABox2D(x, y);
                            tileBorders.DrawBox();
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool IsAPlatform(PlanetTileMap.Tile tile)
        {
            Debug.Log(tile.MaterialType);
            return tile.MaterialType == PlanetTileMap.TileMaterialType.Platform;
        }
    }
}