using Entitas;

namespace Agent
{
    public class AgentList
    {
        private readonly AgentContext context;
        public IGroup<AgentEntity> agentsWithSprite;
        
        public AgentList()
        {
            context = Contexts.sharedInstance.agent;
            agentsWithSprite = context.GetGroup(AgentMatcher.Sprite2D);
        }
    }
}

