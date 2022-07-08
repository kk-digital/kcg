using System;
using Entitas;
using KMath;

namespace Projectile
{
    [Projectile]
    public class LinearDragComponent : IComponent
    {
        public bool canDrag;
        public float Drag;
        public float CutOff;
    }
}

