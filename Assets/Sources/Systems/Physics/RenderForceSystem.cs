using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class RenderForceSystem : ReactiveSystem<GameEntity>
{
    public RenderForceSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Force, GameMatcher.Rigidbody));
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasForce;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            ApplyForces(e);
            ApplyTorque(e);

            e.force.torque = 0;
            e.force.relativeForces.Clear();
        }
    }

    private void ApplyTorque(GameEntity entity)
    {
        entity.rigidbody.rigidbody.AddTorque(entity.force.torque, UnityEngine.ForceMode2D.Impulse);
    }

    private void ApplyForces(GameEntity entity)
    {
        foreach (var force in entity.force.relativeForces)
            entity.rigidbody.rigidbody.AddRelativeForce(force, UnityEngine.ForceMode2D.Impulse);
    }


}
