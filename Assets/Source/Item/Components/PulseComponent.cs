using Entitas;
using UnityEngine;

namespace Item.PulseWeapon
{
    [ItemParticle, ItemInventory]
    public class PulseComponent : IComponent
    {
        public bool GrenadeMode;
        [Range(0, 12)]
        public int NumberOfGrenades;
    }
}

