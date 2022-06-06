using Entitas;
using UnityEngine;

namespace TileMap
{
    public struct NaturalLayer
    {
        public Vector2Int Size;
        public Vector2Int ChunkSize;
        public int Property;

        public NaturalLayer[] List;

        public NaturalLayer(Vector2Int size, Vector2Int chunkSize) : this()
        {
            ChunkSize = chunkSize;
            Size = new Vector2Int(size.x / ChunkSize.x + 1, size.y / ChunkSize.y + 1);
            List = new NaturalLayer[Size.x * Size.y];
        }
        
        public ref NaturalLayer GetNaturalLayerChunk(int x, int y)
        {
            int index = x / ChunkSize.x + (y / ChunkSize.y) * Size.x;

            return ref List[index];
        }
    }
}

