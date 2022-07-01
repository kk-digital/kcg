using KMath;
using UnityEngine;

namespace ECSInput
{
    public class InputProcessSystem
    {
        public void Update(Contexts contexts)
        {
            var AgentsWithXY = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.ECSInput, GameMatcher.ECSInputXY));

            bool jump = Input.GetKeyDown(KeyCode.UpArrow);
            bool dash = Input.GetKeyDown(KeyCode.LeftShift);
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

            
                

                var input = entity.eCSInputXY;
                var movable = entity.physicsMovable;

                movable.Acceleration = input.Value * movable.Speed * 50.0f;

                var movementState = entity.agentMovementState;

                movementState.DashCooldown -= Time.deltaTime;

                if (!movementState.Jumping)
                {
                    movementState.Dashing = false;
                    movementState.Jumping = false;

                    if (dash && movementState.DashCooldown <= 0.0f)
                    {
                        movable.Acceleration.X += 500.0f * x;
                        movable.Velocity.X = 30.0f * x;
                        movementState.Dashing = true;
                        movementState.DashCooldown = 1.5f;
                    }
                    else if (jump)
                    {
                        movable.Landed = false;
                        movable.Acceleration.Y += 0.0f;
                        movable.Velocity.Y = 8.5f;
                        movementState.Jumping = true;
                    }

                }
                else
                {
                    if (jump && !movementState.DoubleJumping)
                    {
                        movable.Acceleration.Y += 0.0f;
                        movable.Velocity.Y = 6.5f;
                        movementState.DoubleJumping = true;
                    }
                }

                if (movable.Landed)
                {
                    movementState.DoubleJumping = false;
                    movementState.Jumping = false;
                    movementState.Dashing = false;
                }

                entity.ReplaceAgentMovementState(movementState.Jumping, movementState.DoubleJumping,
                                    movementState.Dashing, movementState.Flying, movementState.DashCooldown);
                entity.ReplacePhysicsMovable(movable.Speed, movable.Velocity, movable.Acceleration, movable.Landed);

            }



            //  Open Inventory with Tab.
            var PlayerWithInventory = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentInventory, GameMatcher.AgentPlayer));
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
            var PlayerWithToolBar = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.AgentPlayer, GameMatcher.AgentToolBar));
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
