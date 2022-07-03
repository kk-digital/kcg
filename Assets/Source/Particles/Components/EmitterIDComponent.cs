using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Particle
{

    [Particle]
    public class EmitterIDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ParticleEmitterId;
    }
}