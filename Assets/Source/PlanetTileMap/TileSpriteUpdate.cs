
using System;
using Enums.Tile;
using KMath;


namespace PlanetTileMap
{

    public class TileSpriteUpdate
    {
        // Updating the Sprite id requires checking the neighboring tiles
        // each sprite Rule respresent a different way of looking at the neighbors
        // to determine the sprite ids
        public static void UpdateBackNeighbourTiles(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetBackTile(x, y);
            
            if (tile.MaterialType != TileMaterialType.Error)
            {
                ref TileMaterial material = ref GameState.TileCreationApi.GetMaterial(tile.MaterialType);
                if (material.SpriteRuleType != SpriteRuleType.NoRule )
                {
                    if (material.SpriteRuleType == SpriteRuleType.R1)
                    {
                        SpriteRule_R1.UpdateBackSprite(x, y, tileMap);
                    }
                    else if (material.SpriteRuleType == SpriteRuleType.R2)
                    {
                        SpriteRule_R2.UpdateBackSprite(x, y, tileMap);
                    }
                    else if (material.SpriteRuleType == SpriteRuleType.R3)
                    {
                        SpriteRule_R3.UpdateBackSprite(x, y, tileMap);
                    }
                }
                else
                {
                    tile.TileID = -1;
                }
            }
            else
            {
                tile.TileID = -1;
            }

            tileMap.NeedsUpdate[(int) MapLayerType.Back] = true;
        }



        // Updating the Sprite id requires checking the neighboring tiles
        // each sprite Rule respresent a different way of looking at the neighbors
        // to determine the sprite ids
        public static void UpdateMidNeighbourTiles(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetMidTile(x, y);
            
            if (tile.MaterialType != TileMaterialType.Error)
            {
                ref TileMaterial material = ref GameState.TileCreationApi.GetMaterial(tile.MaterialType);
                if (material.SpriteRuleType != SpriteRuleType.NoRule)
                {
                    if (material.SpriteRuleType == SpriteRuleType.R1)
                    {
                        SpriteRule_R1.UpdateMidSprite(x, y, tileMap);
                    }
                    else if (material.SpriteRuleType == SpriteRuleType.R2)
                    {
                        SpriteRule_R2.UpdateMidSprite(x, y, tileMap);
                    }
                    else if (material.SpriteRuleType == SpriteRuleType.R3)
                    {
                        SpriteRule_R3.UpdateMidSprite(x, y, tileMap);
                    }
                }
                else
                {
                    tile.TileID = -1;
                }
            }
            else
            {
                tile.TileID = -1;
            }

            tileMap.NeedsUpdate[(int) MapLayerType.Back] = true;
        }



        // Updating the Sprite id requires checking the neighboring tiles
        // each sprite Rule respresent a different way of looking at the neighbors
        // to determine the sprite ids
        public static void UpdateFrontNeighbourTiles(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetFrontTile(x, y);
            
 
            if ( tile.MaterialType != TileMaterialType.Error)
            {
                ref TileMaterial material = ref GameState.TileCreationApi.GetMaterial(tile.MaterialType);
                if (material.SpriteRuleType != SpriteRuleType.NoRule )
                {
                    if (material.SpriteRuleType == SpriteRuleType.R1)
                    {
                        SpriteRule_R1.UpdateFrontSprite(x, y, tileMap);
                    }
                    else if (material.SpriteRuleType == SpriteRuleType.R2)
                    {
                        SpriteRule_R2.UpdateFrontSprite(x, y, tileMap);
                    }
                    else if (material.SpriteRuleType == SpriteRuleType.R3)
                    {
                        
                        SpriteRule_R3.UpdateFrontSprite(x, y, tileMap);
                    }
                }
                else
                {
                    tile.TileID = -1;
                }
            }
            else
            {
                tile.TileID = -1;
            }

            tileMap.NeedsUpdate[(int) MapLayerType.Back] = true;
        }





        // when a tile is (deleted/changed) tile sprite ids
        // of all the neighbors must be re-evaluated
        public static void UpdateBackTile(int x, int y, TileMap tileMap)
        {
            for(int i = x - 1; i <= x + 1; i++)
            {
                if (!tileMap.IsValid(i, 0)) continue;
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if (!tileMap.IsValid(i, j)) continue;
                    UpdateBackNeighbourTiles(i, j, tileMap);
                }
            }
        }

        



        // when a tile is (deleted/changed) tile sprite ids
        // of all the neighbors must be re-evaluated
        public static void UpdateMidTile(int x, int y, TileMap tileMap)
        {
            for(int i = x - 1; i <= x + 1; i++)
            {
                if (!tileMap.IsValid(i, 0)) continue;
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if (!tileMap.IsValid(i, j)) continue;
                    UpdateMidNeighbourTiles(i, j, tileMap);
                }
            }
        }


        // updates all the sprite ids in the layer
        public static void UpdateBackTileMapPositions(TileMap tileMap, int i, int j)
        {
            int bucketX = i / 64;
            int bucketY = j / 64;

            for(int y = 0; y < tileMap.MapSize.Y; y++)
            {
                int testBucketY = y / 64;
                for(int x = 0; x < tileMap.MapSize.X; x++)
                {
                    int testBucketX = x / 64;
                    if (bucketX == testBucketX && bucketY == testBucketY)
                    {
                        UpdateBackNeighbourTiles(x, y, tileMap);
                    }
                }
            }

            for(int y = 0; y < tileMap.MapSize.Y; y++)
            {
                int testBucketY = y / 64;
                for(int x = 0; x < tileMap.MapSize.X; x++)
                {
                    int testBucketX = x / 64;
                    if (bucketX != testBucketX || bucketY != testBucketY)
                    {
                        UpdateBackNeighbourTiles(x, y, tileMap);
                    }
                }
            }
        }

        // updates all the sprite ids in the layer
        public static void UpdateMidTileMapPositions(TileMap tileMap, int i, int j)
        {
            int bucketX = i / 64;
            int bucketY = j / 64;

            for(int y = 0; y < tileMap.MapSize.Y; y++)
            {
                int testBucketY = y / 64;
                for(int x = 0; x < tileMap.MapSize.X; x++)
                {
                    int testBucketX = x / 64;
                    if (bucketX == testBucketX && bucketY == testBucketY)
                    {
                        UpdateMidNeighbourTiles(x, y, tileMap);
                    }
                }
            }

            for(int y = 0; y < tileMap.MapSize.Y; y++)
            {
                int testBucketY = y / 64;
                for(int x = 0; x < tileMap.MapSize.X; x++)
                {
                    int testBucketX = x / 64;
                    if (bucketX != testBucketX || bucketY != testBucketY)
                    {
                        UpdateMidNeighbourTiles(x, y, tileMap);
                    }
                }
            }
        }

        // updates all the sprite ids in the layer
        public static void UpdateFrontTileMapPositions(TileMap tileMap, int i, int j)
        {
            int bucketX = i / 64;
            int bucketY = j / 64;

            for(int y = 0; y < tileMap.MapSize.Y; y++)
            {
                int testBucketY = y / 64;
                for(int x = 0; x < tileMap.MapSize.X; x++)
                {
                    int testBucketX = x / 64;
                    if (bucketX == testBucketX && bucketY == testBucketY)
                    {
                        UpdateFrontNeighbourTiles(x, y, tileMap);
                    }
                }
            }

            for(int y = 0; y < tileMap.MapSize.Y; y++)
            {
                int testBucketY = y / 64;
                for(int x = 0; x < tileMap.MapSize.X; x++)
                {
                    int testBucketX = x / 64;
                    if (bucketX != testBucketX || bucketY != testBucketY)
                    {
                        UpdateFrontNeighbourTiles(x, y, tileMap);
                    }
                }
            }
        }



        // when a tile is (deleted/changed) tile sprite ids
        // of all the neighbors must be re-evaluated
        public static void UpdateFrontTile(int x, int y, TileMap tileMap)
        {
            for(int i = x - 1; i <= x + 1; i++)
            {
                if (!tileMap.IsValid(i, 0)) continue;
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if (!tileMap.IsValid(i, j)) continue;
                    UpdateFrontNeighbourTiles(i, j, tileMap);
                }
            }
        }
    }
}