using System;
using Entitas;
using KMath;

namespace Projectile
{
    [Projectile]
    public class QuadraticDragComponent : IComponent
    {
        public bool canDrag;
        public float Drag;
    }
}

