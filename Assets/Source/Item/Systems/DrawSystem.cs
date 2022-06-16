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
                Sprites.Sprite sprite = Game.State.SpriteAtlasManager.GetSprite(SpriteID, Enums.AtlasType.Particle);

                // Draw all items with same sprite.
                var ItemsOfType = EntitasContext.game.GetEntitiesWithItemIDItemType(ItemTypeEntity.itemAttributes.ItemType);
                foreach (var entity in ItemsOfType)
                {
                    if (entity.hasPhysicsPosition2D == false || ItemTypeEntity.hasItemAttributeSize == false) // Test if Item is Drawable.
                        continue;
                    float x = entity.physicsPosition2D.Value.X;
                    float y = entity.physicsPosition2D.Value.Y;
                    float w = ItemTypeEntity.itemAttributeSize.Size.X;
                    float h = ItemTypeEntity.itemAttributeSize.Size.Y;
                    Utility.Render.DrawSprite(x, y, w, h, sprite, Object.Instantiate(material), transform, drawOrder);
                }

            }
        }
    }
}

