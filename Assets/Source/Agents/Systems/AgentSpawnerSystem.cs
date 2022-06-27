using System.Collections.Generic;
using KMath;
using UnityEngine;

namespace Agent
{
    public class AgentSpawnerSystem
    {

        AgentCreationApi AgentCreationApi;
        public AgentSpawnerSystem(AgentCreationApi agentCreationApi)
        {
            AgentCreationApi = agentCreationApi;
        }

        //NOTE(Mahdi): Deprecated, will be removed soon
        public GameEntity SpawnPlayer(int spriteId, int width, int height, Vec2f position,
        int agentId, int startingAnimation, int playerHealth, float attackCoolDown)
        {
            var entity = Contexts.sharedInstance.game.CreateEntity();

            var spriteSize = new Vec2f(width / 32f, height / 32f);

            entity.isAgentPlayer = true;
            entity.isECSInput = true;
            entity.AddECSInputXY(new Vec2f(0, 0), false);

            entity.AddAgentID(agentId);
            entity.AddAnimationState(1.0f, new Animation.Animation{Type=startingAnimation});
            entity.AddAgentSprite2D(spriteId, spriteSize); // adds the sprite  component to the entity
            entity.AddPhysicsPosition2D(position, newPreviousValue: default);
            var size = new Vec2f(spriteSize.X - 0.5f, spriteSize.Y);
            entity.AddPhysicsBox2DCollider(size, new Vec2f(0.25f, .0f));
            entity.AddPhysicsMovable(newSpeed: 1f, newVelocity: Vec2f.Zero, newAcceleration: Vec2f.Zero);
            entity.AddAgentActionScheduler(new List<int>(), new List<int>());
            entity.AddAgentStats(playerHealth, attackCoolDown);

            // Add Inventory and toolbar.
            var attacher = Inventory.InventoryAttacher.Instance;
            attacher.AttachInventoryToAgent(6, 5, agentId);
            attacher.AttachToolBarToPlayer(10, agentId);
            return entity;
        }


        public GameEntity Spawn(Vec2f position, int agentId, AgentType agentType)
        {
            var entity = Contexts.sharedInstance.game.CreateEntity();

            ref Agent.AgentProperties properties = ref AgentCreationApi.GetRef((int)agentType);

            var spriteSize = properties.SpriteSize;
            var spriteId = 0;
            entity.AddAgentID(agentId); // agent id 
            entity.AddPhysicsBox2DCollider(properties.CollisionDimensions, properties.CollisionOffset);
            entity.AddPhysicsPosition2D(position, newPreviousValue: default); // 2d position
            entity.AddAgentSprite2D(spriteId, spriteSize); // adds the sprite  component to the entity
            entity.AddPhysicsMovable(newSpeed: 1f, newVelocity: Vec2f.Zero, newAcceleration: Vec2f.Zero); // used for physics simulation
            entity.AddAnimationState(1.0f, new Animation.Animation{Type=properties.StartingAnimation});

            if (agentType == Agent.AgentType.Player)
            {
                entity.isAgentPlayer = true;
                entity.isECSInput = true;
                entity.AddECSInputXY(new Vec2f(0, 0), false);
   
                entity.AddAgentActionScheduler(new List<int>(), new List<int>());

                // Add Inventory and toolbar.
                var attacher = Inventory.InventoryAttacher.Instance;
                attacher.AttachInventoryToAgent(6, 5, agentId);
                attacher.AttachToolBarToPlayer(10, agentId);
            }
            else if (agentType == Agent.AgentType.Agent)
            {
                
            }
            else if (agentType == Agent.AgentType.Enemy)
            {
                entity.AddAgentEnemy(properties.EnemyBehaviour, properties.DetectionRadius);
                entity.AddAgentStats(properties.Health, properties.AttackCooldown);
            }

            return entity;
        }

        //NOTE(Mahdi): Deprecated, will be removed soon
        public GameEntity SpawnAgent(int spriteId, int width, int height, Vec2f position,
        int agentId, int startingAnimation)
        {
            var entity = Contexts.sharedInstance.game.CreateEntity();

            var spriteSize = new Vec2f(width / 32f, height / 32f);

            entity.AddAgentID(agentId);

            Vec2f box2dCollider = new Vec2f(0.5f, 1.5f);
            entity.AddPhysicsBox2DCollider(box2dCollider, new Vec2f(0.25f, 0.0f));
            entity.AddAnimationState(1.0f, new Animation.Animation{Type=startingAnimation});
            entity.AddAgentSprite2D(spriteId, spriteSize); // adds the sprite  component to the entity
            entity.AddPhysicsPosition2D(position, newPreviousValue: default);
            entity.AddPhysicsMovable(newSpeed: 1f, newVelocity: Vec2f.Zero, newAcceleration: Vec2f.Zero);

            return entity;
        }

        //NOTE(Mahdi): Deprecated, will be removed soon
        public GameEntity SpawnEnemy(int spriteId, int width, int height, Vec2f position,
        int agentId, int startingAnimation)
        {
            var entity = Contexts.sharedInstance.game.CreateEntity();
            
            var spriteSize = new Vec2f(width / 32f, height / 32f);
            
            entity.AddAgentID(agentId);

            Vec2f box2dCollider = new Vec2f(0.75f, 0.5f);
            entity.AddPhysicsBox2DCollider(box2dCollider, new Vec2f(0.125f, 0.0f));
            entity.AddAnimationState(1.0f, new Animation.Animation{Type=startingAnimation});
            entity.AddAgentSprite2D(spriteId, spriteSize); // adds the sprite  component to the entity
            entity.AddPhysicsPosition2D(position, newPreviousValue: default);
            entity.AddPhysicsMovable(newSpeed: 1f, newVelocity: Vec2f.Zero, newAcceleration: Vec2f.Zero);
            entity.AddAgentEnemy(0, 4.0f);
            entity.AddAgentStats(100.0f, 0.8f);

            return entity;
        }

    }
}
