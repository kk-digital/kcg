using Entitas;

public class DestroyEntitySystem : IExecuteSystem
{
       readonly IGroup<GameEntity> _group;

    // Initialize System
    public DestroyEntitySystem(Contexts contexts)
    {
        _group = contexts.game.GetGroup(GameMatcher.Destroyed);
    }
    // Execute Syetem
    public void Execute()
    {
        foreach (var e in _group.GetEntities())
        {
            e.Destroy();
        }
    }
}
