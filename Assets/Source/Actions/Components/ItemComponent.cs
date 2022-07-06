using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action
{
    // Action may be directly related to items.
    [Action]
    public class ItemComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ItemID;
    }
}
