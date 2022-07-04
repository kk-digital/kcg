using Entitas;

namespace Inventory
{
    [Inventory]
    public sealed class SizeComponent : IComponent
    {
        public int  Width;
        public int  Height;
    }
}
