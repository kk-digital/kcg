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
        
        /// <summary>
        /// Count of existing chunks
        /// </summary>
        public int Count { get; private set; }

        public ref Chunk this[int tileX, int tileY]
        {
            get
            {
                var chunkIndex = GetChunkIndex(tileX, tileY);

                if (chunkList[chunkIndex].Type == MapChunkType.Error)
                {
                    AddChunk(chunkIndex);
                }
            
                return ref chunkList[chunkIndex];
            }
        }
        
        public ChunkList(Vec2i mapSize)
        {
            // xCount & 0x0f == xCount AND 15
            // (>> 4) == (/ 16)
            
            mapSizeX = mapSize.X;
            Count = 0;
            capacity = 4096;

            chunkList = Enumerable.Repeat(new Chunk(MapChunkType.Error), capacity).ToArray();
        }
        
        [MethodImpl((MethodImplOptions) 256)]
        // (>> 4) == (/ 16)
        // (>> 8) == (/ 256)
        public int GetChunkIndex(int x, int y) => ((x >> 4) + y * mapSizeX) >> 8;

        public void AddChunk(int chunkIndex)
        {
            chunkList[chunkIndex].Init(MapChunkType.Empty);
            Count++;
        }

        private void IncreaseChunkCapacity()
        {
            var newCapacity = capacity + 4096;
            
            Array.Resize(ref chunkList, newCapacity);

            for (int i = capacity; i < newCapacity; i++)
            {
                chunkList[i] = new Chunk(MapChunkType.Error);
            }
        }
    }
}
