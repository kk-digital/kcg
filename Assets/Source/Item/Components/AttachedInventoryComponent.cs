using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Item
{
    [Item]
    public class AttachedInventoryComponent : IComponent // Indicates item is inside a Inventory.
    {
        [EntityIndex]
        public int      InventoryID;
        public int      SlotNumber;
    }
}
