using Entitas;
using KMath;
using UnityEngine;

namespace Agent
{
    public struct PositionDiscrete2DComponent : IComponent
    {
        public Vec2i Value;
        public Vec2i PreviousValue;
    }
}
