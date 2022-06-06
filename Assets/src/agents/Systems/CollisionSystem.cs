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

        public void Update(PlanetTileMap.PlanetTileMap tileMap)
        {
            float deltaTime = Time.deltaTime;
            var entities = EntitasContext.game.GetGroup(GameMatcher.AllOf(GameMatcher.ComponentsBox2DCollider,
                             GameMatcher.AgentPosition2D));


            foreach (var entity in entities)
            {
                var boxComponent = entity.componentsBox2DCollider;
                var pos = entity.agentPosition2D;
                var movable = entity.agentMovable;

                if (Physics.BoxCollision.IsCollidingBottom(tileMap, pos.Value, boxComponent.Size))
                {
                    entity.ReplaceAgentPosition2D(new Vector2(pos.Value.x, pos.PreviousValue.y), pos.PreviousValue);
                    entity.ReplaceAgentMovable(movable.Speed, movable.Velocity, new Vector2(movable.Acceleration.x, 0.0f), movable.AccelerationTime);
                }
            }
        }
    }
}

