using Entitas;

namespace Item.PulseWeapon
{
    [ItemParticle, ItemInventory]
    public class PulseComponent : IComponent
    {
        public bool GrenadeMode;
        public int NumberOfGrenades;
    }
}

