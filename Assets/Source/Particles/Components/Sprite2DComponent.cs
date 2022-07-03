using Entitas;
using KMath;
using UnityEngine;

namespace Particle
{
    [Particle]
    public class Sprite2DComponent : IComponent
    {
        public int SpriteId;
        public Vec2f Size;
    }
}
