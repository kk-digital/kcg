using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class GunFiringSystem : ReactiveSystem<GameEntity>
{
    readonly Contexts _contexts;
    readonly IGroup<GameEntity> _guns;

    public GunFiringSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _guns = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Gun, GameMatcher.Controllable, GameMatcher.View,GameMatcher.Fireable));
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Input);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasInput;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            if (e.input.fire)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        
        foreach (var entity in _guns.GetEntities())
        {
            var pos = entity.view.gameObject.transform.position;
            var rot = entity.view.gameObject.transform.rotation.eulerAngles.z;

            var e =_contexts.game.CreateEntity();
            e.isBullet = true;
            e.AddPosition(new Vector3(pos.x,pos.y,0));
            e.AddRotation(rot);
            e.AddAge(0);
            e.AddMaxAge(3f);
            e.isWrappedAroundGameBounds = true;
            e.AddForce(new List<Vector2> { new Vector2(0, 10) }, 0);
            e.AddResource("prefabs/Bullet");

            entity.isFireable = false;
            entity.ReplaceGun(entity.gun.minimumShotInterval, 0);
        }
    }

}
