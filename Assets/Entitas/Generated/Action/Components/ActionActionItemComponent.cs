//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ActionEntity {

    public Action.ItemComponent actionItem { get { return (Action.ItemComponent)GetComponent(ActionComponentsLookup.ActionItem); } }
    public bool hasActionItem { get { return HasComponent(ActionComponentsLookup.ActionItem); } }

    public void AddActionItem(int newItemID) {
        var index = ActionComponentsLookup.ActionItem;
        var component = (Action.ItemComponent)CreateComponent(index, typeof(Action.ItemComponent));
        component.ItemID = newItemID;
        AddComponent(index, component);
    }

    public void ReplaceActionItem(int newItemID) {
        var index = ActionComponentsLookup.ActionItem;
        var component = (Action.ItemComponent)CreateComponent(index, typeof(Action.ItemComponent));
        component.ItemID = newItemID;
        ReplaceComponent(index, component);
    }

    public void RemoveActionItem() {
        RemoveComponent(ActionComponentsLookup.ActionItem);
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

    static Entitas.IMatcher<ActionEntity> _matcherActionItem;

    public static Entitas.IMatcher<ActionEntity> ActionItem {
        get {
            if (_matcherActionItem == null) {
                var matcher = (Entitas.Matcher<ActionEntity>)Entitas.Matcher<ActionEntity>.AllOf(ActionComponentsLookup.ActionItem);
                matcher.componentNames = ActionComponentsLookup.componentNames;
                _matcherActionItem = matcher;
            }

            return _matcherActionItem;
        }
    }
}