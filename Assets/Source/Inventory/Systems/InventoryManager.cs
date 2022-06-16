using System.Collections;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager
    {
        public InventoryManager()
        {
        }

        public void OpenInventory(int inventoryID)
        {
            var inventory = Contexts.sharedInstance.game.GetEntityWithInventoryID(inventoryID);
            inventory.isInventoryDrawable = true;
        }

        public void CloseInventory(int inventoryID)
        {
            var inventory = Contexts.sharedInstance.game.GetEntityWithInventoryID(inventoryID);

            inventory.isInventoryDrawable = false;
        }

        public void AddItem(GameEntity entity, int inventoryID)
        {
            var inventory = Contexts.sharedInstance.game.GetEntityWithInventoryID(inventoryID);

            var EntityAttribute = Contexts.sharedInstance.game.GetEntityWithItemAttributes(entity.itemID.ItemType);

            // If stackable check if there are any available stack in the inventory.
            if (EntityAttribute.hasItemAttributeStackable)
            {
                var Group = Contexts.sharedInstance.game.GetEntitiesWithItemAttachedInventory(inventoryID); // Todo: Use multiple Entity Index. To narrow down the search with item type.

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
                        if (EntityITCount == EntityAttribute.itemAttributeStackable.MaxStackSize)
                            continue;
                    }
                    else
                    {
                        entityIT.AddItemStack(NewEntityCount + EntityITCount);
                        entity.Destroy();
                        return;
                    }
                    
                    if (NewEntityCount + EntityITCount <= EntityAttribute.itemAttributeStackable.MaxStackSize)
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

        public void PickUp(GameEntity entity, int inventoryID)
        {
            entity.RemovePhysicsPosition2D();
            entity.RemovePhysicsMovable();
            entity.RemovePhysicsBox2DCollider();
            AddItem(entity, inventoryID);
        }

        public void RemoveItem(GameEntity entity, int slot)
        {
            var inventoryEntity = Contexts.sharedInstance.game.GetEntityWithInventoryID(entity.itemAttachedInventory.InventoryID);
            inventoryEntity.inventorySlots.Values.Set(slot, false);
            entity.RemoveItemAttachedInventory();
        }
        
        public void ChangeSlot(int newSelectedSlot, int inventoryID)
        {
            var inventory = Contexts.sharedInstance.game.GetEntityWithInventoryID(inventoryID);
            var SlotComponent = inventory.inventorySlots;

            inventory.ReplaceInventorySlots(SlotComponent.Values, newSelectedSlot);
        }

        public bool IsFull(int inventoryID)
        {
            GameEntity inventoryEntity = Contexts.sharedInstance.game.GetEntityWithInventoryID(inventoryID);
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

        public GameEntity GetItemInSlot(int inventoryID, int slot)
        {
            var items = Contexts.sharedInstance.game.GetEntitiesWithItemAttachedInventory(inventoryID);

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
