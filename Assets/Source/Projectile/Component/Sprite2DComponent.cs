using Entitas;
using UnityEngine;

namespace Projectile
{
    public struct Sprite2DComponent : IComponent
    {
        public Texture2D Texture;
        public Vector2 Size;
    }
}
