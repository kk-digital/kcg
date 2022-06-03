using Entitas;
using UnityEngine;

namespace Agent
{
    public struct Sprite2DComponent : IComponent
    {
        public int SpriteID;
        public string SpritePath;
        
        /// <summary>
        /// <p>x = PngSize.x / 32f</p>
        /// <p>y = PngSize.y / 32f</p>
        /// </summary>
        public Vector2 Size;
        public Vector2Int PngSize;
        public Material Material;
        
        public Mesh Mesh;
    }
}
