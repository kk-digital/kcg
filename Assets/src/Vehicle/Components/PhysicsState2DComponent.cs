using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Vehicle
{
    public class PhysicsState2DComponent : IComponent
    {
        public Vector2 Position = Vector2.zero;
        public Vector2 TempPosition = Vector2.zero;

        public Vector2 Scale = Vector2.zero;
        public Vector2 TempScale = Vector2.zero;

        public Vector2 Velocity;

        public float angularMass = 1.0f;
        public float angularVelocity = 1.0f;
        public float angularAcceleration = 1.0f;

        public float centerOfGravity = 1.0f;
        public float centerOfRotation = 1.0f;
    }
}

