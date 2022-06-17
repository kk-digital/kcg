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

                // Create Box Borders
                var entityBoxBorders = entity.physicsBox2DCollider.CreateEntityBoxBorders(new Vector2(pos.TempPosition.x, pos.Position.y));

                // If is colliding bottom-top stop y movement
                if (entityBoxBorders.IsCollidingBottom(tileMap, pos.angularVelocity))
                {
                    entity.ReplaceVehiclePhysicsState2D(new Vector2(pos.Position.x, pos.TempPosition.y), pos.TempPosition, pos.Scale, pos.TempScale, 
                        new Vector2(pos.angularVelocity.x, 0.0f), pos.angularMass, pos.angularAcceleration, pos.centerOfGravity, pos.centerOfRotation);
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, pos.angularVelocity))
                {
                    entity.ReplaceVehiclePhysicsState2D(new Vector2(pos.Position.x, pos.TempPosition.y), pos.TempPosition, pos.Scale, pos.TempScale,
                      new Vector2(pos.angularVelocity.x, 0.0f), pos.angularMass, pos.angularAcceleration, pos.centerOfGravity, pos.centerOfRotation);
                }

                pos = entity.vehiclePhysicsState2D;
                entityBoxBorders = entity.physicsBox2DCollider.CreateEntityBoxBorders(new Vector2(pos.Position.x, pos.TempPosition.y));

                // If is colliding left-right stop x movement
                if (entityBoxBorders.IsCollidingLeft(tileMap, pos.angularVelocity))
                {
                    entity.ReplaceVehiclePhysicsState2D(new Vector2(pos.Position.x, pos.TempPosition.y), pos.TempPosition, pos.Scale, pos.TempScale,
                      new Vector2(0.0f, pos.angularVelocity.y), pos.angularMass, pos.angularAcceleration, pos.centerOfGravity, pos.centerOfRotation);
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, pos.angularVelocity))
                {
                    entity.ReplaceVehiclePhysicsState2D(new Vector2(pos.Position.x, pos.TempPosition.y), pos.TempPosition, pos.Scale, pos.TempScale,
                      new Vector2(0.0f, pos.angularVelocity.y), pos.angularMass, pos.angularAcceleration, pos.centerOfGravity, pos.centerOfRotation);
                }
            }
        }
    }
}

