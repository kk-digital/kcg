using KMath;
using UnityEngine;

namespace ECSInput
{
    public class InputProcessSystem
    {
        public void Update(ref Planet.PlanetState planet)
        {
            Contexts contexts = planet.EntitasContext;

            var AgentsWithXY = contexts.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.ECSInput, AgentMatcher.ECSInputXY));

            bool jump = Input.GetKeyDown(KeyCode.UpArrow);
            bool dash = Input.GetKeyDown(KeyCode.Space);
            bool running = Input.GetKey(KeyCode.LeftAlt);
            bool flying = Input.GetKey(KeyCode.F);

            float x = 0.0f;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                x = 1.0f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                x = -1.0f;
            }

            foreach (var entity in AgentsWithXY)
            {
                entity.ReplaceECSInputXY(new Vec2f(x, 0.0f), jump, dash);

                var pos = entity.physicsPosition2D;
                var input = entity.eCSInputXY;
                var movable = entity.physicsMovable;
                var stats = entity.agentStats;

                var movementState = entity.agentMovementState;
                movementState.Running = running;

                if (movementState.Running)
                {
                    movable.Acceleration.X = input.Value.X * movable.Speed * 100.0f * 2;
                }
                else
                {
                    movable.Acceleration.X = input.Value.X * movable.Speed * 100.0f;
                }

                //movable.AffectedByGravity = false;
            

                movementState.DashCooldown -= Time.deltaTime;

                // dash
                if (dash && movementState.DashCooldown <= 0.0f)
                {
                    movable.Acceleration.X += 500.0f * x;
                    movable.Velocity.X = 90.0f * x;

                    movable.Velocity.Y = 0.0f;
                    movable.Acceleration.Y = 0.0f;

                    movable.Invulnerable = true;
                    movable.AffectedByGravity = false;
                    movementState.Dashing = true;
                    movementState.DashCooldown = 1.0f;
                }

                if (!movementState.Jumping)
                {
    
                    // jump
                    if (jump && !movementState.Dashing)
                    {
                        movable.Landed = false;
                        movable.Acceleration.Y = 100.0f;
                        movable.Velocity.Y = 11.5f;
                        movementState.Jumping = true;
                        movable.AffectedByGroundFriction = false;
                        movementState.JumpCounter++;
                    }

                }
                else
                {
                    // double jump
                    if (jump && movementState.JumpCounter <= 1)
                    {
                        movable.Acceleration.Y = 100.0f;
                        movable.Velocity.Y = 8.5f;
                        movementState.JumpCounter++;
                    }
                }

                // if the fly button is pressed
                movementState.Flying = flying && stats.Fuel > 1.0f;

                if (movementState.Flying)
                {
                    movable.Acceleration.Y = 0;
                    movable.Velocity.Y = 3.5f;
                }

                // the end of dashing
                // we can do this using a fixed amount of time
                if (System.Math.Abs(movable.Velocity.X) <= 6.0f)
                {
                    movable.AffectedByGravity = true;
                    movementState.Dashing = false;
                    movable.Invulnerable = false;
                }

                if (movable.Landed)
                {
                    movementState.JumpCounter = 0;
                    movementState.Jumping = false;
                    movable.AffectedByGroundFriction = true;
                }

                if (movementState.Flying)
                {
                    stats.Fuel -= 1.0f;
                    if (stats.Fuel <= 1.0f)
                    {
                        stats.Fuel -= 20.0f;
                    }
                    planet.AddParticleEmitter(pos.Value, Particle.ParticleEmitterType.DustEmitter);
                }
                else
                {
                    stats.Fuel += 1.0f;
                }

                if (stats.Fuel > 100) 
                {
                    stats.Fuel = 100;
                }

                if (movementState.Dashing)
                {
                    planet.AddParticleEmitter(pos.Value, Particle.ParticleEmitterType.DustEmitter);
                }

            }



            //  Open Inventory with Tab.
            var PlayerWithInventory = contexts.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentInventory, AgentMatcher.AgentPlayer));
            foreach (var entity in PlayerWithInventory)
            {
                int inventoryID = entity.agentInventory.InventoryID;
                InventoryEntity inventoryEntity = contexts.inventory.GetEntityWithInventoryID(inventoryID);

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    inventoryEntity.isInventoryDrawable = !inventoryEntity.isInventoryDrawable;
                }
            }

            // Change Item Selection with nums.
            var PlayerWithToolBar = contexts.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentPlayer, AgentMatcher.AgentToolBar));
            foreach (var entity in PlayerWithInventory)
            {
                int inventoryID = entity.agentToolBar.ToolBarID;
                InventoryEntity inventoryEntity = contexts.inventory.GetEntityWithInventoryID(inventoryID);
                var SlotComponent = inventoryEntity.inventorySlots;

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, 0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, 1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, 2);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, 3);
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, 4);
                }
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, 5);
                }
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, 6);
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, 7);
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, 8);
                }
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    inventoryEntity.ReplaceInventorySlots(SlotComponent.Values, 9);
                }
            }
        }
    }
}