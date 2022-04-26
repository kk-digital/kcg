using Entitas;
using System.Collections.Generic;
using UnityEngine;

public sealed class CreatPlayerSystem : IInitializeSystem
{
    readonly Contexts _contexts;

    public CreatPlayerSystem(Contexts contexts)
    {
        _contexts = contexts;
    }
    
    public void Initialize()
    {
        var e = _contexts.game.CreateEntity();

        e.isPlayer = true;
        e.AddPosition(new Vector3(0, 0, 0));
        e.AddShip(0.5f, 0.02f);
        e.AddCollisionRadius(1);
        e.isControllable = true;
        e.isWrappedAroundGameBounds = true;
        e.AddForce(new List<Vector2>(), 0);
        e.AddResource("Prefabs/ship");

        e.AddHealth(100);

    }
}
