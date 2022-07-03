using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Particle
{

    [Particle]
    public class Emitter2dPositionComponent : IComponent
    {
        public Vector2 Position;
        public Vector2 Acceleration;
        public Vector2 Velocity;
    }
}