using System;
using Entitas;
using UnityEngine;

namespace Item
{
    public struct StackComponent : IComponent
    {
        /// <summary>
        /// Number of Component in the stack.
        /// </summary>
        public int Count;
    }
}
