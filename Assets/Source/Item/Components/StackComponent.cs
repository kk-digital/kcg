using System;
using Entitas;
using UnityEngine;

namespace Item
{
    [Item]
    public class StackComponent : IComponent
    {
        /// <summary>
        /// Number of Component in the stack.
        /// </summary>
        public int Count;
    }
}
