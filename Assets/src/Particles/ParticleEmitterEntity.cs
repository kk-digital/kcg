using UnityEngine;
using Entitas;
using Entitas.Unity;

namespace Entity
{
    public struct ParticleEmitterEntity
    {
        public Vector2 Position;

        // these properties are used to update
        // the particles
        public float ParticleDecayRate;
        public Vector2 ParticleAcceleration;
        public float ParticleDeltaRotation;
        public float ParticleDeltaScale;


        // we can use a mix of sprites for the particles
        public int[] SpriteIds;

        // the starting properties of the particles
        public Vector2 ParticleStartingVelocity;
        public float ParticleStartingRotation;
        public float ParticleStartingScale;
        public Color ParticleStartingColor;
        public float ParticleAnimationSpeed;


        public float Duration;
        public bool Loop;
        public int ParticleCount;
        public float TimeBetweenEmissions;

        public float CurrentTime;


        public ParticleEmitterEntity(Vector2 position, float decayRate,
            Vector2 acceleration, float deltaRotation, float deltaScale,
            int[] spriteIds, Vector2 startingVelocity,
            float startingRotation, float startingScale, Color startingColor,
            float animationSpeed, float duration, bool loop, int particleCount, 
            float timeBetweenEmissions)
        {
            Position = position;
            ParticleDecayRate = decayRate;
            ParticleAcceleration = acceleration;
            ParticleDeltaRotation = deltaRotation;
            ParticleDeltaScale = deltaScale;
            SpriteIds = spriteIds;
            ParticleStartingVelocity = startingVelocity;
            ParticleStartingRotation = startingRotation;
            ParticleStartingScale = startingScale;
            ParticleStartingColor = startingColor;
            ParticleAnimationSpeed = animationSpeed;
            Duration = duration;
            Loop = loop;
            ParticleCount = particleCount;
            TimeBetweenEmissions = timeBetweenEmissions;
            CurrentTime = TimeBetweenEmissions;
        }

        // Used to spawn particles off the emitter
        public void Update(Contexts contexts, GameObject prefab)
        {

            //TODO(Mahdi): use the Duration, Loop, TimeBetweenEmissions
            // to have a smooth emission

            if (CurrentTime <= 0.0f)
            {
                for(int i = 0; i < ParticleCount; i++)
                {
                    System.Random random = new System.Random(); 
                    int spriteId = (random.Next() % SpriteIds.Length);
                    float randomX = (float)random.NextDouble() * 2.0f - 1.0f;

                    var e = contexts.game.CreateEntity();
                    var gameObject = Object.Instantiate(prefab);
                    e.AddParticleState(gameObject, 1.0f, ParticleDecayRate, ParticleDeltaRotation, ParticleDeltaScale);
                    e.AddParticle2dPosition(Position, ParticleAcceleration, new Vector2(ParticleStartingVelocity.x + randomX, ParticleStartingVelocity.y));
                }

                CurrentTime = TimeBetweenEmissions;
            }
            else
            {
                CurrentTime -= Time.deltaTime;
            }
        }


        // TODO(Mahdi): Implement loading from file
        void load(string filePath)
        {

        }
    }
}