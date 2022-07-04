using Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Projectile
{
    public class MeshBuilderSystem
    {
        public Utility.FrameMesh Mesh;

        public void Initialize(Material material, Transform transform, int drawOrder = 0)
        {
            Mesh = new Utility.FrameMesh("ProjectilesGameObject", material, transform,
                GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Particle), drawOrder);
        }

        public void UpdateMesh(ProjectileContext context)
        {
            var projectilessWithSprite = context.GetGroup(ProjectileMatcher.AllOf(ProjectileMatcher.ProjectileSprite2D));

            Mesh.Clear();
            int index = 0;
            foreach (var entity in projectilessWithSprite)
            {
                int spriteId = entity.projectileSprite2D.SpriteId;

                if (entity.hasAnimationState)
                {
                    var animation = entity.animationState;
                    spriteId = animation.State.GetSpriteId();
                }

                Vector4 textureCoords = GameState.SpriteAtlasManager.GetSprite(spriteId, Enums.AtlasType.Particle).TextureCoords;

                var x = entity.projectilePosition2D.Value.X;
                var y = entity.projectilePosition2D.Value.Y;
                var width = entity.projectileSprite2D.Size.X;
                var height = entity.projectileSprite2D.Size.Y;

                if (!Utility.ObjectMesh.isOnScreen(x, y))
                    continue;

                // Update UVs
                Mesh.UpdateUV(textureCoords, (index) * 4);
                // Update Vertices
                Mesh.UpdateVertex((index * 4), x, y, width, height);
                index++;
            }
        }
    }
}
