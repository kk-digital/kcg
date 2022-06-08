using Enums.Tile;
using UnityEngine;

namespace Planet.TileMap
{
    public class Chunk
    {
        // readonly means const(in runtime) after initialization
        public static readonly Vector2Int Size = new(16, 16);
        public MapChunkType Type;
        public Tile.Model[][] Tiles;
        
        public int Seq;
        
        public Chunk()
        {
            Seq = 0;
            Type = MapChunkType.Explored;
            Tiles = new Tile.Model[Layers.Count][];

            for (int i = 0; i < Layers.Count; i++)
            {
                Tiles[i] = new Tile.Model[Size.x * Size.y];
            }
        }

        public Chunk(MapChunkType type) : this()
        {
            Type = type;
        }
    }
}
