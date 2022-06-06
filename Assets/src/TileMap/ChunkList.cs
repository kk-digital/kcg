using System.Linq;
using Enums;
using UnityEngine;

namespace TileMap
{
    public struct ChunkList
    {
        public Vector2Int Size;
        public Chunk[] List;
        public int Next;

        public Chunk Error; // todo: fill this with error tiles
        public Chunk Empty;

        public ChunkList(Vector2Int size) : this()
        {
            Size = size;
            List = Enumerable.Repeat(new Chunk(size), size.x * size.y).ToArray();

            Error = new Chunk
            {
                Behaviour = ChunkBehaviour.Error
            };
            Empty = new Chunk
            {
                Behaviour = ChunkBehaviour.Empty
            };
        }

        public void MakeAllChunksExplored()
        {
            for (int i = 0; i < List.Length; i++)
            {
                List[i].Behaviour = ChunkBehaviour.Explored;
            }
        }
    }
}

