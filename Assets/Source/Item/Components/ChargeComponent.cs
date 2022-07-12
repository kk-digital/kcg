using Entitas;
using Entitas.CodeGeneration.Attributes;
using Enums;

namespace Item.FireWeapon
{
    [ItemInventory]
    public class ChargeComponent : IComponent
    {
        public bool CanCharge;
        public float ChargeRate;
        public float ChargeMin;
        public float ChargeMax;
    }
}

