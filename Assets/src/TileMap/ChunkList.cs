using System;
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
        
        public PlanetWrapBehavior WrapBehavior;

        public Chunk Error; // todo: fill this with error tiles
        public Chunk Empty;

        public ChunkList(Vector2Int size, PlanetWrapBehavior wrapBehavior) : this()
        {
            Size = size;
            List = Enumerable.Repeat(new Chunk(size), size.x * size.y).ToArray();

            WrapBehavior = wrapBehavior;

            Error = new Chunk
            {
                Behaviour = ChunkBehaviour.Error
            };
            Empty = new Chunk
            {
                Behaviour = ChunkBehaviour.Empty
            };
        }
        
        private Chunk CreateNewChunk(int x, int y)
        {
            Array.Resize(ref List, Next + 1);

            var chunk = new Chunk();

            List[Next] = chunk;
            Next++;
            return chunk;
        }

        public int GetChunkIndex(int x, int y)
        {
            if (WrapBehavior == PlanetWrapBehavior.WrapAround) x %= Size.x;

            var chunkIndex = (x >> 4) * (Size.y >> 4) + (y >> 4);

            return chunkIndex;
        }
        
        public Chunk GetChunk(int x, int y)
        {
            var chunkIndex = GetChunkIndex(x, y);
            var chunk = List[chunkIndex];

            switch (chunk.Behaviour)
            {
                case ChunkBehaviour.Error: return Error;
                case ChunkBehaviour.Empty: return Empty;
                case ChunkBehaviour.Unexplored: return Empty;
                case ChunkBehaviour.Explored:
                {
                    List[chunkIndex].Usage++;
                    return List[chunkIndex];
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetChunk(int x, int y, Tile.Component[,] tiles)
        {
            var chunk = GetChunk(x, y);
            switch (chunk.Behaviour)
            {
                case ChunkBehaviour.Error:
                    return;
                case not ChunkBehaviour.Explored:
                    chunk = CreateNewChunk(x, y);
                    break;
            }

            chunk.Seq++;

            for (int i = 0; i < 16; i++)
            for (int j = 0; j < 16; j++)
                chunk.Tiles[i, j] = tiles[i, j];
        }
        
        public void Swap(int index1, int index2)
        {
            // Swap chunks
            (List[index1], List[index2]) = (List[index2], List[index1]);
        }
        
        // TODO: Move out from here to Utility
        private void QuickSort(int start, int end)
        {
            if (start >= end) return;

            int p = Partition(start, end);
            QuickSort(start, p - 1);
            QuickSort(p + 1, end);
        }

        public void SortChunks()
        {
            // Sort chunks from most used to least used
            if (List == null || List.Length == 0) return;

            QuickSort(0, Next - 1);
        }
        
        private int Partition(int start, int end)
        {
            // Use negative of the usage to have the list sorted from most used to least used without having to reverse afterwards
            int p = -List[start].Usage;

            int count = 0;
            for (int k = start + 1; k <= end; k++)
                if (-List[k].Usage <= p)
                    count++;

            int pi = start + count;
            Swap(pi, start);

            int i = start, j = end;

            while (i < pi && j > pi)
            {
                while (-List[i].Usage <= p) i++;
                while (-List[j].Usage > p) j--;

                if (i < pi && j > pi)
                    Swap(i++, j--);
            }

            return pi;
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

