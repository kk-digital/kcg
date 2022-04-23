using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System;

public sealed class AddViewSystem : ReactiveSystem<GameEntity>
{
    readonly Transform _viewContainer = new GameObject("Views").transform;

    public AddViewSystem(Contexts contexts) : base(contexts.game)
    {

    }
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Resource);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasResource;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var res = UnityEngine.Resources.Load<GameObject>(e.resource.name);
            GameObject gameObject = null;
            try
            {
                gameObject = UnityEngine.Object.Instantiate(res);
            }
            catch (Exception)
            {
                Debug.Log("Cannot instantiate " + res);
            }

            if (gameObject != null)
            {
                gameObject.transform.SetParent(_viewContainer, false);
                e.AddView(gameObject);

                if (e.hasPosition)
                    gameObject.transform.position = new Vector3(e.position.value.x, e.position.value.y);
            }
        }
    }
}
