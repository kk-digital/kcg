using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Systems
{
     public class ParticleUpdateSystem
    {
        List<GameEntity> ToDestroy = new List<GameEntity>();
        Contexts EntitasContext;
        public ParticleUpdateSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public void Execute()
        {
            ToDestroy.Clear();

            float deltaTime = Time.deltaTime;
            IGroup<GameEntity> entities = EntitasContext.game.GetGroup(GameMatcher.Particle2dPosition);
            foreach (var gameEntity in entities)
            {
                var state = gameEntity.particleState;

                float NewHealth = state.Health - state.DecayRate * deltaTime;
                gameEntity.ReplaceParticleState(state.GameObject, NewHealth, state.DecayRate, state.DeltaRotation, state.DeltaScale);

                var pos = gameEntity.particle2dPosition;
                Vector2 Displacement = 
                        0.5f * pos.Acceleration * (deltaTime * deltaTime) + pos.Velocity * deltaTime;
                Vector2 NewVelocity = pos.Acceleration * deltaTime + pos.Velocity;

                Vector2 NewPosition = pos.Position + Displacement;
                gameEntity.ReplaceParticle2dPosition(NewPosition, pos.Acceleration, NewVelocity);

                state.GameObject.transform.position = new Vector3(NewPosition.x, NewPosition.y, 0.0f);
                state.GameObject.transform.Rotate(0.0f, 0.0f, state.DeltaRotation, Space.Self);
                
                if (NewHealth <= 0)
                {
                    ToDestroy.Add(gameEntity);
                }
            }

            foreach(var gameEntity in ToDestroy)
            {
                Object.Destroy(gameEntity.particleState.GameObject);
                gameEntity.Destroy();
            }
        }
    }
}


