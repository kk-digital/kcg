using Enums.Tile;
using KMath;

namespace PlanetTileMap
{
    public class TileMap
    {
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

            // >> 4 == / 16
            ChunkArrayCapacity = (mapSize.X + mapSize.Y) >> 4;

            // & 0x0F == & 15
            // 17 & 15 = 1
            // 16 & 15 = 0
            if (((mapSize.X + mapSize.Y) & 0x0F) != 0)
            {
                ChunkArrayCapacity++;
            }
            ChunkIndexLookup = new int[ChunkArrayCapacity];
            ChunkArray = new Chunk[ChunkArrayCapacity];

            // Initialize all chunks. They all be empty
            for (int chunkIndex = 0; chunkIndex < ChunkArray.Length; chunkIndex++)
            {
                ChunkArray[chunkIndex].Type = MapChunkType.Empty;
                ChunkArray[chunkIndex].TileArray = new Tile[256];

                var tileArrayLength = ChunkArray[chunkIndex].TileArray.Length;

                // For each tile...
                for (int tileIndex = 0; tileIndex < tileArrayLength; tileIndex++)
                {
                    // ... initialize air tile
                    ref var tile = ref ChunkArray[chunkIndex].TileArray[tileIndex];
                    tile.BackTileID = TileID.Air;
                    tile.BackTileSpriteID = -1;
                    
                    tile.MidTileID = TileID.Air;
                    tile.MidTileSpriteID = -1;
                    
                    tile.FrontTileID = TileID.Air;
                    tile.FrontTileSpriteID = -1;
                }
            
                ChunkArrayLength++;
            }

            MapSize = mapSize;
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

        public ref TileID GetBackTileID(int x, int y)
        {
            Utils.Assert(IsValid(x, y));
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = (xChunkIndex + yChunkIndex);

            ref var chunk = ref ChunkArray[chunkIndex];
            
            Utils.Assert(chunk.Type != MapChunkType.Error);

            var xIndex = x & 0x0f;
            var yIndex = y & 0x0f;
            var tileIndex = xIndex + (yIndex << 4);
            
            chunk.ReadCount++;
            
            return ref chunk.TileArray[tileIndex].BackTileID;
        }
        
        public ref Tile GetTile(int x, int y)
        {
            Utils.Assert(IsValid(x, y));
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = (xChunkIndex + yChunkIndex);

            ref var chunk = ref ChunkArray[chunkIndex];
            
            Utils.Assert(chunk.Type != MapChunkType.Error);

            var xIndex = x & 0x0f;
            var yIndex = y & 0x0f;
            var tileIndex = xIndex + (yIndex << 4);
            
            chunk.ReadCount++;
            
            return ref chunk.TileArray[tileIndex];
        }
        
        public ref TileID GetMidTileID(int x, int y)
        {
            Utils.Assert(IsValid(x, y));
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = (xChunkIndex + yChunkIndex);

            ref var chunk = ref ChunkArray[chunkIndex];
            
            Utils.Assert(chunk.Type != MapChunkType.Error);

            var xIndex = x & 0x0f;
            var yIndex = y & 0x0f;
            var tileIndex = xIndex + (yIndex << 4);
            
            chunk.ReadCount++;
            
            return ref chunk.TileArray[tileIndex].MidTileID;
        }
        
        public ref TileID GetFrontTileID(int x, int y)
        {
            Utils.Assert(IsValid(x, y));
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = (xChunkIndex + yChunkIndex);

            ref var chunk = ref ChunkArray[chunkIndex];
            
            Utils.Assert(chunk.Type != MapChunkType.Error);

            var xIndex = x & 0x0f;
            var yIndex = y & 0x0f;
            var tileIndex = xIndex + (yIndex << 4);
            
            chunk.ReadCount++;
            
            return ref chunk.TileArray[tileIndex].FrontTileID;
        }

        #endregion

        #region Tile removers

        public void RemoveBackTile(int x, int y)
        {
            Utils.Assert(IsValid(x, y));
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = (xChunkIndex + yChunkIndex);

            ref var chunk = ref ChunkArray[chunkIndex];
            
            Utils.Assert(chunk.Type != MapChunkType.Error);

            var xIndex = x & 0x0f;
            var yIndex = y & 0x0f;
            var tileIndex = xIndex + (yIndex << 4);
            
            chunk.ReadCount++;

            chunk.TileArray[tileIndex].BackTileID = TileID.Air;
            chunk.TileArray[tileIndex].BackTileSpriteID = -1;
        }
        public void RemoveMidTile(int x, int y)
        {
            Utils.Assert(IsValid(x, y));
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = (xChunkIndex + yChunkIndex);

            ref var chunk = ref ChunkArray[chunkIndex];
            
            Utils.Assert(chunk.Type != MapChunkType.Error);

            var xIndex = x & 0x0f;
            var yIndex = y & 0x0f;
            var tileIndex = xIndex + (yIndex << 4);
            
            chunk.ReadCount++;

            chunk.TileArray[tileIndex].MidTileID = TileID.Air;
            chunk.TileArray[tileIndex].MidTileSpriteID = -1;
        }
        public void RemoveFrontTile(int x, int y)
        {
            Utils.Assert(IsValid(x, y));
            
            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = (xChunkIndex + yChunkIndex);

            ref var chunk = ref ChunkArray[chunkIndex];
            
            Utils.Assert(chunk.Type != MapChunkType.Error);

            var xIndex = x & 0x0f;
            var yIndex = y & 0x0f;
            var tileIndex = xIndex + (yIndex << 4);
            
            chunk.ReadCount++;

            chunk.TileArray[tileIndex].FrontTileID = TileID.Air;
            chunk.TileArray[tileIndex].FrontTileSpriteID = -1;

            chunk.TileArray[tileIndex].CollisionIsoType1 = TileShapeAndRotation.EmptyBlock;
            chunk.TileArray[tileIndex].CollisionIsoType2 = TileAdjacencyType.EmptyBlock;
        }

        #endregion

        #region Tile setters

        public void SetBackTile(int x, int y, TileID backTileID)
        {
            Utils.Assert(IsValid(x, y));

            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = xChunkIndex + yChunkIndex;

            ref var chunk = ref ChunkArray[chunkIndex];
            Utils.Assert(chunk.Type != MapChunkType.Error);

            var xTileIndex = x & 0x0f;
            var yTileIndex = y & 0x0f;
            var tileIndex = xTileIndex + (yTileIndex << 4);

            chunk.TileArray[tileIndex].BackTileID = backTileID;
            chunk.TileArray[tileIndex].BackTileSpriteID = GameResources.LoadingTilePlaceholderSpriteId;
            chunk.Sequence++;

            TileSpriteUpdateQueue.Add(x, y, MapLayerType.Back);
        }
        public void SetMidTile(int x, int y, TileID midTileID)
        {
            Utils.Assert(IsValid(x, y));

            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = xChunkIndex + yChunkIndex;

            ref var chunk = ref ChunkArray[chunkIndex];
            Utils.Assert(chunk.Type != MapChunkType.Error);

            var xTileIndex = x & 0x0f;
            var yTileIndex = y & 0x0f;
            var tileIndex = xTileIndex + (yTileIndex << 4);

            chunk.TileArray[tileIndex].MidTileID = midTileID;
            chunk.TileArray[tileIndex].MidTileSpriteID = GameResources.LoadingTilePlaceholderSpriteId;
            chunk.Sequence++;

            TileSpriteUpdateQueue.Add(x, y, MapLayerType.Mid);
        }
        public void SetFrontTile(int x, int y, TileID frontTileID)
        {
            Utils.Assert(IsValid(x, y));

            var xChunkIndex = x >> 4;
            var yChunkIndex = ((y >> 4) * MapSize.X) >> 4;
            var chunkIndex = xChunkIndex + yChunkIndex;

            ref var chunk = ref ChunkArray[chunkIndex];
            Utils.Assert(chunk.Type != MapChunkType.Error);
            
            if (frontTileID != TileID.Air)
            {
                chunk.Type = MapChunkType.NotEmpty;
            }

            var xTileIndex = x & 0x0f;
            var yTileIndex = y & 0x0f;
            var tileIndex = xTileIndex + (yTileIndex << 4);

            chunk.TileArray[tileIndex].FrontTileID = frontTileID;
            chunk.TileArray[tileIndex].FrontTileSpriteID = GameResources.LoadingTilePlaceholderSpriteId;
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
