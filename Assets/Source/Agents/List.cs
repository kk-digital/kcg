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
        public IGroup<GameEntity> AgentsWithXY;
        public IGroup<GameEntity> AgentsWithVelocity;
        
        public List()
        {
            gameContext = Contexts.sharedInstance.game;
            AgentsWithSprite = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.AgentSprite2D));
            AgentsWithXY = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.ECSInput, GameMatcher.ECSInputXY));
            AgentsWithVelocity = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.AgentMovable, GameMatcher.AgentPosition2D));
        }
    }
}

