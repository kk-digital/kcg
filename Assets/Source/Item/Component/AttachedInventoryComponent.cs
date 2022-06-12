using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Item
{
    public struct AttachedInventoryComponent : IComponent // Indicates item is inside a Inventory.
    {
        [EntityIndex]
        public int      InventoryID;
        public int      SlotNumber;
    }
}
