using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Agent
{
    public class AgentMeshBuilderSystem
    {
        public Utility.FrameMesh Mesh;

        public void Initialize(Material material, Transform transform, int drawOrder = 0)
        {
            Mesh = new Utility.FrameMesh("AgentsGameObject", material, transform,
                GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Agent), drawOrder);

        }

        public void UpdateMesh()
        {
            var AgentsWithSprite = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentSprite2D));

            int index = 0;
            foreach (var entity in AgentsWithSprite)
            {
                int spriteId = entity.agentSprite2D.SpriteId;

                if (entity.hasAnimationState)
                {
                    var animation = entity.animationState;
                    spriteId = animation.State.GetSpriteId();
                }

                Vector4 textureCoords = GameState.SpriteAtlasManager.GetSprite(spriteId, Enums.AtlasType.Agent).TextureCoords;

                var x = entity.physicsPosition2D.Value.X;
                var y = entity.physicsPosition2D.Value.Y;
                var width = entity.agentSprite2D.Size.X;
                var height = entity.agentSprite2D.Size.Y;

                // Update UVs
                Mesh.UpdateUV(textureCoords, (index) * 4);
                // Update Vertices
                Mesh.UpdateVertex((index * 4), x, y, width, height);
                index++;
            }
        }
    }
}
