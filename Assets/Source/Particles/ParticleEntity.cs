using Entitas;

namespace Particle
{

    public struct ParticlesEntity
    {
        public int ParticleId;
        public bool IsInitialized;
        public ParticleEntity Entity;
    }
}