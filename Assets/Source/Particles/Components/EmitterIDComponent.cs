using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Particle
{

    [Particle]
    public struct EmitterIDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ParticleEmitterId;
    }
}