using Enums.Tile;
using System;

namespace PlanetTileMap
{


     public partial class TileMapping
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



        public static void UpdateSpriteRule_R1(int x, int y, MapLayerType planetLayer,
                    ref TileMap tileMap)
        {
            ref var tile = ref tileMap.GetTile(x, y, planetLayer);
            ref var property = ref GameState.TileCreationApi.GetTileProperty(tile.ID);
            
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
                ref var neighborTile = ref tileMap.GetTile(x + 1, y, planetLayer);
                neighbors[(int) Neighbor.Right] = neighborTile.ID;
            }

            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x - 1, y, planetLayer);
                neighbors[(int) Neighbor.Left] = neighborTile.ID;
            }

            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y + 1, planetLayer);
                neighbors[(int) Neighbor.Up] = neighborTile.ID;
            }

            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y - 1, planetLayer);
                neighbors[(int) Neighbor.Down] = neighborTile.ID;
            }


            var tilePosition = GetTilePosition(neighbors, tile.ID);

            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.SpriteID = property.BaseSpriteId + tilePositionToTileSet[(int) tilePosition];
        }


        public static void UpdateSpriteRule_R2(int x, int y, MapLayerType planetLayer,
                                    ref TileMap tileMap)
        {
            ref var tile = ref tileMap.GetTile(x, y, planetLayer);
            ref var property = ref GameState.TileCreationApi.GetTileProperty(tile.ID);
            
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
            var neighbors = new TileID[4];

            for (int i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = TileID.Air;
            }

            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetTile(x + 1, y, planetLayer);
                neighbors[(int) Neighbor.Right] = neighborTile.ID;
            }

            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x - 1, y, planetLayer);
                neighbors[(int) Neighbor.Left] = neighborTile.ID;
            }

            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y + 1, planetLayer);
                neighbors[(int) Neighbor.Up] = neighborTile.ID;
            }

            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y - 1, planetLayer);
                neighbors[(int) Neighbor.Down] = neighborTile.ID;
            }


            var tilePosition = GetTilePosition(neighbors, tile.ID);

            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.SpriteID = property.BaseSpriteId + tilePositionToTileSet[(int) tilePosition];
        }

        public static void UpdateSpriteRule_R3(int x, int y, MapLayerType planetLayer,
                                    ref TileMap tileMap)
        {
            ref var tile = ref tileMap.GetTile(x, y, planetLayer);
            ref var property = ref GameState.TileCreationApi.GetTileProperty(tile.ID);

            var neighbors = new TileID[8];

            for (int i = 0; i < neighbors.Length; i++)
            {
                neighbors[i] = TileID.Air;
            }

            if (x + 1 < tileMap.MapSize.X)
            {
                ref var neighborTile = ref tileMap.GetTile(x + 1, y, planetLayer);
                neighbors[(int) Neighbor.Right] = neighborTile.ID;
            }

            if (x - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x - 1, y, planetLayer);
                neighbors[(int) Neighbor.Left] = neighborTile.ID;
            }

            if (y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y + 1, planetLayer);
                neighbors[(int) Neighbor.Up] = neighborTile.ID;
            }

            if (y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x, y - 1, planetLayer);
                neighbors[(int) Neighbor.Down] = neighborTile.ID;
            }

            if (x + 1 < tileMap.MapSize.X && y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetTile(x + 1, y + 1, planetLayer);
                neighbors[(int) Neighbor.UpRight] = neighborTile.ID;
            }

            if (x - 1 >= 0 && y + 1 < tileMap.MapSize.Y)
            {
                ref var neighborTile = ref tileMap.GetTile(x - 1, y + 1, planetLayer);
                neighbors[(int) Neighbor.UpLeft] = neighborTile.ID;
            }

            if (x + 1 < tileMap.MapSize.X && y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x + 1, y - 1, planetLayer);
                neighbors[(int) Neighbor.DownRight] = neighborTile.ID;
            }

            if (x - 1 >= 0 && y - 1 >= 0)
            {
                ref var neighborTile = ref tileMap.GetTile(x - 1, y - 1, planetLayer);
                neighbors[(int) Neighbor.DownLeft] = neighborTile.ID;
            }








            int tilePosition = 36;

            
            if (neighbors[(int) Neighbor.Right] == tile.ID && neighbors[(int) Neighbor.Down] == tile.ID &&
                 neighbors[(int) Neighbor.Left] == tile.ID && neighbors[(int) Neighbor.Up] == tile.ID)
            {
                if (neighbors[(int) Neighbor.UpRight] == tile.ID && neighbors[(int) Neighbor.UpLeft] == tile.ID &&
                    neighbors[(int) Neighbor.DownRight] == tile.ID && neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 12;
                }
                else if (neighbors[(int) Neighbor.UpRight] == tile.ID && neighbors[(int) Neighbor.UpLeft] == tile.ID &&
                        neighbors[(int) Neighbor.DownRight] == tile.ID && neighbors[(int) Neighbor.DownLeft] != tile.ID)
                {
                    tilePosition = 17;
                }
                else if (neighbors[(int) Neighbor.UpRight] == tile.ID && neighbors[(int) Neighbor.UpLeft] == tile.ID &&
                        neighbors[(int) Neighbor.DownRight] != tile.ID && neighbors[(int) Neighbor.DownLeft] != tile.ID)
                {
                    tilePosition = 19;
                }
                else if (neighbors[(int) Neighbor.UpRight] == tile.ID && neighbors[(int) Neighbor.UpLeft] == tile.ID &&
                        neighbors[(int) Neighbor.DownRight] != tile.ID && neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 16;
                }
                else if (neighbors[(int) Neighbor.UpRight] != tile.ID && neighbors[(int) Neighbor.UpLeft] == tile.ID &&
                        neighbors[(int) Neighbor.DownRight] == tile.ID && neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 27;
                }
                else if (neighbors[(int) Neighbor.UpRight] != tile.ID && neighbors[(int) Neighbor.UpLeft] != tile.ID &&
                        neighbors[(int) Neighbor.DownRight] == tile.ID && neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 30;
                }
                else if (neighbors[(int) Neighbor.UpRight] == tile.ID && neighbors[(int) Neighbor.UpLeft] != tile.ID &&
                        neighbors[(int) Neighbor.DownRight] == tile.ID && neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 28;
                }

                else if (neighbors[(int) Neighbor.UpRight] != tile.ID && neighbors[(int) Neighbor.UpLeft] == tile.ID &&
                        neighbors[(int) Neighbor.DownRight] != tile.ID && neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 49;
                }
                else if (neighbors[(int) Neighbor.UpRight] == tile.ID && neighbors[(int) Neighbor.UpLeft] != tile.ID &&
                        neighbors[(int) Neighbor.DownRight] == tile.ID && neighbors[(int) Neighbor.DownLeft] != tile.ID)
                {
                    tilePosition = 50;
                }
                else if (neighbors[(int) Neighbor.UpRight] != tile.ID && neighbors[(int) Neighbor.UpLeft] == tile.ID &&
                        neighbors[(int) Neighbor.DownRight] == tile.ID && neighbors[(int) Neighbor.DownLeft] != tile.ID)
                {
                    tilePosition = 9;
                }
                else if (neighbors[(int) Neighbor.UpRight] == tile.ID && neighbors[(int) Neighbor.UpLeft] != tile.ID &&
                        neighbors[(int) Neighbor.DownRight] != tile.ID && neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 20;
                }
                else if (neighbors[(int) Neighbor.UpRight] != tile.ID && neighbors[(int) Neighbor.UpLeft] != tile.ID &&
                        neighbors[(int) Neighbor.DownRight] != tile.ID && neighbors[(int) Neighbor.DownLeft] != tile.ID)
                {
                    tilePosition = 52;
                }

                else if (neighbors[(int) Neighbor.UpRight] != tile.ID && neighbors[(int) Neighbor.UpLeft] != tile.ID &&
                        neighbors[(int) Neighbor.DownRight] != tile.ID && neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 32;
                }
                else if (neighbors[(int) Neighbor.UpRight] != tile.ID && neighbors[(int) Neighbor.UpLeft] != tile.ID &&
                        neighbors[(int) Neighbor.DownRight] == tile.ID && neighbors[(int) Neighbor.DownLeft] != tile.ID)
                {
                    tilePosition = 31;
                }
                else if (neighbors[(int) Neighbor.UpRight] != tile.ID && neighbors[(int) Neighbor.UpLeft] == tile.ID &&
                        neighbors[(int) Neighbor.DownRight] != tile.ID && neighbors[(int) Neighbor.DownLeft] != tile.ID)
                {
                    tilePosition = 43;
                }
                else if (neighbors[(int) Neighbor.UpRight] == tile.ID && neighbors[(int) Neighbor.UpLeft] != tile.ID &&
                        neighbors[(int) Neighbor.DownRight] != tile.ID && neighbors[(int) Neighbor.DownLeft] != tile.ID)
                {
                    tilePosition = 42;
                }

                
            }


            else if (neighbors[(int) Neighbor.Right] == tile.ID && 
                     neighbors[(int) Neighbor.Left ] == tile.ID &&
                     neighbors[(int) Neighbor.Up   ] != tile.ID &&
                     neighbors[(int) Neighbor.Down ] != tile.ID)
            {
                tilePosition = 34;
            }
            else if (neighbors[(int) Neighbor.Right] != tile.ID && 
                     neighbors[(int) Neighbor.Left ] != tile.ID &&
                     neighbors[(int) Neighbor.Up   ] == tile.ID &&
                     neighbors[(int) Neighbor.Down ] == tile.ID)
            {
                tilePosition = 14;
            }



            else if (neighbors[(int) Neighbor.Right] == tile.ID && 
                     neighbors[(int) Neighbor.Left ] != tile.ID &&
                     neighbors[(int) Neighbor.Up   ] != tile.ID &&
                     neighbors[(int) Neighbor.Down ] != tile.ID)
            {
                tilePosition = 33;
            }
            else if (neighbors[(int) Neighbor.Right] != tile.ID && 
                     neighbors[(int) Neighbor.Left ] == tile.ID &&
                     neighbors[(int) Neighbor.Up   ] != tile.ID &&
                     neighbors[(int) Neighbor.Down ] != tile.ID)
            {
                tilePosition = 35;
            }
            else if (neighbors[(int) Neighbor.Right] != tile.ID && 
                     neighbors[(int) Neighbor.Left ] != tile.ID &&
                     neighbors[(int) Neighbor.Up   ] == tile.ID &&
                     neighbors[(int) Neighbor.Down ] != tile.ID)
            {
                tilePosition = 25;
            }
            else if (neighbors[(int) Neighbor.Right] != tile.ID && 
                     neighbors[(int) Neighbor.Left ] != tile.ID &&
                     neighbors[(int) Neighbor.Up   ] != tile.ID &&
                     neighbors[(int) Neighbor.Down ] == tile.ID)
            {
                tilePosition = 3;
            }



            else if (neighbors[(int) Neighbor.Right] == tile.ID && neighbors[(int) Neighbor.Down] == tile.ID &&
                 neighbors[(int) Neighbor.Left] != tile.ID && neighbors[(int) Neighbor.Up] != tile.ID)
            {
                if (neighbors[(int) Neighbor.DownRight] == tile.ID)
                {
                    tilePosition = 0;
                }
                else
                {
                    tilePosition = 4;
                }
            }
            else if (neighbors[(int) Neighbor.Left] == tile.ID && neighbors[(int) Neighbor.Down] == tile.ID &&
                 neighbors[(int) Neighbor.Right] != tile.ID && neighbors[(int) Neighbor.Up] != tile.ID)
            {
                if (neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 2;
                }
                else
                {
                    tilePosition = 7;
                }
            }
            else if (neighbors[(int) Neighbor.Right] == tile.ID && neighbors[(int) Neighbor.Up] == tile.ID &&
                 neighbors[(int) Neighbor.Left] != tile.ID && neighbors[(int) Neighbor.Down] != tile.ID)
            {
                if (neighbors[(int) Neighbor.UpRight] == tile.ID)
                {
                    tilePosition = 22;
                }
                else
                {
                    tilePosition = 37;
                }
            }
            else if (neighbors[(int) Neighbor.Left] == tile.ID && neighbors[(int) Neighbor.Up] == tile.ID &&
                 neighbors[(int) Neighbor.Right] != tile.ID && neighbors[(int) Neighbor.Down] != tile.ID)
            {
                if (neighbors[(int) Neighbor.UpLeft] == tile.ID)
                {
                    tilePosition = 24;
                }
                else
                {
                    tilePosition = 40;
                }
            }


            else if (neighbors[(int) Neighbor.Right] == tile.ID &&
                                neighbors[(int) Neighbor.Left] != tile.ID)
            {
                if (neighbors[(int) Neighbor.UpRight] == tile.ID && 
                    neighbors[(int) Neighbor.DownRight] == tile.ID)
                {
                    tilePosition = 11;
                }
                else if (neighbors[(int) Neighbor.UpRight] == tile.ID)
                {
                    tilePosition = 15;
                }
                else if (neighbors[(int) Neighbor.DownRight] == tile.ID)
                {
                    tilePosition = 26;
                }
                else 
                {
                    tilePosition = 48;
                }
            }


            else if (neighbors[(int) Neighbor.Left] == tile.ID  &&
                        neighbors[(int) Neighbor.Right] != tile.ID)
            {
                if (neighbors[(int) Neighbor.UpLeft] == tile.ID &&
                    neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 13;
                }
                else if (neighbors[(int) Neighbor.UpLeft] == tile.ID)
                {
                    tilePosition = 18;
                }
                else if (neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 29;
                }
                else
                {
                    tilePosition = 51;
                }
            }

            else if (neighbors[(int) Neighbor.Up] == tile.ID &&
                                neighbors[(int) Neighbor.Down] != tile.ID)
            {
                if (neighbors[(int) Neighbor.UpLeft] == tile.ID &&
                    neighbors[(int) Neighbor.UpRight] == tile.ID)
                {
                    tilePosition = 23;
                }
                else if (neighbors[(int) Neighbor.UpLeft] == tile.ID)
                {
                    tilePosition = 38;
                }
                else if (neighbors[(int) Neighbor.UpRight] == tile.ID)
                {
                    tilePosition = 39;
                }
                else
                {
                    tilePosition = 41;
                }
            }

            else if (neighbors[(int) Neighbor.Down] == tile.ID && 
                            neighbors[(int) Neighbor.Up] != tile.ID)
            {
                 if (neighbors[(int) Neighbor.DownLeft] == tile.ID &&
                    neighbors[(int) Neighbor.DownRight] == tile.ID)
                {
                    tilePosition = 1;
                }
                else if (neighbors[(int) Neighbor.DownLeft] == tile.ID)
                {
                    tilePosition = 5;
                }
                else if (neighbors[(int) Neighbor.DownRight] == tile.ID)
                {
                    tilePosition = 6;
                }
                else
                {
                    tilePosition = 8;
                }
            }


            // the sprite ids are next to each other in the sprite atlas
            // we just have to know which one to draw based on the offset
            tile.SpriteID = property.BaseSpriteId + tilePosition;
        }
    }
}