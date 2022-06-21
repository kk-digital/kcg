//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Action.BeginCoolDownComponent actionBeginCoolDown { get { return (Action.BeginCoolDownComponent)GetComponent(GameComponentsLookup.ActionBeginCoolDown); } }
    public bool hasActionBeginCoolDown { get { return HasComponent(GameComponentsLookup.ActionBeginCoolDown); } }

    public void AddActionBeginCoolDown(float newStartTime) {
        var index = GameComponentsLookup.ActionBeginCoolDown;
        var component = (Action.BeginCoolDownComponent)CreateComponent(index, typeof(Action.BeginCoolDownComponent));
        component.StartTime = newStartTime;
        AddComponent(index, component);
    }

    public void ReplaceActionBeginCoolDown(float newStartTime) {
        var index = GameComponentsLookup.ActionBeginCoolDown;
        var component = (Action.BeginCoolDownComponent)CreateComponent(index, typeof(Action.BeginCoolDownComponent));
        component.StartTime = newStartTime;
        ReplaceComponent(index, component);
    }

    public void RemoveActionBeginCoolDown() {
        RemoveComponent(GameComponentsLookup.ActionBeginCoolDown);
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

    static Entitas.IMatcher<GameEntity> _matcherActionBeginCoolDown;

    public static Entitas.IMatcher<GameEntity> ActionBeginCoolDown {
        get {
            if (_matcherActionBeginCoolDown == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ActionBeginCoolDown);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherActionBeginCoolDown = matcher;
            }

            return _matcherActionBeginCoolDown;
        }
    }
}