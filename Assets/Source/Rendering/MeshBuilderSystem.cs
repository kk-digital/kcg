using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rendering
{
    public class MeshBuilderSystem
    {
        public Utility.FrameMesh Mesh;

        public void Initialize(Material material, Transform transform, Enums.AtlasType atlasType, int drawOrder = 0)
        {
            Mesh = new Utility.FrameMesh("AgentsGameObject", material, transform,
                GameState.SpriteAtlasManager.GetSpriteAtlas(atlasType), drawOrder);
        }

        public void UpdateMesh(VehicleContext context)
        {
            var VehiclesWithSprite = context.GetGroup(VehicleMatcher.AllOf(VehicleMatcher.VehicleSprite2D));

            Mesh.Clear();
            int index = 0;
            foreach (var entity in VehiclesWithSprite)
            {
                Vector4 textureCoords = GameState.SpriteAtlasManager.GetSprite(entity.vehicleSprite2D.SpriteId, Enums.AtlasType.Particle).TextureCoords;

                var x = entity.vehiclePhysicsState2D.Position.X;
                var y = entity.vehiclePhysicsState2D.Position.Y;
                var width = entity.vehicleSprite2D.Size.X;
                var height = entity.vehicleSprite2D.Size.Y;

                // Update UVs
                Mesh.UpdateUV(textureCoords, (index) * 4);
                // Update Vertices
                Mesh.UpdateVertex((index++ * 4), x, y, width, height);
            }
        }
    }
}
