using System;
using System.Linq;
using System.Runtime.CompilerServices;
using KMath;

namespace Planet
{
    public struct ChunkList
    {
        private Chunk[] data;
        private readonly int mapSizeX;

        public ChunkList(Vec2i mapSize)
        {
            // xCount & 0x0f == xCount AND 15
            // (>> 4) == (/ 16)
            
            mapSizeX = mapSize.X;
            var firstChunkCount = 1;

            data = Enumerable.Repeat(new Chunk(Enums.Tile.MapChunkType.Empty), firstChunkCount).ToArray();
        }
        
        [MethodImpl((MethodImplOptions) 256)]
        // (>> 4) == (/ 16)
        // (>> 8) == (/ 256)
        public int GetChunkIndex(int x, int y) => ((x >> 4) + y * mapSizeX) >> 8;
        public ref Chunk GetChunkRef(int tileX, int tileY) => ref data[GetChunkIndex(tileX, tileY)];
        
        public void AddChunk(int count = 1)
        {
            var oldChunkCountOnX = data.Length;
            var newChunkCountOnX = oldChunkCountOnX + count;
            
            Array.Resize(ref data, newChunkCountOnX);

            for (int i = oldChunkCountOnX; i < newChunkCountOnX; i++)
            {
                data[i] = new Chunk(Enums.Tile.MapChunkType.Empty);
            }
        }
    }
}
