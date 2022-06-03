using System;
using UnityEngine;

namespace Agent
{
    public sealed class MovableSystem
    {
        public static readonly MovableSystem Instance;
        private GameContext gameContext;

        static MovableSystem()
        {
            Instance = new MovableSystem();
        }

        private MovableSystem()
        {
            gameContext = Contexts.sharedInstance.game;
        }

        public void CalculatePosition(ref List list)
        {
            foreach (var entity in list.AgentsWithVelocity)
            {
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
            }
        }
    }
}

