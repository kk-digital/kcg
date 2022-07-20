using System;
using Entitas;
using UnityEngine;

namespace Item
{
    [ItemInventory, ItemParticle]
    public class StackComponent : IComponent
    {
        /// <summary>
        /// Number of Component in the stack.
        /// </summary>
        public int Count;
    }
}
