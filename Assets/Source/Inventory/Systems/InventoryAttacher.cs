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

        public void AttachInventoryToAgent(GameContext gameContext, int width, int height, GameEntity agentEntity)
        {
            GameEntity entity = agentEntity;
            entity.AddAgentInventory(InventoryID);
            MakeInventoryEntity(width, height);
        }

        public void AttachInventorytoItem(GameContext gameContext, int width, int height, int ItemID)
        {
            GameEntity entity = gameContext.GetEntityWithItemIDID(ItemID);
            entity.AddAgentInventory(InventoryID);
            MakeInventoryEntity(width, height);
        }

        public void AttachToolBarToPlayer(GameContext gameContext, int size, GameEntity agentEntity)
        {
            GameEntity playerEntity = agentEntity;
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
