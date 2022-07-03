using Entitas;
using KMath;
using UnityEngine;

namespace Projectile
{
    [Projectile]
    public class Sprite2DComponent : IComponent
    {
        public int SpriteId;
        public Vec2f Size;
    }
}
