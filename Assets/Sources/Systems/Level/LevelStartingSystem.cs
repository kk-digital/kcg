using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class LevelStartingSystem : ReactiveSystem<GameEntity>
{
    readonly IGroup<GameEntity> _games;
    readonly GameContext _contexts;
    readonly IGroup<GameEntity> _collideables;

    public LevelStartingSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts.game;
        _games = contexts.game.GetGroup(GameMatcher.Game);
        _collideables = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.CollisionRadius, GameMatcher.Position));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Level);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasLevel;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var game = _games.GetSingleEntity();
        if (game == null || !game.isPlaying)
            return;

        foreach (var entity in entities)
            StartLevel(entity.level.level, game);
    }

    private void StartLevel(int level, GameEntity game)
    {
        //var e = _contexts.CreateEntity();

        //e.isPlayer = true;
        //e.AddPosition(new Vector3(0, 0, 0));
        //e.AddShip(0.5f, 0.02f);
        //e.AddCollisionRadius(1);
        //e.isControllable = true;
        //e.isWrappedAroundGameBounds = true;
        //e.AddForce(new List<Vector2>(), 0);
        //e.AddResource("Prefabs/ship");
        // _pool.CreatePlayer(true);
        for (var i = 0; i < level + 1; i++)
            CreateAsteroid(game);
    }

    private void CreateAsteroid(GameEntity game)
    {
        var size = AsteroidSize.Large;
        var force = UnityEngine.Random.insideUnitCircle.normalized;
        var e = _contexts.CreateEntity();

        e.AddAsteroid(size);
        e.AddPosition(new Vector3(0, 0, 0));
        e.AddHitpoints(1);
        e.AddCollisionRadius(AsteroidData.Radii[size]);
        e.isWrappedAroundGameBounds = true;
        e.AddResource("prefabs/" + AsteroidData.Resources[size]);

        e.ReplaceForce(new List<Vector2> { force }, force.x);
        e.FindEmptyPosition(AsteroidData.Radii[size], game.bounds.bounds, _collideables.GetEntities());
    }

}
