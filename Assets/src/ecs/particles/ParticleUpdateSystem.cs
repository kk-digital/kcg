using Components;
using System.Numerics;
using System.Drawing;

namespace Systems
{
    public class ParticleUpdateSystem
    {

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

                    ParticlesUpdated++;
                    if (ParticlesUpdated >= particleList.Size)
                    {
                        break;
                    }
                }
            }

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

            for (int particleId = 0; particleId < particleList.Capacity; particleId++)
            {
                ref ParticleStateComponent StateComponent = ref particleList.StateList[particleId];

                

                if (StateComponent.State == ParticleState.Running)
                {

                    ParticlesUpdated++;
                    if (ParticlesUpdated >= particleList.Size)
                    {
                        break;
                    }
                }
            }

            for (int particleId = 0; particleId < particleList.Capacity; particleId++)
            {
                ref ParticleStateComponent StateComponent = ref particleList.StateList[particleId];

                

                if (StateComponent.State == ParticleState.Running)
                {

                    ParticlesUpdated++;
                    if (ParticlesUpdated >= particleList.Size)
                    {
                        break;
                    }
                }
            }

            for (int particleId = 0; particleId < particleList.Capacity; particleId++)
            {
                ref ParticleStateComponent StateComponent = ref particleList.StateList[particleId];

                

                if (StateComponent.State == ParticleState.Running)
                {

                    ParticlesUpdated++;
                    if (ParticlesUpdated >= particleList.Size)
                    {
                        break;
                    }
                }
            }
        }

        //TODO(Mahdi): update the animation component
        public Particle2dRotationComponent[] RotationList;
        public Particle2dScaleComponent[] ScaleList;
        public Particle2dSpriteComponent[] SpriteList;
        public Particle2dAnimationComponent[] AnimationList;

            
    }
}


