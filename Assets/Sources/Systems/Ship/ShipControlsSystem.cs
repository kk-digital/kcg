using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class ShipControlsSystem : ReactiveSystem<GameEntity>
{
    readonly IGroup<GameEntity> _ships;

    public ShipControlsSystem(Contexts contexts) : base(contexts.game)
    {
        _ships = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Ship, GameMatcher.Controllable, GameMatcher.Force));
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
        foreach (var entity in entities)
        {
            if (entity.input.accelerate)
                Accelerate(1);

            if (entity.input.decelerate)
                Accelerate(-1);

            if (entity.input.turnLeft)
                Rotate(1);

            if (entity.input.turnRight)
                Rotate(-1);

            if (entity.input.strafeLeft)
                Strafe(-1);

            if (entity.input.strafeRight)
                Strafe(1);
        }
    }

    private void Rotate(int direction)
    {
        foreach (var entity in _ships.GetEntities())
        {
            var relativeForces = entity.force.relativeForces;
            var torque = entity.force.torque + entity.ship.rotationRate * direction;
            entity.ReplaceForce(relativeForces, torque);
        }
    }

    private void Accelerate(int direction)
    {
        foreach (var entity in _ships.GetEntities())
        {
            var relativeForces = entity.force.relativeForces;
            relativeForces.Add(new Vector2(0, entity.ship.accelerationRate * direction));
            var torque = entity.force.torque;
            entity.ReplaceForce(relativeForces, torque);
        }
    }

    private void Strafe(int direcction)
    {
        foreach (var entity in _ships.GetEntities())
        {
            var relativeForces = entity.force.relativeForces;
            relativeForces.Add(new Vector2(entity.ship.accelerationRate*direcction, 0));
            var torque = entity.force.torque;
            entity.ReplaceForce(relativeForces, torque);
        }
    }

}
