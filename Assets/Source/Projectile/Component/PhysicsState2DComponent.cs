using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Projectile
{
    public class PhysicsState2DComponent : IComponent
    {
        public Vector2 Position = Vector2.zero;
        public Vector2 TempPosition = Vector2.zero;

        [Range(-1.0f, 1.0f)]
        public Vector2 angularVelocity = Vector2.zero;

        public float angularMass = 1.0f;
        public float angularAcceleration = 3.0f;

        public float centerOfGravity = 0.0f;
        public Vector2 centerOfRotation = Vector2.one;
    }
}

