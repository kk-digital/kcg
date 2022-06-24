using System.Collections.Generic;
using Entitas;
using KMath;
using UnityEngine;

namespace Particle
{
    public class ParticleEmitterSpawnerSystem
    {
        public ParticleEntity Spawn(ParticleContext context, Material material, Vec2f position, Vec2f size,
                                     int spriteId, int particleEmitterId)
        {
            // use an api to create different emitter entities
            ParticleEntity entity = CreateParticleEmitterEntity(context, null, new Vector2(position.X, position.Y),
             1.0f, new Vector2(0, -20.0f), 1.7f, 0.0f, new int[]{spriteId}, size, 
                new Vector2(1.0f, 10.0f),
            0.0f, 1.0f, new Color(255.0f, 255.0f, 255.0f, 255.0f),
            0.2f, 3.0f, true, 1, 0.05f);

            return entity;
        }

        private ParticleEntity CreateParticleEmitterEntity(ParticleContext context, GameObject emitterPrefab, Vector2 position, float decayRate,
            Vector2 acceleration, float deltaRotation, float deltaScale,
            int[] spriteIds, Vec2f size, Vector2 startingVelocity,
            float startingRotation, float startingScale, Color startingColor,
            float animationSpeed, float duration, bool loop, int particleCount, 
            float timeBetweenEmissions)
        {
            var e = context.CreateEntity();
            e.AddParticleEmitter2dPosition(position, new Vector2(), new Vector2());
            e.AddParticleEmitterState(null, null, decayRate, acceleration, deltaRotation,
            deltaScale, spriteIds, size, startingVelocity, startingRotation, startingScale, startingColor,
            animationSpeed, duration, loop, particleCount, timeBetweenEmissions, 0.0f);

            return e;
        }
    }
}
