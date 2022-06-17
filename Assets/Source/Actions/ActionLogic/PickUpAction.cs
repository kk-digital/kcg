using Entitas;
using System.Windows.Forms;
using UnityEngine;

namespace Action
{
    public class PickUpAction : ActionBase
    {
        private GameEntity ItemEntity;

        public PickUpAction(int actionID, int agentID, int itemID) : base(actionID, agentID)
        {
            ItemEntity = Contexts.sharedInstance.game.GetEntityWithItemIDID(itemID);

        }

        public override void OnEnter()
        {
            // ItemDoesnt Exist.
            if (ItemEntity == null)
            {
                ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                return;
            }

            // Check space in toolBar
            if (AgentEntity.hasAgentToolBar)
            {
                int toolBarID = AgentEntity.agentToolBar.ToolBarID;

                // Try ading item to toolBar.
                if (!GameState.InventoryManager.IsFull(toolBarID))
                {
                    GameState.InventoryManager.PickUp(ItemEntity, toolBarID);
                    AgentEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
                    return;
                }

            }

            if (AgentEntity.hasAgentInventory)
            {
                int inventoryID = AgentEntity.agentInventory.InventoryID;

                // Try ading item to Inventory.
                if (!GameState.InventoryManager.IsFull(inventoryID))
                {
                    GameState.InventoryManager.PickUp(ItemEntity, inventoryID);
                    ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
                    return;
                }
            }

            // Inventory and ToolBar full or non existent. 
            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
        }
    }
}
