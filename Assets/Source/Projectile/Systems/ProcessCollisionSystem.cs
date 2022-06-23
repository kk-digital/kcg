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

                // Create Box Borders
                var entityBoxBorders = new AABB2D(new Vec2f(pos.TempPosition.X, pos.Position.Y), entity.projectileSprite2D.Size);

                // If is colliding bottom-top stop y movement
                if (entityBoxBorders.IsCollidingBottom(tileMap, pos.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                        return;
                    }
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, pos.angularVelocity))
                {
                    if(entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                        return;
                    }
                }

                pos = entity.projectilePhysicsState2D;

                entityBoxBorders = new AABB2D(new Vec2f(pos.Position.X, pos.TempPosition.Y), entity.projectileSprite2D.Size);

                // If is colliding left-right stop x movement
                if (entityBoxBorders.IsCollidingLeft(tileMap, pos.angularVelocity))
                {
                    if (entity.projectileCollider.isFirstSolid)
                    {
                        entity.Destroy();
                        return;
                    }
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, pos.angularVelocity))
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

