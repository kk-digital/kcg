//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ActionPropertiesEntity {

    public Action.Property.ShieldComponent actionPropertyShield { get { return (Action.Property.ShieldComponent)GetComponent(ActionPropertiesComponentsLookup.ActionPropertyShield); } }
    public bool hasActionPropertyShield { get { return HasComponent(ActionPropertiesComponentsLookup.ActionPropertyShield); } }

    public void AddActionPropertyShield(bool newShieldActive) {
        var index = ActionPropertiesComponentsLookup.ActionPropertyShield;
        var component = (Action.Property.ShieldComponent)CreateComponent(index, typeof(Action.Property.ShieldComponent));
        component.ShieldActive = newShieldActive;
        AddComponent(index, component);
    }

    public void ReplaceActionPropertyShield(bool newShieldActive) {
        var index = ActionPropertiesComponentsLookup.ActionPropertyShield;
        var component = (Action.Property.ShieldComponent)CreateComponent(index, typeof(Action.Property.ShieldComponent));
        component.ShieldActive = newShieldActive;
        ReplaceComponent(index, component);
    }

    public void RemoveActionPropertyShield() {
        RemoveComponent(ActionPropertiesComponentsLookup.ActionPropertyShield);
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
public sealed partial class ActionPropertiesMatcher {

    static Entitas.IMatcher<ActionPropertiesEntity> _matcherActionPropertyShield;

    public static Entitas.IMatcher<ActionPropertiesEntity> ActionPropertyShield {
        get {
            if (_matcherActionPropertyShield == null) {
                var matcher = (Entitas.Matcher<ActionPropertiesEntity>)Entitas.Matcher<ActionPropertiesEntity>.AllOf(ActionPropertiesComponentsLookup.ActionPropertyShield);
                matcher.componentNames = ActionPropertiesComponentsLookup.componentNames;
                _matcherActionPropertyShield = matcher;
            }

            return _matcherActionPropertyShield;
        }
    }
}
