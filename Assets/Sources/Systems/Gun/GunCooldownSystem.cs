using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class GunCooldownSystem : ReactiveSystem<GameEntity>
{
    readonly IGroup<GameEntity> _guns;
    public GunCooldownSystem(Contexts contexts) : base(contexts.game)
    {
        _guns = contexts.game.GetGroup(GameMatcher.Gun);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Tick));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasTick;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            Cooldown(e.tick.delta);
        }
    }

    private void Cooldown(float delta)
        {
            foreach (var entity in _guns)
            {
                entity.ReplaceGun(entity.gun.minimumShotInterval, entity.gun.timeSinceLastShot + delta);
                entity.isFireable = entity.gun.timeSinceLastShot >= entity.gun.minimumShotInterval;
            }
        }




}
