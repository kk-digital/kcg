using Entitas;
using KMath;
using UnityEngine;

namespace ECSInput
{
    [Agent]
    public class XYComponent : IComponent
    {
        public Vec2f Value;
        public bool Jump;
    }
}

