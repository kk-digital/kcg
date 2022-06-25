using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Item.Property
{
    [ItemProperties]
    public struct ActionComponent : IComponent
    {
        [EntityIndex]
        public int ActionTypeID;
    }
}
