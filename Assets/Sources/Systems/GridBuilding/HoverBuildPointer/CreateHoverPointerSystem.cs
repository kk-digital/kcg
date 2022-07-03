using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class CreateHoverPointerSystem : IInitializeSystem
{
    readonly Contexts _contexts;


    public CreateHoverPointerSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Initialize()
    {
        var e = _contexts.game.CreateEntity();

        e.isPointer = true;
        e.isHoverPointer = true;
        e.AddResource("Prefabs/SpaceStationHover");
        e.AddPosition(Camera.main.ScreenPointToRay(Input.mousePosition).origin);
    }

}
