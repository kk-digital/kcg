using System;
using System.Linq;
using Enums.Tile;

namespace PlanetTileMap
{
    public struct ChunkList
    {
        public Chunk[] chunkList;
        /// <summary>
        /// Chunk Array capacity
        /// </summary>
        private int capacity;

        public ChunkList(int firstCapacity)
        {
            capacity = firstCapacity;

            chunkList = Enumerable.Repeat(new Chunk(MapChunkType.Error), capacity).ToArray();
        }

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
