using System;
using Enums.Tile;
using KMath;
using UnityEngine;
using Utility;

namespace PlanetTileMap
{
    public struct TileMap
    {
        public static Tile AirTile = new() {ID = TileID.Air, SpriteID = -1};
        public static readonly int LayerCount = Enum.GetNames(typeof(MapLayerType)).Length;
        
        FrameMesh[] LayerMeshes;
        public bool[] NeedsUpdate;
        
        public Vec2i MapSize;
        
        //Array that maps to Chunk List
        public int[] ChunkIndexLookup;
        //Store Chunks
        public Chunk[] ChunkArray;
        public int ChunkArrayLength;
        public int ChunkArrayCapacity;

        public TileMap(Vec2i mapSize)
        {
            ChunkArrayLength = 0;
            
            var chunkSizeX = mapSize.X >> 4;
            var chunkSizeY = mapSize.Y >> 4;

            if ((chunkSizeX & 0x0f) != 0) chunkSizeX++;
            if ((chunkSizeY & 0x0f) != 0) chunkSizeY++;
            
            ChunkArrayCapacity = chunkSizeX + chunkSizeY;
            ChunkIndexLookup = new int[ChunkArrayCapacity];
            ChunkArray = new Chunk[ChunkArrayCapacity];

            MapSize = mapSize;
            
            LayerMeshes = new FrameMesh[LayerCount];
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
        
        private ref Tile GetTile(int x, int y, MapLayerType planetLayer)
        {
            Utils.Assert(x >= 0 && x < MapSize.X &&
                         y >= 0 && y < MapSize.Y);
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
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
            Utils.Assert(x >= 0 && x < MapSize.X &&
                         y >= 0 && y < MapSize.Y);
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
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
            
            return ref chunk.TileArray[(int)MapLayerType.Back][tileIndex];
        }
        public ref Tile GetMidTile(int x, int y)
        {
            Utils.Assert(x >= 0 && x < MapSize.X &&
                         y >= 0 && y < MapSize.Y);
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
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
            
            return ref chunk.TileArray[(int)MapLayerType.Mid][tileIndex];
        }
        public ref Tile GetFrontTile(int x, int y)
        {
            Utils.Assert(x >= 0 && x < MapSize.X &&
                         y >= 0 && y < MapSize.Y);
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
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
            
            return ref chunk.TileArray[(int)MapLayerType.Front][tileIndex];
        }

        #endregion

        #region Tile removers

        public void RemoveBackTile(int x, int y)
        {
            ref var backTile = ref GetBackTile(x, y);
            backTile.ID = TileID.Air;
            backTile.SpriteID = -1;
            UpdateBackTile(x, y);
        }
        public void RemoveMidTile(int x, int y)
        {
            ref var midTile = ref GetBackTile(x, y);
            midTile.ID = TileID.Air;
            midTile.SpriteID = -1;
            UpdateBackTile(x, y);
        }
        public void RemoveFrontTile(int x, int y)
        {
            ref var frontTile = ref GetBackTile(x, y);
            frontTile.ID = TileID.Air;
            frontTile.SpriteID = -1;
            UpdateBackTile(x, y);
        }

        #endregion

        #region Tile setters

        public void SetBackTile(int x, int y, TileID tileID)
        {
            Utils.Assert(IsValid(x, y));

            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = xChunkIndex + yChunkIndex;

            ref var chunk = ref ChunkArray[chunkIndex];
            
            if (chunk.Type == MapChunkType.Error)
            {
                NewEmptyChunk(chunkIndex);
                if (tileID != TileID.Air) chunk.Type = MapChunkType.NotEmpty;
            }

            var xTileIndex = x & 0x0f;
            var yTileIndex = y & 0x0f;
            var tileIndex = xTileIndex + (yTileIndex << 4);

            chunk.TileArray[(int) MapLayerType.Back][tileIndex].ID = tileID;
            chunk.Sequence++;
            UpdateBackTile(x, y);
        }
        public void SetMidTile(int x, int y, TileID tileID)
        {
            Utils.Assert(IsValid(x, y));

            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = xChunkIndex + yChunkIndex;

            ref var chunk = ref ChunkArray[chunkIndex];
            
            if (chunk.Type == MapChunkType.Error)
            {
                NewEmptyChunk(chunkIndex);
                if (tileID != TileID.Air) chunk.Type = MapChunkType.NotEmpty;
            }

            var xTileIndex = x & 0x0f;
            var yTileIndex = y & 0x0f;
            var tileIndex = xTileIndex + (yTileIndex << 4);

            chunk.TileArray[(int) MapLayerType.Mid][tileIndex].ID = tileID;
            chunk.Sequence++;
            UpdateMidTile(x, y);
        }
        public void SetFrontTile(int x, int y, TileID tileID)
        {
            Utils.Assert(IsValid(x, y));

            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = xChunkIndex + yChunkIndex;

            ref var chunk = ref ChunkArray[chunkIndex];
            
            if (chunk.Type == MapChunkType.Error)
            {
                NewEmptyChunk(chunkIndex);
                if (tileID != TileID.Air) chunk.Type = MapChunkType.NotEmpty;
            }

            var xTileIndex = x & 0x0f;
            var yTileIndex = y & 0x0f;
            var tileIndex = xTileIndex + (yTileIndex << 4);

            chunk.TileArray[(int) MapLayerType.Front][tileIndex].ID = tileID;
            chunk.Sequence++;
            UpdateFrontTile(x, y);
        }

        #endregion
        
        // Update data of tile, update sprites of tile and etc.
        #region Tile updater

        private void UpdateBackTile(int x, int y)
        {
            for(int i = x - 1; i <= x + 1; i++)
            {
                if (!IsValid(i, 0)) continue;
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if (!IsValid(i, j)) continue;
                    UpdateNeighbourTiles(i, j, MapLayerType.Back);
                }
            }
        }
        private void UpdateMidTile(int x, int y)
        {
            for(int i = x - 1; i <= x + 1; i++)
            {
                if (!IsValid(i, 0)) continue;
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if (!IsValid(i, j)) continue;
                    UpdateNeighbourTiles(i, j, MapLayerType.Mid);
                }
            }
        }
        private void UpdateFrontTile(int x, int y)
        {
            for(int i = x - 1; i <= x + 1; i++)
            {
                if (!IsValid(i, 0)) continue;
                for(int j = y - 1; j <= y + 1; j++)
                {
                    if (!IsValid(i, j)) continue;
                    UpdateNeighbourTiles(i, j, MapLayerType.Front);
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

        #region Chunks

        public int NewEmptyChunk(int chunkArrayIndex) 
        {
            int chunkIndex = chunkArrayIndex;
            ChunkArray[chunkIndex].Type = MapChunkType.Empty;
            ChunkArray[chunkIndex].TileArray = new Tile[LayerCount][];

            // For each layer...
            for (int layerIndex = 0; layerIndex < ChunkArray[chunkIndex].TileArray.Length; layerIndex++)
            {
                // ... create new tile array and...
                ChunkArray[chunkIndex].TileArray[layerIndex] = new Tile[256];
                // ... for each tile in tile array...
                for (int tileIndex = 0; tileIndex < ChunkArray[chunkIndex].TileArray[layerIndex].Length; tileIndex++)
                {
                    // ... set tile to Air
                    ChunkArray[chunkIndex].TileArray[layerIndex][tileIndex].ID = TileID.Air;
                    ChunkArray[chunkIndex].TileArray[layerIndex][tileIndex].SpriteID = -1;
                }
            }
            
            ChunkArrayLength++; //increment
            return chunkIndex;
        }

        #endregion
        
        #region Layers

        public void InitializeLayerMesh(Material material, Transform transform, int drawOrder)
        {
            for (int i = 0; i < LayerCount; i++)
            {
                LayerMeshes[i] = new Utility.FrameMesh(("layerMesh" + i), material, transform,
                    GameState.TileSpriteAtlasManager.GetSpriteAtlas(0), drawOrder + i);
            }
        }
        public void DrawLayer(MapLayerType planetLayer)
        {
            Render.DrawFrame(ref LayerMeshes[(int)planetLayer], GameState.TileSpriteAtlasManager.GetSpriteAtlas(0));
        }

        public void UpdateBackLayerMesh()
        {
            LayerMeshes[(int)MapLayerType.Back].Clear();
            int index = 0;
            for (int y = 0; y < MapSize.Y; y++)
            {
                for (int x = 0; x < MapSize.X; x++)
                {
                    ref var tile = ref GetBackTile(x, y);

                    var spriteId = tile.SpriteID;

                    if (spriteId >= 0)
                    {
                        Vector4 textureCoords = GameState.TileSpriteAtlasManager.GetSprite(spriteId).TextureCoords;

                        const float width = 1;
                        const float height = 1;

                        // Update UVs
                        LayerMeshes[(int)MapLayerType.Back].UpdateUV(textureCoords, (index) * 4);
                        // Update Vertices
                        LayerMeshes[(int)MapLayerType.Back].UpdateVertex((index * 4), x, y, width, height);
                        index++;
                    }
                }
            }
        }
        public void UpdateMidLayerMesh()
        {
            LayerMeshes[(int)MapLayerType.Mid].Clear();
            int index = 0;
            for (int y = 0; y < MapSize.Y; y++)
            {
                for (int x = 0; x < MapSize.X; x++)
                {
                    ref var tile = ref GetMidTile(x, y);

                    var spriteId = tile.SpriteID;

                    if (spriteId >= 0)
                    {
                        Vector4 textureCoords = GameState.TileSpriteAtlasManager.GetSprite(spriteId).TextureCoords;

                        const float width = 1;
                        const float height = 1;

                        // Update UVs
                        LayerMeshes[(int)MapLayerType.Mid].UpdateUV(textureCoords, (index) * 4);
                        // Update Vertices
                        LayerMeshes[(int)MapLayerType.Mid].UpdateVertex((index * 4), x, y, width, height);
                        index++;
                    }
                }
            }
        }
        public void UpdateFrontLayerMesh()
        {
            LayerMeshes[(int)MapLayerType.Front].Clear();
            int index = 0;
            for (int y = 0; y < MapSize.Y; y++)
            {
                for (int x = 0; x < MapSize.X; x++)
                {
                    ref var tile = ref GetFrontTile(x, y);

                    var spriteId = tile.SpriteID;

                    if (spriteId >= 0)
                    {
                        Vector4 textureCoords = GameState.TileSpriteAtlasManager.GetSprite(spriteId).TextureCoords;

                        const float width = 1;
                        const float height = 1;

                        // Update UVs
                        LayerMeshes[(int)MapLayerType.Front].UpdateUV(textureCoords, (index) * 4);
                        // Update Vertices
                        LayerMeshes[(int)MapLayerType.Front].UpdateVertex((index * 4), x, y, width, height);
                        index++;
                    }
                }
            }
        }

        #endregion
    }
}
