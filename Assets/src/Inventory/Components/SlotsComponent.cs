using System.Collections;
using Entitas;

namespace Inventory
{
    public class SlotsComponent : IComponent
    {
        // Used to fast searching fist empty slot. Should has the size of width * Heigh.
        /// <summary>
        /// Current Slots
        /// </summary>
        public BitArray Values; // Todo: Implement faster bitfield implementation.
        /// <summary>
        /// Selected slot
        /// </summary>
        public int  Selected;
    }
}

