using System;
using System.Collections;
using UnityEngine;
using Entitas;

namespace Systems
{
    public class InventoryManager
    {
        public Contexts _contexts;

        public InventoryManager(Contexts contexts)
        {
            _contexts = contexts;
        }

        public void AddItem(GameEntity entity, int inventoryID)
        {
            var group = _contexts.game.GetEntitiesWithInventoryItem(inventoryID); // Todo: Use multiple Entity Index. To narrow down the search with item type.

            // Check if there any not full stack of this item in the inventory.
            if (entity.hasItemStack)
            {
                foreach (GameEntity entityIT in group)
                {
                    if (entityIT.item.ItemType != entity.item.ItemType)
                    {
                        continue;
                    }
                    if ((entityIT.itemStack.StackCount + entity.itemStack.StackCount) <= entityIT.itemStack.MaxStackSize)
                    {
                        entityIT.itemStack.StackCount += entity.itemStack.StackCount;
                        entity.Destroy();
                        return;
                    }
                }
            }

            GameEntity InventoryEntity = _contexts.game.GetEntityWithAgent2dInventory(inventoryID);
            int FistEmptySlot = GetFirstEmptySlot(InventoryEntity.agent2dInventory.Slots);
            entity.AddInventoryItem(inventoryID, FistEmptySlot);
            InventoryEntity.agent2dInventory.Slots.Set(FistEmptySlot, true);
        }

        public void RemoveItem(GameEntity entity, int slot)
        {
            GameEntity InventoryEntity = _contexts.game.GetEntityWithAgent2dInventory(entity.inventoryItem.InventoryID);
            InventoryEntity.agent2dInventory.Slots.Set(slot, false);
            entity.RemoveInventoryItem();
        }
        
        public void ChangeSlot()
        {
            // TODO: Allow user change slot in which item is alocatted.
        }

        // TODO: Item selection and event Handling. 
        public void Update()
        {
        }

        private int GetFirstEmptySlot(BitArray Slots)
        {
            BitArray bits = new BitArray(Slots.Count, true);
            if (Slots.Equals(bits))
            {
                return -1; // Inventory is full.
            }

            for (int i = 0; i < Slots.Length; i++)
            {
                if (!Slots[i])
                    return i;
            }

            return -1;  
        }
    }
}