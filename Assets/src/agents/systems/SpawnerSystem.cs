namespace Systems.Agent
{
    public class SpawnerSystem
    {
        public static SpawnerSystem instance;
        public static SpawnerSystem Instance => instance ??= new SpawnerSystem();

        public AgentContext AgentContext;

        public SpawnerSystem()
        {
            AgentContext = Contexts.sharedInstance.agent;
        }

        public void SpawnPlayer()
        {
            
        }
    }
}

