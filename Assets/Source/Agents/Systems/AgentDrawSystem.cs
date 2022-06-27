using System.Collections.Generic;
using UnityEngine;

namespace Agent
{
    public class AgentDrawSystem
    {
    
        public void Draw(Material material, Transform transform, int drawOrder)
        {
            var AgentsWithSprite = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentSprite2D));

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

        public void DrawEx(Material material, Transform transform, int drawOrder)
        {
            var AgentsWithSprite = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentSprite2D));

            foreach (var entity in AgentsWithSprite)
            {
                var spr = entity.agentSprite2D;
                int spriteId = spr.SpriteId;
                GameObject gameObject = spr.GameObject;

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

                Utility.Render.DrawSpriteEx(gameObject, x, y, width, height, sprite, Material.Instantiate(material), drawOrder++);
            }
        }
    }
}

