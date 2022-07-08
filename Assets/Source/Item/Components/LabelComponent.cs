using Entitas;

namespace Item
{
    [ItemInventory, ItemParticle]
    public class LabelComponent : IComponent
    {
        public string ItemName;
    }
}
