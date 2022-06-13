using System.Collections;

namespace Inventory
{
    public class ManagerSystem
    {
        public Contexts EntitasContext;

        public ManagerSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public ManagerSystem()
        {
            EntitasContext = Contexts.sharedInstance;
        }

        public void OpenInventory(int inventoryID)
        {
            var inventoryEntity = EntitasContext.game.GetEntityWithInventoryID(inventoryID);
            inventoryEntity.isInventoryDrawable = true;
        }

        public void CloseInventory(int inventoryID)
        {
            var inventoryEntity = EntitasContext.game.GetEntityWithInventoryID(inventoryID);
            inventoryEntity.isInventoryDrawable = false;
        }

        public void AddItem(GameEntity entity, int inventoryID)
        {
            var EntityAttribute = EntitasContext.game.GetEntityWithItemAttributes(entity.itemID.ItemType);

            // If stackable check if there are any available stack in the inventory.
            if (EntityAttribute.hasItemAttributeStackable)
            {
                var Group = EntitasContext.game.GetEntitiesWithItemAttachedInventory(inventoryID); // Todo: Use multiple Entity Index. To narrow down the search with item type.

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

            var inventory = EntitasContext.game.GetEntityWithInventoryID(inventoryID);
            int fistEmptySlot = GetFirstEmptySlot(inventory.inventorySlots.Values);
            entity.AddItemAttachedInventory(inventoryID, fistEmptySlot);
            inventory.inventorySlots.Values.Set(fistEmptySlot, true);
        }

        public void RemoveItem(GameEntity entity, int slot)
        {
            var gameEntity = EntitasContext.game.GetEntityWithInventoryID(entity.itemAttachedInventory.InventoryID);
            gameEntity.inventorySlots.Values.Set(slot, false);
            entity.RemoveItemAttachedInventory();
        }
        
        public void ChangeSlot(int inventoryID, int newSelectedSlot)
        {
            GameEntity inventoryEntity = Contexts.sharedInstance.game.GetEntityWithInventoryID(inventoryID);
            var SlotComponent = inventoryEntity.inventorySlots;

            inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, newSelectedSlot);
        }

        // TODO: Use events to update selected slots, pick ups, drops and etc. 
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
