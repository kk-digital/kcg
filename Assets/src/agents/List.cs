using Entitas;

namespace Agent
{
    public class List
    {
        private readonly GameContext gameContext;
        /// <summary>
        /// New agents will be Added to that List if new Entity created with a Sprite Component
        /// </summary>
        public IGroup<GameEntity> AgentsWithSprite;
        public IGroup<GameEntity> AgentsWithInput;
        public IGroup<GameEntity> AgentsWithVelocity;
        
        public List()
        {
            gameContext = Contexts.sharedInstance.game;
            AgentsWithSprite = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.AgentID, GameMatcher.AgentSprite2D));
            AgentsWithInput = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.AgentID, GameMatcher.ECSInput));
            AgentsWithVelocity = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.AgentID, GameMatcher.AgentVelocity, GameMatcher.AgentPosition2D));
        }
    }
}

