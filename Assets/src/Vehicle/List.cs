using Entitas;

namespace Vehicle
{
    public class List
    {
        private readonly GameContext gameContext;

        public IGroup<GameEntity> VehiclesWithSprite;
        public IGroup<GameEntity> VehiclesWithInput;
        public IGroup<GameEntity> VehiclesWithVelocity;

        public List()
        {
            gameContext = Contexts.sharedInstance.game;
            VehiclesWithSprite = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.VehicleID, GameMatcher.VehicleSprite2D));
            VehiclesWithInput = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.VehicleID, GameMatcher.ECSInput));
            VehiclesWithVelocity = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.VehicleID, GameMatcher.VehicleVelocity, GameMatcher.VehiclePosition2D));
        }
    }
}

