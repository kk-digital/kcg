using System;
using KMath;
using Planet;
using UnityEngine;
using System.Collections.Generic;

namespace Action
{
    public class ToolActionShield : ActionBase
    {
        private Item.FireWeaponPropreties WeaponProperty;
        private ItemInventoryEntity ItemEntity;

        public ToolActionShield(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            ItemEntity = EntitasContext.itemInventory.GetEntityWithItemID(ActionEntity.actionTool.ItemID);
            WeaponProperty = GameState.ItemCreationApi.GetWeapon(ItemEntity.itemType.Type);

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

            ActionEntity.actionExecution.State = Enums.ActionState.Running;
        }

        public override void OnUpdate(float deltaTime, ref Planet.PlanetState planet)
        {
            ActionEntity.actionExecution.State = Enums.ActionState.Success;
        }

        public override void OnExit(ref PlanetState planet)
        {
            base.OnExit(ref planet);
        }
    }

    /// <summary>
    /// Factory Method
    /// </summary>
    public class ToolActionShieldCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID)
        {
            return new ToolActionShield(entitasContext, actionID);
        }
    }
}

