/*using UnityEngine;
using Entitas;
using KMath;

namespace Projectile
{
    public sealed class ProcessVelocitySystem
    {
        // Singleton
        public static readonly ProcessVelocitySystem Instance;

        float distance;
        Vec2f direction;

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
                var position = projectile.projectilePosition2D;
                var physicsState = projectile.projectilePhysicsState2D;
                position.PreviousValue = position.Value;

                // Accelerate the vehicle
                position.Value += projectile.projectilePhysicsState2D.angularVelocity * Time.deltaTime;

                // Update the position
                projectile.ReplaceProjectilePosition2D(position.Value, position.PreviousValue);
                projectile.ReplaceProjectilePhysicsState2D(physicsState.angularVelocity, physicsState.angularMass,
                    physicsState.angularAcceleration, physicsState.centerOfGravity, physicsState.centerOfRotation);
            }
        }

        // Physics Tick
        public void Update(Vec3f difference)
        {
            // Get Projectile Entites
            IGroup<GameEntity> entities =
            Contexts.sharedInstance.game.GetGroup(GameMatcher.ProjectilePhysicsState2D);
            foreach (var projectile in entities)
            {
                // Get position from component
                var position = projectile.projectilePosition2D;
                var physicsState = projectile.projectilePhysicsState2D;

                position.PreviousValue = position.Value;
                if (!projectile.projectileCollider.isFired)
                {
                    // Calculate distance
                    distance = difference.Magnitude;
                    // Calculate direction
                    direction = (Vec2f)difference / distance;
                    // Normalize the Direction
                    direction.Normalize();
                    // Set Angular velocity with new direciton
                    physicsState.angularVelocity = (direction * 3000.0f) * Time.deltaTime;
                }
                projectile.projectileCollider.isFired = true;
                // Process the velocity
                position.Value += projectile.projectilePhysicsState2D.angularVelocity * Time.deltaTime;
                // Update the position
                projectile.ReplaceProjectilePosition2D(position.Value, position.PreviousValue);
                projectile.ReplaceProjectilePhysicsState2D(physicsState.angularVelocity, physicsState.angularMass, physicsState.angularAcceleration, physicsState.centerOfGravity, physicsState.centerOfRotation);
            }
        }
    }
}
*/
