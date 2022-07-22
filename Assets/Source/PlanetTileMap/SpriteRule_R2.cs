using Enums.Tile;
using System;


namespace PlanetTileMap
{


    public static class SpriteRule_R2
    {

         // TODO: Refactor
        public static TilePosition GetTilePosition(TileMaterialType[] neighbors, TileMaterialType materialType)
        {
            int biggestMatch = 0;
            TilePosition tilePosition = 0;

            // we have 16 different values for the spriteId
            foreach(var position in (TilePosition[])Enum.GetValues(typeof(TilePosition)))
            {
                int match = CheckTile(neighbors, position, materialType);

                // pick only tiles with the biggest match count
                if (match > biggestMatch)
                {
                    biggestMatch = match;
                    tilePosition = position;
                }
            }

            return tilePosition;
        }



        // TODO: Refactor
        public static int CheckTile(TileMaterialType[] neighbors, TilePosition rules, TileMaterialType materialType)
        {
            // 16 different values can be stored
            // using only 4 bits for the
            // adjacent tiles 

            int[] neighborBit = {
                0x1, 0x2, 0x4, 0x8
            };

            int match = 0;
            // number of total neighbors is 4 right/left/down/up
            for(int i = 0; i < neighbors.Length; i++)
            {
                // check if we have to have the same tileId
                // in this particular neighbor                      
                if (((int)rules & neighborBit[i]) == neighborBit[i])
                {
                    // if this neighbor does not match return -1 immediately
                    if (neighbors[i] != materialType) return -1;
                    match++;
                }
            }


            return match;
        }

        public static void UpdateBackSprite(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetBackTile(x, y);
            ref TileMaterial tileMaterial = ref GameState.TileCreationApi.GetMaterial(tile.MaterialType);
            
            // standard sheet mapping
            // every tile has a constant offset
            // in the sprite atlas

            // example: 15 is (3,3)
            //           8 is (0,2)
            //           1 is (1,0)
            int[] tilePositionToTileSet = {12, 13, 15, 14, 0, 1, 3, 2, 8, 9, 11, 10, 4, 5, 7, 6};

            // we have 4 neighbors per tile
            // could be more but its 4 for now
            // right/left/down/up
            var neighbors = new TileMaterialType[4];

            for (int i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = TileMaterialType.Air;
            }

            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x + 1, y);
                neighbors[(int) Neighbor.Right] = neighborTile.MaterialType;
            }

            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x - 1, y);
                neighbors[(int) Neighbor.Left] = neighborTile.MaterialType;
            }

            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x, y + 1);
                neighbors[(int) Neighbor.Up] = neighborTile.MaterialType;
            }

            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetBackTile(x, y - 1);
                neighbors[(int) Neighbor.Down] = neighborTile.MaterialType;
            }


            var tilePosition = GetTilePosition(neighbors, tile.MaterialType);

            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.TileID = tileMaterial.StartTileIndex + tilePositionToTileSet[(int) tilePosition];
        }


        public static void UpdateMidSprite(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetMidTile(x, y);
            ref TileMaterial tileMaterial = ref GameState.TileCreationApi.GetMaterial(tile.MaterialType);
            
            // standard sheet mapping
            // every tile has a constant offset
            // in the sprite atlas

            // example: 15 is (3,3)
            //           8 is (0,2)
            //           1 is (1,0)
            int[] tilePositionToTileSet = {12, 13, 15, 14, 0, 1, 3, 2, 8, 9, 11, 10, 4, 5, 7, 6};

            // we have 4 neighbors per tile
            // could be more but its 4 for now
            // right/left/down/up
            var neighbors = new TileMaterialType[4];

            for (int i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = TileMaterialType.Air;
            }

            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x + 1, y);
                neighbors[(int) Neighbor.Right] = neighborTile.MaterialType;
            }

            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x - 1, y);
                neighbors[(int) Neighbor.Left] = neighborTile.MaterialType;
            }

            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x, y + 1);
                neighbors[(int) Neighbor.Up] = neighborTile.MaterialType;
            }

            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetMidTile(x, y - 1);
                neighbors[(int) Neighbor.Down] = neighborTile.MaterialType;
            }


            var tilePosition = GetTilePosition(neighbors, tile.MaterialType);

            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.TileID = tileMaterial.StartTileIndex + tilePositionToTileSet[(int) tilePosition];
        }


        public static void UpdateFrontSprite(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetFrontTile(x, y);
            ref TileMaterial tileMaterial = ref GameState.TileCreationApi.GetMaterial(tile.MaterialType);
            
            // standard sheet mapping
            // every tile has a constant offset
            // in the sprite atlas

            // example: 15 is (3,3)
            //           8 is (0,2)
            //           1 is (1,0)
            int[] tilePositionToTileSet = {12, 13, 15, 14, 0, 1, 3, 2, 8, 9, 11, 10, 4, 5, 7, 6};

            // we have 4 neighbors per tile
            // could be more but its 4 for now
            // right/left/down/up
            var neighbors = new TileMaterialType[4];

            for (int i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = TileMaterialType.Air;
            }

            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x + 1, y);
                neighbors[(int) Neighbor.Right] = neighborTile.MaterialType;
            }

            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x - 1, y);
                neighbors[(int) Neighbor.Left] = neighborTile.MaterialType;
            }

            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x, y + 1);
                neighbors[(int) Neighbor.Up] = neighborTile.MaterialType;
            }

            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetFrontTile(x, y - 1);
                neighbors[(int) Neighbor.Down] = neighborTile.MaterialType;
            }


            var tilePosition = GetTilePosition(neighbors, tile.MaterialType);

            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.TileID = tileMaterial.StartTileIndex + tilePositionToTileSet[(int) tilePosition];
        }
    }
}