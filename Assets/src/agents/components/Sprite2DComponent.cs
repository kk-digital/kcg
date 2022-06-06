using Entitas;
using UnityEngine;

namespace Agent
{
    public struct Sprite2DComponent : IComponent
    {
        public Texture2D Texture;
        
        /// <summary>
        /// <p>x = PngSize.x / 32f</p>
        /// <p>y = PngSize.y / 32f</p>
        /// </summary>
        public Vector2 Size;
    }
}
