using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Systems
{
     public class ParticleUpdateSystem : IExecuteSystem
    {
        List<GameEntity> ToDestroy = new List<GameEntity>();
        Contexts Contexts;
        public ParticleUpdateSystem(Contexts contexts)
        {
            Contexts = contexts;
        }

        public void Execute()
        {
            ToDestroy.Clear();

            float deltaTime = 0.016f;
            IGroup<GameEntity> entities = Contexts.game.GetGroup(GameMatcher.Particle2dPosition);
            foreach (var gameEntity in entities)
            {
                var health = gameEntity.particle2dHealth;
                float NewHealth = health.Health - health.DecayRate * deltaTime;
                gameEntity.ReplaceParticle2dHealth(NewHealth, health.DecayRate);
                var pos = gameEntity.particle2dPosition;
                Vector2 Displacement = 
                        0.5f * pos.Acceleration * (deltaTime * deltaTime) + pos.Velocity * deltaTime;
                Vector2 NewVelocity = pos.Acceleration * deltaTime + pos.Velocity;

                Vector2 NewPosition = pos.Position + Displacement;
                gameEntity.ReplaceParticle2dPosition(NewPosition, pos.Acceleration, NewVelocity);
            

                if (NewHealth <= 0)
                {
                    ToDestroy.Add(gameEntity);
                }
            }

            foreach(var gameEntity in ToDestroy)
            {
                gameEntity.Destroy();
            }
        }
    }
}


