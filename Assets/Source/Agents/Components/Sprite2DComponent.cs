using Entitas;
using KMath;
using UnityEngine;

namespace Agent
{
    public struct Sprite2DComponent : IComponent
    {
        public int SpriteId;
        public Vec2f Size;
    }
}
