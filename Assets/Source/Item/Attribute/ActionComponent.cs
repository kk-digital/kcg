using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Item.Attribute
{
    public struct ActionComponent : IComponent
    {
        [EntityIndex]
        public int ActionID;
    }
}
