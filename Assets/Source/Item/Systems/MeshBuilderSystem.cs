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

        public void UpdateMesh()
        {
            var ItemPropertyWithSprite = Contexts.sharedInstance.itemProperties.GetGroup(ItemPropertiesMatcher.AllOf(ItemPropertiesMatcher.ItemPropertySprite));
            Mesh.Clear();
            int index = 0;
            foreach (var ItemTypeEntity in ItemPropertyWithSprite)
            {
                int SpriteID = ItemTypeEntity.itemPropertySprite.ID;
                Vector4 textureCoords = GameState.SpriteAtlasManager.GetSprite(SpriteID, Enums.AtlasType.Particle).TextureCoords;
                
                // Draw all items with same sprite.
                var ItemsOfType = Contexts.sharedInstance.game.GetEntitiesWithItemIDItemType(ItemTypeEntity.itemProperty.ItemType);

                foreach (var entity in ItemsOfType)
                {
                    // Test if Item is Drawable.
                    if (!ItemTypeEntity.hasItemPropertySize) // Test if Item is Drawable.
                        continue;
                    
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
                    
                    float w = ItemTypeEntity.itemPropertySize.Size.X;
                    float h = ItemTypeEntity.itemPropertySize.Size.Y;

                    // Update UVs
                    Mesh.UpdateUV(textureCoords, (index) * 4);
                    // Update Vertices
                    Mesh.UpdateVertex((index * 4), x, y, w, h);
                    index++;
                }
            }
        }
    }
}

