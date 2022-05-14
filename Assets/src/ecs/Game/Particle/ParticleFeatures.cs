using src.ecs.Game.Particle.ParticleSpawn;

namespace src.ecs.Game.Particle
{
    public class ParticleFeatures : Feature
    {
        public ParticleFeatures(Contexts contexts)
        {
            Add(new ParticleSpawnSystem(contexts));
            Add(new GameObjectPositionSystem(contexts));
        }
    }
}