using Entitas;

namespace Particle
{

    public struct ParticleEmitterEntity
    {
        public int ParticleEmitterId;
        public bool IsInitialized;
        public ParticleEntity Entity;
    }
}