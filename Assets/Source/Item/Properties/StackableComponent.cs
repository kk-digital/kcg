using Entitas;
using UnityEngine;

namespace Item.Property
{
    [ItemProperties]
    public struct StackableComponent : IComponent
    {
        public int MaxStackSize;
    }
}
