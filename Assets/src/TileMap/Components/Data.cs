using Entitas;
using UnityEngine;

namespace TileMap
{
    public struct Data : IComponent
    {
        public ChunkList Chunks;
        public Top Top;
        public Texture2D[] LayerTextures;
    }
}
