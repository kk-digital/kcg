using System.Collections.Generic;
using Entitas;

namespace src.ecs.Game.Particle.ParticleSpawn
{
    public class GameObjectPositionSystem : ReactiveSystem<GameEntity>
    {
        public GameObjectPositionSystem(Contexts contexts) : base(contexts.game)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.GameObject, GameMatcher.Position));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGameObject && entity.hasPosition;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var gameEntity in entities)
            {
                gameEntity.gameObject.value.transform.position = gameEntity.position.value;
            }
        }
    }
}