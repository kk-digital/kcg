using System.Collections;
using Entitas;

namespace Inventory
{
    public class InventorySlotComponent : IComponent
    {
        public int  SelectedSlot;
        // Used to fast searching fist empty slot. Should has the size of width * Heigh.
        public BitArray Slots; // Todo: Implement faster bitfield implementation.
    }
}

