using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Particle
{
    public class ParticleEmitterUpdateSystem_old
    {
        List<ParticleEntity> ToDestroy = new List<ParticleEntity>();

        public void Execute(ParticleContext context)
        {
            ToDestroy.Clear();

            float deltaTime = Time.deltaTime;
            IGroup<ParticleEntity> entities = context.GetGroup(ParticleMatcher.ParticleEmitterState);
            foreach (var gameEntity in entities)
            {
                var state = gameEntity.particleEmitterState;
                var position = gameEntity.particleEmitter2dPosition;

                if (state.CurrentTime <= 0.0f)
                {
                    for(int i = 0; i < state.ParticleCount; i++)
                    {
                        System.Random random = new System.Random(); 
                        int spriteId = state.SpriteIds[(random.Next() % state.SpriteIds.Length)];
                        float randomX = (float)random.NextDouble() * 2.0f - 1.0f;

                        var e = context.CreateEntity();
                        var gameObject = Object.Instantiate(state.Prefab);
                        e.AddParticleState(gameObject, 1.0f, state.ParticleDecayRate, state.ParticleDeltaRotation, state.ParticleDeltaScale);
                        e.AddParticlePosition2D(position.Position, state.ParticleAcceleration, new Vector2(state.ParticleStartingVelocity.x + randomX, state.ParticleStartingVelocity.y), 0);
                    }

                    state.CurrentTime = state.TimeBetweenEmissions;
                }
                else
                {
                    state.CurrentTime -= Time.deltaTime;
                }

                gameEntity.ReplaceParticleEmitterState(state.GameObject, state.Prefab, 
                state.ParticleDecayRate, state.ParticleAcceleration, state.ParticleDeltaRotation, state.ParticleDeltaScale,
                state.SpriteIds, state.ParticleSize, state.ParticleStartingVelocity, state.ParticleStartingRotation, state.ParticleStartingScale, 
                state.ParticleStartingColor, state.ParticleAnimationSpeed, state.Duration, state.Loop,
                state.ParticleCount, state.TimeBetweenEmissions, state.CurrentTime);
            }

            foreach(var gameEntity in ToDestroy)
            {
                Object.Destroy(gameEntity.particleState.GameObject);
                gameEntity.Destroy();
            }
        }
    }
}


