using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class TickingSystem : IExecuteSystem
{
    readonly GameContext _contexts;

    public TickingSystem(Contexts contexts)
    {
        _contexts = contexts.game;
    }
    public void Execute()
    {
       // UnityEngine.Debug.Log("Ticking" + Time.deltaTime);
        var e = _contexts.CreateEntity();
        e.AddTick(Time.deltaTime);
        e.isDestroyed = true;

    }

    
}
