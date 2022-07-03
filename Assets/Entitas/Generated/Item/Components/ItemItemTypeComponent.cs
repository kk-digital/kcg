//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ItemEntity {

    public Item.TypeComponent itemType { get { return (Item.TypeComponent)GetComponent(ItemComponentsLookup.ItemType); } }
    public bool hasItemType { get { return HasComponent(ItemComponentsLookup.ItemType); } }

    public void AddItemType(Enums.ItemType newType) {
        var index = ItemComponentsLookup.ItemType;
        var component = (Item.TypeComponent)CreateComponent(index, typeof(Item.TypeComponent));
        component.Type = newType;
        AddComponent(index, component);
    }

    public void ReplaceItemType(Enums.ItemType newType) {
        var index = ItemComponentsLookup.ItemType;
        var component = (Item.TypeComponent)CreateComponent(index, typeof(Item.TypeComponent));
        component.Type = newType;
        ReplaceComponent(index, component);
    }

    public void RemoveItemType() {
        RemoveComponent(ItemComponentsLookup.ItemType);
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

    static Entitas.IMatcher<ItemEntity> _matcherItemType;

    public static Entitas.IMatcher<ItemEntity> ItemType {
        get {
            if (_matcherItemType == null) {
                var matcher = (Entitas.Matcher<ItemEntity>)Entitas.Matcher<ItemEntity>.AllOf(ItemComponentsLookup.ItemType);
                matcher.componentNames = ItemComponentsLookup.componentNames;
                _matcherItemType = matcher;
            }

            return _matcherItemType;
        }
    }
}
