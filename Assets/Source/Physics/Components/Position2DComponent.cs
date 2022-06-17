using Entitas;
using UnityEngine;

namespace Physics
{
    public struct Position2DComponent : IComponent
    {
        public Vector2 Value;
        public Vector2 PreviousValue;
        
        public static Vector2 operator +(Position2DComponent velocity, Vector2 other) => velocity.Value + other;
    }
}
