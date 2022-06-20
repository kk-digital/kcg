using Entitas;
using System.Windows.Forms;
using UnityEngine;
using KMath;

namespace Action
{
    public class PickUpAction : ActionBase
    {
        private GameEntity ItemEntity;
        private float Speed = 3.0f;
        private float aceleration = 0.5f;

        public PickUpAction(int actionID, int agentID, int itemID) : base(actionID, agentID)
        {
            ItemEntity = Contexts.sharedInstance.game.GetEntityWithItemIDID(itemID);

        }

        public override void OnEnter()
        {

#if DEBUG
            // Item Doesnt Exist
            if (ItemEntity == null)
            {
                ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                return;
            }

            // Check if Agent has too bar or an inventory.
            if (!(AgentEntity.hasAgentToolBar || AgentEntity.hasAgentInventory))
            {
                ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                return;
            }
#endif

            Vec2f drawPos = ItemEntity.physicsPosition2D.Value;
            ItemEntity.ReplaceItemDrawPosition2D(drawPos, Vec2f.Zero);

            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Active);
        }

        public override void OnUpdate(float deltaTime)
        {
            // Update item pos.

            // Center position Item.
            Vec2f itemSize = Contexts.sharedInstance.game.GetEntityWithItemAttributes(ItemEntity.itemID.ItemType).itemAttributeSize.Size;
            Vec2f itemCenterPos = ItemEntity.itemDrawPosition2D.Value + itemSize / 2.0f;
            Vec2f agentCenterPos = AgentEntity.physicsPosition2D.Value + new Vec2f(1.0f, 1.5f)/2f; // Todo: Add agentSizeCompenent

            if ((itemCenterPos - agentCenterPos).Magnitude < 0.1f)
            {
                if (AgentEntity.hasAgentToolBar)
                {
                    int toolBarID = AgentEntity.agentToolBar.ToolBarID;

                    // Try ading item to toolBar.
                    if (!GameState.InventoryManager.IsFull(toolBarID))
                    {
                        GameState.InventoryManager.PickUp(ItemEntity, toolBarID);
                        ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
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

                // Inventory and toolbar are full.
                ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
            }

            Speed += aceleration * deltaTime;
            float speed = Speed * deltaTime;

            // Update Draw Position.
            Vec2f mov = (agentCenterPos - itemCenterPos).Normalized * speed;
            ItemEntity.ReplaceItemDrawPosition2D(ItemEntity.itemDrawPosition2D.Value + mov, ItemEntity.itemDrawPosition2D.Value);
        }

        public override void OnExit()
        {
            ItemEntity.RemoveItemDrawPosition2D();
            base.OnExit();
        }
    }
}
