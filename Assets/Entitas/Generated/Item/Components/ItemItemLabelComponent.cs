//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ItemEntity {

    public Item.LabelComponent itemLabel { get { return (Item.LabelComponent)GetComponent(ItemComponentsLookup.ItemLabel); } }
    public bool hasItemLabel { get { return HasComponent(ItemComponentsLookup.ItemLabel); } }

    public void AddItemLabel(string newItemName) {
        var index = ItemComponentsLookup.ItemLabel;
        var component = (Item.LabelComponent)CreateComponent(index, typeof(Item.LabelComponent));
        component.ItemName = newItemName;
        AddComponent(index, component);
    }

    public void ReplaceItemLabel(string newItemName) {
        var index = ItemComponentsLookup.ItemLabel;
        var component = (Item.LabelComponent)CreateComponent(index, typeof(Item.LabelComponent));
        component.ItemName = newItemName;
        ReplaceComponent(index, component);
    }

    public void RemoveItemLabel() {
        RemoveComponent(ItemComponentsLookup.ItemLabel);
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
public sealed partial class ItemMatcher {

    static Entitas.IMatcher<ItemEntity> _matcherItemLabel;

    public static Entitas.IMatcher<ItemEntity> ItemLabel {
        get {
            if (_matcherItemLabel == null) {
                var matcher = (Entitas.Matcher<ItemEntity>)Entitas.Matcher<ItemEntity>.AllOf(ItemComponentsLookup.ItemLabel);
                matcher.componentNames = ItemComponentsLookup.componentNames;
                _matcherItemLabel = matcher;
            }

            return _matcherItemLabel;
        }
    }
}
