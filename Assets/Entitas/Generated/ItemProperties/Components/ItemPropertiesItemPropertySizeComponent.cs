//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ItemPropertiesEntity {

    public Item.Property.SizeComponent itemPropertySize { get { return (Item.Property.SizeComponent)GetComponent(ItemPropertiesComponentsLookup.ItemPropertySize); } }
    public bool hasItemPropertySize { get { return HasComponent(ItemPropertiesComponentsLookup.ItemPropertySize); } }

    public void AddItemPropertySize(KMath.Vec2f newSize) {
        var index = ItemPropertiesComponentsLookup.ItemPropertySize;
        var component = (Item.Property.SizeComponent)CreateComponent(index, typeof(Item.Property.SizeComponent));
        component.Size = newSize;
        AddComponent(index, component);
    }

    public void ReplaceItemPropertySize(KMath.Vec2f newSize) {
        var index = ItemPropertiesComponentsLookup.ItemPropertySize;
        var component = (Item.Property.SizeComponent)CreateComponent(index, typeof(Item.Property.SizeComponent));
        component.Size = newSize;
        ReplaceComponent(index, component);
    }

    public void RemoveItemPropertySize() {
        RemoveComponent(ItemPropertiesComponentsLookup.ItemPropertySize);
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
public sealed partial class ItemPropertiesMatcher {

    static Entitas.IMatcher<ItemPropertiesEntity> _matcherItemPropertySize;

    public static Entitas.IMatcher<ItemPropertiesEntity> ItemPropertySize {
        get {
            if (_matcherItemPropertySize == null) {
                var matcher = (Entitas.Matcher<ItemPropertiesEntity>)Entitas.Matcher<ItemPropertiesEntity>.AllOf(ItemPropertiesComponentsLookup.ItemPropertySize);
                matcher.componentNames = ItemPropertiesComponentsLookup.componentNames;
                _matcherItemPropertySize = matcher;
            }

            return _matcherItemPropertySize;
        }
    }
}
