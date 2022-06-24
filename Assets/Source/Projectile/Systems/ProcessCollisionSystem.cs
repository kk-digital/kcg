using UnityEngine;
using Physics;
using KMath;

namespace Projectile
{
    public class ProcessCollisionSystem
    {
        public void Update(ref PlanetTileMap.TileMap tileMap)
        {
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
    }
}
