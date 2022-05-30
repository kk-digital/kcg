using Entitas;
using UnityEngine;

namespace Components
{
    public class ItemStackComponent : IComponent // Make item Stackable.
    {
        public int StackCount;
        public int MaxStackSize;
    }
}
