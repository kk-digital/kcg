using Entitas;
using UnityEngine;

namespace Action
{
    public class DropAction : ActionBase
    {
        public override void OnEnter(int actionID, int agentID)
        {
            // Doing everything here for now.

            var gameContext = Contexts.sharedInstance.game;
            GameEntity agentEntity = gameContext.GetEntityWithAgentID(agentID);
            GameEntity actionEntity = gameContext.GetEntityWithActionID(actionID);

            if (agentEntity.hasAgentToolBar)
            {
                int toolBarID = agentEntity.agentToolBar.ToolBarID;
                GameEntity toolBarEntity = gameContext.GetEntityWithInventoryID(toolBarID);

                int selected = toolBarEntity.inventorySlots.Selected;

                // Try ading item to toolBar.
                if (!GameState.InventoryManager.IsFull(toolBarID))
                {
                    GameEntity item = GameState.InventoryManager.GetItemInSlot(toolBarID, selected);
                    if (item == null)
                    {
                        actionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                        return;
                    }
                    GameState.InventoryManager.RemoveItem(item, selected);

                    Vector2 pos = agentEntity.physicsPosition2D.Value;
                    Vector2 size = Contexts.sharedInstance.game.GetEntityWithItemAttributes(item.itemID.ItemType).itemAttributeSize.Size;

                    item.AddPhysicsPosition2D(pos, pos);
                    item.AddPhysicsBox2DCollider(size, Vector2.zero);
                    item.AddPhysicsMovable(0f, Vector2.zero, Vector2.zero, 0f);
                    actionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
                    return;
                }

            }
            // Inventory and ToolBar full or non existent. 
            actionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
        }
    }
}
