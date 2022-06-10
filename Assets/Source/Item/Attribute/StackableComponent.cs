using Entitas;
using UnityEngine;

namespace Item.Attribute
{
    public struct StackableComponent : IComponent
    {
        public int MaxStackSize;
    }
}
