//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial interface IItemFireWeaponSpreadEntity {

    Item.FireWeapon.SpreadComponent itemFireWeaponSpread { get; }
    bool hasItemFireWeaponSpread { get; }

    void AddItemFireWeaponSpread(float newSpreadAngle);
    void ReplaceItemFireWeaponSpread(float newSpreadAngle);
    void RemoveItemFireWeaponSpread();
}