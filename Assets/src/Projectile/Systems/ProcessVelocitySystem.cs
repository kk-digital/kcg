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
            contexts.game.GetGroup(GameMatcher.ProjectilePosition2D);
            foreach (var projectile in entities)
            {
                // Get position from component
                var position = projectile.projectilePosition2D;
                position.TempPosition = position.Position;

                // Accelerate the vehicle
                position.Position += projectile.projectileVelocity.Value * Time.deltaTime;

                // Update the position
                projectile.ReplaceProjectilePosition2D(position.Position, position.TempPosition);
            }
        }
    }
}

