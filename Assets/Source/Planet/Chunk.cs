using System.Linq;
using KMath;
using UnityEngine;

namespace Planet
{
    public struct Chunk
    {
        // readonly means const(in runtime) after initialization
        public static readonly Vec2i Size = new(16, 16);
        public Enums.Tile.MapChunkType Type;
        
        public int Seq;

        public Chunk(Enums.Tile.MapChunkType type) : this()
        {
            Seq = 0;
            Type = Enums.Tile.MapChunkType.Explored;
        }
        
        /// <summary>
        /// Getting Tile index by Chunk Dimensions
        /// </summary>
        /// <param name="x">TileMap coordinates</param>
        /// <param name="y">TileMap coordiantes</param>
        /// <returns>Tile index</returns>
        public static int GetTileIndex(int x, int y)
        {
            var chunkX = x & 0x0f;
            var chunkY = (y & 0x0f) << 4;

            return chunkX + chunkY;
        }
    }
}
