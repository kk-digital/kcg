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
                var entityBoxBorders = new AABB2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y), entity.projectileSprite2D.Size);

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

                entityBoxBorders = new AABB2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y), entity.projectileSprite2D.Size);

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
                var entityBoxBorders = new AABB2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y), entity.projectileSprite2D.Size);

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

                entityBoxBorders = new AABB2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y), entity.projectileSprite2D.Size);

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


            foreach (var entity in ToRemoveList)
            {
                var pos = entity.projectilePosition2D;
                planet.AddParticleEmitter(pos.Value, Particle.ParticleEmitterType.DustEmitter);
                planet.RemoveProjectile(entity.projectileID.ID);
            }
        }
    }
}
