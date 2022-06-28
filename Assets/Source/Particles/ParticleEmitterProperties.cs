
using KMath;

namespace Particle
{


    public struct ParticleEmitterProperties
    {
        public int PropertiesId;
        public string Name;
        
        public ParticleType ParticleType;

        public Vec2f VelocityIntervalBegin; // marks the begining of the velocity interval
                                        // we choose a random value for the velocity 
                                        // inside the interval

        public Vec2f VelocityIntervalEnd; // marks the end of the velocity interval
                                        // we choose a random value for the velocity 
                                        // inside the interval

        public float SpawnRadius; // particles spawn randomly inside the given radius
        public float Duration; // particle emitter is removed after the duration is over
        public bool Loop;
        public int ParticleCount; // how many particles are spawned per emission
        public float TimeBetweenEmissions;

        public float CurrentTime; // keep track of the time
    }
}