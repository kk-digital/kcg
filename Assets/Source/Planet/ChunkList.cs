using System;
using System.Linq;
using System.Runtime.CompilerServices;
using KMath;

namespace Planet
{
    public struct ChunkList
    {
        private Chunk[][] data;
        public int ChunkCount => data.Sum(yChunks => yChunks.Length);

        public ChunkList(Vec2i mapSize)
        {
            // xCount & 0x0f == xCount AND 15
            // (>> 4) == (/ 16)

            var xCount = mapSize.X >> 4;
            if ((mapSize.X & 0x0f) != 0)
                xCount++;
            
            var yCount = mapSize.Y >> 4;
            if ((mapSize.Y & 0x0f) != 0)
                yCount++;

            data = new Chunk[yCount][];
            
            for (int y = 0; y < yCount; y++)
            {
                data[y] = Enumerable.Repeat(new Chunk(Enums.Tile.MapChunkType.Empty), xCount).ToArray();
            }
        }

        public int GetXAxisChunksCount(int y)
        {
            return data[y].Length;
        }

        public int GetYAxisChunksCount(int x)
        {
            var count = 0;

            for (int y = 0; y < data.Length; y++)
            {
                if (GetXAxisChunksCount(y) > x)
                {
                    count++;
                }
            }

            return count;
        }

        [MethodImpl((MethodImplOptions) 256)]
        public Vec2i GetChunkIndex(int x, int y)
        {
            // (>> 4) == (/ 16)
            return new Vec2i(x >> 4, y >> 4);
        }
        
        public ref Chunk GetChunkRef(int tileX, int tileY)
        {
            var chunkIndex = GetChunkIndex(tileX, tileY);

            return ref data[chunkIndex.Y][chunkIndex.X];
        }
        
        public void AddChunkOnX(int y, int count = 1)
        {
            var oldChunkCountOnX = data[y].Length;
            var newChunkCountOnX = oldChunkCountOnX + count;
            
            Array.Resize(ref data[y], newChunkCountOnX);

            for (int x = oldChunkCountOnX; x < newChunkCountOnX; x++)
            {
                data[y][x] = new Chunk(Enums.Tile.MapChunkType.Empty);
            }
        }
    }
}
