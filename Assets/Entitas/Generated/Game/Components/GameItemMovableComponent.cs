//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Item.MovableComponent itemMovable { get { return (Item.MovableComponent)GetComponent(GameComponentsLookup.ItemMovable); } }
    public bool hasItemMovable { get { return HasComponent(GameComponentsLookup.ItemMovable); } }

    public void AddItemMovable(float newSpeed, UnityEngine.Vector2 newVelocity, UnityEngine.Vector2 newAcceleration, float newAccelerationTime) {
        var index = GameComponentsLookup.ItemMovable;
        var component = (Item.MovableComponent)CreateComponent(index, typeof(Item.MovableComponent));
        component.Speed = newSpeed;
        component.Velocity = newVelocity;
        component.Acceleration = newAcceleration;
        component.AccelerationTime = newAccelerationTime;
        AddComponent(index, component);
    }

    public void ReplaceItemMovable(float newSpeed, UnityEngine.Vector2 newVelocity, UnityEngine.Vector2 newAcceleration, float newAccelerationTime) {
        var index = GameComponentsLookup.ItemMovable;
        var component = (Item.MovableComponent)CreateComponent(index, typeof(Item.MovableComponent));
        component.Speed = newSpeed;
        component.Velocity = newVelocity;
        component.Acceleration = newAcceleration;
        component.AccelerationTime = newAccelerationTime;
        ReplaceComponent(index, component);
    }

    public void RemoveItemMovable() {
        RemoveComponent(GameComponentsLookup.ItemMovable);
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

    static Entitas.IMatcher<GameEntity> _matcherItemMovable;

    public static Entitas.IMatcher<GameEntity> ItemMovable {
        get {
            if (_matcherItemMovable == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ItemMovable);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherItemMovable = matcher;
            }

            return _matcherItemMovable;
        }
    }
}