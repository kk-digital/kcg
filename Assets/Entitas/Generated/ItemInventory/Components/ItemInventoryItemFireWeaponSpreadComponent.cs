//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ItemInventoryEntity {

    public Item.FireWeapon.SpreadComponent itemFireWeaponSpread { get { return (Item.FireWeapon.SpreadComponent)GetComponent(ItemInventoryComponentsLookup.ItemFireWeaponSpread); } }
    public bool hasItemFireWeaponSpread { get { return HasComponent(ItemInventoryComponentsLookup.ItemFireWeaponSpread); } }

    public void AddItemFireWeaponSpread(float newSpreadAngle) {
        var index = ItemInventoryComponentsLookup.ItemFireWeaponSpread;
        var component = (Item.FireWeapon.SpreadComponent)CreateComponent(index, typeof(Item.FireWeapon.SpreadComponent));
        component.SpreadAngle = newSpreadAngle;
        AddComponent(index, component);
    }

    public void ReplaceItemFireWeaponSpread(float newSpreadAngle) {
        var index = ItemInventoryComponentsLookup.ItemFireWeaponSpread;
        var component = (Item.FireWeapon.SpreadComponent)CreateComponent(index, typeof(Item.FireWeapon.SpreadComponent));
        component.SpreadAngle = newSpreadAngle;
        ReplaceComponent(index, component);
    }

    public void RemoveItemFireWeaponSpread() {
        RemoveComponent(ItemInventoryComponentsLookup.ItemFireWeaponSpread);
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
public partial class ItemInventoryEntity : IItemFireWeaponSpreadEntity { }

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class ItemInventoryMatcher {

    static Entitas.IMatcher<ItemInventoryEntity> _matcherItemFireWeaponSpread;

    public static Entitas.IMatcher<ItemInventoryEntity> ItemFireWeaponSpread {
        get {
            if (_matcherItemFireWeaponSpread == null) {
                var matcher = (Entitas.Matcher<ItemInventoryEntity>)Entitas.Matcher<ItemInventoryEntity>.AllOf(ItemInventoryComponentsLookup.ItemFireWeaponSpread);
                matcher.componentNames = ItemInventoryComponentsLookup.componentNames;
                _matcherItemFireWeaponSpread = matcher;
            }

            return _matcherItemFireWeaponSpread;
        }
    }
}
