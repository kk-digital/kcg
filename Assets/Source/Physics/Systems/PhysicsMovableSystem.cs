using System;
using KMath;
using UnityEngine;

namespace Physics
{
    public class PhysicsMovableSystem
    {
        public void Update(GameContext gameContext)
        {
            float Gravity = 800.0f;
            float MaxAcceleration = 300.0f;
            // maximum Y velocity
            float MaxVelocityY = 15.0f;


            float deltaTime = Time.deltaTime;
            var EntitiesWithVelocity = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.PhysicsMovable, GameMatcher.PhysicsPosition2D));
            foreach (var entity in EntitiesWithVelocity)
            {

                var pos = entity.physicsPosition2D;
                var movable = entity.physicsMovable;

                movable.Acceleration.Y -= Gravity * deltaTime;

                // maximum acceleration in the game
                if (movable.Acceleration.Y <= -MaxAcceleration)
                {
                    movable.Acceleration.Y = -MaxAcceleration;
                }

                if (movable.Acceleration.Y >= MaxAcceleration)
                {
                    movable.Acceleration.Y = MaxAcceleration;
                }


                // maximum velocity in the game
                if (movable.Velocity.Y > MaxVelocityY)
                {
                    movable.Velocity.Y = MaxVelocityY;
                }
                if (movable.Velocity.Y < -MaxVelocityY)
                {
                    movable.Velocity.Y = -MaxVelocityY;
                }

                Vec2f displacement = 
                        0.5f * movable.Acceleration * (deltaTime * deltaTime) + movable.Velocity * deltaTime;
                Vec2f newVelocity = movable.Acceleration * deltaTime + movable.Velocity;
                newVelocity.X *= 0.7f;


                // maximum velocity in the game
                if (newVelocity.Y > MaxVelocityY)
                {
                    newVelocity.Y = MaxVelocityY;
                }
                if (newVelocity.Y < -MaxVelocityY)
                {
                    newVelocity.Y = -MaxVelocityY;
                }


                Vec2f newPosition = pos.Value + displacement;

                entity.ReplacePhysicsMovable(entity.physicsMovable.Speed, newVelocity, movable.Acceleration, movable.Landed);
                entity.ReplacePhysicsPosition2D(newPosition, pos.Value);
            }
        }
    }
}

