using System;
using UnityEngine;

namespace Agent
{
    public class CollisionSystem
    {
        Contexts EntitasContext;
        public CollisionSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public void Update(Planet.TileMap.Model tileMap)
        {
            float deltaTime = Time.deltaTime;
            var entities = EntitasContext.game.GetGroup(GameMatcher.AllOf(GameMatcher.ComponentsBox2DCollider,
                             GameMatcher.AgentPosition2D));


            foreach (var entity in entities)
            {
                var boxComponent = entity.componentsBox2DCollider;
                var pos = entity.agentPosition2D;
                var movable = entity.agentMovable;

                if (Physics.BoxCollision.IsCollidingBottom(tileMap, pos.Value, boxComponent.Size) && 
                movable.Velocity.y <= 0.0f)
                {
                    entity.ReplaceAgentPosition2D(new Vector2(pos.Value.x, pos.PreviousValue.y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, new Vector2(movable.Velocity.x, 0.0f), new Vector2(movable.Acceleration.x, 0.0f), movable.AccelerationTime);
                }
                else if (Physics.BoxCollision.IsCollidingTop(tileMap, pos.Value, boxComponent.Size) &&
                movable.Velocity.y >= 0.0f)
                {
                    entity.ReplaceAgentPosition2D(new Vector2(pos.Value.x, pos.PreviousValue.y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, new Vector2(movable.Velocity.x, 0.0f), new Vector2(movable.Acceleration.x, 0.0f), movable.AccelerationTime);
                }
            }
        }
    }
}

