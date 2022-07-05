//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class AgentEntity {

    public Physics.MovableComponent physicsMovable { get { return (Physics.MovableComponent)GetComponent(AgentComponentsLookup.PhysicsMovable); } }
    public bool hasPhysicsMovable { get { return HasComponent(AgentComponentsLookup.PhysicsMovable); } }

    public void AddPhysicsMovable(float newSpeed, KMath.Vec2f newVelocity, KMath.Vec2f newAcceleration, bool newAffectedByGravity, bool newAffectedByGroundFriction, bool newInvulnerable, bool newLanded) {
        var index = AgentComponentsLookup.PhysicsMovable;
        var component = (Physics.MovableComponent)CreateComponent(index, typeof(Physics.MovableComponent));
        component.Speed = newSpeed;
        component.Velocity = newVelocity;
        component.Acceleration = newAcceleration;
        component.AffectedByGravity = newAffectedByGravity;
        component.AffectedByGroundFriction = newAffectedByGroundFriction;
        component.Invulnerable = newInvulnerable;
        component.Landed = newLanded;
        AddComponent(index, component);
    }

    public void ReplacePhysicsMovable(float newSpeed, KMath.Vec2f newVelocity, KMath.Vec2f newAcceleration, bool newAffectedByGravity, bool newAffectedByGroundFriction, bool newInvulnerable, bool newLanded) {
        var index = AgentComponentsLookup.PhysicsMovable;
        var component = (Physics.MovableComponent)CreateComponent(index, typeof(Physics.MovableComponent));
        component.Speed = newSpeed;
        component.Velocity = newVelocity;
        component.Acceleration = newAcceleration;
        component.AffectedByGravity = newAffectedByGravity;
        component.AffectedByGroundFriction = newAffectedByGroundFriction;
        component.Invulnerable = newInvulnerable;
        component.Landed = newLanded;
        ReplaceComponent(index, component);
    }

    public void RemovePhysicsMovable() {
        RemoveComponent(AgentComponentsLookup.PhysicsMovable);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class AgentEntity : IPhysicsMovableEntity { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class AgentMatcher {

    static Entitas.IMatcher<AgentEntity> _matcherPhysicsMovable;

    public static Entitas.IMatcher<AgentEntity> PhysicsMovable {
        get {
            if (_matcherPhysicsMovable == null) {
                var matcher = (Entitas.Matcher<AgentEntity>)Entitas.Matcher<AgentEntity>.AllOf(AgentComponentsLookup.PhysicsMovable);
                matcher.componentNames = AgentComponentsLookup.componentNames;
                _matcherPhysicsMovable = matcher;
            }

            return _matcherPhysicsMovable;
        }
    }
}
