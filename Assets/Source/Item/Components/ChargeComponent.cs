using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.FireWeapon
{
    [ItemParticle, ItemInventory]
    public class ChargeComponent : IComponent
    {
        public bool CanCharge;
        public float ChargeRate;
        public float ChargeRatio;
        public float ChargePerShot;
        public float ChargeMin;
        public float ChargeMax;
    }
}

