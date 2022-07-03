using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class PositionUpdateSystem : IExecuteSystem
{
    readonly IGroup<GameEntity> _positions;

    public PositionUpdateSystem(Contexts contexts)
    {
        _positions = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.View, GameMatcher.Position));
    }

    public void Execute()
    {
        foreach (var e in _positions)
        {
            var x = e.view.gameObject.transform.position.x;
            var y = e.view.gameObject.transform.position.y;
            if (x != e.position.value.x || y != e.position.value.y)
            {
                e.ReplacePosition(new Vector3(x, y, 0));
            }
        }
    }
}
