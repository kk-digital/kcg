using System;
using System.Linq;
using KMath;
using UnityEngine;

namespace Planet
{
    public class ChunkList
    {
        public Vec2i MapSize;

        public Chunk[] Data;
        private Chunk errorChunk = new(Enums.Tile.MapChunkType.Error);
        private Chunk emptyChunk = new(Enums.Tile.MapChunkType.Empty);

        public ChunkList(Vec2i mapSize)
        {
            MapSize = mapSize;

            var tileCount = mapSize.X * mapSize.Y;
            var chunkCount = (tileCount + (tileCount & 0x0f)) >> 4;
            
            Data = Enumerable.Repeat(new Chunk(Enums.Tile.MapChunkType.Empty), chunkCount).ToArray();
        }
        
        private int AddChunk(int x, int y)
        {
            var chunkCount = Data.Length;
            
            // I feel like resizing by 1 each time is not very efficient... Change it later?
            Array.Resize(ref Data, chunkCount + 1);

            Data[chunkCount] = new Chunk(Enums.Tile.MapChunkType.Empty);
            
            // Return Chunk Last Index
            return chunkCount;
        }
        
        public int GetChunkIndex(int x, int y)
        {
            int chunkMulti = Chunk.Size.X * Chunk.Size.Y;
            
            return (x * Chunk.Size.X + y * MapSize.X) / chunkMulti;
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
