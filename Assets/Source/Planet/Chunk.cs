using System.Linq;
using UnityEngine;

namespace Planet
{
    public struct Chunk
    {
        // readonly means const(in runtime) after initialization
        public static readonly Vector2Int Size = new(16, 16);
        public Enums.Tile.MapChunkType Type;
        public Tile.Tile[][] Tiles;
        
        public int Seq;

        public Chunk(Enums.Tile.MapChunkType type) : this()
        {
            Seq = 0;
            Type = Enums.Tile.MapChunkType.Explored;
            Tiles = new Tile.Tile[Layers.Count][];

            for (int i = 0; i < Layers.Count; i++)
            {
                Tiles[i] = Enumerable.Repeat(Tile.Tile.EmptyTile, Size.x * Size.y).ToArray();
            }
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
