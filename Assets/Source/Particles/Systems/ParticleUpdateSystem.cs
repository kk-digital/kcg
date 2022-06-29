using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Particle
{
     public class ParticleUpdateSystem
    {
        List<ParticleEntity> ToDestroy = new List<ParticleEntity>();


        public void Update(Planet.PlanetState planetState, ParticleContext particleContext)
        {
            ToDestroy.Clear();

            float deltaTime = Time.deltaTime;
            IGroup<ParticleEntity> entities = particleContext.GetGroup(ParticleMatcher.ParticleState);
            foreach (var gameEntity in entities)
            {

                if (gameEntity.hasParticleAnimation)
                {
                    var animation = gameEntity.particleAnimation;
                    animation.State.Update(deltaTime, animation.AnimationSpeed);

                    gameEntity.ReplaceParticleAnimation(animation.AnimationSpeed, animation.State);
                }

                var state = gameEntity.particleState;

                float newHealth = state.Health - state.DecayRate * deltaTime;
                gameEntity.ReplaceParticleState(newHealth, state.DecayRate, state.DeltaRotation, state.DeltaScale);

                var pos = gameEntity.particlePosition2D;
                Vector2 displacement = 
                        0.5f * pos.Acceleration * (deltaTime * deltaTime) + pos.Velocity * deltaTime;
                Vector2 newVelocity = pos.Acceleration * deltaTime + pos.Velocity;

                Vector2 newPosition = pos.Position + displacement;

                float newRotation = pos.Rotation + state.DeltaRotation * deltaTime;
                
                gameEntity.ReplaceParticlePosition2D(newPosition, pos.Acceleration, newVelocity, newRotation);

                /*state.GameObject.transform.position = new Vector3(newPosition.x, newPosition.y, 0.0f);
                state.GameObject.transform.Rotate(0.0f, 0.0f, state.DeltaRotation, Space.Self);*/
                
                if (newHealth <= 0)
                {
                    ToDestroy.Add(gameEntity);
                }
            }

            foreach(var gameEntity in ToDestroy)
            {
                //Object.Destroy(gameEntity.particleState.GameObject);
                //gameEntity.Destroy();
                planetState.RemoveParticle(gameEntity.particleID.ID);
            }
        }
    }
}


