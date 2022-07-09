//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class ItemEntity {

    public Item.FireWeapon.ChargeComponent itemFireWeaponCharge { get { return (Item.FireWeapon.ChargeComponent)GetComponent(ItemComponentsLookup.ItemFireWeaponCharge); } }
    public bool hasItemFireWeaponCharge { get { return HasComponent(ItemComponentsLookup.ItemFireWeaponCharge); } }

    public void AddItemFireWeaponCharge(bool newCanCharge, float newChargeRate, float newChargeMin, float newChargeMax) {
        var index = ItemComponentsLookup.ItemFireWeaponCharge;
        var component = (Item.FireWeapon.ChargeComponent)CreateComponent(index, typeof(Item.FireWeapon.ChargeComponent));
        component.CanCharge = newCanCharge;
        component.ChargeRate = newChargeRate;
        component.ChargeMin = newChargeMin;
        component.ChargeMax = newChargeMax;
        AddComponent(index, component);
    }

    public void ReplaceItemFireWeaponCharge(bool newCanCharge, float newChargeRate, float newChargeMin, float newChargeMax) {
        var index = ItemComponentsLookup.ItemFireWeaponCharge;
        var component = (Item.FireWeapon.ChargeComponent)CreateComponent(index, typeof(Item.FireWeapon.ChargeComponent));
        component.CanCharge = newCanCharge;
        component.ChargeRate = newChargeRate;
        component.ChargeMin = newChargeMin;
        component.ChargeMax = newChargeMax;
        ReplaceComponent(index, component);
    }

    public void RemoveItemFireWeaponCharge() {
        RemoveComponent(ItemComponentsLookup.ItemFireWeaponCharge);
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

    static Entitas.IMatcher<ItemEntity> _matcherItemFireWeaponCharge;

    public static Entitas.IMatcher<ItemEntity> ItemFireWeaponCharge {
        get {
            if (_matcherItemFireWeaponCharge == null) {
                var matcher = (Entitas.Matcher<ItemEntity>)Entitas.Matcher<ItemEntity>.AllOf(ItemComponentsLookup.ItemFireWeaponCharge);
                matcher.componentNames = ItemComponentsLookup.componentNames;
                _matcherItemFireWeaponCharge = matcher;
            }

            return _matcherItemFireWeaponCharge;
        }
    }
}
