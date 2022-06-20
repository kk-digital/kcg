using System.Linq;
using Enums.Tile;
using KMath;

namespace Planet
{
    public struct Chunk
    {
        public MapChunkType Type;
        public Tile.Tile[][] Tiles;
        
        public int Seq;

        public Chunk(MapChunkType type) : this()
        {
            if (type is MapChunkType.Error) return;
            Type = type;

            Tiles = new Tile.Tile[Layers.Count][];

            for (int i = 0; i < Layers.Count; i++)
            {
                Tiles[i] = Enumerable.Repeat(Tile.Tile.Empty, 16 * 16).ToArray();
            }
        }
    }
}
