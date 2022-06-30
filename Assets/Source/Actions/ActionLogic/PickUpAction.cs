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

        public PickUpAction(Contexts entitasContext, int actionID, int agentID) : base(entitasContext, actionID, agentID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            ItemEntity = EntitasContext.game.GetEntityWithItemIDID(ActionEntity.actionItem.ItemID);

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
            ItemEntity.AddItemDrawPosition2D(drawPos, Vec2f.Zero);
            ItemEntity.isItemUnpickable = true;

            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Running);
        }

        public override void OnUpdate(float deltaTime, ref Planet.PlanetState planet)
        {
            // Update item pos.

            // Center position Item.
            Vec2f itemSize = EntitasContext.itemProperties.GetEntityWithItemProperty(ItemEntity.itemID.ItemType).itemPropertySize.Size;
            Vec2f itemCenterPos = ItemEntity.itemDrawPosition2D.Value + itemSize / 2.0f;
            Vec2f agentCenterPos = AgentEntity.physicsPosition2D.Value + new Vec2f(1.0f, 1.5f)/2f; // Todo: Add agentSizeCompenent

            if ((itemCenterPos - agentCenterPos).Magnitude < 0.1f)
            {
                if (AgentEntity.hasAgentToolBar)
                {
                    int toolBarID = AgentEntity.agentToolBar.ToolBarID;

                    // Try ading item to toolBar.
                    if (!GameState.InventoryManager.IsFull(EntitasContext, toolBarID))
                    {
                        GameState.InventoryManager.PickUp(EntitasContext, ItemEntity, toolBarID);
                        ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
                        return;
                    }
                }

                if (AgentEntity.hasAgentInventory)
                {
                    int inventoryID = AgentEntity.agentInventory.InventoryID;

                    // Try ading item to Inventory.
                    if (!GameState.InventoryManager.IsFull(EntitasContext, inventoryID))
                    {
                        GameState.InventoryManager.PickUp(EntitasContext, ItemEntity, inventoryID);
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

        public override void OnExit(ref Planet.PlanetState planet)
        {
            ItemEntity.RemoveItemDrawPosition2D();
            base.OnExit(ref planet);
        }
    }

    public class PickUpActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID, int agentID)
        {
            return new PickUpAction(entitasContext, actionID, agentID);
        }
    }
}
