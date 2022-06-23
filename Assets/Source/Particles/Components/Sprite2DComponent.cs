using Entitas;
using KMath;
using UnityEngine;

namespace Particle
{
    [Particle]
    public struct Sprite2DComponent : IComponent
    {
        public int SpriteId;
        public Vec2f Size;
    }
}
