using Entitas;
using UnityEngine;

namespace Item
{
    public class Position2DComponent : IComponent
    {
        public Vector2 Value;
        public Vector2 PreviousValue;
    }
}