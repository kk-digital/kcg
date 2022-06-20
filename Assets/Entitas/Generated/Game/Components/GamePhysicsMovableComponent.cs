//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Physics.MovableComponent physicsMovable { get { return (Physics.MovableComponent)GetComponent(GameComponentsLookup.PhysicsMovable); } }
    public bool hasPhysicsMovable { get { return HasComponent(GameComponentsLookup.PhysicsMovable); } }

    public void AddPhysicsMovable(float newSpeed, KMath.Vec2f newVelocity, KMath.Vec2f newAcceleration) {
        var index = GameComponentsLookup.PhysicsMovable;
        var component = (Physics.MovableComponent)CreateComponent(index, typeof(Physics.MovableComponent));
        component.Speed = newSpeed;
        component.Velocity = newVelocity;
        component.Acceleration = newAcceleration;
        AddComponent(index, component);
    }

    public void ReplacePhysicsMovable(float newSpeed, KMath.Vec2f newVelocity, KMath.Vec2f newAcceleration) {
        var index = GameComponentsLookup.PhysicsMovable;
        var component = (Physics.MovableComponent)CreateComponent(index, typeof(Physics.MovableComponent));
        component.Speed = newSpeed;
        component.Velocity = newVelocity;
        component.Acceleration = newAcceleration;
        ReplaceComponent(index, component);
    }

    public void RemovePhysicsMovable() {
        RemoveComponent(GameComponentsLookup.PhysicsMovable);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherPhysicsMovable;

    public static Entitas.IMatcher<GameEntity> PhysicsMovable {
        get {
            if (_matcherPhysicsMovable == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.PhysicsMovable);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherPhysicsMovable = matcher;
            }

            return _matcherPhysicsMovable;
        }
    }
}