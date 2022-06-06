using Enums;
using UnityEngine;

namespace TileMap
{
    public struct Chunk
    {
        public Tile.Component[,] Tiles;

        public ChunkBehaviour Behaviour;

        public int Usage; // Used for sorting chunks by usage
        public int Seq;

        public Chunk(Vector2Int chunkSize)
        {
            Tiles = new Tile.Component[chunkSize.x, chunkSize.y];
            Behaviour = ChunkBehaviour.Unexplored;
            Usage = 0;
            Seq = 0;
        }
    }
}
