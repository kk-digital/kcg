using System;
using Entitas;
using UnityEngine;

namespace Item
{
    public struct StackComponent : IComponent
    {
        public int Count; // Number of Component in the stack.
    }
}
