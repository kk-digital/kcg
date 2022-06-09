using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Particle
{
     public class UpdateSystem
    {
        List<GameEntity> ToDestroy = new List<GameEntity>();
        public void Execute()
        {
            ToDestroy.Clear();

            float deltaTime = Time.deltaTime;
            IGroup<GameEntity> entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.ParticleState);
            foreach (var gameEntity in entities)
            {
                var state = gameEntity.particleState;

                float newHealth = state.Health - state.DecayRate * deltaTime;
                gameEntity.ReplaceParticleState(state.GameObject, newHealth, state.DecayRate, state.DeltaRotation, state.DeltaScale);

                var pos = gameEntity.particlePosition2D;
                Vector2 displacement = 
                        0.5f * pos.Acceleration * (deltaTime * deltaTime) + pos.Velocity * deltaTime;
                Vector2 newVelocity = pos.Acceleration * deltaTime + pos.Velocity;

                Vector2 newPosition = pos.Position + displacement;
                gameEntity.ReplaceParticlePosition2D(newPosition, pos.Acceleration, newVelocity);

                state.GameObject.transform.position = new Vector3(newPosition.x, newPosition.y, 0.0f);
                state.GameObject.transform.Rotate(0.0f, 0.0f, state.DeltaRotation, Space.Self);
                
                if (newHealth <= 0)
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


