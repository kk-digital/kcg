using Entitas;
using UnityEngine;

namespace Components
{
    [Agent]
    public struct Position2DComponent : IComponent
    {
        public Vector2 Position;
        public Vector2 PreviousPosition;
    }
}
