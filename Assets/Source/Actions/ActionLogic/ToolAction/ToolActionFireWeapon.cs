using System;
using KMath;
using Planet;
using UnityEngine;
using System.Collections;

namespace Action
{
    public class ToolActionFireWeapon : ActionBase
    {
        private Item.FireWeaponPropreties WeaponProperty; 
        private ProjectileEntity ProjectileEntity;
        private ItemInventoryEntity ItemEntity;
        private Vec2f StartPos;

        public ToolActionFireWeapon(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            ItemEntity = EntitasContext.itemInventory.GetEntityWithItemID(ActionEntity.actionTool.ItemID);
            WeaponProperty = GameState.ItemCreationApi.GetWeapon(ItemEntity.itemType.Type);

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = worldPosition.x;
            float y = worldPosition.y;


            int numBullet = ItemEntity.itemFireWeaponClip.NumOfBullets;
            if (numBullet == 0)
            {
                Debug.Log("Clip is empty. Press R to reload.");
                ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                return;
            }

            ItemEntity.itemFireWeaponClip.NumOfBullets = (--numBullet);

            // Start positiom
            StartPos = AgentEntity.physicsPosition2D.Value;
            StartPos.X += 0.3f;
            StartPos.Y += 0.5f;

            ProjectileEntity = planet.AddProjectile(StartPos, new Vec2f(x - StartPos.X, y - StartPos.Y).Normalized, Enums.ProjectileType.Bullet);

            /* ProjectileEntity = GameState.ProjectileSpawnerSystem.SpawnBullet(GameResources.OreIcon, 4, 4, StartPos, 
                 new Vec2f(x - StartPos.X, y - StartPos.Y).Normalized * speed, Vec2f.Zero, Enums.ProjectileType.Bullet, Enums.ProjectileDrawType.Standard);*/

            ActionEntity.actionExecution.State = Enums.ActionState.Running;

            GameState.ActionCoolDownSystem.SetCoolDown(EntitasContext, ActionEntity.actionID.TypeID, AgentEntity.agentID.ID, WeaponProperty.CoolDown);
        }

        public override void OnUpdate(float deltaTime, ref Planet.PlanetState planet)
        {
            float range = WeaponProperty.Range;
            float damage = WeaponProperty.BasicDemage;

            // Check if projectile has hit something and was destroyed.
            if (!ProjectileEntity.isEnabled)
            {
                ActionEntity.actionExecution.State = Enums.ActionState.Success;
                return;
            }

            // Check if projectile is inside in weapon range.
            if ((ProjectileEntity.projectilePosition2D.Value - StartPos).Magnitude > range)
            {
                ActionEntity.actionExecution.State = Enums.ActionState.Success;
            }

            // Check if projectile has hit a enemy.
            var entities = EntitasContext.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentID));

            // Todo: Create a agent colision system?
            foreach (var entity in entities)
            {
                if (entity == AgentEntity)
                    continue;

                Vec2f entityPos = entity.physicsPosition2D.Value;
                Vec2f bulletPos = ProjectileEntity.projectilePosition2D.Value;
                var movable = entity.physicsMovable;

                Vec2f diff = bulletPos - entityPos;

                float Len = diff.Magnitude;
                diff.Y = 0;
                diff.Normalize();

                if (entity.hasAgentStats && Len <= 0.5f)
                {
                    Vector2 oppositeDirection = new Vector2(-diff.X, -diff.Y);
                    var stats = entity.agentStats;
                    entity.ReplaceAgentStats(stats.Health - (int)damage, stats.Food, stats.Water, stats.Oxygen, 
                        stats.Fuel, stats.AttackCooldown);

                    // spawns a debug floating text for damage 
                    planet.AddFloatingText("" + damage, 0.5f, new Vec2f(oppositeDirection.x * 0.05f, oppositeDirection.y * 0.05f), new Vec2f(entityPos.X, entityPos.Y + 0.35f));
                    ActionEntity.actionExecution.State = Enums.ActionState.Success;
                }
            }
        }

        public override void OnExit(ref PlanetState planet)
        {
            if (ProjectileEntity != null)
            {
                if (ProjectileEntity.isEnabled)
                {
                    planet.RemoveProjectile(ProjectileEntity.projectileID.ID);
                } 
            }
            base.OnExit(ref planet);
        }
    }

    /// <summary>
    /// Factory Method
    /// </summary>
    public class ToolActionFireWeaponCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID)
        {
            return new ToolActionFireWeapon(entitasContext, actionID);
        }
    }
}

