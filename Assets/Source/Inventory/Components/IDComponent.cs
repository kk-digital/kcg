using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Inventory
{
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int  InventoryID;
    }
}

