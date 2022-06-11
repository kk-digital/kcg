using System;
using UnityEngine;
using System.Collections.Generic;

namespace Agent
{
    public class EnemyAiSystem
    {
        List <GameEntity> ToRemoveAgents = new List<GameEntity>();
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

                    float Len = direction.magnitude;
                    if (entity.hasAgentStats && Len <= 0.6f)
                    {
                        var stats = entity.agentStats;
                        entity.ReplaceAgentStats(stats.Health - 1.0f, stats.AttackCooldown);
                    }

                    if (Len <= entity.agentEnemy.DetectionRadius && Len >= 0.5f)
                    {
                        direction.y = 0;
                        direction.Normalize();

                        bool jump = Math.Abs(movable.Acceleration.x) <= 0.01f;
                        movable.Acceleration = direction * movable.Speed * 25.0f;
                        if (jump)
                        {
                            movable.Acceleration.y += 100.0f;
                            movable.Velocity.y = 5.0f;
                        }

                        entity.ReplaceAgentMovable(movable.Speed, movable.Velocity, movable.Acceleration, movable.AccelerationTime);
                    }
                    else
                    {
                        //Idle
                        movable.Acceleration = new Vector2();
                        entity.ReplaceAgentMovable(movable.Speed, movable.Velocity, movable.Acceleration, movable.AccelerationTime);
                    }


                    if (entity.hasAgentStats && entity.agentStats.Health <= 0.0f)
                    {
                        ToRemoveAgents.Add(entity);
                    }
                }
            }

            foreach(var entity in ToRemoveAgents)
            {
                entity.Destroy();
            }
            ToRemoveAgents.Clear();
        }
    }
}

