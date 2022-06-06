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

        public void Execute(PlanetTileMap.PlanetTileMap tileMap)
        {
            float deltaTime = Time.deltaTime;
            var entities = EntitasContext.game.GetGroup(GameMatcher.AllOf(GameMatcher.ComponentsBox2DCollider,
                             GameMatcher.AgentPosition2D));
            foreach (var entity in entities)
            {
                var boxComponent = entity.componentsBox2DCollider;
                var pos = entity.agentPosition2D;

                if (Physics.BoxCollision.IsCollidingBottom(tileMap, pos.Value, boxComponent.Size))
                {
                    entity.ReplaceAgentPosition2D(pos.PreviousValue, pos.PreviousValue);
                }
            }
        }
    }
}

