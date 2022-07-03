//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class InventoryEntity {

    public Inventory.SlotsComponent inventorySlots { get { return (Inventory.SlotsComponent)GetComponent(InventoryComponentsLookup.InventorySlots); } }
    public bool hasInventorySlots { get { return HasComponent(InventoryComponentsLookup.InventorySlots); } }

    public void AddInventorySlots(System.Collections.BitArray newValues, int newSelected) {
        var index = InventoryComponentsLookup.InventorySlots;
        var component = (Inventory.SlotsComponent)CreateComponent(index, typeof(Inventory.SlotsComponent));
        component.Values = newValues;
        component.Selected = newSelected;
        AddComponent(index, component);
    }

    public void ReplaceInventorySlots(System.Collections.BitArray newValues, int newSelected) {
        var index = InventoryComponentsLookup.InventorySlots;
        var component = (Inventory.SlotsComponent)CreateComponent(index, typeof(Inventory.SlotsComponent));
        component.Values = newValues;
        component.Selected = newSelected;
        ReplaceComponent(index, component);
    }

    public void RemoveInventorySlots() {
        RemoveComponent(InventoryComponentsLookup.InventorySlots);
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
public sealed partial class InventoryMatcher {

    static Entitas.IMatcher<InventoryEntity> _matcherInventorySlots;

    public static Entitas.IMatcher<InventoryEntity> InventorySlots {
        get {
            if (_matcherInventorySlots == null) {
                var matcher = (Entitas.Matcher<InventoryEntity>)Entitas.Matcher<InventoryEntity>.AllOf(InventoryComponentsLookup.InventorySlots);
                matcher.componentNames = InventoryComponentsLookup.componentNames;
                _matcherInventorySlots = matcher;
            }

            return _matcherInventorySlots;
        }
    }
}
