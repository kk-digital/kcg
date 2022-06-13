using System;
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

                if (movable.Acceleration.y <= -30.0f)
                {
                    movable.Acceleration.y = -30.0f;
                }

                if (movable.Acceleration.y >= 30.0f)
                {
                    movable.Acceleration.y = 30.0f;
                }

                Vector2 displacement = 
                        0.5f * movable.Acceleration * (deltaTime * deltaTime) + movable.Velocity * deltaTime;
                Vector2 newVelocity = movable.Acceleration * deltaTime + movable.Velocity;
                newVelocity.x *= 0.7f;

                if (newVelocity.y > 5.0f)
                {
                    newVelocity.y = 5.0f;
                }
                if (newVelocity.y < -5.0f)
                {
                    newVelocity.y = -5.0f;
                }


                Vector2 newPosition = pos.Value + displacement;

                entity.ReplacePhysicsMovable(entity.physicsMovable.Speed, newVelocity, movable.Acceleration, entity.physicsMovable.AccelerationTime);
                entity.ReplacePhysicsPosition2D(newPosition, pos.Value);
            }
        }
    }
}

