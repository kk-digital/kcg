using UnityEngine;

namespace Agent
{
    public class SpawnerSystem
    {
        private static SpawnerSystem instance;
        public static SpawnerSystem Instance => instance ??= new SpawnerSystem();

        public AgentContext AgentContext;

        public SpawnerSystem()
        {
            AgentContext = Contexts.sharedInstance.agent;
        }

        public AgentEntity SpawnPlayer()
        {
            var agent = AgentContext.CreateEntity();

            var spriteSize = new Vector2Int(32, 48);
            agent.AddSprite2D(default, default, spriteSize);
            agent.sprite2D.InitSprite();
            
            agent.AddPosition2D(new Vector2(2f, 2f), default);

            return agent;
        }
        
        
    }
}

