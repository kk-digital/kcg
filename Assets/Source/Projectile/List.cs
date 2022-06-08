using Entitas;

namespace Projectile
{
    public class List
    {
        private readonly GameContext gameContext;

        public IGroup<GameEntity> ProjectilesWithSprite;
        public IGroup<GameEntity> ProjectilesWithInput;
        public IGroup<GameEntity> ProjectilesWithVelocity;

        // List of projectiles
        public List()
        {
            gameContext = Contexts.sharedInstance.game;
            ProjectilesWithSprite = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectileID, GameMatcher.ProjectileSprite2D));
            ProjectilesWithInput = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectileID, GameMatcher.ECSInput));
            ProjectilesWithVelocity = gameContext.GetGroup(GameMatcher.AllOf(GameMatcher.ProjectileID, GameMatcher.ProjectileVelocity, GameMatcher.ProjectilePosition2D));
        }
    }
}

