using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class LevelEndingSystem : ReactiveSystem<GameEntity>
{
    readonly IGroup<GameEntity> _asteroids;
    readonly GameContext _contexts;
    readonly IGroup<GameEntity> _levels;
    readonly IGroup<GameEntity> _players;

    public LevelEndingSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts.game;
        _asteroids = contexts.game.GetGroup(GameMatcher.Asteroid);
        _levels = contexts.game.GetGroup(GameMatcher.Level);
        _players = contexts.game.GetGroup(GameMatcher.Player);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Asteroid).Removed());
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.hasAsteroid;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        if (_asteroids.count != 0)
            return;

        var e = _contexts.CreateEntity();
            e.AddAge(0);
            e.AddMaxAge(2);
        e.OnEntityReleased += OnLevelEndTimerReleased;
    }

    

    private void OnLevelEndTimerReleased(IEntity entity)
    {
        foreach (var player in _players.GetEntities())
            player.Destroy();

        var lvl = _levels.GetSingleEntity();
        lvl.ReplaceLevel(lvl.level.level + 1);
    }

}
