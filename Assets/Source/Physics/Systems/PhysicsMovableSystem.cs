using System;
using KMath;
using UnityEngine;

namespace Physics
{
    public class PhysicsMovableSystem
    {
        public void Update(AgentContext agentContext)
        {
            float deltaTime = Time.deltaTime;
            var EntitiesWithVelocity = agentContext.GetGroup(AgentMatcher.AllOf(AgentMatcher.PhysicsMovable, AgentMatcher.PhysicsPosition2D));
            foreach (var entity in EntitiesWithVelocity)
            {

                var pos = entity.physicsPosition2D;
                var movable = entity.physicsMovable;

                movable.Acceleration.Y -= 400.0f * deltaTime;

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

                entity.ReplacePhysicsMovable(entity.physicsMovable.Speed, newVelocity, movable.Acceleration);
                entity.ReplacePhysicsPosition2D(newPosition, pos.Value);
            }
        }

        public void Update(ItemContext Context)
        {
            float deltaTime = Time.deltaTime;
            var EntitiesWithVelocity = Context.GetGroup(ItemMatcher.AllOf(ItemMatcher.PhysicsMovable, ItemMatcher.PhysicsPosition2D));
            foreach (var entity in EntitiesWithVelocity)
            {

                var pos = entity.physicsPosition2D;
                var movable = entity.physicsMovable;

                movable.Acceleration.Y -= 400.0f * deltaTime;

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

                entity.ReplacePhysicsMovable(entity.physicsMovable.Speed, newVelocity, movable.Acceleration);
                entity.ReplacePhysicsPosition2D(newPosition, pos.Value);
            }
        }

    }
}

