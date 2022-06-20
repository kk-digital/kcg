using Entitas;
using KMath;
using UnityEngine;

namespace Projectile
{
    public struct Sprite2DComponent : IComponent
    {
        public Texture2D Texture;
        public Vec2f Size;
    }
}
