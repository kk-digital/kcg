using Entitas;
using Item;
using Planet;

namespace Admin
{
    // Admin API
    public static class AdminAPI
    {
        // Player position for spawning items near
        static KMath.Vec2f playerPos;

        // Spawn Item Function
        public static ItemEntity SpawnItem(Enums.ItemType itemID, Contexts contexts)
        {
            if(contexts == null)
                return null;

            // Get Player Entites
            IGroup<AgentEntity> entities =
                Contexts.sharedInstance.agent.GetGroup(AgentMatcher.AgentPlayer);
            foreach (var agent in entities)
            {
                // Get Player Position
                if (agent.agentID.ID == 0)
                    playerPos = agent.physicsPosition2D.Value;
            }

            // Spawn Item
            ItemEntity item = GameState.ItemSpawnSystem.SpawnInventoryItem(contexts.item, itemID);

            // Return Item
            return item;
        }
    }
}
