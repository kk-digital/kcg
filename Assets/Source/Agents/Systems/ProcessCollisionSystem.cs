using KMath;
using UnityEngine;
using Physics;

namespace Agent
{
    public class ProcessCollisionSystem
    {
        public void Update(Planet.TileMap tileMap)
        {
            float deltaTime = Time.deltaTime;
            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.PhysicsBox2DCollider, GameMatcher.AgentPosition2D));


            foreach (var entity in entities)
            {
                var pos = entity.agentPosition2D;
                var size = entity.physicsBox2DCollider.Size;
                var entityBoxBorders = Box.Create(new Vec2f(pos.PreviousValue.X, pos.Value.Y), size);
                var movable = entity.agentMovable;
                
                if (entityBoxBorders.IsCollidingBottom(tileMap, movable.Velocity))
                {
                    entity.ReplaceAgentPosition2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, new Vec2f(movable.Velocity.X, 0.0f), new Vec2f(movable.Acceleration.X, 0.0f), movable.AccelerationTime);
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, movable.Velocity))
                {
                    entity.ReplaceAgentPosition2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, new Vec2f(movable.Velocity.X, 0.0f), new Vec2f(movable.Acceleration.X, 0.0f), movable.AccelerationTime);
                }
                
                pos = entity.agentPosition2D;
                size = entity.physicsBox2DCollider.Size;
                entityBoxBorders = Box.Create(new Vec2f(pos.Value.X, pos.PreviousValue.Y), size);
                movable = entity.agentMovable;
                
                if (entityBoxBorders.IsCollidingLeft(tileMap, movable.Velocity))
                {
                    entity.ReplaceAgentPosition2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, new Vec2f(0.0f, movable.Velocity.Y), new Vec2f(0.0f, movable.Acceleration.Y), movable.AccelerationTime);
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, movable.Velocity))
                {
                    entity.ReplaceAgentPosition2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, new Vec2f(0.0f, movable.Velocity.Y), new Vec2f(0.0f, movable.Acceleration.Y), movable.AccelerationTime);
                }
            }
        }
    }
}

