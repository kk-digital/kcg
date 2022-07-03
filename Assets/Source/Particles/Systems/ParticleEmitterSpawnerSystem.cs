using System.Collections.Generic;
using Entitas;
using KMath;
using UnityEngine;

namespace Particle
{
    public class ParticleEmitterSpawnerSystem
    {

        ParticleEmitterCreationApi ParticleEmitterCreationApi;
        ParticleCreationApi ParticleCreationApi;
        public ParticleEmitterSpawnerSystem(ParticleEmitterCreationApi particleEmitterCreationApi,
                                            ParticleCreationApi particleCreationApi)
        {
            ParticleEmitterCreationApi = particleEmitterCreationApi;
            ParticleCreationApi = particleCreationApi;
        }

        //Note(Mahdi): Deprecated will be removed later
        public ParticleEntity Spawn(ParticleContext context, Material material, Vec2f position, Vec2f size,
                                     int spriteId, int particleEmitterId)
        {
            // use an api to create different emitter entities
            ParticleEntity entity = CreateParticleEmitterEntity(context, Particle.ParticleEmitterType.OreFountain, 
                                            position,
                                            particleEmitterId);

            return entity;
        }

        public ParticleEntity Spawn(ParticleContext context, Particle.ParticleEmitterType type, 
                                        Vec2f position, int particleEmitterId)
        {
            ParticleEntity entity = CreateParticleEmitterEntity(context, type, position, particleEmitterId);

            return entity;
        }

        private ParticleEntity CreateParticleEmitterEntity(ParticleContext context, 
                                Particle.ParticleEmitterType type, Vec2f position, int particleEmitterId)
        {
            ParticleEmitterProperties emitterProperties = 
                        ParticleEmitterCreationApi.Get((int)type);
            ParticleProperties particleProperties = 
                        ParticleCreationApi.Get((int)emitterProperties.ParticleType);
            var e = context.CreateEntity();
            e.AddParticleEmitterID(particleEmitterId);
            e.AddParticleEmitter2dPosition(new Vector2(position.X, position.Y), new Vector2(), new Vector2());
            e.AddParticleEmitterState(emitterProperties.ParticleType, type, emitterProperties.Duration, 0.0f);

            return e;
        }
    }
}
