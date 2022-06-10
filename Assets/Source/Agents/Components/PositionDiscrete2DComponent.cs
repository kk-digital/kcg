using Entitas;
using UnityEngine;

namespace Agent
{
    public struct PositionDiscrete2DComponent : IComponent
    {
        public Vector2Int Value;
        public Vector2Int PreviousValue;
    }
}
