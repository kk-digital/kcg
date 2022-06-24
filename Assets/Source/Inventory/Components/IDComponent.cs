using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Inventory
{
    [Inventory]
    public struct IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int  ID;
    }
}

