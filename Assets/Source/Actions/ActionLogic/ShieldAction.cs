using Entitas;
using Planet;
using UnityEngine;

namespace Action
{
    public class ShieldAction : ActionBase
    {
        private InventoryEntity InventoryEntity;
        private ItemInventoryEntity ItemEntity;

        public ShieldAction(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
        }

        public override void OnEnter(ref PlanetState planet)
        {
            // Todo: start playing some animation
            if (AgentEntity.hasAgentToolBar)
            {
                int toolBarID = AgentEntity.agentToolBar.ToolBarID;
                InventoryEntity = planet.EntitasContext.inventory.GetEntityWithInventoryID(toolBarID);
                int selectedSlot = InventoryEntity.inventorySlots.Selected;
                ItemEntity = GameState.InventoryManager.GetItemInSlot(planet.EntitasContext.itemInventory, toolBarID, selectedSlot);

                if(ItemEntity.itemType.Type== Enums.ItemType.Sword || ItemEntity.itemType.Type == Enums.ItemType.StunBaton)
                {
                    if (!ActionPropertyEntity.actionPropertyShield.ShieldActive)
                    {
                        ActionPropertyEntity.actionPropertyShield.ShieldActive = true;
                        AgentEntity.physicsMovable.Invulnerable = true;
                    }
                    else
                    {
                        ActionPropertyEntity.actionPropertyShield.ShieldActive = false;
                        AgentEntity.physicsMovable.Invulnerable = false;
                    }
                }


                ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Running);
            }
            ActionEntity.actionExecution.State = Enums.ActionState.Fail;
        }

        public override void OnUpdate(float deltaTime, ref PlanetState planet)
        {

            ActionEntity.actionExecution.State = Enums.ActionState.Success;
        }

        public override void OnExit(ref PlanetState planet)
        {

            base.OnExit(ref planet);
        }
    }


    public class ShieldActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID)
        {
            return new ShieldAction(entitasContext, actionID);
        }
    }
}
