using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Inventory
{
    [Inventory]
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int  ID;
    }
}

