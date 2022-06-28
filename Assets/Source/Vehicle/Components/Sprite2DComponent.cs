using Entitas;
using KMath;
using UnityEngine;

namespace Vehicle
{
    public struct Sprite2DComponent : IComponent
    {
        public int SpriteId;
        public Vec2f Size;
    }
}
