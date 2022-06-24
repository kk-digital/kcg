using Entitas;
using UnityEngine;

namespace Item.Attribute
{
    [ItemProperties]
    public struct StackableComponent : IComponent
    {
        public int MaxStackSize;
    }
}
