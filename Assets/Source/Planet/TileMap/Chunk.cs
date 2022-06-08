using System.Linq;
using Enums.Tile;
using UnityEngine;

namespace Planet.TileMap
{
    public struct Chunk
    {
        // readonly means const(in runtime) after initialization
        public static readonly Vector2Int Size = new(16, 16);
        public MapChunkType Type;
        public Tile.Model[][] Tiles;
        
        public int Seq;

        public Chunk(MapChunkType type) : this()
        {
            Seq = 0;
            Type = MapChunkType.Explored;
            Tiles = new Tile.Model[Layers.Count][];

            for (int i = 0; i < Layers.Count; i++)
            {
                Tiles[i] = Enumerable.Repeat(Tile.Model.EmptyTile, Size.x * Size.y).ToArray();
            }
        }
    }
}
