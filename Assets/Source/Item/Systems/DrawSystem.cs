using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class DrawSystem
    {

        Contexts EntitasContext;

        public DrawSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public void Draw(Material material, Transform transform, int drawOrder)
        {
            var ItemAttributesWithSprite = EntitasContext.game.GetGroup(GameMatcher.AllOf(GameMatcher.ItemAttributeSprite));
            foreach (var ItemTypeEntity in ItemAttributesWithSprite)
            {
                int SpriteID = ItemTypeEntity.itemAttributeSprite.ID;
                Sprites.Sprite sprite = GameState.SpriteAtlasManager.GetSprite(SpriteID, Enums.AtlasType.Particle);

                // Draw all items with same sprite.
                var ItemsOfType = EntitasContext.game.GetEntitiesWithItemIDItemType(ItemTypeEntity.itemAttributes.ItemType);
                foreach (var entity in ItemsOfType)
                {
                    // Test if Item is Drawable.
                    if (!ItemTypeEntity.hasItemAttributeSize) // Test if Item is Drawable.
                        continue;

                    float x, y;
                    if (entity.hasItemDrawPosition2D)
                    {
                        x = entity.itemDrawPosition2D.Value.x;
                        y = entity.itemDrawPosition2D.Value.y;
                    }
                    else
                    {
                        if (entity.hasPhysicsPosition2D)
                        {
                            x = entity.physicsPosition2D.Value.x;
                            y = entity.physicsPosition2D.Value.y;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    float w = ItemTypeEntity.itemAttributeSize.Size.x;
                    float h = ItemTypeEntity.itemAttributeSize.Size.y;
                    Utility.Render.DrawSprite(x, y, w, h, sprite, Object.Instantiate(material), transform, drawOrder);
                }

            }
        }
    }
}

