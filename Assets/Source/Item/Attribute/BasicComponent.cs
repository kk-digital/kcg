using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.Attributes
{
    public struct BasicComponent : IComponent
    {
        [PrimaryEntityIndex]
        public ItemType     ItemType;

        public string       Label;
    }
}
