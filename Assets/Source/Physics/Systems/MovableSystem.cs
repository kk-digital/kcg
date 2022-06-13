using System;
using KMath;
using UnityEngine;

namespace Physics
{
    public class MovableSystem
    {
        public void Update()
        {
            float deltaTime = Time.deltaTime;
            var EntitiesWithVelocity = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.PhysicsMovable, GameMatcher.PhysicsPosition2D));
            foreach (var entity in EntitiesWithVelocity)
            {

                var pos = entity.physicsPosition2D;
                var movable = entity.physicsMovable;

                movable.Acceleration.y -= 400.0f * deltaTime;

                if (movable.Acceleration.Y <= -30.0f)
                {
                    movable.Acceleration.Y = -30.0f;
                }

                if (movable.Acceleration.Y >= 30.0f)
                {
                    movable.Acceleration.Y = 30.0f;
                }

                Vec2f displacement = 
                        0.5f * movable.Acceleration * (deltaTime * deltaTime) + movable.Velocity * deltaTime;
                Vec2f newVelocity = movable.Acceleration * deltaTime + movable.Velocity;
                newVelocity.X *= 0.7f;

                if (newVelocity.Y > 5.0f)
                {
                    newVelocity.Y = 5.0f;
                }
                if (newVelocity.Y < -5.0f)
                {
                    newVelocity.Y = -5.0f;
                }


                Vec2f newPosition = pos.Value + displacement;

                entity.ReplacePhysicsMovable(entity.physicsMovable.Speed, newVelocity, movable.Acceleration, entity.physicsMovable.AccelerationTime);
                entity.ReplacePhysicsPosition2D(newPosition, pos.Value);
            }
        }
    }
}

