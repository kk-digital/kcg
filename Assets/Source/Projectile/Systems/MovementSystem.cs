using Entitas;
using KMath;
using UnityEngine;

namespace Projectile
{
    public class MovementSystem
    {
        ProjectileCreationApi ProjectileCreationApi;
        public MovementSystem(ProjectileCreationApi projectileCreationApi)
        {
            ProjectileCreationApi = projectileCreationApi;
        }

        public void Update()
        {
            float deltaTime = Time.deltaTime;
            var projectiles = Contexts.sharedInstance.game.GetGroup(GameMatcher.ProjectileMovable);
            foreach (var projectile in projectiles)
            {
                var pos = projectile.projectilePosition2D;
                var movable = projectile.projectileMovable;
                var type = projectile.projectileType.Type;

                ProjectileProperties projectileProperties = 
                                    ProjectileCreationApi.GetRef((int)type);

                Vec2f displacement =
                    0.5f * movable.Acceleration * (deltaTime * deltaTime) + movable.Velocity * deltaTime;

                Vec2f newVelocity = movable.Acceleration * deltaTime + movable.Velocity;

                float newRotation = pos.Rotation + projectileProperties.DeltaRotation * deltaTime;

                projectile.ReplaceProjectileMovable(newVelocity, movable.Acceleration);
                projectile.ReplaceProjectilePosition2D(pos.Value + displacement, pos.Value, newRotation);
                projectile.ReplaceProjectilePhysicsState2D(newVelocity, projectile.projectilePhysicsState2D.angularMass, 
                    projectile.projectilePhysicsState2D.angularAcceleration, projectile.projectilePhysicsState2D.centerOfGravity, 
                    projectile.projectilePhysicsState2D.centerOfRotation);
            }
        }
    }
}
