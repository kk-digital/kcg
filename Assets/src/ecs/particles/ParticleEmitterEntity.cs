namespace Entity
{
    public struct ParticleEmitter
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
        public Vector2 ParticleStartingDirection;
        public float ParticleStartingRotation;
        public float ParticleStartingScale;
        public Color ParticleStartingColor;
        public float ParticleAnimationSpeed;


        public float Duration;
        public bool Loop;
        public int ParticleCount;
        public float TimeBetweenEmissions;


        public ParticleEmitter()
        {

        }

        // Used to spawn particles off the emitter
        public void update()
        {

            //TODO(Mahdi): use the Duration, Loop, TimeBetweenEmissions
            // to have a smooth emission
            for(int i = 0; i < ParticleCount; i++)
            {
                System.Random random = new System.Random(); 
                int spriteId = (random.Next() % SpriteIds.Length);
                GameState.ParticleList.AddParticle(ParticleDecayRate, Position, ParticleStartingVelocity,
                     ParticleStartingDirection, ParticleStartingRotation, ParticleDeltaRotation,
                     ParticleStartingScale, ParticleDeltaScale, spriteId,
                    ParticleStartingColor, ParticleAnimationSpeed);
            }
        }


        // TODO(Mahdi): Implement loading from file
        void load(string filePath)
        {

        }
    }
}