using System.Collections;
using Entitas;
using UnityEngine;


namespace Inventory
{
    public class InventoryAttacher
    {
        private static InventoryAttacher instance;
        public static InventoryAttacher Instance => instance ??= new InventoryAttacher();

        private static int InventoryID = 0;

        public void AttachInventoryToAgent(Contexts entitasContext, int width, int height, AgentEntity agentEntity)
        {
            AgentEntity entity = agentEntity;
            entity.AddAgentInventory(InventoryID);
            MakeInventoryEntity(entitasContext, width, height);
        }

        public void AttachInventorytoItem(Contexts entitasContext, int width, int height, int ItemID)
        {
            ItemEntity entity = entitasContext.item.GetEntityWithItemID(ItemID);
            entity.AddItemInventory(InventoryID);
            MakeInventoryEntity(entitasContext, width, height);
        }

        public void AttachToolBarToPlayer(Contexts entitasContext, int size, AgentEntity agentEntity)
        {
            AgentEntity playerEntity = agentEntity;
            playerEntity.AddAgentToolBar(InventoryID);
            InventoryEntity entity = MakeInventoryEntity(entitasContext, size, 1);
            entity.inventorySize.Width = 5;

            entity.isInventoryToolBar = true;
            entity.isInventoryDrawable = true;
        }

        private InventoryEntity MakeInventoryEntity(Contexts entitasContext, int width, int height)
        {
            InventoryEntity entity = entitasContext.inventory.CreateEntity();
            entity.AddInventoryID(InventoryID++);
            entity.AddInventorySize(width, height);
            BitArray bitArray = new BitArray(width* height, false);
            entity.AddInventorySlots(bitArray, 0);


            
            return entity;
        }
    }
}
