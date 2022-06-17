using UnityEngine;
using System.Collections.Generic;
using Entitas;
using Physics;

namespace Agent
{
    public class SpawnerSystem
    {
        public GameEntity SpawnPlayer(Material material, int spriteId, int width, int height, Vector2 position,
        int AgentId, int startingAnimation)
        {
            var entity = Contexts.sharedInstance.game.CreateEntity();

            var spriteSize = new Vector2(width / 32f, height / 32f);

            entity.isAgentPlayer = true;
            entity.isECSInput = true;
            entity.AddECSInputXY(new Vector2(0, 0), false);

            entity.AddAgentID(AgentId);
            entity.AddAnimationState(1.0f, new Animation.Animation{Type=startingAnimation});
            entity.AddAgentSprite2D(spriteId, spriteSize);
            entity.AddPhysicsPosition2D(position, newPreviousValue: default);
            Vector2 box2dCollider = new Vector2(0.5f, 1.5f);
            entity.AddPhysicsBox2DCollider(box2dCollider, new Vector2(0.25f, 0.0f));
            entity.AddPhysicsMovable(newSpeed: 1f, newVelocity: Vector2.zero, newAcceleration: Vector2.zero, newAccelerationTime: 2f);
            
            // Add Inventory and toolbar.
            var attacher = Inventory.InventoryAttacher.Instance;
            attacher.AttachInventoryToAgent(6, 5, AgentId);
            attacher.AttachToolBarToPlayer(10, AgentId);

            return entity;
        }

        public GameEntity SpawnAgent(Material material, int spriteId, int width, int height, Vector2 position,
        int AgentId, int startingAnimation)
        {
            var entity = Contexts.sharedInstance.game.CreateEntity();

            var spriteSize = new Vector2(width / 32f, height / 32f);

            entity.AddAgentID(AgentId);

            Vector2 box2dCollider = new Vector2(0.5f, 1.5f);
            entity.AddPhysicsBox2DCollider(box2dCollider, new Vector2(0.25f, 0.0f));
            entity.AddAnimationState(1.0f, new Animation.Animation{Type=startingAnimation});
            entity.AddAgentSprite2D(spriteId, spriteSize);
            entity.AddPhysicsPosition2D(position, newPreviousValue: default);
            entity.AddPhysicsMovable(newSpeed: 1f, newVelocity: Vector2.zero, newAcceleration: Vector2.zero, newAccelerationTime: 2f);
            return entity;
        }

        public GameEntity SpawnEnemy(Material material, int spriteId, int width, int height, Vector2 position,
        int AgentId, int startingAnimation)
        {
            var entity = Contexts.sharedInstance.game.CreateEntity();
            
            var spriteSize = new Vector2(width / 32f, height / 32f);
            
            entity.AddAgentID(AgentId);

            Vector2 box2dCollider = new Vector2(0.5f, 1.5f);
            entity.AddPhysicsBox2DCollider(box2dCollider, new Vector2(0.25f, 0.0f));
            entity.AddAnimationState(1.0f, new Animation.Animation{Type=startingAnimation});
            entity.AddAgentSprite2D(spriteId, spriteSize);
            entity.AddPhysicsPosition2D(position, newPreviousValue: default);
            entity.AddPhysicsMovable(newSpeed: 1f, newVelocity: Vector2.zero, newAcceleration: Vector2.zero, newAccelerationTime: 2f);
            entity.AddAgentEnemy(0, 4.0f);
            entity.AddAgentStats(100.0f, 0.8f);

            return entity;
        }

    }
}

