//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiInterfaceGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial interface IItemLabelEntity {

    Item.LabelComponent itemLabel { get; }
    bool hasItemLabel { get; }

    void AddItemLabel(string newItemName);
    void ReplaceItemLabel(string newItemName);
    void RemoveItemLabel();
}
