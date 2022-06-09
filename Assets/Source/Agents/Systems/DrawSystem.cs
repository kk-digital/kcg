using System.Collections.Generic;
using UnityEngine;

namespace Agent
{
    public class DrawSystem
    {

        Contexts EntitasContext;
        
        List<int> triangles = new();
        List<Vector2> uvs = new();
        List<Vector3> verticies = new();

        public DrawSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public void Draw(Material material, Transform transform, int drawOrder)
        {
            var AgentsWithSprite = EntitasContext.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentSprite2D));
            foreach (var entity in AgentsWithSprite)
            {
                var sprite = new Sprites.Model
                {
                    Texture = entity.agentSprite2D.Texture,
                    TextureCoords = new Vector4(0, 0, 1, 1)
                };

                var x = entity.agentPosition2D.Value.x;
                var y = entity.agentPosition2D.Value.y;
                var width = entity.agentSprite2D.Size.x;
                var height = entity.agentSprite2D.Size.y;

                Utility.Render.DrawSprite(x, y, width, height, sprite, material, transform, drawOrder);
            }
        }
    }
}

