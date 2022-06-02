using Entitas;

namespace Agent
{
    public class AgentList
    {
        private readonly GameContext gameContext;
        /// <summary>
        /// New agents will be Added to that List if new Entity created with a Sprite Component
        /// </summary>
        public IGroup<GameEntity> agentsWithSprite;
        
        public AgentList()
        {
            gameContext = Contexts.sharedInstance.game;
            agentsWithSprite = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.AgentID, GameMatcher.AgentSprite2D));
        }
    }
}

