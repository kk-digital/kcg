using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class ShipRespawningSystem : ReactiveSystem<GameEntity>
{
    readonly GameContext _contexts;
    readonly IGroup<GameEntity> _game;
    readonly IGroup<GameEntity> _asteroids;
    readonly IGroup<GameEntity> _waiting;
    readonly IGroup<GameEntity> _lives;

    public ShipRespawningSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts.game;
        _contexts.GetGroup(GameMatcher.AllOf(GameMatcher.ShipDeathroes)).OnEntityRemoved += OnDeaththroesRemoved;
        _waiting = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.WaitingForSpace, GameMatcher.Ship));
        _asteroids = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Asteroid, GameMatcher.CollisionRadius));
        _game = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Game,GameMatcher.Bounds));
        _lives = contexts.game.GetGroup(GameMatcher.Lives);
        
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Ship);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCollision;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var game = _game.GetSingleEntity();
        if (game == null)
            return;

        var bounds = game.bounds.bounds;
        foreach (var entity in _waiting.GetEntities())
        {
            var pos = bounds.RandomPosition();
            if (HasSpace(pos, entity))
                Respawn(entity);
        }
    }

    private void OnDeaththroesRemoved(IGroup group, Entity entity, int index, IComponent component)
    {
        if (_lives.GetSingleEntity().lives.lives == 0)
            return;

        var e = _contexts.CreateEntity();
        e.AddShip(0.5f, 0.02f);
        e.AddCollisionRadius(5);
          e.isWaitingForSpace = true;
    }
    private void Respawn(GameEntity entity)
    {
        var e = _contexts.CreateEntity();
        e.isPlayer = true;
        e.AddPosition(new Vector3(0, 0,0));
        e.AddShip(0.5f, 0.02f);
        e.AddCollisionRadius(1);
        e.isControllable = true;
        e.isWrappedAroundGameBounds = true;
        e.AddForce(new List<Vector2>(), 0);
        e.AddResource("Prefabs/ship");

        entity.isDestroyed = true;
    }

    private bool HasSpace(Vector2 pos, GameEntity entity)
    {
        return entity.CollidesWithOthers(pos, _asteroids.GetEntities());
    }

}
