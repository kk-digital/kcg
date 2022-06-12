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
                Sprites.Model sprite = GameState.SpriteAtlasManager.GetSprite(SpriteID, Enums.AtlasType.Particle);

                float width = sprite.Texture.width / 32.0f;
                float height = sprite.Texture.height / 32.0f;

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

