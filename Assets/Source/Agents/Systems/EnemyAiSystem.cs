using System;
using UnityEngine;
using System.Collections.Generic;
using KMath;

namespace Agent
{
    public class EnemyAiSystem
    {
        List<AgentEntity> ToRemoveAgents = new List<AgentEntity>();
        public void Update(ref Planet.PlanetState planetState)
        {
            var players = planetState.EntitasContext.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentPlayer));
            var entities = planetState.EntitasContext.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentEnemy));


            if (players.count > 0)
            {
                AgentEntity closestPlayer = players.GetEntities()[0];

                foreach (var entity in entities)
                {
                    var targetPos = closestPlayer.physicsPosition2D;
                    var pos = entity.physicsPosition2D;
                    var movable = entity.physicsMovable;

                    Vec2f direction = targetPos.Value - pos.Value;

                    float Len = direction.Magnitude;
                    direction.Y = 0;
                    direction.Normalize();

                    if (entity.hasAgentStats && Len <= 0.6f)
                    {
                        Vector2 oppositeDirection = new Vector2(-direction.X, -direction.Y);
                        var stats = entity.agentStats;
                        float damage = 20.0f;
                        entity.ReplaceAgentStats(stats.Health - (int)damage, stats.Food, stats.Water, stats.Oxygen,
                            stats.Fuel, stats.AttackCooldown);

                        // spawns a debug floating text for damage 
                        planetState.AddFloatingText("" + damage, 0.5f, new Vec2f(oppositeDirection.x * 0.05f, oppositeDirection.y * 0.05f), new Vec2f(pos.Value.X, pos.Value.Y + 0.35f));

                        // knockback test
                        movable.Acceleration.X += 800.0f * oppositeDirection.x;
                        movable.Velocity.X = 20.0f * oppositeDirection.x;
                    }

                    if (Len <= entity.agentEnemy.DetectionRadius && Len >= 0.5f)
                    {
                        bool jump = Math.Abs(movable.Acceleration.X) <= 0.01f && 
                                        movable.Acceleration.Y <= 0.01f && 
                                        movable.Acceleration.Y >= -0.01f;
                        movable.Acceleration = direction * movable.Speed * 25.0f;
                        if (jump)
                        {
                            movable.Acceleration.Y += 100.0f;
                            movable.Velocity.Y = 5.0f;
                        }

                        entity.ReplacePhysicsMovable(movable.Speed, movable.Velocity, movable.Acceleration);
                    }
                    else
                    {
                        //Idle
                        movable.Acceleration = new Vec2f();
                        entity.ReplacePhysicsMovable(movable.Speed, movable.Velocity, movable.Acceleration);
                    }


                    if (entity.hasAgentStats && entity.agentStats.Health <= 0.0f)
                    {
                        ToRemoveAgents.Add(entity);
                    }
                }
            }

            foreach (var entity in ToRemoveAgents)
            {
                planetState.RemoveAgent(entity.agentID.ID);
            }
            ToRemoveAgents.Clear();
        }
    }
}
