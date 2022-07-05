
namespace Admin
{
    // Admin API
    public static class AdminAPI
    {
        // Spawn Item Function
        public static ItemEntity SpawnItem(Enums.ItemType itemID, Contexts contexts)
        {
            if(contexts == null)
                return null;

            // Spawn Item
            ItemEntity item = GameState.ItemSpawnSystem.SpawnInventoryItem(contexts.item, itemID);

            // Return Item
            return item;
        }

        // Give Item to Active Agent Function
        public static void AddItem(Inventory.InventoryManager manager, int inventoryID, Enums.ItemType itemID, Contexts contexts)
        {
            if (contexts == null)
                return;

            manager.AddItem(contexts, SpawnItem(itemID, contexts), inventoryID);
        }

        // Give Item to Agent Function
        public static void AddItem(Inventory.InventoryManager manager, AgentEntity agentID, Enums.ItemType itemID, Contexts contexts)
        {
            if (contexts == null)
                return;

            manager.AddItem(contexts, SpawnItem(itemID, contexts), agentID.agentInventory.InventoryID);
        }
    }
}
