using Entitas;
using UnityEngine;

namespace Agent
{
    public struct Sprite2DComponent : IComponent
    {
        public Texture2D Texture;
        public Vector2 Size;
    }
}
