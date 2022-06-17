using Entitas;
using UnityEngine;

namespace Action
{
    public class DropAction : ActionBase
    {
        public DropAction(int actionID, int agentID) : base(actionID, agentID)
        { 
        }

        public override void OnEnter()
        {
            // Doing everything here for now.

            var gameContext = Contexts.sharedInstance.game;

            if (AgentEntity.hasAgentToolBar)
            {
                int toolBarID = AgentEntity.agentToolBar.ToolBarID;
                GameEntity toolBarEntity = gameContext.GetEntityWithInventoryID(toolBarID);

                int selected = toolBarEntity.inventorySlots.Selected;

                // Try ading item to toolBar.
                if (!GameState.InventoryManager.IsFull(toolBarID))
                {
                    GameEntity item = GameState.InventoryManager.GetItemInSlot(toolBarID, selected);
                    if (item == null)
                    {
                        ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                        return;
                    }
                    GameState.InventoryManager.RemoveItem(item, selected);

                    Vector2 pos = AgentEntity.physicsPosition2D.Value;
                    Vector2 size = Contexts.sharedInstance.game.GetEntityWithItemAttributes(item.itemID.ItemType).itemAttributeSize.Size;

                    item.AddPhysicsPosition2D(pos, pos);
                    item.AddPhysicsBox2DCollider(size, Vector2.zero);
                    item.AddPhysicsMovable(0f, Vector2.zero, Vector2.zero, 0f);
                    ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
                    return;
                }

            }
            // Inventory and ToolBar full or non existent. 
            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
        }
    }
}
