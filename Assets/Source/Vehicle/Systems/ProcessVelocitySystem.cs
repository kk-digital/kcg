using UnityEngine;
using Entitas;
using System.Collections;

namespace Vehicle
{
    public sealed class ProcessVelocitySystem
    {
        // Singleton
        public static readonly ProcessVelocitySystem Instance;

        // Game Context
        private GameContext gameContext;

        // Static Constructor
        static ProcessVelocitySystem()
        {
            Instance = new ProcessVelocitySystem();
        }

        // Constructor
        public ProcessVelocitySystem()
        {
            gameContext = Contexts.sharedInstance.game;
        }

        public void ProcessMovement(Vector2 newSpeed, Contexts contexts)
        {
            // Get Vehicle Entites
            IGroup<GameEntity> entities =
            contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
            foreach (var vehicle in entities)
            {
                // Get position from component
                var position = vehicle.vehiclePhysicsState2D;
                position.TempPosition = position.Position;


                // Update the position
                vehicle.ReplaceVehiclePhysicsState2D(position.Position, position.TempPosition, position.Scale, position.TempScale,
                    newSpeed, vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                         vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);

                // Add velocity to position
                position.Position += vehicle.vehiclePhysicsState2D.angularVelocity * Time.deltaTime;

                // Update the position
                vehicle.ReplaceVehiclePhysicsState2D(position.Position, position.TempPosition, position.Scale, position.TempScale,
                    vehicle.vehiclePhysicsState2D.angularVelocity, vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                         vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);

                return;

            }
        }

        public void UpdateGravity(Contexts contexts)
        {
            // Get Vehicle Entites
            IGroup<GameEntity> entities =
            contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
            foreach (var vehicle in entities)
            {
                if (vehicle.vehiclePhysicsState2D.angularVelocity.y > 0.5f)
                    return;

                // Get position from component
                var position = vehicle.vehiclePhysicsState2D;
                position.TempPosition = position.Position;

                // Update the position
                vehicle.ReplaceVehiclePhysicsState2D(position.Position, position.TempPosition, position.Scale, position.TempScale,
                    new Vector2(vehicle.vehiclePhysicsState2D.angularVelocity.x, (vehicle.vehiclePhysicsState2D.angularVelocity.y - vehicle.vehiclePhysicsState2D.centerOfGravity) * Time.deltaTime), vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                         vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);

                // Add velocity to position
                position.Position += vehicle.vehiclePhysicsState2D.angularVelocity * Time.deltaTime;

                // Update the position
                vehicle.ReplaceVehiclePhysicsState2D(position.Position, position.TempPosition, position.Scale, position.TempScale,
                    vehicle.vehiclePhysicsState2D.angularVelocity, vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                         vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);

                return;
            }
        }

        public IEnumerator Break(bool xAxis, Vector2 angularVelocity, Contexts contexts)
        {
            IGroup<GameEntity> entities =
            contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
            foreach (var vehicle in entities)
            {
                // Get position from component
                var position = vehicle.vehiclePhysicsState2D;
                position.TempPosition = position.Position;
                float newVelo;

                if (xAxis)
                {
                    float elapsed = 0.0f;
                    float duration = 1.0f;
                    while (elapsed < duration)
                    {
                        newVelo = Mathf.Lerp(angularVelocity.x, 0.0f, elapsed / duration);
                        // Update the position
                        vehicle.ReplaceVehiclePhysicsState2D(position.Position, position.TempPosition, position.Scale, position.TempScale,
                            new Vector2(newVelo, position.angularVelocity.y), vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                                 vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);
                        elapsed += Time.deltaTime;
                        yield return null;
                    }
                    angularVelocity.x = 0.0f;
                }
                else
                {
                    float elapsed = 0.0f;
                    float duration = 1.0f;
                    while (elapsed < duration)
                    {
                        newVelo = Mathf.Lerp(angularVelocity.y, 0.0f, elapsed / duration);
                        // Update the position
                        vehicle.ReplaceVehiclePhysicsState2D(position.Position, position.TempPosition, position.Scale, position.TempScale,
                            new Vector2(position.angularVelocity.x, newVelo), vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                                 vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);
                        elapsed += Time.deltaTime;
                        yield return null;
                    }
                    angularVelocity.y = 0.0f;
                }
            }

            yield return null;
        }
    }
}

