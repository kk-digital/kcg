//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ItemPropertiesEntity {

    public Item.Property.FireWeapon.MultiShootComponent itemPropertyFireWeaponMultiShoot { get { return (Item.Property.FireWeapon.MultiShootComponent)GetComponent(ItemPropertiesComponentsLookup.ItemPropertyFireWeaponMultiShoot); } }
    public bool hasItemPropertyFireWeaponMultiShoot { get { return HasComponent(ItemPropertiesComponentsLookup.ItemPropertyFireWeaponMultiShoot); } }

    public void AddItemPropertyFireWeaponMultiShoot(float newSpreadAngle, int newNumOfBullets) {
        var index = ItemPropertiesComponentsLookup.ItemPropertyFireWeaponMultiShoot;
        var component = (Item.Property.FireWeapon.MultiShootComponent)CreateComponent(index, typeof(Item.Property.FireWeapon.MultiShootComponent));
        component.SpreadAngle = newSpreadAngle;
        component.NumOfBullets = newNumOfBullets;
        AddComponent(index, component);
    }

    public void ReplaceItemPropertyFireWeaponMultiShoot(float newSpreadAngle, int newNumOfBullets) {
        var index = ItemPropertiesComponentsLookup.ItemPropertyFireWeaponMultiShoot;
        var component = (Item.Property.FireWeapon.MultiShootComponent)CreateComponent(index, typeof(Item.Property.FireWeapon.MultiShootComponent));
        component.SpreadAngle = newSpreadAngle;
        component.NumOfBullets = newNumOfBullets;
        ReplaceComponent(index, component);
    }

    public void RemoveItemPropertyFireWeaponMultiShoot() {
        RemoveComponent(ItemPropertiesComponentsLookup.ItemPropertyFireWeaponMultiShoot);
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

    static Entitas.IMatcher<ItemPropertiesEntity> _matcherItemPropertyFireWeaponMultiShoot;

    public static Entitas.IMatcher<ItemPropertiesEntity> ItemPropertyFireWeaponMultiShoot {
        get {
            if (_matcherItemPropertyFireWeaponMultiShoot == null) {
                var matcher = (Entitas.Matcher<ItemPropertiesEntity>)Entitas.Matcher<ItemPropertiesEntity>.AllOf(ItemPropertiesComponentsLookup.ItemPropertyFireWeaponMultiShoot);
                matcher.componentNames = ItemPropertiesComponentsLookup.componentNames;
                _matcherItemPropertyFireWeaponMultiShoot = matcher;
            }

            return _matcherItemPropertyFireWeaponMultiShoot;
        }
    }
}