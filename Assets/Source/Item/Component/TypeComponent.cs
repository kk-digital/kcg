using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Item
{
    [Item]
    public class TypeComponent : IComponent
    {
        [EntityIndex]
        public Enums.ItemType Type;
    }
}
