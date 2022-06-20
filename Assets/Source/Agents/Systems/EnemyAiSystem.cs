using System;
using UnityEngine;
using System.Collections.Generic;
using KMath;

namespace Agent
{
    public class EnemyAiSystem
    {
        List <GameEntity> ToRemoveAgents = new List<GameEntity>();
        public void Update(Planet.PlanetState planetState)
        {
            var players = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentPlayer));
            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentEnemy));


            if (players.count > 0)
            {
                GameEntity closestPlayer = players.GetEntities()[0];
                
                foreach (var entity in entities)
                {
                    var targetPos = closestPlayer.physicsPosition2D;
                    var pos = entity.physicsPosition2D;
                    var movable = entity.physicsMovable;

                    Vector2 direction = targetPos.Value - pos.Value;

                    float Len = direction.magnitude;
                    direction.y = 0;
                    direction.Normalize();

                    if (entity.hasAgentStats && Len <= 0.6f)
                    {
                        Vector2 oppositeDirection = new Vector2(-direction.x, -direction.y);
                        var stats = entity.agentStats;
                        float damage = 20.0f;
                        entity.ReplaceAgentStats(stats.Health - damage, stats.AttackCooldown);

                        // spawns a debug floating text for damage 
                        planetState.AddFloatingText("" + damage, 0.5f, new Vec2f(oppositeDirection.x * 0.05f, oppositeDirection.y * 0.05f), new Vec2f(pos.Value.x, pos.Value.y + 0.35f));

                        // knockback test
                        movable.Acceleration.x += 800.0f * oppositeDirection.x;
                        movable.Velocity.x = 20.0f * oppositeDirection.x;
                    }

                    if (Len <= entity.agentEnemy.DetectionRadius && Len >= 0.5f)
                    {
                        bool jump = Math.Abs(movable.Acceleration.x) <= 0.01f && 
                                        movable.Acceleration.y <= 0.01f && 
                                        movable.Acceleration.y >= -0.01f;
                        movable.Acceleration = direction * movable.Speed * 25.0f;
                        if (jump)
                        {
                            movable.Acceleration.y += 100.0f;
                            movable.Velocity.y = 5.0f;
                        }

                        entity.ReplacePhysicsMovable(movable.Speed, movable.Velocity, movable.Acceleration, movable.AccelerationTime);
                    }
                    else
                    {
                        //Idle
                        movable.Acceleration = new Vector2();
                        entity.ReplacePhysicsMovable(movable.Speed, movable.Velocity, movable.Acceleration, movable.AccelerationTime);
                    }


                    if (entity.hasAgentStats && entity.agentStats.Health <= 0.0f)
                    {
                        ToRemoveAgents.Add(entity);
                    }
                }
            }

            foreach(var entity in ToRemoveAgents)
            {
                planetState.RemoveAgent(entity.agentID.ID);
            }
            ToRemoveAgents.Clear();
        }
    }
}

