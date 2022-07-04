using System.Collections.Generic;
using Entitas;

// Reactive Execute system
public sealed class LogHealthSystem : ReactiveSystem<GameEntity>
{
    public LogHealthSystem(Contexts contexts) : base(contexts.game)
    {

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
            var health = e.health.value;
            UnityEngine.Debug.Log("health: " + health);
        };
    }

}


// Execute system

//public sealed class LogHealthSystem : IExecuteSystem {

//    readonly IGroup<GameEntity> _entities;

//    public LogHealthSystem(Contexts contexts)
//    {
//        _entities = contexts.game.GetGroup(GameMatcher.Health);
//    }

//    public void Execute()
//    {
//        foreach (var e in _entities)
//        {
//            var health = e.health.value;
//            UnityEngine.Debug.Log("health: " + health);
//        }
//    }
//}
