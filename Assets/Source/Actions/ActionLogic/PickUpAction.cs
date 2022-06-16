using Entitas;
using UnityEngine;

namespace Action
{
    public class PickUpAction : ActionBase
    {
        public override void OnEnter(int actionID, int agentID)
        {
            // Doing everything here for now.

            var gameContext = Contexts.sharedInstance.game;
            GameEntity agentEntity = gameContext.GetEntityWithAgentID(agentID);
            GameEntity actionEntity = gameContext.GetEntityWithActionID(actionID);

            var items = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.ItemID, GameMatcher.PhysicsPosition2D));

            Vector2 agentPos = agentEntity.physicsPosition2D.Value;
            GameEntity pickableItem = null;
            foreach (var item in items)
            {
                if ((agentPos - item.physicsPosition2D.Value).magnitude <= 0.5)
                {
                    pickableItem = item;
                    break;
                }
            }

            if (pickableItem == null)
            {
                actionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                return;
            }

            // Check space in toolBar
            if (agentEntity.hasAgentToolBar)
            {
                int toolBarID = agentEntity.agentToolBar.ToolBarID;

                // Try ading item to toolBar.
                if (!GameState.InventoryManager.IsFull(toolBarID))
                {
                    GameState.InventoryManager.PickUp(pickableItem, toolBarID);
                    actionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
                    return;
                }

            }

            if (agentEntity.hasAgentInventory)
            {
                int inventoryID = agentEntity.agentInventory.InventoryID;

                // Try ading item to Inventory.
                if (!GameState.InventoryManager.IsFull(inventoryID))
                {
                    GameState.InventoryManager.PickUp(pickableItem, inventoryID);
                    actionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
                    return;
                }
            }

            // Inventory and ToolBar full or non existent. 
            actionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
        }
    }
}
