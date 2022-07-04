using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class InputDestructionSystem : ICleanupSystem
{
    readonly GameContext _context;
    readonly IGroup<GameEntity> _Input;
    public InputDestructionSystem(Contexts contexts)
    {
        _context = contexts.game;
        _Input = _context.GetGroup(GameMatcher.Input);
    }

    public void Cleanup()
    {
        foreach (var e in _Input.GetEntities())
        {
            e.Destroy();
        }
    }
}
