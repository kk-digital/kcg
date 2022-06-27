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
        public GameObject GameObject; // used for unity rendering
    }
}
