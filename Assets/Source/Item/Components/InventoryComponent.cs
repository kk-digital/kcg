using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Item
{
    [ItemInventory]
    public class InventoryComponent : IComponent // Indicates item is inside a Inventory.
    {
        [EntityIndex]
        public int      InventoryID;
        public int      SlotNumber;
    }
}
