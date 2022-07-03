using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class ShipCollisionSystem : ReactiveSystem<GameEntity>
{
    readonly IGroup<GameEntity> _shipslist;
    readonly GameContext _contexts;
    readonly IGroup<GameEntity> _lives;

    public ShipCollisionSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts.game;
        _lives = contexts.game.GetGroup(GameMatcher.Lives);
        _shipslist = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Ship, GameMatcher.View));
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

           
            foreach (var ship in _shipslist.GetEntities())
            {
               
                if (ship != null && ship.view.gameObject == obj)
                {
                    Debug.Log("here");
                    Collide(ship);
                }
                    
            }
           
        }
    }

    private void Collide(GameEntity ship)
    {
        
        ship.isDestroyed = true;
        DecrementLives();
        var e = _contexts.CreateEntity();
        e.AddResource("prefabs/ShipExplosionEffects");
        e.AddAge(0);
        e.AddPosition(new Vector3(ship.position.value.x, ship.position.value.y, ship.position.value.z));
        e.AddMaxAge(3);
        e.isShipDeathroes = true;
    }

    private void DecrementLives()
    {
        var lives = _lives.GetSingleEntity();
        lives.ReplaceLives(lives.lives.lives - 1);
    }

}
