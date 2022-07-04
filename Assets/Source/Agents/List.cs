using Entitas;

namespace Agent
{
    public class List
    {
        private readonly AgentContext agentContext;
        /// <summary>
        /// New agents will be Added to that List if new Entity created with a Sprite Component
        /// </summary>
        public IGroup<AgentEntity> AgentsWithSprite;
        public IGroup<AgentEntity> AgentsWithXY;
        public IGroup<AgentEntity> AgentsWithVelocity;
        
        public List()
        {
            agentContext = Contexts.sharedInstance.agent;
            AgentsWithSprite = agentContext.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentSprite2D));
            AgentsWithXY = agentContext.GetGroup(AgentMatcher.AllOf(AgentMatcher.ECSInput, AgentMatcher.ECSInputXY));
            AgentsWithVelocity = agentContext.GetGroup(AgentMatcher.AllOf(AgentMatcher.PhysicsMovable, AgentMatcher.PhysicsPosition2D));
        }
    }
}

