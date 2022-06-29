using System.Collections.Generic;
using Entitas;
using KMath;
using UnityEngine;

namespace Particle
{
    public class ParticleSpawnerSystem
    {

        ParticleCreationApi ParticleCreationApi;
        public ParticleSpawnerSystem(ParticleCreationApi particleCreationApi)
        {
            ParticleCreationApi = particleCreationApi;
        }

        public ParticleEntity Spawn(ParticleContext context, Particle.ParticleType particleType, 
                                        Vec2f position, Vec2f velocity, int particleId)
        {
            ParticleProperties particleProperties = 
                        ParticleCreationApi.Get((int)particleType);

            var entity = context.CreateEntity();
            entity.AddParticleID(particleId);
            entity.AddParticleState(1.0f, particleProperties.DecayRate, particleProperties.DeltaRotation, particleProperties.DeltaScale);
            entity.AddParticlePosition2D(new Vector2(position.X, position.Y), particleProperties.Acceleration, new Vector2(velocity.X, velocity.Y), 0);
            entity.AddParticleSprite2D(particleProperties.SpriteId, particleProperties.Size);

            if (particleProperties.HasAnimation)
            {
                entity.AddParticleAnimation(1.0f, new Animation.Animation{Type=(int)particleProperties.AnimationType});
            }
            return entity;
        }

    }
}
