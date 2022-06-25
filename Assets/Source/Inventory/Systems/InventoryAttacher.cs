using System.Collections;
using Entitas;
using UnityEngine;


namespace Inventory
{
    public class InventoryAttacher
    {
        private static InventoryAttacher instance;
        public static InventoryAttacher Instance => instance ??= new InventoryAttacher();

        public Contexts EntitasContext = Contexts.sharedInstance;

        private static int InventoryID = 0;

        public void AttachInventoryToAgent(int width, int height, int AgentID)
        {
            GameEntity entity = EntitasContext.game.GetEntityWithAgentID(AgentID);
            entity.AddAgentInventory(InventoryID);
            MakeInventoryEntity(width, height);
        }

        public void AttachInventorytoItem(int width, int height, int ItemID)
        {
            GameEntity entity = EntitasContext.game.GetEntityWithItemIDID(ItemID);
            entity.AddAgentInventory(InventoryID);
            MakeInventoryEntity(width, height);
        }

        public void AttachToolBarToPlayer(int size, int AgentID)
        {
            GameEntity playerEntity = EntitasContext.game.GetEntityWithAgentID(AgentID);
            playerEntity.AddAgentToolBar(InventoryID);
            InventoryEntity entity = MakeInventoryEntity(size, 1);
            entity.isInventoryToolBar = true;
            entity.isInventoryDrawable = true;
        }

        private InventoryEntity MakeInventoryEntity(int width, int height)
        {
            InventoryEntity entity = EntitasContext.inventory.CreateEntity();
            entity.AddInventoryID(InventoryID++);
            entity.AddInventorySize(width, height);
            BitArray bitArray = new BitArray(width* height, false);
            entity.AddInventorySlots(bitArray, 0);
            
            return entity;
        }
    }
}
