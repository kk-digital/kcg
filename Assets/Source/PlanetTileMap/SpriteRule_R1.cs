using Enums.Tile;
using System;


namespace PlanetTileMap
{


    public static class SpriteRule_R1
    {

         // TODO: Refactor
        public static TilePosition GetTilePosition(TileID[] neighbors, TileID tileId)
        {
            int biggestMatch = 0;
            TilePosition tilePosition = 0;

            // we have 16 different values for the spriteId
            foreach(var position in (TilePosition[])Enum.GetValues(typeof(TilePosition)))
            {
                int match = CheckTile(neighbors, position, tileId);

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
        public static int CheckTile(TileID[] neighbors, TilePosition rules, TileID tileId)
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
                    if (neighbors[i] != tileId) return -1;
                    match++;
                }
            }


            return match;
        }

        public static void UpdateBackSprite(int x, int y, TileMap tileMap)
        {
            var tile = tileMap.GetTile(x, y);
            ref var property = ref GameState.TileCreationApi.GetTileProperty(tile.BackTileID);
            
            // standard sheet mapping
            // every tile has a constant offset
            // in the sprite atlas

            // example: 15 is (3,3)
            //           8 is (0,2)
            //           1 is (1,0)
            int[] tilePositionToTileSet = {15, 12, 14, 13, 3, 0, 2, 1, 11, 8, 10, 9, 7, 4, 6, 5};

            // we have 4 neighbors per tile
            // could be more but its 4 for now
            // right/left/down/up
            var neighbors = new TileID[4];

            for (int i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = TileID.Air;
            }

            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetTile(x + 1, y);
                neighbors[(int) Neighbor.Right] = neighborTile.BackTileID;
            }

            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x - 1, y);
                neighbors[(int) Neighbor.Left] = neighborTile.BackTileID;
            }

            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y + 1);
                neighbors[(int) Neighbor.Up] = neighborTile.BackTileID;
            }

            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y - 1);
                neighbors[(int) Neighbor.Down] = neighborTile.BackTileID;
            }


            var tilePosition = GetTilePosition(neighbors, tile.BackTileID);

            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.BackTileSpriteID = property.BaseSpriteId + tilePositionToTileSet[(int) tilePosition];
        }




        public static void UpdateMidSprite(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetTile(x, y);
            ref var property = ref GameState.TileCreationApi.GetTileProperty(tile.MidTileID);
            
            // standard sheet mapping
            // every tile has a constant offset
            // in the sprite atlas

            // example: 15 is (3,3)
            //           8 is (0,2)
            //           1 is (1,0)
            int[] tilePositionToTileSet = {15, 12, 14, 13, 3, 0, 2, 1, 11, 8, 10, 9, 7, 4, 6, 5};

            // we have 4 neighbors per tile
            // could be more but its 4 for now
            // right/left/down/up
            var neighbors = new TileID[4];

            for (int i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = TileID.Air;
            }

            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetTile(x + 1, y);
                neighbors[(int) Neighbor.Right] = neighborTile.MidTileID;
            }

            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x - 1, y);
                neighbors[(int) Neighbor.Left] = neighborTile.MidTileID;
            }

            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y + 1);
                neighbors[(int) Neighbor.Up] = neighborTile.MidTileID;
            }

            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y - 1);
                neighbors[(int) Neighbor.Down] = neighborTile.MidTileID;
            }


            var tilePosition = GetTilePosition(neighbors, tile.MidTileID);

            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.MidTileSpriteID = property.BaseSpriteId + tilePositionToTileSet[(int) tilePosition];
        }



        public static void UpdateFrontSprite(int x, int y, TileMap tileMap)
        {
            ref var tile = ref tileMap.GetTile(x, y);
            ref var property = ref GameState.TileCreationApi.GetTileProperty(tile.MidTileID);
            
            // standard sheet mapping
            // every tile has a constant offset
            // in the sprite atlas

            // example: 15 is (3,3)
            //           8 is (0,2)
            //           1 is (1,0)
            int[] tilePositionToTileSet = {15, 12, 14, 13, 3, 0, 2, 1, 11, 8, 10, 9, 7, 4, 6, 5};

            // we have 4 neighbors per tile
            // could be more but its 4 for now
            // right/left/down/up
            var neighbors = new TileID[4];

            for (int i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = TileID.Air;
            }

            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetTile(x + 1, y);
                neighbors[(int) Neighbor.Right] = neighborTile.FrontTileID;
            }

            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x - 1, y);
                neighbors[(int) Neighbor.Left] = neighborTile.FrontTileID;
            }

            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y + 1);
                neighbors[(int) Neighbor.Up] = neighborTile.FrontTileID;
            }

            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y - 1);
                neighbors[(int) Neighbor.Down] = neighborTile.FrontTileID;
            }


            var tilePosition = GetTilePosition(neighbors, tile.FrontTileID);

            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.FrontTileSpriteID = property.BaseSpriteId + tilePositionToTileSet[(int) tilePosition];
        }
    }
}