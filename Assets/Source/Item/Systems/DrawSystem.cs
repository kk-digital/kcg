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
                var sprite = new Sprites.Model
                {
                    Texture = ItemTypeEntity.itemAttributeSprite.Texture,
                    TextureCoords = new Vector4(0, 0, 1, 1)
                };

                float width = ItemTypeEntity.itemAttributeSprite.Size.x;
                float height = ItemTypeEntity.itemAttributeSprite.Size.y;

                // Draw all items with same sprite.
                var ItemsOfType = EntitasContext.game.GetEntitiesWithItemIDItemType(ItemTypeEntity.itemAttributesBasic.ItemType);
                foreach (var entity in ItemsOfType)
                {
                    if (entity.hasAgentPosition2D == false) // Test if Item is Drawable.
                        continue;
                    float x = entity.itemPosition2D.Value.x;
                    float y = entity.itemPosition2D.Value.y;
                    Utility.Render.DrawSprite(x, y, width, height, sprite, material, transform, drawOrder);
                }

            }
        }
    }
}

