using System;
using Entitas;
using KMath;

namespace Projectile
{
    [Projectile]
    public struct MovableComponent : IComponent
    {
        public Vec2f Velocity;
        public Vec2f Acceleration;
    }
}

