using UnityEngine;
using Physics;
using KMath;

namespace Projectile
{
    public class ProcessCollisionSystem
    {
        public void Update(Planet.TileMap tileMap)
        {
            // Get Delta Time
            float deltaTime = Time.deltaTime;

            // Get Vehicle Physics Entity
            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.PhysicsBox2DCollider, GameMatcher.ProjectilePhysicsState2D));

            foreach (var entity in entities)
            {
                // Set Vehicle Physics to variable
                var pos = entity.projectilePhysicsState2D;

                var size = entity.physicsBox2DCollider.Size;

                // Create Box Borders
                var entityBoxBorders = new AABB2D(new Vec2f(pos.TempPosition.x, pos.Position.y), size);

                // If is colliding bottom-top stop y movement
                if (entityBoxBorders.IsCollidingBottom(tileMap, new Vec2f(pos.angularVelocity.x, pos.angularVelocity.y)))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                    }
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, new Vec2f(pos.angularVelocity.x, pos.angularVelocity.y)))
                {
                    if(entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                    }
                }

                pos = entity.projectilePhysicsState2D;
                size = entity.physicsBox2DCollider.Size;
                entityBoxBorders = new AABB2D(new Vec2f(pos.Position.x, pos.TempPosition.y), size);

                // If is colliding left-right stop x movement
                if (entityBoxBorders.IsCollidingLeft(tileMap, new Vec2f(pos.angularVelocity.x, pos.angularVelocity.y)))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                    }
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, new Vec2f(pos.angularVelocity.x, pos.angularVelocity.y)))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                    }
                }
            }
        }
    }
}

