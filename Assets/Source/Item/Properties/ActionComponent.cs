using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Item.Attribute
{
    [ItemProperties]
    public struct ActionComponent : IComponent
    {
        [EntityIndex]
        public int ActionTypeID;
    }
}
