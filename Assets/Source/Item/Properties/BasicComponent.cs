using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.Attributes
{
    [ItemProperties]
    public struct Component : IComponent
    {
        [PrimaryEntityIndex]
        public ItemType     ItemType;

        public string       Label;
    }
}
