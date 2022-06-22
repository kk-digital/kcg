using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action
{
    public struct ItemComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ItemID;
    }
}
