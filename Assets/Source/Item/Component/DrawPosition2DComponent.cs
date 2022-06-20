using Entitas;
using UnityEngine;
using KMath;

namespace Item
{
    // Used by pickup actions.
    public struct DrawPosition2DComponent : IComponent
    {
        public Vec2f Value;
        public Vec2f PreviousValue;

        public static Vec2f operator +(DrawPosition2DComponent velocity, Vec2f other) => velocity.Value + other;
    }
}