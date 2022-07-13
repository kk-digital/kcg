using UnityEngine;
using KMath;
using System.Collections.Generic;
using Collisions;

namespace Projectile
{
    public class ProcessCollisionSystem
    {
        List<ProjectileEntity> ToRemoveList = new List<ProjectileEntity>();
        public void Update(ref PlanetTileMap.TileMap tileMap)
        {
            // Get Delta Time
            float deltaTime = Time.deltaTime;

            // Get Vehicle Physics Entity
            var entities = Contexts.sharedInstance.projectile.GetGroup(ProjectileMatcher.AllOf(ProjectileMatcher.PhysicsBox2DCollider, ProjectileMatcher.ProjectilePhysicsState2D));

            foreach (var entity in entities)
            {
                // Set Vehicle Physics to variable
                var pos = entity.projectilePosition2D;
                var physicsState = entity.projectilePhysicsState2D;

                // Create Box Borders
                var entityBoxBorders = new AABox2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y), entity.projectileSprite2D.Size);

                // If is colliding bottom-top stop y movement
                if (entityBoxBorders.IsCollidingBottom(tileMap, physicsState.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                        return;
                    }
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, physicsState.angularVelocity))
                {
                    if(entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                        return;
                    }
                }

                entityBoxBorders = new AABox2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y), entity.projectileSprite2D.Size);

                // If is colliding left-right stop x movement
                if (entityBoxBorders.IsCollidingLeft(tileMap, physicsState.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                        return;
                    }
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, physicsState.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                        return;
                    }
                }
            }
        }


        // new version of the update function
        // uses the planet state to remove the projectile
        public void UpdateEx(ref Planet.PlanetState planet)
        {
            ToRemoveList.Clear();

            // Get Delta Time
            float deltaTime = Time.deltaTime;
            ref PlanetTileMap.TileMap tileMap = ref planet.TileMap;

            // Get Vehicle Physics Entity
            var entities = planet.EntitasContext.projectile.GetGroup(ProjectileMatcher.AllOf(ProjectileMatcher.PhysicsBox2DCollider, ProjectileMatcher.ProjectilePhysicsState2D));

            foreach (var entity in entities)
            {
                // Set Vehicle Physics to variable
                var pos = entity.projectilePosition2D;
                var physicsState = entity.projectilePhysicsState2D;

                // Create Box Borders
                var entityBoxBorders = new AABox2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y), entity.projectileSprite2D.Size);

                // If is colliding bottom-top stop y movement
                if (entityBoxBorders.IsCollidingBottom(tileMap, physicsState.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        //entity.Destroy();
                        ToRemoveList.Add(entity);
                        continue;
                    }
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, physicsState.angularVelocity))
                {
                    if(entity.projectileCollider.isFirstSolid)
                    {
                        //entity.Destroy();
                        ToRemoveList.Add(entity);
                         continue;
                    }
                }

                entityBoxBorders = new AABox2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y), entity.projectileSprite2D.Size);

                // If is colliding left-right stop x movement
                if (entityBoxBorders.IsCollidingLeft(tileMap, physicsState.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        //entity.Destroy();
                        ToRemoveList.Add(entity);
                         continue;
                    }
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, physicsState.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        //entity.Destroy();
                        ToRemoveList.Add(entity);
                         continue;
                    }
                }
            }

            foreach (var entityP in ToRemoveList)
            {
                if(entityP.projectileType.Type == Enums.ProjectileType.Grenade)
                {
                    planet.AddParticleEmitter(entityP.projectilePosition2D.Value, Particle.ParticleEmitterType.DustEmitter);
                    // Check if projectile has hit a enemy.
                    var entitiesA = planet.EntitasContext.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentID));

                    // Todo: Create a agent colision system?
                    foreach (var entity in entitiesA)
                    {
                        if (!entity.isAgentPlayer)
                        {
                            float dist = Vector2.Distance(new Vector2(entity.physicsPosition2D.Value.X, entity.physicsPosition2D.Value.Y), new Vector2(entityP.projectilePosition2D.Value.X, entityP.projectilePosition2D.Value.Y));

                            float radius = 2.0f;

                            // Note (Mert): This is broken, change it.
                            if (dist < radius)
                            {
                                Vec2f entityPos = entity.physicsPosition2D.Value;
                                Vec2f bulletPos = entityP.projectilePosition2D.Value;
                                Vec2f diff = bulletPos - entityPos;
                                diff.Y = 0;
                                diff.Normalize();

                                Vector2 oppositeDirection = new Vector2(-diff.X, -diff.Y);

                                if (entity.hasAgentStats)
                                {
                                    var stats = entity.agentStats;
                                    entity.ReplaceAgentStats(stats.Health - 25, stats.Food, stats.Water, stats.Oxygen,
                                        stats.Fuel, stats.AttackCooldown);

                                    // spawns a debug floating text for damage 
                                    planet.AddFloatingText("" + 25, 0.5f, new Vec2f(oppositeDirection.x * 0.05f, oppositeDirection.y * 0.05f), new Vec2f(entityPos.X, entityPos.Y + 0.35f));
                                }
                            }
                        }
                    }
                }
                else if (entityP.projectileType.Type == Enums.ProjectileType.Bullet)
                {
                    planet.AddParticleEmitter(entityP.projectilePosition2D.Value, Particle.ParticleEmitterType.DustEmitter);
                }

                planet.RemoveProjectile(entityP.projectileID.ID);
            }
        }
    }
}
