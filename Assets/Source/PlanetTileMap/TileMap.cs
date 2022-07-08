

using System;
using Enums.Tile;
using KMath;
using UnityEngine;
using Utility;
using System.Collections.Generic;

namespace PlanetTileMap
{
    public struct TileMap
    {
        public static Tile AirTile = new() {ID = TileID.Air, SpriteID = -1};
        public static readonly int LayerCount = Enum.GetNames(typeof(MapLayerType)).Length;
        
        public bool[] NeedsUpdate;
        
        public Vec2i MapSize;
        public Vec2i ChunkSize;
        List<UpdateTile> ToUpdateTiles;
        
        //Array that maps to Chunk List
        public int[] ChunkIndexLookup;
        //Store Chunks
        public Chunk[] ChunkArray;
        public int ChunkArrayLength;
        public int ChunkArrayCapacity;

        public TileMap(Vec2i mapSize)
        {
            ChunkArrayLength = 0;
            ToUpdateTiles = new List<UpdateTile>();

            ChunkSize = new Vec2i(mapSize.X / 16 + 1, mapSize.Y / 16 + 1);
            
            ChunkArrayCapacity = ChunkSize.X * ChunkSize.Y;
            ChunkIndexLookup = new int[ChunkArrayCapacity];
            ChunkArray = new Chunk[ChunkArrayCapacity];

            // Initialize all chunks. They all be empty
            for (int chunkIndex = 0; chunkIndex < ChunkArray.Length; chunkIndex++)
            {
                ChunkArray[chunkIndex].Type = MapChunkType.Empty;
                ChunkArray[chunkIndex].TileArray = new Tile[LayerCount][];

                var layerLength = ChunkArray[chunkIndex].TileArray.Length;

                // For each layer...
                for (int layerIndex = 0; layerIndex < layerLength; layerIndex++)
                {
                    // ... create new tile array and...
                    ref var layer = ref ChunkArray[chunkIndex].TileArray[layerIndex];
                    layer = new Tile[256];
                    // ... for each tile in layer of tile array...
                    for (int tileIndex = 0; tileIndex < layer.Length; tileIndex++)
                    {
                        // ... set tile to Air
                        layer[tileIndex].ID = TileID.Air;
                        layer[tileIndex].SpriteID = -1;
                    }
                }
            
                ChunkArrayLength++;
            }

            MapSize = mapSize;
            
            NeedsUpdate = new bool[LayerCount];

            for(int layerIndex = 0; layerIndex < LayerCount; layerIndex++)
            {
                NeedsUpdate[layerIndex] = true;
            }
        }
        
        /// <summary>
        /// Checks if position is inside Map Size
        /// </summary>
        /// <param name="x">TileMap coordinates</param>
        /// <param name="y">TileMap coordinates</param>
        public bool IsValid(int x, int y)
        {
            return x >= 0 && x < MapSize.X &&
                   y >= 0 && y < MapSize.Y;
        }
        
        #region Tile getters
        
        public void SetTile(int x, int y, Enums.Tile.TileID tileId, MapLayerType layer)
        {
            ref Tile tile = ref GetTile(x, y, layer);
            tile.ID = tileId;
            ToUpdateTiles.Add(new UpdateTile(new Vec2i(x, y), layer));
            //UpdateTile(x, y, layer);
        }

        public ref Tile GetTile(int x, int y, MapLayerType planetLayer)
        {
            Utils.Assert(x >= 0 && x < MapSize.X &&
                         y >= 0 && y < MapSize.Y);
            
            var xChunkIndex = x / 16;
            var yChunkIndex = ((y / 16) * ChunkSize.X);
            var chunkIndex = (xChunkIndex + yChunkIndex);

            ref var chunk = ref ChunkArray[chunkIndex];
            
            if (chunk.Type == MapChunkType.Error)
            {
                return ref AirTile;
            }
            
            var xIndex = x & 0x0f;
            var yIndex = y & 0x0f;
            var tileIndex = xIndex + (yIndex << 4);
            
            chunk.ReadCount++;
            
            return ref chunk.TileArray[(int)planetLayer][tileIndex];
        }
        
        public ref Tile GetBackTile(int x, int y)
        {
            return ref GetTile(x, y, MapLayerType.Back);
        }
        public ref Tile GetMidTile(int x, int y)
        {
             return ref GetTile(x, y, MapLayerType.Mid);
        }
        public ref Tile GetFrontTile(int x, int y)
        {
             return ref GetTile(x, y, MapLayerType.Front);
        }

        #endregion

        #region Tile removers

        public void RemoveBackTile(int x, int y)
        {
            ref var backTile = ref GetBackTile(x, y);
            backTile.ID = TileID.Air;
            backTile.SpriteID = -1;
            ToUpdateTiles.Add(new UpdateTile(new Vec2i(x, y), MapLayerType.Back));
            //UpdateBackTile(x, y);
        }
        public void RemoveMidTile(int x, int y)
        {
            ref var midTile = ref GetMidTile(x, y);
            midTile.ID = TileID.Air;
            midTile.SpriteID = -1;
            ToUpdateTiles.Add(new UpdateTile(new Vec2i(x, y), MapLayerType.Mid));
          //  UpdateBackTile(x, y);
        }
        public void RemoveFrontTile(int x, int y)
        {
            ref var frontTile = ref GetFrontTile(x, y);
            frontTile.ID = TileID.Air;
            frontTile.SpriteID = -1;
            ToUpdateTiles.Add(new UpdateTile(new Vec2i(x, y), MapLayerType.Front));
            //UpdateBackTile(x, y);
        }

        #endregion

        #region Tile setters

        public void SetBackTile(int x, int y, TileID tileID)
        {
           SetTile(x, y, tileID, MapLayerType.Back);
        }
        public void SetMidTile(int x, int y, TileID tileID)
        {
            SetTile(x, y, tileID, MapLayerType.Mid);
        }
        public void SetFrontTile(int x, int y, TileID tileID)
        {
            SetTile(x, y, tileID, MapLayerType.Front);
        }

        #endregion
        
        // Update data of tile, update sprites of tile and etc.
        #region Tile updater

        private void UpdateTile(int x, int y, MapLayerType type)
        {
            for(int i = x - 1; i <= x + 1; i++)
            {
                if (!IsValid(i, 0)) continue;
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if (!IsValid(i, j)) continue;
                    UpdateNeighbourTiles(i, j, type);
                }
            }
        }

        public void UpdateTileMapPositions(MapLayerType planetLayer)
        {
            for(int y = 0; y < MapSize.Y; y++)
            {
                for(int x = 0; x < MapSize.X; x++)
                {
                    UpdateNeighbourTiles(x, y, planetLayer);
                }
            }
        }

        #endregion
        
        #region Tile neighbour getter

        // TODO: Refactor
        public int CheckTile(TileID[] neighbors, TilePosition rules, TileID tileId)
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
        // TODO: Refactor
        public TilePosition GetTilePosition(TileID[] neighbors, TileID tileId)
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

        #endregion

        // Update neighbour sprites of tiles
        #region Tile neighbour updater
        
        private void UpdateNeighbourTiles(int x, int y, MapLayerType planetLayer)
        {
            // standard sheet mapping
            // every tile has a constant offset
            // in the sprite atlas

            // example: 15 is (3,3)
            //           8 is (0,2)
            //           1 is (1,0)
            int[] tilePositionToTileSet = {15, 12, 14, 13, 3, 0, 2, 1, 11, 8, 10, 9, 7, 4, 6, 5};

            ref var tile = ref GetTile(x, y, planetLayer);
            
            if (tile.ID != TileID.Error)
            {
                ref var property = ref GameState.TileCreationApi.GetTileProperty(tile.ID);
                if (property.IsAutoMapping)
                {
                    // we have 4 neighbors per tile
                    // could be more but its 4 for now
                    // right/left/down/up
                    var neighbors = new TileID[4];

                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        neighbors[i] = TileID.Air;
                    }

                    if (x + 1 < MapSize.X)
                    {
                        ref var neighborTile = ref GetTile(x + 1, y, planetLayer);
                        neighbors[(int) Neighbor.Right] = neighborTile.ID;
                    }

                    if (x - 1 >= 0)
                    {
                        ref var neighborTile = ref GetTile(x - 1, y, planetLayer);
                        neighbors[(int) Neighbor.Left] = neighborTile.ID;
                    }

                    if (y + 1 < MapSize.Y)
                    {
                        ref var neighborTile = ref GetTile(x, y + 1, planetLayer);
                        neighbors[(int) Neighbor.Up] = neighborTile.ID;
                    }

                    if (y - 1 >= 0)
                    {
                        ref var neighborTile = ref GetTile(x, y - 1, planetLayer);
                        neighbors[(int) Neighbor.Down] = neighborTile.ID;
                    }


                    var tilePosition = GetTilePosition(neighbors, tile.ID);

                    // the sprite ids are next to each other in the sprite atlas
                    // we just have to know which one to draw based on the offset
                    tile.SpriteID = property.BaseSpriteId + tilePositionToTileSet[(int) tilePosition];
                }

                else
                {
                    tile.SpriteID = property.BaseSpriteId;
                }
            }
            else
            {
                tile.SpriteID = -1;
            }

            NeedsUpdate[(int) planetLayer] = true;
        }

        #endregion

        public void UpdateTiles()
        {
            for(int i = 0; i < 128 && i < ToUpdateTiles.Count; i++)
            {
                UpdateTile updateTile = ToUpdateTiles[i];
                UpdateTile(updateTile.Position.X, updateTile.Position.Y, updateTile.Layer);
            }
            ToUpdateTiles.RemoveRange(0, Math.Min(128, ToUpdateTiles.Count));
        }
    }
}