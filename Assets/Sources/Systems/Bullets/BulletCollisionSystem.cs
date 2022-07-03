using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class BulletCollisionSystem : ReactiveSystem<GameEntity>
{
    readonly IGroup<GameEntity> _bullets;
    readonly IGroup<GameEntity> _damageable;

    public BulletCollisionSystem(Contexts contexts) : base(contexts.game)
    {
        _bullets = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Bullet, GameMatcher.View));
        _damageable = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Hitpoints, GameMatcher.View));
    }
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Collision);
    }
    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCollision;
    }
    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            var obj = entity.collision.collision.gameObject;
          

            foreach (var bullet in _bullets.GetEntities())
            {
                if (bullet != null && bullet.view.gameObject == obj)
                    Collide(bullet, entity.collision.collision.contacts);
            }
          
            
        }
    }

    private void Collide(GameEntity bullet, ContactPoint2D[] contacts)
    {
        foreach (var contact in contacts)
        {

            foreach (var damageable in _damageable)
            {
                if (damageable != null && damageable != bullet && damageable.view.gameObject == contact.otherCollider.gameObject)
                {
                    damageable.ReplaceHitpoints(damageable.hitpoints.hp - 1);
                    bullet.isDestroyed = true;
                }
            }
        }
      //  var damageable = _damageable.GetSingleEntity();
        //.FirstOrDefault(e => e != bullet &&
          //                               e.view.gameObject == contact.otherCollider.gameObject);

     //   damageable.view.gameObject = contact.otherCollider.gameObject;
       
    }


}
