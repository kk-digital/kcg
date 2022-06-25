using Entitas;

namespace Inventory
{
    [Inventory]
    public struct SizeComponent : IComponent
    {
        public int  Width;
        public int  Height;
    }
}
