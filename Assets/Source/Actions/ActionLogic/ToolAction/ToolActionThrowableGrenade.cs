using System;
using KMath;
using Planet;
using UnityEngine;
using System.Collections;

namespace Action
{
    public class ToolActionThrowableGrenade : ActionBase
    {
        private Item.FireWeaponPropreties WeaponProperty;
        private ProjectileEntity ProjectileEntity;
        private ItemInventoryEntity ItemEntity;
        private Vec2f StartPos;
        float radius = 0.0f;

        public ToolActionThrowableGrenade(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            ItemEntity = EntitasContext.itemInventory.GetEntityWithItemID(ActionEntity.actionTool.ItemID);
            WeaponProperty = GameState.ItemCreationApi.GetWeapon(ItemEntity.itemType.Type);

            if (ItemEntity.itemType.Type == Enums.ItemType.GrenadeLauncher)
                radius = 2.0f;
            else if (ItemEntity.itemType.Type == Enums.ItemType.RPG)
                radius = 4.0f;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = worldPosition.x;
            float y = worldPosition.y;
            int bulletsPerShot = ItemEntity.itemFireWeaponClip.BulletsPerShot;

            // Check if gun got any ammo
            if (ItemEntity.hasItemFireWeaponClip)
            {
                int numBullet = ItemEntity.itemFireWeaponClip.NumOfBullets;
                if (numBullet == 0)
                {
                    Debug.Log("Clip is empty. Press R to reload.");
                    ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                    return;
                }
            }

            // Decrase number of bullets when shoot
            if (ItemEntity.hasItemFireWeaponClip)
                ItemEntity.itemFireWeaponClip.NumOfBullets -= bulletsPerShot;

            // Start position
            StartPos = AgentEntity.physicsPosition2D.Value;
            StartPos.X += 0.5f;
            StartPos.Y += 0.5f;

            if(ItemEntity.itemType.Type == Enums.ItemType.GrenadeLauncher)
            {
                ProjectileEntity = planet.AddProjectile(StartPos, new Vec2f(x - StartPos.X, y - StartPos.Y).Normalized, Enums.ProjectileType.Grenade);
                planet.AddFloatingText(WeaponProperty.GrenadeFlags.ToString(), 2.0f, new Vec2f(0, 0), new Vec2f(AgentEntity.physicsPosition2D.Value.X + 0.5f, AgentEntity.physicsPosition2D.Value.Y));
            }
            else if (ItemEntity.itemType.Type == Enums.ItemType.RPG)
            {
                ProjectileEntity = planet.AddProjectile(StartPos, new Vec2f(x - StartPos.X, y - StartPos.Y).Normalized, Enums.ProjectileType.Rocket);
            }

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

                planet.AddParticleEmitter(ProjectileEntity.projectilePosition2D.Value, Particle.ParticleEmitterType.DustEmitter);

                // Check if projectile has hit a enemy.
                var entities = EntitasContext.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentID));

                // Todo: Create a agent colision system?
                foreach (var entity in entities)
                {
                    float dist = Vector2.Distance(new Vector2(AgentEntity.physicsPosition2D.Value.X, AgentEntity.physicsPosition2D.Value.Y), new Vector2(ProjectileEntity.projectilePosition2D.Value.X, ProjectileEntity.projectilePosition2D.Value.Y));

                    if (dist < radius)
                    {
                        Vec2f entityPos = entity.physicsPosition2D.Value;
                        Vec2f bulletPos = ProjectileEntity.projectilePosition2D.Value;
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
                ActionEntity.actionExecution.State = Enums.ActionState.Success;
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
    public class ToolActionThrowableGrenadeCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID)
        {
            return new ToolActionThrowableGrenade(entitasContext, actionID);
        }
    }
}

