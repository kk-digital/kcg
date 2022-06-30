using System.Collections;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager
    {
        public InventoryManager()
        {
        }

        public void OpenInventory(Contexts contexts, int inventoryID)
        {
            var inventory = contexts.inventory.GetEntityWithInventoryID(inventoryID);
            inventory.isInventoryDrawable = true;
        }

        public void CloseInventory(Contexts entitasContext, int inventoryID)
        {
            var inventory = entitasContext.inventory.GetEntityWithInventoryID(inventoryID);

            inventory.isInventoryDrawable = false;
        }

        public void AddItem(Contexts contexts, GameEntity entity, int inventoryID)
        {
            var inventory = contexts.inventory.GetEntityWithInventoryID(inventoryID);

            var EntityAttribute = contexts.itemProperties.GetEntityWithItemProperty(entity.itemID.ItemType);

            // If stackable check if there are any available stack in the inventory.
            if (EntityAttribute.hasItemPropertyStackable)
            {
                var Group = contexts.game.GetEntitiesWithItemAttachedInventory(inventoryID); // Todo: Use multiple Entity Index. To narrow down the search with item type.

                int NewEntityCount = 1;
                if (entity.hasItemStack)
                    NewEntityCount = entity.itemStack.Count;

                foreach (GameEntity entityIT in Group)
                {
                    if (entityIT.itemID.ItemType != entity.itemID.ItemType)
                    {
                        continue;
                    }
                    
                    int EntityITCount = 1;
                    if (entityIT.hasItemStack)
                    {
                        EntityITCount = entityIT.itemStack.Count;
                        if (EntityITCount == EntityAttribute.itemPropertyStackable.MaxStackSize)
                            continue;
                    }
                    else
                    {
                        entityIT.AddItemStack(NewEntityCount + EntityITCount);
                        entity.Destroy();
                        return;
                    }
                    
                    if (NewEntityCount + EntityITCount <= EntityAttribute.itemPropertyStackable.MaxStackSize)
                    {
                        entityIT.ReplaceItemStack(NewEntityCount + EntityITCount);
                        entity.Destroy();
                        return;
                    }
                }
            }

            int fistEmptySlot = GetFirstEmptySlot(inventory.inventorySlots.Values);
            entity.AddItemAttachedInventory(inventoryID, fistEmptySlot);
            inventory.inventorySlots.Values.Set(fistEmptySlot, true);
        }

        public void PickUp(Contexts entitasContext, GameEntity entity, int inventoryID)
        {
            entity.RemovePhysicsPosition2D();
            entity.RemovePhysicsMovable();
            entity.RemovePhysicsBox2DCollider();
            AddItem(entitasContext, entity, inventoryID);
        }

        public void RemoveItem(Contexts contexts, GameEntity entity, int slot)
        {
            var inventoryEntity = contexts.inventory.GetEntityWithInventoryID(entity.itemAttachedInventory.InventoryID);
            inventoryEntity.inventorySlots.Values.Set(slot, false);
            entity.RemoveItemAttachedInventory();
        }
        
        public void ChangeSlot(Contexts contexts, int newSelectedSlot, int inventoryID)
        {
            var inventory = contexts.inventory.GetEntityWithInventoryID(inventoryID);
            var SlotComponent = inventory.inventorySlots;

            inventory.ReplaceInventorySlots(SlotComponent.Values, newSelectedSlot);
        }

        public bool IsFull(Contexts contexts, int inventoryID)
        {
            InventoryEntity inventoryEntity = contexts.inventory.GetEntityWithInventoryID(inventoryID);
            BitArray Slots = inventoryEntity.inventorySlots.Values;
            if (IsFull(Slots)) // Test if all bits are set to one.
                return true;

            return false;
        }

        private bool IsFull(BitArray Slots)
        {
            BitArray bits = new BitArray(Slots.Count, true);
            if (Slots.Equals(bits))
            {
                return true; // Inventory is full.
            }
            return false;
        }

        public GameEntity GetItemInSlot(GameContext gameContext, int inventoryID, int slot)
        {
            var items = gameContext.GetEntitiesWithItemAttachedInventory(inventoryID);

            foreach (var item in items)
            {
                if (item.itemAttachedInventory.SlotNumber == slot)
                {
                    return item;
                }
            }

            return null; // No item in selected slot.
        }

        private int GetFirstEmptySlot(BitArray Slots)
        {
            if (IsFull(Slots))
            {
                return -1;
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
