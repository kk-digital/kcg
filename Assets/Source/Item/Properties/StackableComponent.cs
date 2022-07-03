using Entitas;
using UnityEngine;

namespace Item.Property
{
    [ItemProperties]
    public class StackableComponent : IComponent
    {
        public int MaxStackSize;
    }
}
