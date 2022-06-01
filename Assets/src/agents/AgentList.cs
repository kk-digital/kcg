using Entitas;

namespace Agent
{
    public class AgentList
    {
        private readonly AgentContext context;
        /// <summary>
        /// New agents will be Added to that List if new Entity created with a Sprite Component
        /// </summary>
        public IGroup<AgentEntity> agentsWithSprite;
        
        public AgentList()
        {
            context = Contexts.sharedInstance.agent;
            agentsWithSprite = context.GetGroup(AgentMatcher.Sprite2D);
        }
    }
}

