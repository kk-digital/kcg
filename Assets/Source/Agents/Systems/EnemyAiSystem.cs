using System;
using UnityEngine;

namespace Agent
{
    public class EnemyAiSystem
    {
        public void Update()
        {
            var players = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentPlayer));
            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentEnemy));


            if (players.count > 0)
            {
                GameEntity closestPlayer = players.GetEntities()[0];
                
                foreach (var entity in entities)
                {
                    var targetPos = closestPlayer.agentPosition2D;
                    var pos = entity.agentPosition2D;
                    var movable = entity.agentMovable;

                    Vector2 direction = targetPos.Value - pos.Value;
                    direction.y = 0;
                    direction.Normalize();

                    movable.Acceleration = direction * movable.Speed * 25.0f;
                    entity.ReplaceAgentMovable(movable.Speed, movable.Velocity, movable.Acceleration, movable.AccelerationTime);
                }
            }
        }
    }
}

