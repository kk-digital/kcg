using UnityEngine;
using System.Collections.Generic;

using Enums;
using KMath;

namespace Item
{
    public class SpawnerSystem
    {
        private static int ItemID;

        public ItemEntity SpawnItem(Contexts entitasContext, ItemType itemType, Vec2f position)
        {
            ItemPropertiesEntity entityAttribute = entitasContext.itemProperties.GetEntityWithItemProperty(itemType);
            Vec2f size = entityAttribute.itemPropertySize.Size;

            var entity = entitasContext.item.CreateEntity();
            entity.AddItemID(ItemID);
            entity.AddItemType(itemType);
            entity.AddPhysicsPosition2D(position, Vec2f.Zero);
            entity.AddPhysicsBox2DCollider(size, Vec2f.Zero);
            entity.AddPhysicsMovable(0f, Vec2f.Zero, Vec2f.Zero, true, true, false, false);

            ItemID++;
            return entity;
        }

        public ItemEntity SpawnInventoryItem(ItemContext context, ItemType itemType)
        {
            var entity = context.CreateEntity();
            entity.AddItemID(ItemID);
            entity.AddItemType(itemType);

            ItemID++;
            return entity;
        }
    }
}

