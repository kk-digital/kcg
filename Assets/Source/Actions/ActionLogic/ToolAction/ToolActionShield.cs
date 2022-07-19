using System;
using KMath;
using Planet;
using UnityEngine;
using System.Collections.Generic;

namespace Action
{
    public class ToolActionShield : ActionBase
    {
        // Weapon Property
        private Item.FireWeaponPropreties WeaponProperty;

        // Item Entity
        private ItemInventoryEntity ItemEntity;

        // Constructor
        public ToolActionShield(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            // Initialize Item Entity
            ItemEntity = EntitasContext.itemInventory.GetEntityWithItemID(ActionEntity.actionTool.ItemID);

            // Initialize Weapon Property
            WeaponProperty = GameState.ItemCreationApi.GetWeapon(ItemEntity.itemType.Type);

            // If Shield Is Active
            if (!ActionPropertyEntity.actionPropertyShield.ShieldActive)
            {
                // Set Shield True
                ActionPropertyEntity.actionPropertyShield.ShieldActive = true;

                // Set Invulnerable True
                AgentEntity.physicsMovable.Invulnerable = true;
            }
            else
            {
                // Set Shield False
                ActionPropertyEntity.actionPropertyShield.ShieldActive = false;

                // Set Invulnerable False
                AgentEntity.physicsMovable.Invulnerable = false;
            }

            // Execute Update
            ActionEntity.actionExecution.State = Enums.ActionState.Running;
        }

        public override void OnUpdate(float deltaTime, ref Planet.PlanetState planet)
        {
            // Execute Exit
            ActionEntity.actionExecution.State = Enums.ActionState.Success;
        }

        public override void OnExit(ref PlanetState planet)
        {
            // Exit()
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

