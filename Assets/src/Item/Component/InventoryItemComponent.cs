using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Components
{
    [Game]
    sealed public class InventoryItemComponent : IComponent // Indicates item is inside a Inventory.
    {
        [EntityIndex]
        public int      InventoryID;
        public int      SlotNumber;
    }
}
