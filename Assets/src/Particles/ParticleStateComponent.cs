using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Components
{
    public struct ParticleStateComponent : IComponent
    {
        public GameObject GameObject;

        public float Health;
        public float DecayRate;

        public float DeltaRotation;
        public float DeltaScale;
    }
}