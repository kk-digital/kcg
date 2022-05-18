using Entitas;
using System.Collections.Generic;
using UnityEngine;

public sealed class CreatSpaceStationSystem : IInitializeSystem
{
    readonly Contexts _contexts;

    public CreatSpaceStationSystem(Contexts contexts)
    {
        _contexts = contexts;
    }
    
    public void Initialize()
    {
        var e = _contexts.game.CreateEntity();

        
        e.AddPosition(new Vector3(5, 5, 0));
        e.isSpaceStation = true;
        e.AddGrid(new GridSize(5, 5));
        e.AddResource("Prefabs/SpaceStation");
        e.AddHealth(100);


        //var e2 = _contexts.game.CreateEntity();
        //e2.AddPosition(new Vector3(-7, 5, 0));
        //e2.isSpaceStation = true;
        //e2.AddGrid(new GridSize(5, 5));
        //e2.AddResource("Prefabs/SpaceStation");
        //e2.AddHealth(100);
    }
}
