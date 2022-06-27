using UnityEngine;
using Physics;
using KMath;
using System.Collections.Generic;

namespace Projectile
{
    public class ProcessCollisionSystem
    {
        List<GameEntity> ToRemoveList = new List<GameEntity>();
        public void Update(ref Planet.PlanetState planet)
        {
            ToRemoveList.Clear();

            ref PlanetTileMap.TileMap tileMap = ref planet.TileMap;

            // Get Delta Time
            float deltaTime = Time.deltaTime;

            // Get Vehicle Physics Entity
            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.PhysicsBox2DCollider, GameMatcher.ProjectilePhysicsState2D));

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

                    }
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, physicsState.angularVelocity))
                {
                    if(entity.projectileCollider.isFirstSolid)
                    {
                        //entity.Destroy();
                        ToRemoveList.Add(entity);

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
                    }
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, physicsState.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        //entity.Destroy();
                        ToRemoveList.Add(entity);
                    }
                }
            }


            foreach(var entity in ToRemoveList)
            {
                planet.RemoveProjectile(entity.projectileID.ID);
            }

        }
    }
}
