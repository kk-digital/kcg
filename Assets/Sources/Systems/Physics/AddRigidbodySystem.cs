using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class AddRigidbodySystem : ReactiveSystem<GameEntity>
{
    public AddRigidbodySystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.View.Added());
    }
    protected override bool Filter(GameEntity entity)
    {
        return entity.hasView;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var rigidbody = e.view.gameObject.GetComponent<Rigidbody2D>();
            if (rigidbody != null)
            {
                e.AddRigidbody(rigidbody);
            }
        }
    }


}
