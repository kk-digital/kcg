//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ActionEntity {

    public Action.InterruptComponent actionInterrupt { get { return (Action.InterruptComponent)GetComponent(ActionComponentsLookup.ActionInterrupt); } }
    public bool hasActionInterrupt { get { return HasComponent(ActionComponentsLookup.ActionInterrupt); } }

    public void AddActionInterrupt(int newID) {
        var index = ActionComponentsLookup.ActionInterrupt;
        var component = (Action.InterruptComponent)CreateComponent(index, typeof(Action.InterruptComponent));
        component.ID = newID;
        AddComponent(index, component);
    }

    public void ReplaceActionInterrupt(int newID) {
        var index = ActionComponentsLookup.ActionInterrupt;
        var component = (Action.InterruptComponent)CreateComponent(index, typeof(Action.InterruptComponent));
        component.ID = newID;
        ReplaceComponent(index, component);
    }

    public void RemoveActionInterrupt() {
        RemoveComponent(ActionComponentsLookup.ActionInterrupt);
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
public sealed partial class ActionMatcher {

    static Entitas.IMatcher<ActionEntity> _matcherActionInterrupt;

    public static Entitas.IMatcher<ActionEntity> ActionInterrupt {
        get {
            if (_matcherActionInterrupt == null) {
                var matcher = (Entitas.Matcher<ActionEntity>)Entitas.Matcher<ActionEntity>.AllOf(ActionComponentsLookup.ActionInterrupt);
                matcher.componentNames = ActionComponentsLookup.componentNames;
                _matcherActionInterrupt = matcher;
            }

            return _matcherActionInterrupt;
        }
    }
}
