using Item;
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

        public void AddItem(Contexts contexts, ItemInventoryEntity entity, int inventoryID)
        {
            var inventory = contexts.inventory.GetEntityWithInventoryID(inventoryID);

            ItemProprieties proprieties = GameState.ItemCreationApi.Get(entity.itemType.Type);

            // If stackable check if there are any available stack in the inventory.
            if (proprieties.IsStackable())
            {
                var Group = contexts.itemInventory.GetEntitiesWithItemInventory(inventoryID); // Todo: Use multiple Entity Index. To narrow down the search with item type.

                int NewEntityCount = 1;
                if (entity.hasItemStack)
                    NewEntityCount = entity.itemStack.Count;

                foreach (ItemInventoryEntity entityIT in Group)
                {
                    if (entityIT.itemType.Type != entity.itemType.Type)
                    {
                        continue;
                    }
                    
                    int EntityITCount = 1;
                    int MaxStackSize = 64;
                    if (entityIT.hasItemStack)
                    {
                        EntityITCount = entityIT.itemStack.Count;
                        if (EntityITCount == MaxStackSize)
                            continue;
                    }
                    else
                    {
                        entityIT.AddItemStack(NewEntityCount + EntityITCount);
                        entity.Destroy();
                        return;
                    }
                    
                    if (NewEntityCount + EntityITCount <= MaxStackSize)
                    {
                        entityIT.ReplaceItemStack(NewEntityCount + EntityITCount);
                        entity.Destroy();
                        return;
                    }
                }
            }

            int fistEmptySlot = GetFirstEmptySlot(inventory.inventorySlots.Values);
            entity.AddItemInventory(inventoryID, fistEmptySlot);
            inventory.inventorySlots.Values.Set(fistEmptySlot, true);
        }

        public void PickUp(Contexts entitasContext, ItemParticleEntity entity, int inventoryID)
        {
            AddItem(entitasContext, GameState.ItemSpawnSystem.SpawnInventoryItem(entitasContext, entity), 
                inventoryID);
        }

        public void RemoveItem(Contexts contexts, ItemInventoryEntity entity, int slot)
        {
            var inventoryEntity = contexts.inventory.GetEntityWithInventoryID(entity.itemInventory.InventoryID);
            inventoryEntity.inventorySlots.Values.Set(slot, false);
            entity.itemInventory.InventoryID = -1;
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

        public ItemInventoryEntity GetItemInSlot(ItemInventoryContext itemContext, int inventoryID, int slot)
        {
            var items = itemContext.GetEntitiesWithItemInventory(inventoryID);

            foreach (var item in items)
            {
                if (item.itemInventory.SlotNumber == slot)
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
