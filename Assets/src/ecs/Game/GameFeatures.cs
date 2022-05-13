using src.ecs.Game.Particle;

namespace src.ecs.Game
{
    public class GameFeatures : Feature
    {
        public GameFeatures(Contexts contexts)
        {
            Add(new ParticleFeatures(contexts));
        }
    }
}