using Entitas;
using Planet;
using UnityEngine;

namespace Action
{
    public class ReloadAction : ActionBase
    {
        private Item.FireWeaponPropreties WeaponPropreties;
        private InventoryEntity InventoryEntity;
        private ItemInventoryEntity ItemEntity;
        private float runningTime = 0f;

        public ReloadAction(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
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
                WeaponPropreties = GameState.ItemCreationApi.GetWeapon(ItemEntity.itemType.Type);

                bool isReloadable = ItemEntity.hasItemFireWeaponClip;

                if (isReloadable)
                {
                    ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Running);
                    return;
                }
            }
            ActionEntity.actionExecution.State = Enums.ActionState.Fail;
        }

        public override void OnUpdate(float deltaTime, ref PlanetState planet)
        {
            runningTime += deltaTime;
            if (runningTime >= WeaponPropreties.ReloadTime)
            {
                ItemEntity.itemFireWeaponClip.NumOfBullets = WeaponPropreties.ClipSize;
                ActionEntity.actionExecution.State =  Enums.ActionState.Success;
            }
        }

        public override void OnExit(ref PlanetState planet)
        {
            if (ActionEntity.actionExecution.State == Enums.ActionState.Fail)
            {
                Debug.Log("Reload Failed.");
            }
            else
            {
                Debug.Log("Weapon Reloaded." + ItemEntity.itemFireWeaponClip.NumOfBullets.ToString() + " Ammo in the clip.");
            }

            base.OnExit(ref planet);
        }
    }


    public class ReloadActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID)
        {
            return new ReloadAction(entitasContext, actionID);
        }
    }
}
