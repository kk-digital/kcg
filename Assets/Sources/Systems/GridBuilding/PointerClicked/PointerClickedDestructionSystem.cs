using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class PointerClickedDestructionSystem : ICleanupSystem
{
   
    readonly GameContext _context;
    readonly IGroup<GameEntity> _Input;
    public PointerClickedDestructionSystem(Contexts contexts)
    {
        _context = contexts.game;
        _Input = _context.GetGroup(GameMatcher.PointerClicked);
    }

    public void Cleanup()
    {
        foreach (var e in _Input.GetEntities())
        {
            e.Destroy();
        }
    }
}
