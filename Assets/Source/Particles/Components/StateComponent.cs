using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Particle
{
    [Particle]
    public struct StateComponent : IComponent
    {
        public GameObject GameObject;

        public float Health;
        public float DecayRate;

        public float DeltaRotation;
        public float DeltaScale;
    }
}