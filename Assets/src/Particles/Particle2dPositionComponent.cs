using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Components
{

    public struct Particle2dPositionComponent : IComponent
    {
        public Vector2 Position;
        public Vector2 Acceleration;
        public Vector2 Velocity;
    }
}