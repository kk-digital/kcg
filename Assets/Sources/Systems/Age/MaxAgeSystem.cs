using System.Collections.Generic;
using Entitas;

public sealed class MaxAgeSystem : ReactiveSystem<GameEntity>
{
    public MaxAgeSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Age, GameMatcher.MaxAge));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAge;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (e.age.age >= e.maxAge.maxAge)
            {
                e.isDestroyed = true;
            }
        }
    }


}
