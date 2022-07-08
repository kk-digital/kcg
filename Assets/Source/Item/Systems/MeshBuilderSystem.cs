using System.Collections.Generic;
using UnityEngine;
using Entitas;
using KMath;
using Sprites;

namespace Item
{
    public class MeshBuilderSystem
    {
        public Utility.FrameMesh Mesh;

        public void Initialize(Material material, Transform transform, int drawOrder = 0)
        {
            Mesh = new Utility.FrameMesh("ItemsGameObject", material, transform,
                GameState.SpriteAtlasManager.GetSpriteAtlas(Enums.AtlasType.Particle), drawOrder);
        }

        public void UpdateMesh(Contexts context)
        {            
            Mesh.Clear();
            int index = 0;

            ItemEntity[] items = context.item.GetEntities();
            foreach (var entity in items)
            {
                ItemProprieties proprieties = GameState.ItemCreationApi.Get(entity.itemType.Type);

                int SpriteID = proprieties.SpriteID;
                Vector4 textureCoords = GameState.SpriteAtlasManager.GetSprite(SpriteID, Enums.AtlasType.Particle).TextureCoords;


                float x, y;
                if (entity.hasItemDrawPosition2D)
                {
                    x = entity.itemDrawPosition2D.Value.X;
                    y = entity.itemDrawPosition2D.Value.Y;
                }
                else
                {
                    if (entity.hasPhysicsPosition2D)
                    {
                        x = entity.physicsPosition2D.Value.X;
                        y = entity.physicsPosition2D.Value.Y;
                    }
                    else
                    {
                        continue;
                    }
                }

                float w = proprieties.SpriteSize.X;
                float h = proprieties.SpriteSize.Y;

                if (!Utility.ObjectMesh.isOnScreen(x, y))
                    continue;

                // Update UVs
                Mesh.UpdateUV(textureCoords, (index) * 4);
                // Update Vertices
                Mesh.UpdateVertex((index * 4), x, y, w, h);
                index++;
            }
        }
    }
}
