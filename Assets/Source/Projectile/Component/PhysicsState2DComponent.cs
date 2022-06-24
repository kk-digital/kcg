using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using KMath;

namespace Projectile
{
    public class PhysicsState2DComponent : IComponent
    {
        [Range(-1.0f, 1.0f)]
        public Vec2f angularVelocity = Vec2f.Zero;

        public float angularMass = 1.0f;
        public float angularAcceleration = 3.0f;

        public float centerOfGravity = 0.0f;
        public Vec2f centerOfRotation = Vec2f.One;
    }
}
