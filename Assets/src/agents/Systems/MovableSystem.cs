using System;
using UnityEngine;

namespace Agent
{
    public class MovableSystem
    {
        Contexts EntitasContext;

        public MovableSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }


        public void Update(ref List list)
        {
            float deltaTime = Time.deltaTime;
            foreach (var entity in list.AgentsWithVelocity)
            {
                // NOTE(Mahdi): lets try another way to update the agents
                // we can comment this code for now

                /*{
                    var position = entity.agentPosition2D;
                    position.PreviousValue = position.Value;

                    var newVelocity = entity.agentMovable.Velocity;
                    var speed = new Vector2(entity.agentMovable.Speed, entity.agentMovable.Speed) * entity.eCSInputXY.Value;
                    var newAcceleration = Vector2.SmoothDamp(entity.agentMovable.Velocity, speed, ref newVelocity, entity.agentMovable.AccelerationTime, entity.agentMovable.Speed);

                    if (speed.x == 0)
                    {
                        newVelocity.x.CheckPrecision();
                    }
                    if (speed.y == 0)
                    {
                        newVelocity.y.CheckPrecision();
                    }
                    
                    var newPosition = position + newVelocity * Time.fixedDeltaTime;

                    entity.ReplaceAgentMovable(entity.agentMovable.Speed, newVelocity, newAcceleration, entity.agentMovable.AccelerationTime);
                    entity.ReplaceAgentPosition2D(newPosition, position.PreviousValue);
                }*/


                var pos = entity.agentPosition2D;
                var movable = entity.agentMovable;



                Vector2 displacement = 
                        0.5f * movable.Acceleration * (deltaTime * deltaTime) + movable.Velocity * deltaTime;
                Vector2 newVelocity = movable.Acceleration * deltaTime + movable.Velocity;
                newVelocity *= 0.7f;


                Vector2 newPosition = pos.Value + displacement;

                movable.Acceleration = entity.eCSInputXY.Value * entity.agentMovable.Speed * 50.0f;

                entity.ReplaceAgentMovable(entity.agentMovable.Speed, newVelocity, movable.Acceleration, entity.agentMovable.AccelerationTime);
                entity.ReplaceAgentPosition2D(newPosition, pos.Value);


            }
        }
    }
}

