
using KMath;

namespace Particle
{


    public struct ParticleEmitterProperties
    {
        public int PropertiesId;
        public string Name;
        
        public ParticleType ParticleType;

        public float Duration;
        public bool Loop;
        public int ParticleCount;
        public float TimeBetweenEmissions;

        public float CurrentTime;
    }
}