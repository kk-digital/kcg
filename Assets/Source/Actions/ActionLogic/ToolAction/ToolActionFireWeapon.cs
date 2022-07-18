using System;
using KMath;
using Planet;
using UnityEngine;
using System.Collections.Generic;

namespace Action
{
    public class ToolActionFireWeapon : ActionBase
    {
        // Weapon Property
        private Item.FireWeaponPropreties WeaponProperty;

        // Projectile Entity
        private ProjectileEntity ProjectileEntity;

        // Item Entity
        private ItemInventoryEntity ItemEntity;

        // Start Position
        private Vec2f StartPos;

        // List for Drawing Debug Cone
        private List<ProjectileEntity> EndPointList = new List<ProjectileEntity>();

        // Constructor
        public ToolActionFireWeapon(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            // Item Entity
            ItemEntity = EntitasContext.itemInventory.GetEntityWithItemID(ActionEntity.actionTool.ItemID);

            // Weapon Property
            WeaponProperty = GameState.ItemCreationApi.GetWeapon(ItemEntity.itemType.Type);

            // Cursor Position
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = worldPosition.x;
            float y = worldPosition.y;

            // Bullets Per Shot
            int bulletsPerShot = ItemEntity.itemFireWeaponClip.BulletsPerShot;

            // If entity has clip comp
            if (ItemEntity.hasItemFireWeaponClip)
            {
                // Number of grenades
                int numBullet = ItemEntity.itemFireWeaponClip.NumOfBullets;

                // If clip is empty
                if (numBullet <= 0)
                {
                    // Error log
                    Debug.Log("Clip is empty. Press R to reload.");
                    ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Fail);
                    return;
                }
            }

            // Decrease number of bullets in the clip when shoot
            if (ItemEntity.hasItemFireWeaponClip)
                ItemEntity.itemFireWeaponClip.NumOfBullets -= bulletsPerShot;

            // Start positiom
            StartPos = AgentEntity.physicsPosition2D.Value;
            StartPos.X += 0.3f;
            StartPos.Y += 0.5f;

            // Check if entity has spread component
            if (ItemEntity.hasItemFireWeaponSpread)
            {
                // Get Spread Component
                var spread = ItemEntity.itemFireWeaponSpread;
                for(int i = 0; i < bulletsPerShot; i++)
                {
                    // Spread Calculations
                    var random = UnityEngine.Random.Range(-spread.SpreadAngle, spread.SpreadAngle);

                    // Spawn Projectile in Spread Angle
                    ProjectileEntity = planet.AddProjectile(StartPos, new Vec2f((x - StartPos.X) - random, y - StartPos.Y).Normalized, Enums.ProjectileType.Bullet);

                    // End Point List
                    EndPointList.Add(ProjectileEntity);
                }
            }

            // If The weapon is Bow then spawn arche type projectile
            if(ItemEntity.itemType.Type == Enums.ItemType.Bow)
            {
                // Add Arche Projectile
                ProjectileEntity = planet.AddProjectile(StartPos, new Vec2f(x - StartPos.X, y - StartPos.Y).Normalized, Enums.ProjectileType.Arrow);
            }
            else
            {
                // If Bullets per shot is higher than zero
                if(ItemEntity.itemFireWeaponClip.BulletsPerShot > 0)
                {
                    for(int i = 0; i < bulletsPerShot; i++)
                    {
                        // Spawn them by each (so it's not gets overlap)
                        ProjectileEntity = planet.AddProjectile(StartPos, new Vec2f(x - StartPos.X, y - StartPos.Y).Normalized, Enums.ProjectileType.Bullet);
                    }
                }
                else
                {
                    // If not, just spawn 1 bullet
                    ProjectileEntity = planet.AddProjectile(StartPos, new Vec2f(x - StartPos.X, y - StartPos.Y).Normalized, Enums.ProjectileType.Bullet);
                }
            }

            // Add to List
            EndPointList.Add(ProjectileEntity);
            
            // Execute Update
            ActionEntity.actionExecution.State = Enums.ActionState.Running;

            // Set Fire Cool Down
            GameState.ActionCoolDownSystem.SetCoolDown(EntitasContext, ActionEntity.actionID.TypeID, AgentEntity.agentID.ID, WeaponProperty.CoolDown);
        }

        public override void OnUpdate(float deltaTime, ref Planet.PlanetState planet)
        {
            // Get Range from Weapon Property
            float range = WeaponProperty.Range;

            // Get Basic Damage from Weapon Property
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

            // Draw Gizmos Start (Spread, Fire, Angle, Recoil Cone)
#if UNITY_EDITOR
            for (int i = 0; i < EndPointList.Count; i++)
            {
                if (EndPointList[i].hasProjectilePosition2D)
                    Debug.DrawLine(new Vector3(StartPos.X, StartPos.Y, 0), new Vector3(EndPointList[i].projectilePosition2D.Value.X, EndPointList[i].projectilePosition2D.Value.Y, 0), Color.red, 2.0f, false);
            }
#endif
            // Draw Gizmos End (Spread, Fire, Angle, Recoil Cone)

            // Check if projectile has hit a enemy.
            var entities = EntitasContext.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentID));

            // Todo: Create a agent colision system?
            foreach (var entity in entities)
            {
                // If Entity Equals to Agent Entity
                if (entity == AgentEntity)
                    continue;

                //
                Vec2f entityPos = entity.physicsPosition2D.Value;

                // Get Bullet Position from Projectile Position Component
                Vec2f bulletPos = ProjectileEntity.projectilePosition2D.Value;

                // Get Agent Physics Movable Component
                var movable = entity.physicsMovable;

                // Difference Calculation
                Vec2f diff = bulletPos - entityPos;

                // Difference Calculation
                float Len = diff.Magnitude;
                diff.Y = 0;
                diff.Normalize();

                // Entity Agent Stats
                if (entity.hasAgentStats && Len <= 0.5f)
                {
                    // Calculate Opposite Direction
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
                    // Release the projectile before executing exit
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

