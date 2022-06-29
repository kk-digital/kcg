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

        public void UpdateMesh()
        {
            var projectilessWithSprite = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectileSprite2D));

            Mesh.Clear();
            int index = 0;
            foreach (var entity in projectilessWithSprite)
            {
                Vector4 textureCoords = GameState.SpriteAtlasManager.GetSprite(entity.projectileSprite2D.SpriteId, Enums.AtlasType.Particle).TextureCoords;

                var x = entity.projectilePosition2D.Value.X;
                var y = entity.projectilePosition2D.Value.Y;
                var width = entity.projectileSprite2D.Size.X;
                var height = entity.projectileSprite2D.Size.Y;

                // Update UVs
                Mesh.UpdateUV(textureCoords, (index) * 4);
                // Update Vertices
                Mesh.UpdateVertex((index * 4), x, y, width, height);
                index++;
            }
        }
    }
}
