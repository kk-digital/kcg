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

        public override void OnEnter()
        {
            var gameContext = Contexts.sharedInstance.game;

            if (AgentEntity.hasAgentToolBar)
            {
                int toolBarID = AgentEntity.agentToolBar.ToolBarID;
                GameEntity toolBarEntity = gameContext.GetEntityWithInventoryID(toolBarID);

                int selected = toolBarEntity.inventorySlots.Selected;


                ItemEntity = GameState.InventoryManager.GetItemInSlot(toolBarID, selected);
                if (ItemEntity == null)
                {
                    ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                    return;
                }
                GameState.InventoryManager.RemoveItem(ItemEntity, selected);
             
                Vec2f pos = AgentEntity.physicsPosition2D.Value;
                Vec2f size = Contexts.sharedInstance.game.GetEntityWithItemAttributes(ItemEntity.itemID.ItemType).itemAttributeSize.Size;

                ItemEntity.AddPhysicsPosition2D(pos, pos);
                ItemEntity.AddPhysicsBox2DCollider(size, Vec2f.Zero);
                ItemEntity.AddPhysicsMovable(0.0f, new Vec2f(-30.0f, 20.0f), Vec2f.Zero);
                ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Running);
                return;

            }
            // ToolBar is non existent. 
            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
        }

        public override void OnUpdate(float deltaTime)
        {
            ActionEntity.ReplaceActionTime(ActionEntity.actionTime.StartTime + deltaTime);
            if (ActionEntity.actionTime.StartTime < ActionAttributeEntity.actionAttributeTime.Duration)
            {
                return;
            }

            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
        }

        public override void OnExit()
        {
            if(ActionEntity.actionExecution.State == Enums.ActionState.Success)
                ItemEntity.isItemUnpickable = false;
            base.OnExit();
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
