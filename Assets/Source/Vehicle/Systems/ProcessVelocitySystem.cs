using UnityEngine;
using Entitas;

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

        // Move Function
        public void Process(Contexts contexts)
        {
            // Get Vehicle Entites
            IGroup<GameEntity> entities =
            contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
            foreach (var vehicle in entities)
            {
                // Get position from component
                var position = vehicle.vehiclePhysicsState2D;
                position.TempPosition = position.Position;

                // Accelerate the vehicle
                position.Position += vehicle.vehiclePhysicsState2D.angularVelocity * Time.deltaTime;

                // Update the position
                vehicle.ReplaceVehiclePhysicsState2D(position.Position, position.TempPosition, position.Scale, position.TempScale, 
                    position.angularVelocity, vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                         vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);
            }
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
                    new Vector2(vehicle.vehiclePhysicsState2D.angularVelocity.x, (vehicle.vehiclePhysicsState2D.angularVelocity.y - 1.5f) * Time.deltaTime), vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
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

        public void StopMovement(Vector2 newSpeed, Contexts contexts)
        {
            // Update the component
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
            }
        }
    }
}

