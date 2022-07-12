using Entitas;
using Planet;
using UnityEngine;

namespace Action
{
    public class ChargeAction : ActionBase
    {
        private Item.FireWeaponPropreties WeaponPropreties;
        private InventoryEntity InventoryEntity;
        private ItemEntity ItemEntity;
        private float tempCharge;

        public ChargeAction(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
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
                ItemEntity = GameState.InventoryManager.GetItemInSlot(planet.EntitasContext.item, toolBarID, selectedSlot);
                WeaponPropreties = GameState.ItemCreationApi.GetWeapon(ItemEntity.itemType.Type);

                // Get Is the item weapon chargable or not
                bool isChargable = ItemEntity.hasItemFireWeaponCharge;

                // Get the old charge rate
                tempCharge = ItemEntity.itemFireWeaponCharge.ChargeRate;

                // If weapon is chargable
                if (isChargable)
                {
                    // Return true
                    ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Running);
                    return;
                }
            }
            // Fail
            ActionEntity.actionExecution.State = Enums.ActionState.Fail;
        }

        public override void OnUpdate(float deltaTime, ref PlanetState planet)
        {
            if (ItemEntity.itemFireWeaponCharge.ChargeRate < ItemEntity.itemFireWeaponCharge.ChargeMax)
            {
                ItemEntity.itemFireWeaponCharge.ChargeRate += 0.1f;
                ActionEntity.actionExecution.State = Enums.ActionState.Success;
            }
        }

        public override void OnExit(ref PlanetState planet)
        {
            float difference = ItemEntity.itemFireWeaponCharge.ChargeRate - tempCharge;
            if (ActionEntity.actionExecution.State == Enums.ActionState.Fail)
            {
                Debug.Log("Reload Failed.");
            }
            else
            {
                Debug.Log("Weapon Charged: " + difference.ToString());
            }

            base.OnExit(ref planet);
        }
    }
}
