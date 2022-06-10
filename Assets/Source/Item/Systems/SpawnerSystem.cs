using UnityEngine;
using System.Collections.Generic;

using Enums;

namespace Item
{
    public class SpawnerSystem
    {
        public Contexts EntitasContext;

        private static int ItemID;

        public SpawnerSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public GameEntity SpawnItem(ItemType itemType)
        {
            var entity = EntitasContext.game.CreateEntity();
            entity.AddItemID(ItemID, itemType);
            entity.AddItemPosition2D(Vector2.zero, Vector2.zero);
            entity.AddItemMovable(0f, Vector2.zero, Vector2.zero, 0f);

            ItemID++;
            return entity;
        }

        public GameEntity SpawnIventoryItem(ItemType itemType)
        {
            var entity = EntitasContext.game.CreateEntity();
            entity.AddItemID(ItemID, itemType);

            ItemID++;
            return entity;
        }
    }
}

