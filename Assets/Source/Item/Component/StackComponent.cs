using Entitas;
using UnityEngine;

namespace Item
{
    public class StackComponent : IComponent // Make item Stackable.
    {
        public int StackCount;
        public int MaxStackSize;
    }
}
