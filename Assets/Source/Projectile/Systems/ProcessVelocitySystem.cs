using UnityEngine;
using Entitas;

namespace Projectile
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
            // Get Projectile Entites
            IGroup<GameEntity> entities =
            contexts.game.GetGroup(GameMatcher.ProjectilePhysicsState2D);
            foreach (var projectile in entities)
            {
                // Get position from component
                var position = projectile.projectilePhysicsState2D;
                position.TempPosition = position.Position;

                // Accelerate the vehicle
                position.Position += projectile.projectilePhysicsState2D.angularVelocity * Time.deltaTime;

                // Update the position
                projectile.ReplaceProjectilePhysicsState2D(position.Position, position.TempPosition, position.angularVelocity, position.angularMass, 
                    position.angularAcceleration, position.centerOfGravity, position.centerOfRotation);
            }
        }

        // Physics Tick
        public void Update(Vector3 difference, Contexts contexts)
        {
            // Get Projectile Entites
            IGroup<GameEntity> entities =
            contexts.game.GetGroup(GameMatcher.ProjectilePhysicsState2D);
            foreach (var projectile in entities)
            {
                // Get position from component
                var position = projectile.projectilePhysicsState2D;
                position.TempPosition = position.Position;

                // Calculate distance
                float distance = difference.magnitude;

                // Calculate direction
                Vector2 direction = difference / distance;

                // Normalize the Direction
                direction.Normalize();

                // Set Angular velocity with new direciton
                position.angularVelocity = (direction * 2000.0f) * Time.deltaTime;

                // Process the velocity
                position.Position += projectile.projectilePhysicsState2D.angularVelocity * Time.deltaTime;

                // Update the position
                projectile.ReplaceProjectilePhysicsState2D(position.Position, position.TempPosition, position.angularVelocity, position.angularMass, position.angularAcceleration, position.centerOfGravity, position.centerOfRotation);
            }
        }
    }
}

