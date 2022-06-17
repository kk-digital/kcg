using Entitas;
using UnityEngine;

namespace Item
{
    // Used by pickup actions.
    public struct DrawPosition2DComponent : IComponent
    {
        public Vector2 Value;
        public Vector2 PreviousValue;

        public static Vector2 operator +(DrawPosition2DComponent velocity, Vector2 other) => velocity.Value + other;
    }
}