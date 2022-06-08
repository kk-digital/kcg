using System;
using Enums.Tile;
using UnityEngine;

namespace Planet.TileMap
{
    public class ChunkList
    {
        public Vector2Int MapSize;

        public Chunk[] Data;
        private Chunk errorChunk = new(MapChunkType.Error);
        private Chunk emptyChunk = new(MapChunkType.Empty);

        public ChunkList(Vector2Int mapSize)
        {
            MapSize = mapSize;

            var tileCount = mapSize.x * mapSize.y;
            var chunkCount = (tileCount + (tileCount & 0x0f)) >> 4;
            
            Data = new Chunk[chunkCount];
        }
        
        private int AddChunk(int x, int y)
        {
            var chunkCount = Data.Length;
            
            // I feel like resizing by 1 each time is not very efficient... Change it later?
            Array.Resize(ref Data, chunkCount + 1);

            Data[chunkCount] = new Chunk(MapChunkType.Empty);
            
            // Return Chunk Last Index
            return chunkCount;
        }
        
        private int GetChunkIndex(int x, int y)
        {
            int chunkMulti = Chunk.Size.x * Chunk.Size.y;
            
            return (x * Chunk.Size.x + y * MapSize.x) / chunkMulti;
        }
        
        public ref Chunk GetChunkRef(int x, int y)
        {
            int chunkIndex = GetChunkIndex(x, y);

            if (chunkIndex + 1 > Data.Length || chunkIndex + 1 < 0)
            {
                Debug.Log("Chunk does not exist");
                return ref errorChunk;
            }
            
            return ref Data[chunkIndex];
        }
    }
}
