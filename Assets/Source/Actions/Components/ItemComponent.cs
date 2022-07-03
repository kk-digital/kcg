using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action
{
    [Action]
    public class ItemComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ItemID;
    }
}
