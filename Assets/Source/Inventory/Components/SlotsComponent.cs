using System.Collections;
using Entitas;

namespace Inventory
{
    [Inventory]
    public class SlotsComponent : IComponent
    {
        /// <summary>
        /// Current Slots(inventory.Width * inventory.Heigh)
        /// </summary>
        public BitArray Values; // Todo: Implement faster bitfield implementation.
        /// <summary>
        /// Selected slot
        /// </summary>
        public int  Selected;
    }
}

