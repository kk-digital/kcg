using Entitas;
using UnityEngine;

namespace Agent
{
    public struct Position2DComponent : IComponent
    {
        public Vector2 Value;
        public Vector2 PreviousValue;
    }
}
