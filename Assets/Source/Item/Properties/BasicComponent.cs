using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.Property
{
    [ItemProperties]
    public class Component : IComponent
    {
        [PrimaryEntityIndex]
        public ItemType     ItemType;

        public string       Label;
    }
}
