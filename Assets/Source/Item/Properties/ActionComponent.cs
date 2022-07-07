using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Item.Property
{
    [ItemProperties]
    public class ActionComponent : IComponent
    {
        [EntityIndex]
        public Enums.ActionType ActionTypeID;
    }
}
