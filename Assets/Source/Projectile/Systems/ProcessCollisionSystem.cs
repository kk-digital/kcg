using UnityEngine;
using Physics;

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

                // Create Box Borders
                var entityBoxBorders = entity.physicsBox2DCollider.CreateEntityBoxBorders(new Vector2(pos.TempPosition.x, pos.Position.y));

                // If is colliding bottom-top stop y movement
                if (entityBoxBorders.IsCollidingBottom(tileMap, pos.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                    }
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, pos.angularVelocity))
                {
                    if(entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                    }
                }

                pos = entity.projectilePhysicsState2D;
                entityBoxBorders = entity.physicsBox2DCollider.CreateEntityBoxBorders(new Vector2(pos.Position.x, pos.TempPosition.y));

                // If is colliding left-right stop x movement
                if (entityBoxBorders.IsCollidingLeft(tileMap, pos.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                    }
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, pos.angularVelocity))
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

