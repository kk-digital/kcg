using System.Collections.Generic;
using Entitas;

public sealed class AgingSystem : ReactiveSystem<GameEntity>
{
    readonly IGroup<GameEntity> _ages;

    public AgingSystem(Contexts contexts) : base(contexts.game)
    {
        _ages = contexts.game.GetGroup(GameMatcher.Age);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Tick);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasTick;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            Age(e.tick.delta);
        }
    }

    public void Age(float amount)
    {
        foreach (var entity in _ages.GetEntities())
        {
           entity.ReplaceAge(entity.age.age + amount);
        }

    }


}
