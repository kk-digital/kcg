using System.Linq;
using Enums.Tile;
using KMath;

namespace Planet
{
    public struct Chunk
    {
        public static readonly bool DebugChunkReadCount = true;
        
        public MapChunkType Type;
        private Tile.Tile[][] tiles;
        
        public int ReadCount;
        public int Seq;

        public Chunk(MapChunkType type) : this()
        {
            if (type is MapChunkType.Error) return;

            Init(type);
        }
        
        public ref Tile.Tile this[int x, int y] => ref tiles[x][y];

        public void Init(MapChunkType type)
        {
            Type = type;

            tiles = new Tile.Tile[Layers.Count][];

            for (int i = 0; i < Layers.Count; i++)
            {
                // 256 == 0001 0000 0000 == 16 * 16
                tiles[i] = Enumerable.Repeat(Tile.Tile.Empty, 256).ToArray();
            }
        }
    }
}
