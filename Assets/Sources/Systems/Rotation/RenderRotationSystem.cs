using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class RenderRotationSystem : ReactiveSystem<GameEntity>
{

    public RenderRotationSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Rotation,GameMatcher.View));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasRotation;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var r = e.view.gameObject.transform.rotation.eulerAngles;
            e.view.gameObject.transform.rotation = Quaternion.Euler(r.x, r.y, e.rotation.rotation);
        }
    }


}
