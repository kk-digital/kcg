using Agents.Components;

namespace Agents.Systems
{
    public class Physics
    {
        private static Physics instance;
        public static Physics Instance => instance ??= new Physics();

        public void UpdateAgents(ref Planet map)
        {
            
        }
    }
}
