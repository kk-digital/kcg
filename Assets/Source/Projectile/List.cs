using Entitas;

namespace Projectile
{
    public class List
    {
        private readonly GameContext gameContext;

        public IGroup<GameEntity> ProjectilesWithSprite;
        public IGroup<GameEntity> ProjectilesWithInput;
        public IGroup<GameEntity> ProjectilesWithPhysics;

        // List of projectiles
        public List()
        {
            gameContext = Contexts.sharedInstance.game;
            ProjectilesWithSprite = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectileID, GameMatcher.ProjectileSprite2D));
            ProjectilesWithInput = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectileID, GameMatcher.ECSInput));
            ProjectilesWithPhysics = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectileID, GameMatcher.ProjectilePhysicsState2D, GameMatcher.ProjectilePhysicsState2D));
        }
    }
}

