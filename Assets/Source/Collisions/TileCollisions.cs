using Enums.Tile;
using KMath;
using Utility;

namespace Collisions
{
    public static class TileCollisions
    {        
        public static bool RegionTileCollisionCheck(PlanetTileMap.TileMap tileMap, int xmin, int xmax, int ymin, int ymax)
        {
            var xchunkmin = xmin << 4;
            var xchunkmax = xmax << 4;
            var ychunkmin = ymin << 4;
            var ychunkmax = ymax << 4;

            for (int x = xchunkmin; x < xchunkmax; x++) 
            {
                for (int y = ychunkmin; y < ychunkmax; y++)
                {
                    //note: we already divided by 16 above
                    int chunk_index = x + y * tileMap.ChunkSize.X;
                    //check index if chunk is empty
                    if (tileMap.ChunkArray[chunk_index].Type == MapChunkType.Empty)
                    {
                        //skip-chunk its air
                        continue;
                    }
                    
                    //do the checks here for collisions
                    int tmp_xmin = Int.IntMax(xmin, xmin & 0x0f);
                    int tmp_xmax = Int.IntMin(xmax, xmax & 0x0f);
                    int tmp_ymin = Int.IntMax(ymin, xmax & 0x0f);
                    int tmp_ymax = Int.IntMin(ymax, ymax & 0x0f);
                    
                    //Do collision for each tile
                    for (var tmp_x = tmp_xmin; tmp_x < tmp_xmax; tmp_x++) 
                    {
                        for (var tmp_y = tmp_ymin; tmp_y < tmp_ymax; tmp_y++)
                        {
                            var frontTileID = tileMap.GetFrontTileID(tmp_x, tmp_y);
                            if (frontTileID != TileID.Air)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            
            return false;
        }
        
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
                        var frontTileID = tileMap.GetFrontTileID(x, y);
                        if (frontTileID != TileID.Air)
                        {
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
                        var frontTileID = tileMap.GetFrontTileID(x, y);
                        if (frontTileID != TileID.Air)
                        {
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
                        var frontTileID = tileMap.GetFrontTileID(x, y);
                        if (frontTileID != TileID.Air)
                        {
                            var tileBorders = new AABox2D(x, y);
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
                        var frontTileID = tileMap.GetFrontTileID(x, y);
                        if (frontTileID != TileID.Air)
                        {
                            var tileBorders = new AABox2D(x, y);
                            tileBorders.DrawBox();
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}