using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Inventory
{
    public class InventoryIDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int  InventoryID;
    }
}

