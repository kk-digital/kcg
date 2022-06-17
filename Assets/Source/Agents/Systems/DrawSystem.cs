using System.Collections.Generic;
using UnityEngine;

namespace Agent
{
    public class DrawSystem
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
                var sprite = new Sprites.Sprite
                {
                    Texture = Game.State.UnityImage2DCache.Get(spriteId, Enums.AtlasType.Agent),
                    TextureCoords = new Vector4(0, 0, 1, 1)
                };

                var x = entity.physicsPosition2D.Value.X;
                var y = entity.physicsPosition2D.Value.Y;
                var width = entity.agentSprite2D.Size.X;
                var height = entity.agentSprite2D.Size.Y;

                Utility.Render.DrawSprite(x, y, width, height, sprite, Material.Instantiate(material), transform, drawOrder++);
            }
        }
    }
}

