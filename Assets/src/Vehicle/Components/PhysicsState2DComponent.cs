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

        public Vector2 angularVelocity = Vector2.zero;
        public float angularMass = 1.0f;
        public float angularAcceleration = 3.0f;
        
        public float centerOfGravity = 1.0f;
        public Vector2 centerOfRotation = Vector2.one;
    }
}

