using System;
using KMath;
using Planet;
using UnityEngine;
using System.Collections.Generic;

namespace Action
{
    public class ToolActionMeleeAttack : ActionBase
    {
        private Item.FireWeaponPropreties WeaponProperty;
        private ItemInventoryEntity ItemEntity;

        public ToolActionMeleeAttack(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
        }

        public bool IsInRange(Vector2 currentTarget)
        {
            Vector2 yourPos = new Vector2(AgentEntity.physicsPosition2D.Value.X, AgentEntity.physicsPosition2D.Value.Y);
            return WeaponProperty.Range >= Vector2.Distance(yourPos, currentTarget);
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            ItemEntity = EntitasContext.itemInventory.GetEntityWithItemID(ActionEntity.actionTool.ItemID);
            WeaponProperty = GameState.ItemCreationApi.GetWeapon(ItemEntity.itemType.Type);

            float damage = WeaponProperty.BasicDemage;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = worldPosition.x;
            float y = worldPosition.y;

            // Check if projectile has hit a enemy.
            var entities = EntitasContext.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentID));

            planet.AddFloatingText(WeaponProperty.MeleeAttackFlags.ToString(), 1.0f, new Vec2f(0, 0), new Vec2f(AgentEntity.physicsPosition2D.Value.X + 0.2f, AgentEntity.physicsPosition2D.Value.Y));

            // Todo: Create a agent colision system?
            foreach (var entity in entities)
            {
                if (!entity.isAgentPlayer)
                {
                    float dist = Vector2.Distance(new Vector2(AgentEntity.physicsPosition2D.Value.X, AgentEntity.physicsPosition2D.Value.Y), new Vector2(x, y));

                    if (IsInRange(new Vector2(entity.physicsPosition2D.Value.X, entity.physicsPosition2D.Value.Y)))
                    {
                        Vec2f entityPos = entity.physicsPosition2D.Value;
                        Vec2f bulletPos = new Vec2f(x, y);
                        Vec2f diff = bulletPos - entityPos;
                        diff.Y = 0;
                        diff.Normalize();

                        Vector2 oppositeDirection = new Vector2(-diff.X, -diff.Y);

                        if (AgentEntity.hasAgentStats)
                        {
                            var stats = entity.agentStats;
                            entity.ReplaceAgentStats(stats.Health - (int)damage, stats.Food, stats.Water, stats.Oxygen,
                                stats.Fuel, stats.AttackCooldown);

                            // spawns a debug floating text for damage 
                            planet.AddFloatingText("" + damage, 0.5f, new Vec2f(oppositeDirection.x * 0.05f, oppositeDirection.y * 0.05f), new Vec2f(entityPos.X, entityPos.Y + 0.35f));
                            ActionEntity.actionExecution.State = Enums.ActionState.Success;
                        }
                    }
                }
            }

            ActionEntity.actionExecution.State = Enums.ActionState.Running;

            GameState.ActionCoolDownSystem.SetCoolDown(EntitasContext, ActionEntity.actionID.TypeID, AgentEntity.agentID.ID, WeaponProperty.CoolDown);
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
    public class ToolActionMeleeAttackCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID)
        {
            return new ToolActionMeleeAttack(entitasContext, actionID);
        }
    }
}

