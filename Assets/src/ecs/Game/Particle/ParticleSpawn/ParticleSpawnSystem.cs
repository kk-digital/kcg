using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace src.ecs.Game.Particle.ParticleSpawn
{
    public class ParticleSpawnSystem : ReactiveSystem<GameEntity>
    {
        public ParticleSpawnSystem(Contexts contexts) : base(contexts.game)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Particle);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasParticle;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var gameEntity in entities)
            {
                gameEntity.ReplaceGameObject(Object.Instantiate(Resources.Load<GameObject>(gameEntity.particle.resourcePath)));
            }
        }
    }
}