using System.Collections.Generic;
using UnityEngine;
using Entitas;
using KMath;
using Sprites;

namespace Vehicle
{
    public class MeshBuilderSystem
    {
        public Utility.FrameMesh Mesh;

        public void Initialize(Material material, Transform transform, int drawOrder = 0)
        {
            Mesh = new Utility.FrameMesh("vehiclesGameObject", material, transform,
                GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Vehicle), drawOrder);
        }

        public void UpdateMesh()
        {
            var VehiclesWithSprite = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.VehicleSprite2D));

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
