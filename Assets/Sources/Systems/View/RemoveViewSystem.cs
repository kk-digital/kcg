using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class RemoveViewSystem : ISystem
{

    public RemoveViewSystem(Contexts contexts)
    {
        contexts.game.GetGroup(GameMatcher.View).OnEntityRemoved += OnEntityRemoved;
    }
    

    private void OnEntityRemoved(IGroup group, Entity entity, int index, IComponent component)
    {
        var view = (ViewComponent)component;
        Object.Destroy(view.gameObject);
    }
}
