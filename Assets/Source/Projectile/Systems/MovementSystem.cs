using Entitas;
using KMath;
using UnityEngine;

namespace Projectile
{
    public class MovementSystem
    {
        public void Update()
        {
            float deltaTime = Time.deltaTime;
            var projectiles = Contexts.sharedInstance.game.GetGroup(GameMatcher.ProjectileMovable);
            foreach (var projectile in projectiles)
            {
                var pos = projectile.projectilePosition2D;
                var movable = projectile.projectileMovable;

                Vec2f displacement =
                    0.5f * movable.Acceleration * (deltaTime * deltaTime) + movable.Velocity * deltaTime;

                Vec2f newVelocity = movable.Acceleration * deltaTime + movable.Velocity;

                projectile.ReplaceProjectileMovable(newVelocity, movable.Acceleration);
                projectile.ReplaceProjectilePosition2D(pos.Value + displacement, pos.Value);
                projectile.ReplaceProjectilePhysicsState2D(newVelocity, projectile.projectilePhysicsState2D.angularMass, 
                    projectile.projectilePhysicsState2D.angularAcceleration, projectile.projectilePhysicsState2D.centerOfGravity, 
                    projectile.projectilePhysicsState2D.centerOfRotation);
            }
        }
    }
}
