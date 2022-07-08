using Entitas;
using UnityEngine;
using KMath;
using Enums;

namespace Action
{
    public class DropAction : ActionBase
    {
        private ItemInventoryEntity ItemEntity;

        public DropAction(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            if (AgentEntity.hasAgentToolBar)
            {
                int toolBarID = AgentEntity.agentToolBar.ToolBarID;
                InventoryEntity toolBarEntity = EntitasContext.inventory.GetEntityWithInventoryID(toolBarID);

                int selected = toolBarEntity.inventorySlots.Selected;


                ItemEntity = GameState.InventoryManager.GetItemInSlot(planet.EntitasContext.itemInventory, toolBarID, selected);
                if (ItemEntity == null)
                {
                    ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                    return;
                }
                GameState.InventoryManager.RemoveItem(planet.EntitasContext, ItemEntity, selected);
             
                Vec2f pos = AgentEntity.physicsPosition2D.Value;
                GameState.ItemSpawnSystem.SpawnItemParticle(planet.EntitasContext, ItemEntity, pos);

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
        public override ActionBase CreateAction(Contexts entitasContext, int actionID)
        {
            return new DropAction(entitasContext, actionID);
        }
    }
}
