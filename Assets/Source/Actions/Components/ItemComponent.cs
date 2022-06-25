using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action
{
    [Action]
    public struct ItemComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ItemID;
    }
}
