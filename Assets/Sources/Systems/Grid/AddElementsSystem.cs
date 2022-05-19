using System.Collections.Generic;
using Entitas;

public sealed class AddElementsSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _grids;

    public AddElementsSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _grids = contexts.game.GetGroup(GameMatcher.Grid); // grid

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Position.AddedOrRemoved());
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        
    }
}