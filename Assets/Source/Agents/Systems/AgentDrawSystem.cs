using System.Collections.Generic;
using UnityEngine;

namespace Agent
{
    public class AgentDrawSystem
    {
        List<int> triangles = new();
        List<Vector2> uvs = new();
        List<Vector3> verticies = new();
        
        public void Draw(GameContext gameContext, Material material, Transform transform, int drawOrder)
        {
            var AgentsWithSprite = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.AgentSprite2D));

            foreach (var entity in AgentsWithSprite)
            {
                int spriteId = entity.agentSprite2D.SpriteId;

                if (entity.hasAnimationState)
                {
                    var animation = entity.animationState;
                    spriteId = animation.State.GetSpriteId();
                }

                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(spriteId, Enums.AtlasType.Agent);

                var x = entity.physicsPosition2D.Value.X;
                var y = entity.physicsPosition2D.Value.Y;
                var width = entity.agentSprite2D.Size.X;
                var height = entity.agentSprite2D.Size.Y;

                Utility.Render.DrawSprite(x, y, width, height, sprite, Material.Instantiate(material), transform, drawOrder++);
            }
        }
    }
}

