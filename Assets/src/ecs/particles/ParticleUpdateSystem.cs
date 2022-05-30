using Components;
using System.Numerics;
using System.Drawing;

namespace Systems
{
    public class ParticleUpdateSystem
    {

        // we update all components of the particles one component at a time
        // the components are packed in a list so it will be good for the cache
        public void update(ParticleList particleList, float deltaTime)
        {
            int ParticlesUpdated = 0;

            // updating the health component
            for (int particleId = 0; particleId < particleList.Capacity; particleId++)
            {
                ref ParticleStateComponent StateComponent = ref particleList.StateList[particleId];

                if (StateComponent.State == ParticleState.Running)
                {
                    ref Particle2dHealthComponent HealthComponent = ref particleList.HealthList[particleId];
                    HealthComponent.Health -= HealthComponent.DecayRate * deltaTime;

                    if (HealthComponent.Health <= 0)
                    {
                        particleList.RemoveParticle(StateComponent.Id);
                    }

                    ParticlesUpdated++;
                    if (ParticlesUpdated >= particleList.Size)
                    {
                        break;
                    }
                }
            }

            ParticlesUpdated = 0;
            // updating the position component
            for (int particleId = 0; particleId < particleList.Capacity; particleId++)
            {
                ref ParticleStateComponent StateComponent = ref particleList.StateList[particleId];

                if (StateComponent.State == ParticleState.Running)
                {
                    ref Particle2dPositionComponent PositionComponent = ref particleList.PositionList[particleId];
                    Vector2 Displacement = 
                        0.5f * PositionComponent.Acceleration * (deltaTime * deltaTime) + PositionComponent.Velocity * deltaTime;
                    PositionComponent.Velocity = PositionComponent.Acceleration * deltaTime + PositionComponent.Velocity;

                    PositionComponent.Position += Displacement;

                    ParticlesUpdated++;
                    if (ParticlesUpdated >= particleList.Size)
                    {
                        break;
                    }
                }
            }

            ParticlesUpdated = 0;
            for (int particleId = 0; particleId < particleList.Capacity; particleId++)
            {
                ref ParticleStateComponent StateComponent = ref particleList.StateList[particleId];

                if (StateComponent.State == ParticleState.Running)
                {

                    ref Particle2dRotationComponent RotationComponent = ref particleList.RotationList[particleId];

                    RotationComponent.Rotation = RotationComponent.Rotation + RotationComponent.DeltaRotation;
                    ParticlesUpdated++;
                    if (ParticlesUpdated >= particleList.Size)
                    {
                        break;
                    }
                }
            }

            ParticlesUpdated = 0;
            for (int particleId = 0; particleId < particleList.Capacity; particleId++)
            {
                ref ParticleStateComponent StateComponent = ref particleList.StateList[particleId];

                

                if (StateComponent.State == ParticleState.Running)
                {
                    ref Particle2dScaleComponent ScaleComponent = ref particleList.ScaleList[particleId];

                    ScaleComponent.Scale = ScaleComponent.Scale + ScaleComponent.DeltaScale;
                    ParticlesUpdated++;
                    if (ParticlesUpdated >= particleList.Size)
                    {
                        break;
                    }
                }
            }

            ParticlesUpdated = 0;
            //TODO(Mahdi): update the animation component
        }


        
        public void RemoveParticles(ParticleList particleList)
        {
            particleList.RemoveAllParticlesFromQueue();
        }
    }
}


