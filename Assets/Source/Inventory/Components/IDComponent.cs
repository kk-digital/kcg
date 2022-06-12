using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Inventory
{
    public struct IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int  ID;
    }
}

