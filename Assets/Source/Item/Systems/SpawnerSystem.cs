using UnityEngine;
using System.Collections.Generic;

using Enums;
using KMath;

namespace Item
{
    public class SpawnerSystem
    {
        private static int ItemID;

        public GameEntity SpawnItem(Contexts entitasContext, ItemType itemType, Vec2f position)
        {
            ItemPropertiesEntity entityAttribute = entitasContext.itemProperties.GetEntityWithItemAttributes(itemType);
            Vec2f size = entityAttribute.itemAttributeSize.Size;

            var entity = entitasContext.game.CreateEntity();
            entity.AddItemID(ItemID, itemType);
            entity.AddPhysicsPosition2D(position, Vec2f.Zero);
            entity.AddPhysicsBox2DCollider(size, Vec2f.Zero);
            entity.AddPhysicsMovable(0f, Vec2f.Zero, Vec2f.Zero);

            ItemID++;
            return entity;
        }

        public GameEntity SpawnInventoryItem(GameContext gameContext, ItemType itemType)
        {
            var entity = gameContext.CreateEntity();
            entity.AddItemID(ItemID, itemType);

            ItemID++;
            return entity;
        }
    }
}

