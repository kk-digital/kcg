
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
                var entityBoxBorders = entity.physicsBox2DCollider.CreateEntityBoxBorders(new Vector2(pos.PreviousValue.x, pos.Value.y) + entity.physicsBox2DCollider.Offset);
                var movable = entity.agentMovable;
                
                if (entityBoxBorders.IsCollidingBottom(tileMap, movable.Velocity))
                {
                    entity.ReplaceAgentPosition2D(new Vector2(pos.Value.x, pos.PreviousValue.y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, new Vector2(movable.Velocity.x, 0.0f), new Vector2(movable.Acceleration.x, 0.0f), movable.AccelerationTime);
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, movable.Velocity))
                {
                    entity.ReplaceAgentPosition2D(new Vector2(pos.Value.x, pos.PreviousValue.y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, new Vector2(movable.Velocity.x, 0.0f), new Vector2(movable.Acceleration.x, 0.0f), movable.AccelerationTime);
                }
                
                pos = entity.agentPosition2D;
                entityBoxBorders = entity.physicsBox2DCollider.CreateEntityBoxBorders(new Vector2(pos.Value.x, pos.PreviousValue.y) + entity.physicsBox2DCollider.Offset);
                movable = entity.agentMovable;
                
                if (entityBoxBorders.IsCollidingLeft(tileMap, movable.Velocity))
                {
                    entity.ReplaceAgentPosition2D(new Vector2(pos.PreviousValue.x, pos.Value.y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, new Vector2(0.0f, movable.Velocity.y), new Vector2(0.0f, movable.Acceleration.y), movable.AccelerationTime);
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, movable.Velocity))
                {
                    entity.ReplaceAgentPosition2D(new Vector2(pos.PreviousValue.x, pos.Value.y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, new Vector2(0.0f, movable.Velocity.y), new Vector2(0.0f, movable.Acceleration.y), movable.AccelerationTime);
                }
            }
        }
    }
}
