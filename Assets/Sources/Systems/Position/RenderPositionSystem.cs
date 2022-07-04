using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class RenderPositionSystem : ReactiveSystem<GameEntity>
{
    public RenderPositionSystem(Contexts contexts): base(contexts.game)
    {

    }


    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.View));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasPosition;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var x = e.position.value.x;
            var y = e.position.value.y;
            var z = e.view.gameObject.transform.position.z;
            var pos = e.view.gameObject.transform.position;

            if (x != pos.x || y != pos.y)
            {
                e.view.gameObject.transform.position = new Vector3(x, y, z);
            }
        }
    }


}
