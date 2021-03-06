//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ItemInventoryEntity {

    public Item.LabelComponent itemLabel { get { return (Item.LabelComponent)GetComponent(ItemInventoryComponentsLookup.ItemLabel); } }
    public bool hasItemLabel { get { return HasComponent(ItemInventoryComponentsLookup.ItemLabel); } }

    public void AddItemLabel(string newItemName) {
        var index = ItemInventoryComponentsLookup.ItemLabel;
        var component = (Item.LabelComponent)CreateComponent(index, typeof(Item.LabelComponent));
        component.ItemName = newItemName;
        AddComponent(index, component);
    }

    public void ReplaceItemLabel(string newItemName) {
        var index = ItemInventoryComponentsLookup.ItemLabel;
        var component = (Item.LabelComponent)CreateComponent(index, typeof(Item.LabelComponent));
        component.ItemName = newItemName;
        ReplaceComponent(index, component);
    }

    public void RemoveItemLabel() {
        RemoveComponent(ItemInventoryComponentsLookup.ItemLabel);
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
public partial class ItemInventoryEntity : IItemLabelEntity { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class ItemInventoryMatcher {

    static Entitas.IMatcher<ItemInventoryEntity> _matcherItemLabel;

    public static Entitas.IMatcher<ItemInventoryEntity> ItemLabel {
        get {
            if (_matcherItemLabel == null) {
                var matcher = (Entitas.Matcher<ItemInventoryEntity>)Entitas.Matcher<ItemInventoryEntity>.AllOf(ItemInventoryComponentsLookup.ItemLabel);
                matcher.componentNames = ItemInventoryComponentsLookup.componentNames;
                _matcherItemLabel = matcher;
            }

            return _matcherItemLabel;
        }
    }
}
