//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Action.Attribute.TimeComponent actionAttributeTime { get { return (Action.Attribute.TimeComponent)GetComponent(GameComponentsLookup.ActionAttributeTime); } }
    public bool hasActionAttributeTime { get { return HasComponent(GameComponentsLookup.ActionAttributeTime); } }

    public void AddActionAttributeTime(float newDuration) {
        var index = GameComponentsLookup.ActionAttributeTime;
        var component = (Action.Attribute.TimeComponent)CreateComponent(index, typeof(Action.Attribute.TimeComponent));
        component.Duration = newDuration;
        AddComponent(index, component);
    }

    public void ReplaceActionAttributeTime(float newDuration) {
        var index = GameComponentsLookup.ActionAttributeTime;
        var component = (Action.Attribute.TimeComponent)CreateComponent(index, typeof(Action.Attribute.TimeComponent));
        component.Duration = newDuration;
        ReplaceComponent(index, component);
    }

    public void RemoveActionAttributeTime() {
        RemoveComponent(GameComponentsLookup.ActionAttributeTime);
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

    static Entitas.IMatcher<GameEntity> _matcherActionAttributeTime;

    public static Entitas.IMatcher<GameEntity> ActionAttributeTime {
        get {
            if (_matcherActionAttributeTime == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ActionAttributeTime);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherActionAttributeTime = matcher;
            }

            return _matcherActionAttributeTime;
        }
    }
}
