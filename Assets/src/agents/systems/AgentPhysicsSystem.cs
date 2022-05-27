namespace Systems
{
    public class AgentPhysicsSystem
    {
        private static AgentPhysicsSystem instance;
        public static AgentPhysicsSystem Instance => instance ??= new AgentPhysicsSystem();

        public void UpdateAgents(ref PlanetTileMap.PlanetTileMap map)
        {
            
        }
    }
}
