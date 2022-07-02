using Entitas;
using KMath;
using UnityEngine;

namespace Agent
{
    [Agent]
    public class PositionDiscrete2DComponent : IComponent
    {
        public Vec2i Value;
        public Vec2i PreviousValue;
    }
}
