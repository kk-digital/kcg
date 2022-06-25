using Entitas;
using UnityEngine;
using KMath;

namespace Action
{
    public class DropAction : ActionBase
    {
        private GameEntity ItemEntity;

        public DropAction(int actionID, int agentID) : base(actionID, agentID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            var EntitasContext = Contexts.sharedInstance;

            if (AgentEntity.hasAgentToolBar)
            {
                int toolBarID = AgentEntity.agentToolBar.ToolBarID;
                InventoryEntity toolBarEntity = EntitasContext.inventory.GetEntityWithInventoryID(toolBarID);

                int selected = toolBarEntity.inventorySlots.Selected;


                ItemEntity = GameState.InventoryManager.GetItemInSlot(toolBarID, selected);
                if (ItemEntity == null)
                {
                    ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                    return;
                }
                GameState.InventoryManager.RemoveItem(ItemEntity, selected);
             
                Vec2f pos = AgentEntity.physicsPosition2D.Value;
                Vec2f size = Contexts.sharedInstance.itemProperties.GetEntityWithItemProperty(ItemEntity.itemID.ItemType).itemPropertySize.Size;

                ItemEntity.AddPhysicsPosition2D(pos, pos);
                ItemEntity.AddPhysicsBox2DCollider(size, Vec2f.Zero);
                ItemEntity.AddPhysicsMovable(0.0f, new Vec2f(-30.0f, 20.0f), Vec2f.Zero);
                ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Running);
                return;

            }
            // ToolBar is non existent. 
            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
        }

        public override void OnUpdate(float deltaTime, ref Planet.PlanetState planet)
        {
            ActionEntity.ReplaceActionTime(ActionEntity.actionTime.StartTime + deltaTime);
            if (ActionEntity.actionTime.StartTime < ActionPropertyEntity.actionPropertyTime.Duration)
            {
                return;
            }

            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
        }

        public override void OnExit(ref Planet.PlanetState planet)
        {
            if(ActionEntity.actionExecution.State == Enums.ActionState.Success)
                ItemEntity.isItemUnpickable = false;
            base.OnExit(ref planet);
        }
    }
    // Factory Method
    public class DropActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(int actionID, int agentID)
        {
            return new DropAction(actionID, agentID);
        }
    }
}
