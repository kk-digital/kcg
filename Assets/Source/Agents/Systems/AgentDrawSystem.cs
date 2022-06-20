using System.Collections.Generic;
using UnityEngine;

namespace Agent
{
    public class AgentDrawSystem
    {
        List<int> triangles = new();
        List<Vector2> uvs = new();
        List<Vector3> verticies = new();
        
        public void Draw(Material material, Transform transform, int drawOrder)
        {
            var AgentsWithSprite = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentSprite2D));

            int idx = 0;
            foreach (var entity in AgentsWithSprite)
            {
                int spriteId = entity.agentSprite2D.SpriteId;

                if (entity.hasAnimationState)
                {
                    var animation = entity.animationState;
                    spriteId = animation.State.GetSpriteId();
                }

                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(spriteId, Enums.AtlasType.Agent);



                var x = entity.physicsPosition2D.Value.x;
                var y = entity.physicsPosition2D.Value.y;
                var width = entity.agentSprite2D.Size.x;
                var height = entity.agentSprite2D.Size.y;

                Utility.Render.DrawSprite(x, y, width, height, sprite, Material.Instantiate(material), transform, drawOrder++);
            }
        }
    }
}

