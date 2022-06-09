using Entitas;

namespace Vehicle
{
    public class List
    {
        private readonly GameContext gameContext;

        public IGroup<GameEntity> VehiclesWithSprite;
        public IGroup<GameEntity> VehiclesWithInput;
        public IGroup<GameEntity> VehiclesWithPhysics;

        // List of vehicles
        public List()
        {
            gameContext = Contexts.sharedInstance.game;
            VehiclesWithSprite = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.VehicleID, GameMatcher.VehicleSprite2D));
            VehiclesWithInput = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.VehicleID, GameMatcher.ECSInput));
            VehiclesWithPhysics = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.VehicleID, GameMatcher.VehiclePhysicsState2D, GameMatcher.VehiclePhysicsState2D));
        }
    }
}

