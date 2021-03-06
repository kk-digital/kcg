//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class AgentEntity {

    public Physics.Box2DColliderComponent physicsBox2DCollider { get { return (Physics.Box2DColliderComponent)GetComponent(AgentComponentsLookup.PhysicsBox2DCollider); } }
    public bool hasPhysicsBox2DCollider { get { return HasComponent(AgentComponentsLookup.PhysicsBox2DCollider); } }

    public void AddPhysicsBox2DCollider(KMath.Vec2f newSize, KMath.Vec2f newOffset) {
        var index = AgentComponentsLookup.PhysicsBox2DCollider;
        var component = (Physics.Box2DColliderComponent)CreateComponent(index, typeof(Physics.Box2DColliderComponent));
        component.Size = newSize;
        component.Offset = newOffset;
        AddComponent(index, component);
    }

    public void ReplacePhysicsBox2DCollider(KMath.Vec2f newSize, KMath.Vec2f newOffset) {
        var index = AgentComponentsLookup.PhysicsBox2DCollider;
        var component = (Physics.Box2DColliderComponent)CreateComponent(index, typeof(Physics.Box2DColliderComponent));
        component.Size = newSize;
        component.Offset = newOffset;
        ReplaceComponent(index, component);
    }

    public void RemovePhysicsBox2DCollider() {
        RemoveComponent(AgentComponentsLookup.PhysicsBox2DCollider);
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
public partial class AgentEntity : IPhysicsBox2DColliderEntity { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class AgentMatcher {

    static Entitas.IMatcher<AgentEntity> _matcherPhysicsBox2DCollider;

    public static Entitas.IMatcher<AgentEntity> PhysicsBox2DCollider {
        get {
            if (_matcherPhysicsBox2DCollider == null) {
                var matcher = (Entitas.Matcher<AgentEntity>)Entitas.Matcher<AgentEntity>.AllOf(AgentComponentsLookup.PhysicsBox2DCollider);
                matcher.componentNames = AgentComponentsLookup.componentNames;
                _matcherPhysicsBox2DCollider = matcher;
            }

            return _matcherPhysicsBox2DCollider;
        }
    }
}
