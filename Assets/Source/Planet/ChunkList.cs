using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Enums.Tile;
using KMath;

namespace Planet
{
    public struct ChunkList
    {
        private Chunk[] chunkList;
        private readonly int mapSizeX;
        /// <summary>
        /// Chunk Array capacity
        /// </summary>
        private int capacity;

        public ref Chunk this[int tileX, int tileY]
        {
            get
            {
                var chunkIndex = GetChunkIndex(tileX, tileY);
                ref var chunk = ref chunkList[chunkIndex];

                if (chunk.Type == MapChunkType.Error)
                {
                    throw new IndexOutOfRangeException();
                }

                if (Chunk.DebugChunkReadCount)
                {
                    chunk.ReadCount++;
                }

                return ref chunk;
            }
        }
        
        public ChunkList(Vec2i mapSize)
        {
            // xCount & 0x0f == xCount AND 15
            // (>> 4) == (/ 16)
            
            mapSizeX = mapSize.X;
            capacity = 4096;

            chunkList = Enumerable.Repeat(new Chunk(MapChunkType.Error), capacity).ToArray();
            
            // Init first not existed chunk in list
            chunkList[0].Init(MapChunkType.Empty);
        }
        
        [MethodImpl((MethodImplOptions) 256)]
        // (>> 4) == (/ 16)
        // (>> 8) == (/ 256)
        public int GetChunkIndex(int x, int y) => ((x >> 4) + y * mapSizeX) >> 8;

        public void RemoveChunk(int index)
        {
            if(chunkList[index].Type == MapChunkType.Error || index >= capacity) return;
            chunkList[index] = new Chunk(MapChunkType.Error);
        }

        private void IncreaseChunkCapacity()
        {
            var newCapacity = capacity + 4096;
            
            Array.Resize(ref chunkList, newCapacity);

            for (int i = capacity; i < newCapacity; i++)
            {
                chunkList[i] = new Chunk(MapChunkType.Error);
            }

            capacity = newCapacity;
        }
    }
}
