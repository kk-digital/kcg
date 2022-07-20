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
                    var targetMovable = closestPlayer.physicsMovable;

                    var pos = entity.physicsPosition2D;
                    var movable = entity.physicsMovable;

                    Vec2f direction = targetPos.Value - pos.Value;

                    float Len = direction.Magnitude;
                    direction.Y = 0;
                    direction.Normalize();


                    // enemy bumps into the player and takes damage
                    // and gets pushed back in the opposite direction
                    // a floating text with the amount of damage dealt on it
                    // will be spawned at that position
                    if (entity.hasAgentStats && Len <= 0.6f && !targetMovable.Invulnerable)
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


                    // if the enemy is close to the player
                    // the enemy will move towards the player
                    if (Len <= entity.agentEnemy.DetectionRadius && Len >= 0.5f)
                    {
                        // if the enemy is stuck
                        // trigger the jump
                        bool jump = Math.Abs(movable.Acceleration.X) <= 0.01f && 
                                        movable.Acceleration.Y <= 0.01f && 
                                        movable.Acceleration.Y >= -0.01f;

                        // to move the enemy we have to add acceleration 
                        // towards the player
                        movable.Acceleration = direction * movable.Speed * 25.0f;

                        // jumping is just an increase in velocity
                        if (jump)
                        {
                            movable.Acceleration.Y = 0.0f;
                            movable.Velocity.Y = 7.5f;
                        }

                    }
                    else
                    {
                        //Idle
                        movable.Acceleration = new Vec2f();
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
