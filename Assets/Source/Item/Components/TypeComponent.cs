using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Item
{
    [ItemInventory, ItemParticle]
    public class TypeComponent : IComponent
    {
        [EntityIndex]
        public Enums.ItemType Type;
    }
}
