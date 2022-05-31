using Entitas;
using UnityEngine;

namespace Agent
{
    [Agent]
    sealed public class PositionDiscrete2DComponent : IComponent
    {
        public Vector2Int Value;
    }
}
