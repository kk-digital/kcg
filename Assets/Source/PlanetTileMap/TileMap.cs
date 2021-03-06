

using System;
using Enums.Tile;
using KMath;
using UnityEngine;
using Utility;
using System.Collections.Generic;

namespace PlanetTileMap
{
    public class TileMap
    {
        public static Tile AirTile = new() {MaterialType = TileMaterialType.Air, TileID = -1};
        public static readonly int LayerCount = Enum.GetNames(typeof(MapLayerType)).Length;
        
        public bool[] NeedsUpdate;
        
        public Vec2i MapSize;
        public Vec2i ChunkSize;
        TileSpriteUpdateQueue TileSpriteUpdateQueue;
        
        //Array that maps to Chunk List
        public int[] ChunkIndexLookup;
        //Store Chunks
        public Chunk[] ChunkArray;
        public int ChunkArrayLength;
        public int ChunkArrayCapacity;

        public TileMap(Vec2i mapSize)
        {
            ChunkArrayLength = 0;
            TileSpriteUpdateQueue = new TileSpriteUpdateQueue();

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
                        layer[tileIndex].MaterialType = TileMaterialType.Air;
                        layer[tileIndex].TileID = -1;
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

        public ref Tile GetBackTile(int x, int y)
        {
            Utils.Assert(IsValid(x, y));
            
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
            Utils.Assert(IsValid(x, y));
            
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
            Utils.Assert(IsValid(x, y));
            
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
            backTile.MaterialType = TileMaterialType.Air;
            backTile.TileID = GameResources.LoadingTilePlaceholderTileId;
            TileSpriteUpdateQueue.Add(x, y, MapLayerType.Back);
            //UpdateBackTile(x, y);
        }
        public void RemoveMidTile(int x, int y)
        {
            ref var midTile = ref GetMidTile(x, y);
            midTile.MaterialType = TileMaterialType.Air;
            midTile.TileID = GameResources.LoadingTilePlaceholderTileId;
            TileSpriteUpdateQueue.Add(x, y, MapLayerType.Mid);
          //  UpdateBackTile(x, y);
        }
        public void RemoveFrontTile(int x, int y)
        {
            ref var frontTile = ref GetFrontTile(x, y);
            frontTile.MaterialType = TileMaterialType.Air;
            frontTile.TileID = GameResources.LoadingTilePlaceholderTileId;
            TileSpriteUpdateQueue.Add(x, y, MapLayerType.Front);
            //UpdateBackTile(x, y);
        }

        #endregion

        #region Tile setters

        public void SetBackTile(int x, int y, TileMaterialType materialType)
        {
            Utils.Assert(IsValid(x, y));

            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = xChunkIndex + yChunkIndex;

            ref var chunk = ref ChunkArray[chunkIndex];
            
            if (chunk.Type == MapChunkType.Error)
            {
                if (materialType != TileMaterialType.Air) chunk.Type = MapChunkType.NotEmpty;
            }

            var xTileIndex = x & 0x0f;
            var yTileIndex = y & 0x0f;
            var tileIndex = xTileIndex + (yTileIndex << 4);

            chunk.TileArray[(int) MapLayerType.Back][tileIndex].MaterialType = materialType;
            chunk.TileArray[(int) MapLayerType.Back][tileIndex].TileID = GameResources.LoadingTilePlaceholderTileId;
            chunk.Sequence++;

            TileSpriteUpdateQueue.Add(x, y, MapLayerType.Back);
        }

        public void SetMidTile(int x, int y, TileMaterialType materialType)
        {
            Utils.Assert(IsValid(x, y));

            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = xChunkIndex + yChunkIndex;

            ref var chunk = ref ChunkArray[chunkIndex];
            
            if (chunk.Type == MapChunkType.Error)
            {
                if (materialType != TileMaterialType.Air) chunk.Type = MapChunkType.NotEmpty;
            }

            var xTileIndex = x & 0x0f;
            var yTileIndex = y & 0x0f;
            var tileIndex = xTileIndex + (yTileIndex << 4);

            chunk.TileArray[(int) MapLayerType.Mid][tileIndex].MaterialType = materialType;
            chunk.TileArray[(int) MapLayerType.Mid][tileIndex].TileID = GameResources.LoadingTilePlaceholderTileId;
            chunk.Sequence++;

            TileSpriteUpdateQueue.Add(x, y, MapLayerType.Mid);
        }

        public void SetFrontTile(int x, int y, TileMaterialType materialType)
        {
            Utils.Assert(IsValid(x, y));

            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = xChunkIndex + yChunkIndex;

            ref var chunk = ref ChunkArray[chunkIndex];
            
            if (chunk.Type == MapChunkType.Error)
            {
                if (materialType != TileMaterialType.Air) chunk.Type = MapChunkType.NotEmpty;
            }

            var xTileIndex = x & 0x0f;
            var yTileIndex = y & 0x0f;
            var tileIndex = xTileIndex + (yTileIndex << 4);

            chunk.TileArray[(int) MapLayerType.Front][tileIndex].MaterialType = materialType;
            chunk.TileArray[(int) MapLayerType.Front][tileIndex].TileID = GameResources.LoadingTilePlaceholderTileId;
            chunk.Sequence++;

            TileSpriteUpdateQueue.Add(x, y, MapLayerType.Front);
        }

        #endregion

        // Update neighbour sprites of tiles
        #region Tile neighbour updater

        // updates all the sprite ids in the layer
        public void UpdateBackTileMapPositions(int x, int y)
        {
            TileSpriteUpdate.UpdateBackTileMapPositions(this, x, y);
        }

        // updates all the sprite ids in the layer
        public void UpdateMidTileMapPositions(int x, int y)
        {
            TileSpriteUpdate.UpdateMidTileMapPositions(this, x, y);
        }


        // updates all the sprite ids in the layer
        public void UpdateFrontTileMapPositions(int x, int y)
        {
            TileSpriteUpdate.UpdateFrontTileMapPositions(this, x, y);
        }
        
        // this is called every frame to update a limited number of sprite ids
        // the excess will be pushed to the next frame
        public void UpdateTileSprites()
        {
            TileSpriteUpdateQueue.UpdateTileSprites(this);
        }

        #endregion
    }
}
