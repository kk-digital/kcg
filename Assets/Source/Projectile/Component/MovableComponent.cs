using System;
using Entitas;
using KMath;

namespace Projectile
{
    [Projectile]
    public class MovableComponent : IComponent
    {
        public Vec2f Velocity;
        public Vec2f Acceleration;

        public bool AffectedByGravity;
    }
}

