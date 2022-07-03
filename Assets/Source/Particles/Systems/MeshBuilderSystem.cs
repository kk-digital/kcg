using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace Particle
{
    public class MeshBuilderSystem
    {
        public Utility.FrameMesh Mesh;

        public void Initialize(Material material, Transform transform, int drawOrder = 0)
        {
            Mesh = new Utility.FrameMesh("ParticlesGameObject", material, transform,
                GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Particle), drawOrder);
        }

        public void UpdateMesh(ParticleContext context)
        {
            var entities = context.GetGroup(ParticleMatcher.AllOf(ParticleMatcher.ParticleSprite2D));
            
            Mesh.Clear();
            int index = 0;
            foreach (var entity in entities)
            {
                int spriteId = entity.particleSprite2D.SpriteId;
                Vector4 textureCoords = GameState.SpriteAtlasManager.GetSprite(spriteId, Enums.AtlasType.Particle).TextureCoords;

                var pos = entity.particlePosition2D.Position;
                var x = pos.x;
                var y = pos.y;
                var width = entity.particleSprite2D.Size.X;
                var height = entity.particleSprite2D.Size.Y;

                // Update UVs
                Mesh.UpdateUV(textureCoords, (index) * 4);
                // Update Vertices
                Mesh.UpdateVertex((index * 4), x, y, width, height);
                index++;
            }
        }
    }
}
