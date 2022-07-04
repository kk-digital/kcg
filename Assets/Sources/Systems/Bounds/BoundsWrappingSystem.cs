using System.Collections.Generic;
using Entitas;
using UnityEngine;

 public sealed class BoundsWrappingSystem : ReactiveSystem<GameEntity>
 {
    readonly IGroup<GameEntity> _gameBounds;

    public BoundsWrappingSystem(Contexts contexts) : base(contexts.game)
    {
        _gameBounds = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Game, GameMatcher.Bounds));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Position,GameMatcher.WrappedAroundGameBounds));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasPosition;
    }

    protected override void Execute(List<GameEntity> entities)
     {
        var bounds =  _gameBounds.GetSingleEntity();
        if (bounds == null)
            return;

        foreach (var entity in entities)
            Wrap(entity, bounds.bounds.bounds);
    }

    private void Wrap(GameEntity entity, UnityEngine.Bounds bounds)
    {
        var position = entity.position;

        if (position.value.x < bounds.min.x)
            entity.ReplacePosition(new Vector3(position.value.x + bounds.size.x, position.value.y));

        if (position.value.x > bounds.max.x)
            entity.ReplacePosition(new Vector3(position.value.x - bounds.size.x, position.value.y));

        if (position.value.y < bounds.min.y)
            entity.ReplacePosition(new Vector3(position.value.x, position.value.y + bounds.size.y));

        if (position.value.y > bounds.max.y)
            entity.ReplacePosition(new Vector3(position.value.x, position.value.y - bounds.size.y));
    }

    // public TriggerOnEvent trigger
    // {
    //     get { return Matcher.AllOf(Matcher.Position, Matcher.WrappedAroundGameBounds).OnEntityAdded(); }
    // }

    // public void SetPool(Pool pool)
    // {
    //     _gameBounds = pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.Bounds));
    // }


   


 }

