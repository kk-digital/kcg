using KMath;
using UnityEngine;
using Physics;

namespace Vehicle
{
    public class ProcessCollisionSystem
    {
        public void Update(Planet.TileMap tileMap)
        {
            // Get Delta Time
            float deltaTime = Time.deltaTime;

            // Get Vehicle Physics Entity
            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.PhysicsBox2DCollider, GameMatcher.VehiclePhysicsState2D));

            foreach (var entity in entities)
            {
                // Set Vehicle Physics to variable
                var pos = entity.vehiclePhysicsState2D;

                var size = entity.physicsBox2DCollider.Size;

                // Create Box Borders
                var entityBoxBorders = new AABB(new Vec2f(pos.TempPosition.X, pos.Position.Y), size);

                // If is colliding bottom-top stop y movement
                if (entityBoxBorders.IsCollidingBottom(tileMap, pos.angularVelocity))
                {
                    entity.ReplaceVehiclePhysicsState2D(new Vec2f(pos.Position.X, pos.TempPosition.Y), pos.TempPosition, pos.Scale, pos.TempScale, 
                        new Vec2f(pos.angularVelocity.X, 0.0f), pos.angularMass, pos.angularAcceleration, pos.centerOfGravity, pos.centerOfRotation);
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, pos.angularVelocity))
                {
                    entity.ReplaceVehiclePhysicsState2D(new Vec2f(pos.Position.X, pos.TempPosition.Y), pos.TempPosition, pos.Scale, pos.TempScale,
                      new Vec2f(pos.angularVelocity.X, 0.0f), pos.angularMass, pos.angularAcceleration, pos.centerOfGravity, pos.centerOfRotation);
                }

                pos = entity.vehiclePhysicsState2D;
                size = entity.physicsBox2DCollider.Size;
                entityBoxBorders = new AABB(new Vec2f(pos.Position.X, pos.TempPosition.Y), size);

                // If is colliding left-right stop x movement
                if (entityBoxBorders.IsCollidingLeft(tileMap, pos.angularVelocity))
                {
                    entity.ReplaceVehiclePhysicsState2D(new Vec2f(pos.Position.X, pos.TempPosition.Y), pos.TempPosition, pos.Scale, pos.TempScale,
                      new Vec2f(0.0f, pos.angularVelocity.Y), pos.angularMass, pos.angularAcceleration, pos.centerOfGravity, pos.centerOfRotation);
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, pos.angularVelocity))
                {
                    entity.ReplaceVehiclePhysicsState2D(new Vec2f(pos.Position.X, pos.TempPosition.Y), pos.TempPosition, pos.Scale, pos.TempScale,
                      new Vec2f(0.0f, pos.angularVelocity.Y), pos.angularMass, pos.angularAcceleration, pos.centerOfGravity, pos.centerOfRotation);
                }
            }
        }
    }
}

