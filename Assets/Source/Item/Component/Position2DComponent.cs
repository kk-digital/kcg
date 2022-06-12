using Entitas;
using UnityEngine;

namespace Item
{
    public struct Position2DComponent : IComponent
    {
        public Vector2 Value;
        public Vector2 PreviousValue;
    }
}