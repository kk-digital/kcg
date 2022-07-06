using Entitas;
using System.Collections.Generic;
using System.Collections;
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

        public void Update(ProjectileContext gameContext)
        {
            float deltaTime = Time.deltaTime;
            var projectiles = gameContext.GetGroup(ProjectileMatcher.ProjectileMovable);
            foreach (var projectile in projectiles)
            {
                var pos = projectile.projectilePosition2D;
                var movable = projectile.projectileMovable;
                var type = projectile.projectileType.Type;
                var canRamp = projectile.projectileRamp.canRamp;

                ProjectileProperties projectileProperties = 
                                    ProjectileCreationApi.GetRef((int)type);
                
                Vec2f displacement =
                    0.5f * movable.Acceleration * (deltaTime * deltaTime) + movable.Velocity * deltaTime;

                Vec2f newVelocity = new Vec2f(0f, 0f);

                if(canRamp)
                {
                    var startSpeed = projectile.projectileRamp.startVelocity;
                    var maxSpeed = projectile.projectileRamp.maxVelocity;
                    var rampTime = projectile.projectileRamp.rampTime;

                    float elapsed = 0.0f;
                    while (elapsed < rampTime)
                    {
                        if (canRamp)
                        {
                            projectileProperties.Speed = Mathf.Lerp(startSpeed, maxSpeed, elapsed / rampTime);
                            newVelocity = movable.Acceleration * deltaTime + (movable.Velocity * projectileProperties.Speed);
                            elapsed += Time.deltaTime;
                        }
                    }
                    projectileProperties.Speed = projectileProperties.MaxVelocity;
                }
                else
                {
                    newVelocity = movable.Acceleration * deltaTime + movable.Velocity;
                }

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
