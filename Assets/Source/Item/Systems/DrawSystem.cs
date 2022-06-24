using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Item
{
    public class DrawSystem
    {
        
        public void Draw(Contexts contexts, Material material, Transform transform, int drawOrder)
        {
            var ItemAttributesWithSprite = contexts.itemProperties.GetGroup(ItemPropertiesMatcher.AllOf(ItemPropertiesMatcher.ItemAttributeSprite));
            foreach (var ItemTypeEntity in ItemAttributesWithSprite)
            {
                int SpriteID = ItemTypeEntity.itemAttributeSprite.ID;
                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(SpriteID, Enums.AtlasType.Particle);
                
                // Draw all items with same sprite.
                var ItemsOfType = contexts.game.GetEntitiesWithItemIDItemType(ItemTypeEntity.itemAttributes.ItemType);
                foreach (var entity in ItemsOfType)
                {
                    // Test if Item is Drawable.
                    if (!ItemTypeEntity.hasItemAttributeSize) // Test if Item is Drawable.
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
                    
                    float w = ItemTypeEntity.itemAttributeSize.Size.X;
                    float h = ItemTypeEntity.itemAttributeSize.Size.Y;
                    Utility.Render.DrawSprite(x, y, w, h, sprite, Object.Instantiate(material), transform, drawOrder);
                }
                
            }
        }
    }
}

