using System;
using UnityEngine;

namespace Agent
{
    public class MovableSystem
    {
        public void Update()
        {
            float deltaTime = Time.deltaTime;
            var AgentsWithVelocity = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentMovable, GameMatcher.AgentPosition2D));
            foreach (var entity in AgentsWithVelocity)
            {

                var pos = entity.agentPosition2D;
                var movable = entity.agentMovable;

                movable.Acceleration.y -= 400.0f * deltaTime;

                if (movable.Acceleration.y <= -30.0f)
                {
                    movable.Acceleration.y = -30.0f;
                }

                if (movable.Acceleration.y >= 30.0f)
                {
                    movable.Acceleration.y = 30.0f;
                }

                Vector2 displacement = 
                        0.5f * movable.Acceleration * (deltaTime * deltaTime) + movable.Velocity * deltaTime;
                Vector2 newVelocity = movable.Acceleration * deltaTime + movable.Velocity;
                newVelocity.x *= 0.7f;

                if (newVelocity.y > 5.0f)
                {
                    newVelocity.y = 5.0f;
                }
                if (newVelocity.y < -5.0f)
                {
                    newVelocity.y = -5.0f;
                }


                Vector2 newPosition = pos.Value + displacement;

                entity.ReplaceAgentMovable(entity.agentMovable.Speed, newVelocity, movable.Acceleration, entity.agentMovable.AccelerationTime);
                entity.ReplaceAgentPosition2D(newPosition, pos.Value);


            }
        }
    }
}

