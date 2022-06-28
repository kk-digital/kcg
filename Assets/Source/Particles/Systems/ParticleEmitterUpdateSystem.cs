using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Particle
{
    public class ParticleEmitterUpdateSystem
    {
        List<ParticleEntity> ToDestroy = new List<ParticleEntity>();

        public void Update(Planet.PlanetState planetState)
        {
            ToDestroy.Clear();

            ParticleContext context = planetState.ParticleContext;

            float deltaTime = Time.deltaTime;
            IGroup<ParticleEntity> entities = context.GetGroup(ParticleMatcher.ParticleEmitterState);
            foreach (var gameEntity in entities)
            {
                var state = gameEntity.particleEmitterState;
                var position = gameEntity.particleEmitter2dPosition;
                state.Duration -= Time.deltaTime;

                if (state.Duration >= 0)
                {
                    if (state.CurrentTime <= 0.0f)
                    {
                        for(int i = 0; i < state.ParticleCount; i++)
                        {
                            System.Random random = new System.Random(); 
                            int spriteId = state.SpriteIds[(random.Next() % state.SpriteIds.Length)];
                            float randomX = (float)random.NextDouble() * 2.0f - 1.0f;

                            var e = context.CreateEntity();
                            //var gameObject = Object.Instantiate(state.Prefab);
                            e.AddParticleState(null, 1.0f, state.ParticleDecayRate, state.ParticleDeltaRotation, state.ParticleDeltaScale);
                            e.AddParticlePosition2D(position.Position, state.ParticleAcceleration, new Vector2(state.ParticleStartingVelocity.x + randomX, state.ParticleStartingVelocity.y), 0);
                            e.AddParticleSprite2D(state.SpriteIds[0], state.ParticleSize);
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
                else
                {
                    ToDestroy.Add(gameEntity);
                }
            }

            foreach(var entity in ToDestroy)
            {
                planetState.RemoveParticleEmitter(entity.particleEmitterID.ParticleEmitterId);
            }
        }
    }
}


