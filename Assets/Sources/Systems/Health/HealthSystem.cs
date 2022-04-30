using System.Collections.Generic;
using Entitas;

public sealed class HealthSystem : ReactiveSystem<GameEntity>
{
    public HealthSystem(Contexts contexts) : base(contexts.game){
    }


    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Health);
    }
    
    protected override bool Filter(GameEntity entity)
    {
        return entity.hasHealth;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if(e.health.value <= 0)
            {
                e.isDestroyed = true;
            }
        }
    }
}
