using Entitas;
using UnityEngine;

namespace Agent
{
    public struct Sprite2DComponent : IComponent
    {
        public int SpriteID;
        public string SpritePath;
        
        public Vector2 Size;
        public Vector2Int PngSize;
        public Material Material;
        
        public Mesh Mesh;
    }
}
