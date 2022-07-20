using Entitas;
using UnityEngine;
using KMath;

namespace Item
{
    /// <summary>
    /// Used by pickup actions.
    /// </summary>
    [ItemParticle]
    public class DrawPosition2DComponent : IComponent
    {
        public Vec2f Value;
        public Vec2f PreviousValue;

        public static Vec2f operator +(DrawPosition2DComponent velocity, Vec2f other) => velocity.Value + other;
    }
}
