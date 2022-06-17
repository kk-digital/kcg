using UnityEngine;
using Entitas;

namespace Physics
{
    public struct Box2DColliderComponent : IComponent
    {
        public Vector2 Size;
        public Vector2 Offset;
    }
}
