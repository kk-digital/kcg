using System.Collections;

namespace Agent
{
    public class InventoryManagerSystem
    {
        public Contexts _contexts;

        public InventoryManagerSystem(Contexts contexts)
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

            var InventoryEntity = _contexts.inventory.GetEntityWithInventory2D(inventoryID);
            int FistEmptySlot = GetFirstEmptySlot(InventoryEntity.inventory2D.Slots);
            entity.AddInventoryItem(inventoryID, FistEmptySlot);
            InventoryEntity.inventory2D.Slots.Set(FistEmptySlot, true);
        }

        public void RemoveItem(GameEntity entity, int slot)
        {
            var agent = _contexts.inventory.GetEntityWithInventory2D(entity.inventoryItem.InventoryID);
            agent.inventory2D.Slots.Set(slot, false);
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